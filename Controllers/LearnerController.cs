using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo_Booking_Lessons_For_Driving.Models;
namespace Demo_Booking_Lessons_For_Driving.Controllers
{
    public class LearnerController : Controller
    {
        Dictionary<string, int> gList = new Dictionary<string, int>();
        DbEntities DbEntities = new DbEntities();

        // GET: Learner
        [Authorize]
        public ActionResult Book()
        {
            GetTypeOfVehicle();
            GetgategoryLicences();
            LoadProvincesOnView();
            return View();
        }
        [HttpPost]
        public ActionResult Book(Learner learner)
        {
           
            GetgategoryLicences();
            LoadProvincesOnView();
            GetTypeOfVehicle();
            if (ModelState.IsValid)
            {
              var _Learner =  DbEntities.Learners.Find(learner.IDNumber);
               if(_Learner == null)
                {
                DbEntities.Learners.Add(learner);
                DbEntities.SaveChanges();
                ViewBag.Message = "Your bookings have been successfully scheduled!!";
                ViewBag.Status = true;
                return RedirectToAction("Pay","Card",learner);
                }
               else
                {
                    
                    ViewBag.Message = "User Already Exist check your status";
                }
              
            }
            else
            {
                ViewBag.Message = "Oops Something went wrong!!";
                ViewBag.Status = false;
            }
            return View();
        }
        [HttpGet]
        public ActionResult ViewBookings()
        {
            List<Learner> learners = DbEntities.Learners.ToList();
            ViewBag.Learners = learners;
            return View(ViewBag.Learners);
        }
        public ActionResult SelectProvince()
        {
            return View();
        }


        public void LoadProvincesOnView()
        {
            var list = new List<string>();
            list.Add("Free State");
            list.Add("Gauteng");
            list.Add("Kwazulu-Natal");
            list.Add("Limpopo");
            list.Add("North-West");
            list.Add("Eastern Cape");
            list.Add("Northern Cape");
            list.Add("Mpumalanga");
            list.Add("WesternCape");
            ViewBag.Province = list;
        }
        public void GetgategoryLicences()
{
gList.Clear();
            gList.Add("Driver's licence for motor cycle", 3);
            gList.Add("Driver's licence for light motor vehicle", 4);
            gList.Add("Driver's licence for heavy Motor vehicle ", 5);
            ViewBag.Gategories = gList;
        }


        public void GetTypeOfVehicle()
        {
            var list = new List<string>();
            list.Add("A-Motor Cycle,Exceeding 125 CM3");
            list.Add("B- Motor Vehicle,Not Exceeding 3500KG," +
                "EB- Articulated MV,Not Exceeding 3500kg");

            list.Add("A-Motor Cycle,Exceeding 125 CM3");
            list.Add("C-Motor Vehicle,Exceeding 1600..C1- Motor vehicle ,3500KG-16000KG," +
                " EC - Articulated MV Exceeding 16000KG." + "EC1-Articulated MV,3500 KG -16000Kg");

            ViewBag.Descriptions = list;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveModifiedChanges(Learner learner)
        {
            try
            {
              DbEntities.Entry(learner).State = System.Data.Entity.EntityState.Modified;
            DbEntities.SaveChanges();
           
            }catch(Exception e)
            {

            }
            return RedirectToAction("ViewBookings");
        }
    }
}