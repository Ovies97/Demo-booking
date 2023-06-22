using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Demo_Booking_Lessons_For_Driving.Models;



namespace Demo_Booking_Lessons_For_Driving.Controllers
{
    public class EmployeeController : Controller
    {
        DbEntities1 db ;
        DbEntities dbLearners = new DbEntities();
        public EmployeeController()
        {
            db = new DbEntities1();
        }


      //registration action
      [HttpGet ]
      public ActionResult Registration()
        {
            GetAllActors();
            return View();
            
        }

        //registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken ]
        public ActionResult Registration([Bind(Exclude ="IsEmailVerified,ActivationCode,EmployeeID")]Employee employee )
        {
            bool Status = false;
            string message = "";

            //Model validation
          if(ModelState.IsValid )
            {
                #region //Email is already Exist
                 var isExist = IsEmailExist(employee.Email);
                if(isExist )
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(employee);
                }
                #endregion

                #region generate activation code
                employee.ActivationCode = Guid.NewGuid();
                #endregion
                #region password Hashing
                employee.Password = Crypto.Hash(employee.Password);
                employee.ConfirmedPassword = Crypto.Hash(employee.ConfirmedPassword);
                #endregion 
              #region save data to database
                employee.IsEmailVerified = false;
                
                    db .Employees.Add(employee);
                    db.SaveChanges();
                    //Sent Email to the user
                    if(employee.IsAdmin.ToString().Replace(" ","") == "Admin")
                {
                 SentVerificationLinkEmail(employee.Email, employee.ActivationCode.ToString());
                }
                   
                    message = "Registration successfully " + employee.Email;
                    Status = true;

                #endregion
            }
            else
            {
                message = "Ivalid Request";
            }
           
            ViewBag.Message = message;
            ViewBag.Status = Status;

            return View(employee );
        }
      
        [HttpGet ]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;

