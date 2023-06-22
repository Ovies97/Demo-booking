using Demo_Booking_Lessons_For_Driving.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace Demo_Booking_Lessons_For_Driving.Controllers
{
    public class CardController : Controller
    {
        
        DbEntities Db = new DbEntities();
        // GET: Card
        public ActionResult Pay()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Pay(Card card)
        {
            string message = "";
            bool status = false;
            if (ModelState.IsValid)
            {
               var o = Db.Learners.Where(m => m.IDNumber == card.IDNumber).FirstOrDefault();
                if(o != null )
                {
                  var oo = Db.Cards.Find(o.IDNumber);
                    if(oo == null)
                    {
                     Db.Cards.Add(card);
                      Db.SaveChanges();
                      message = "Card Details saved SuccessFully";
                      status = true;
                    }
                    else
                    {
                        message = "user already exist please check your status!!";
                    }
                  
                       
                }
                else
                {
                    message = "Please make sure that card information is inserted correctly .";
                    status = false;
                }
            }
            else
            {
                message = "Oops something went wrong !!";
                return RedirectToAction("Pay","Card");
            }
            ViewBag.Message = message;
            ViewBag.Status = status;
            return RedirectToAction("GetCart","Cart");
        }
       
        public ActionResult ViewPayment()
        {
            return View();
        }
         
    }
}



