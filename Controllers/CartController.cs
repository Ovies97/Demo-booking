using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo_Booking_Lessons_For_Driving.Models;
namespace Demo_Booking_Lessons_For_Driving.Controllers
{
    public class CartController : Controller
    {
        List<string> listOFPrices = new List<string>();
        DbEntities db = new DbEntities();
        // GET: Cart
        public ActionResult GetCart()
        {
            GetListPrices();
            return View();
        }
        [HttpPost]
        public ActionResult GetCart([Bind(Exclude="Total,Id")]Cart cart)
        {
            string message = "";
            GetListPrices();
            if (ModelState.IsValid )
            {
                var ochk = db.Learners.Where(model => model.IDNumber == cart.IDNumber).FirstOrDefault ();
                if(ochk != null )
                {
                    cart.Time = DateTime.UtcNow;
                  Cart cart_Values = CheckPrices(cart,ochk.Category );
                  db.Carts.Add(cart_Values);
                  db.SaveChanges();
                  message = " your bookings has been successfully  scheduled !!";

                    return RedirectToAction("ResultsView", ochk);
                }
                else
                {
                    return RedirectToAction("GetCart");
                } 
            }
            return View();
        }

        private Cart CheckPrices(Cart cart,string catagory)
        {
            if(cart.Price.Contains("500"))
            {
                cart.Price = "500";
                cart.Total = Convert.ToInt32(cart.Session) + Convert.ToInt32(cart.Price);
            }
            else if (cart.Price.Contains("1000"))
            {
                cart.Price = "1000";
                cart.Total = Convert.ToInt32(cart.Session) + Convert.ToInt32(cart.Price);
            }
            else if(cart.Price.Contains("1500"))
            {
                cart.Price = "1500";
                cart.Total = Convert.ToInt32(cart.Session) + Convert.ToInt32(cart.Price);
            }
            return cart;
        }

        public void GetListPrices()
        {
            listOFPrices.Add("motor cycle:500");
            listOFPrices.Add("light motor vehicle:1000");
            listOFPrices.Add("heavy motor vehicle:1500");
            ViewBag.Prices = listOFPrices;
        }
        public ActionResult ResultsView(Learner   cart)
        {
            return View(cart);
        }

        public ActionResult Print()
        {
            return View();
        }
       
        public ActionResult DeleteBooking(string id)
        {
            string message = "";
            bool status = false;
            if(id != "0")
            {

              var _Learner =   db.Learners.Find(Convert.ToInt64 (id));
             if(_Learner != null)
                db.Learners.Remove(_Learner);
                db.SaveChanges();
                var _Card =   db.Cards.Find(Convert.ToInt64(id));
                if(_Card != null)
                   db.Cards.Remove(_Card);
                db.SaveChanges();
                var _Cart =   db.Carts.Find(Convert.ToInt64(id));
                if(_Cart != null )
                   db.Carts.Remove(_Cart);
                db.SaveChanges();
                message = _Learner.Name + "booking is canceled successfully";
                status = true;

            }
            ViewBag.Status = status;
            ViewBag.Message = message;
            return RedirectToAction("ViewBookings","Learner");
        }
        [HttpGet ]
        public ActionResult EditBooking(string id)
        {
            Int64 _ID = Convert.ToInt64(id);
            if(_ID != 0)
            {
                var details = db.Learners.Find(_ID);
                if(details != null )
                {
                    return View(details);
                }
            }
            return View();
        }
    }
}