            db.Configuration.ValidateOnSaveEnabled = false;//Avoid Confirm password does not match issue on save changes
                var v = db.Employees.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if(v != null)
                {
                    v.IsEmailVerified = true;
                db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            
            ViewBag.Status = Status ;

            return View();

        }
        [HttpGet]
        public ActionResult Login()
        {
            GetAllActors();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(EmployeeLogin login, string returnUrl = "")
        {
            string message = "";
            GetAllActors();
            ViewBag.Email = login.Email ;
            var o = db.Employees.Where(m => m.Email == login.Email ).FirstOrDefault();
            if (o != null)
            { 
                if (o.IsAdmin.ToString().Replace(" ", "") == "Admin")
                {

                    var v = db.Employees.Where(a => a.Email == login.Email).FirstOrDefault();
                    if (v != null)
                    {
                        if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                        {
                            int _timeout = login.RememberMe ? 525000 : 20;
                            var _ticket = new FormsAuthenticationTicket(login.Email, login.RememberMe, _timeout);
                            string _encrypt = FormsAuthentication.Encrypt(_ticket);
                            var _cookie = new HttpCookie(FormsAuthentication.FormsCookieName, _encrypt);
                            _cookie.Expires = DateTime.Now.AddMinutes(_timeout);
                            _cookie.HttpOnly = true;
                            Response.Cookies.Add(_cookie);
                            if (Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("LandingPage", "Home");
                            }
                        } else
                        {
                            message = "Invalid credentials provided";
                        }
                    }
                    else
                    {
                        message = "Invalid credentials provided";
                    }
                }
                else if (o.IsAdmin.ToString().Replace(" ", "") == "Instructor")
                {
                    var v = db.Employees.Where(a => a.Email == login.Email).FirstOrDefault();
                    if (v != null)
                    {
                        if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                        {
                            int timeout = login.RememberMe ? 525000 : 20;
                            var ticket = new FormsAuthenticationTicket(login.Email, login.RememberMe, timeout);
                            string encrypt = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypt);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);
                            if (Url.IsLocalUrl(returnUrl))
                            {
                                //return Redirect(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("ViewInstr", "Employee");
                            }
                        }
                    }
                            
                }
                else if (o.IsAdmin.ToString().Replace(" ","") == "Learner")
                {
                    if(ModelState.IsValid )
                    {
var v = db.Employees.Where(a => a.Email == login.Email).FirstOrDefault();
                    if (v != null)
                    {
                        if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                        {
                            int zTimeout = login.RememberMe ? 525000 : 20;
                            var zTicket = new FormsAuthenticationTicket(login.Email, login.RememberMe, zTimeout);
                            string zEncrypt = FormsAuthentication.Encrypt(zTicket);
                            var zCookie = new HttpCookie(FormsAuthentication.FormsCookieName, zEncrypt);
                            zCookie.Expires = DateTime.Now.AddMinutes(zTimeout);
                            zCookie.HttpOnly = true;
                            Response.Cookies.Add(zCookie);
                            if (Url.IsLocalUrl(returnUrl))
                            {
                                //return Redirect(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Book", "Learner");
                            }
                        }
                    }
                    }
                    else
                    {
                        return RedirectToAction("Login", "Employee");
                    }
                    
             
                }
                else
                {
                    return View();
                }
        }
            return View();

        }

        [Authorize ]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login" ,"Employee");
        }
        [NonAction ]
        public bool IsEmailExist(string email)
        {
                var c = db.Employees.Where(a => a.Email == email).FirstOrDefault();
                return c != null;
        }

        [NonAction]
        public void  SentVerificationLinkEmail(string email,string activationCode,string emailFo = "VerifyAccount")
        {
            var verifyUrl = "/Employee/"+ emailFo +"/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var frmEmail = new MailAddress(email , "Municipality Complainant System");
            var toEmail = new MailAddress("ovayomvubu97@gmail.com");
            var frmEmailPassword = "******";//replace with actual password
            string sbj = "";
            string body = "";
            if (emailFo == "VerifyAccount")
            {

              sbj = "Your account is successfully created!";
              body   = "<br/><br/> We are excited to tell you that your account is " +
                "successfully created.please click on the below link to verify your account" +
                "<br/><br/><a href='" + link + "'>" + link + "</a>";
        
            }
            else if(emailFo == "ResetPassword")
            {
                sbj = "Reset Password";
               body = "Hello, <br/><br/> We got request for reset your account password.Please click on this Link to reset your password" +
                    "<br/><br/> <a href='" + link + "'>Reset Password link </a>";

            }
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false  ,
               
                Credentials = new System.Net.NetworkCredential(frmEmail.Address, frmEmailPassword)
                
            };
            MailMessage message = new MailMessage
            {
                Subject = sbj,
                Body = body,
                IsBodyHtml = true
            };
                message.To.Add(toEmail);
                message.From  = frmEmail;
                smtp.Send(message);
        }

        //Forgot password
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            //Verify email ID
            //If is valid generate password link
            //sent link
            string message = "";
           
                var acc = db.Employees.Where(a => a.Email == email).FirstOrDefault();
                if(acc != null )
                {
                    string resetCode = Guid.NewGuid().ToString();
                    SentVerificationLinkEmail(acc.Email, resetCode, "ResetPassword");
                    acc.ResetPasswordCode = resetCode;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                    message = "Reset password link has been sent to your email";
                }
                else
                {
                    message = "Account not found";
                }
            
            ViewBag.Message = message;
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
                var user = db.Employees.Where(a => a.ResetPasswordCode == id).DefaultIfEmpty();
                if(user != null )
                {
                    ResetPasswordModel passwordModel = new ResetPasswordModel();
                    passwordModel.ResetCode = id;
                    return View(passwordModel);
                }
                else
                {
                    return HttpNotFound();
                }
        }

        [HttpPost ]
        [ValidateAntiForgeryToken ]
        public ActionResult ResetPassword(ResetPasswordModel resetPassword )
        {
            var message = "";
            if (ModelState.IsValid )
            {
               
                    var user = db.Employees.Where(a => a.ResetPasswordCode == resetPassword.ResetCode).FirstOrDefault();
                    if(user != null )
                    {
                        user.Password = Crypto.Hash(resetPassword.NewPassword);
                        user.ResetPasswordCode = "";
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        message = "New password updated successfully";
                    }              
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(resetPassword);
        }


        [HttpGet ]
        public ActionResult ViewEmployees()
        {
                List<Employee> emp = db.Employees.ToList();
                ViewBag.Employees = emp;
                 return View(ViewBag.Employees);
        }
        
         
       //This method will allwo admin to edit user details .
        public ActionResult EditEmployee(int?  id)
        
        {     
                if(id != 0 )
                {
                   ViewBag.Employee = db.Employees.Where(a => a.EmployeeID == id).First();
                }
            
            return View(ViewBag.Employee);
        }

        // POST: Employees/Delete/5
       [HttpPost ]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public  ActionResult DeleteEmployee(int? id)
        {

                Employee employee = db.Employees.Find(id);
                db.Employees.Remove(employee);
                db.SaveChanges();
                return RedirectToAction("ViewEmployees");
        }

        public ActionResult AddEmployee()
        {
            GetAllActors();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmployee([Bind(Exclude = "IsEmailVerified,ActivationCode")] Employee employee)
        {

            string message = "";
            bool Status;
            try
            {
               
                    if (ModelState.IsValid)
                    {
                        employee.Password = Crypto.Hash(employee.Password);
                        employee.ConfirmedPassword = Crypto.Hash(employee.ConfirmedPassword);
                        if (IsEmailExist(employee.Email))
                        {
                            message = "Email Exist";
                        }
                        else
                        {
                            db.Employees.Add(employee);
                            db.SaveChanges();
                        }

                        message = "Saved " + employee.Name + " details successfully";
                    }
                    else
                    {
                        message = "Oops Error!!";
                    }
                
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            ViewBag.Message = message;
            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployee([Bind(Exclude = "IsEmailVerified,ActivationCode")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                   employee.Password = Crypto.Hash(employee.Password);
                   employee.ConfirmedPassword = Crypto.Hash(employee.ConfirmedPassword);              
                   db.Entry(employee).State = EntityState.Modified;
                   db.SaveChanges();
                   return RedirectToAction("/Employee/ViewEmployee");
            }
            return View(employee);
        }
        public void GetAllActors()
        {
            var list = new List<string>();
            list.Add("Admin");
            list.Add("Instructor");
            list.Add("Learner");
            ViewBag.Actors = list;
        }
        public ActionResult ViewInstructors()
        {
            var listOfInstructor = new List<Employee>();
            var employees = db.Employees.ToList();
           foreach(var empl in employees )
            {
                if(empl.IsAdmin == "Instructor")
                {
                    listOfInstructor.Add(empl);
                }
            }
            ViewBag.Employees = listOfInstructor;
            return View(ViewBag.Employees);
        }



        #region //Views Only
         public ActionResult ViewEmp()
        {
            return View();
        }

        public ActionResult ViewInstr()
        {
            return View();
        }

        public ActionResult ViewBooking()
        {
            
            ViewBag.Learners = dbLearners.Learners.ToList();
            return View(ViewBag.Learners);
        }
        #endregion
    }


}