using MvcApplication2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects;
using System.Data.Entity;
using System.Data.SqlClient; 
using System.Configuration;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using MvcApplication2.Filters;

namespace MvcApplication2.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        SerieDatabaseEntities _db;

        public HomeController()
        {
            _db = new SerieDatabaseEntities();
        }

        public ActionResult Index()
        {
            if (User.IsInRole("admin"))
            {
                List<Commission> CommissionsToDisplay = new List<Commission>();
                CommissionsToDisplay= _db.Commission.ToList();
                if (CommissionsToDisplay.Count() == 0)
                {
                    ViewBag.CommissionList = null;
                }
                else
                {
                    ViewBag.CommissionList = CommissionsToDisplay;
                }
            }
            else //dealer role
            {
                List<Commission> CommissionsToDisplay = new List<Commission>();
                CommissionsToDisplay = _db.Commission.Where(i => i.Dealer.Name == User.Identity.Name).ToList();
                if (CommissionsToDisplay.Count() == 0)
                {
                    ViewBag.CommissionList = null;
                }
                else
                {
                    ViewBag.CommissionList = CommissionsToDisplay;
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(CommissionFilterModel model)
        {
            List<Commission> CommissionsToDisplay = new List<Commission>();
            if (ModelState.IsValid)
            {
                if (User.IsInRole("admin"))
                {
                    CommissionsToDisplay = _db.Commission.ToList();
                }
                else //dealer role
                {
                    CommissionsToDisplay = _db.Commission.Where(i => i.Dealer.Name == User.Identity.Name).ToList();
                }

                if (model.PaymentStatusId != null)
                {
                    CommissionsToDisplay = CommissionsToDisplay.Where(i => i.PaymentStatusId == model.PaymentStatusId).ToList();
                }
                if (model.StartPeriodeDate != null)
                {
                    CommissionsToDisplay = CommissionsToDisplay.Where(i => i.CreatedDate >= model.StartPeriodeDate).ToList();
                }
                if (model.EndPeriodeDate != null)
                {
                    CommissionsToDisplay = CommissionsToDisplay.Where(i => i.CreatedDate <= model.EndPeriodeDate).ToList();
                }

                if (CommissionsToDisplay.Count() == 0)
                {
                    ViewBag.CommissionList = null;
                }
                else
                {
                    ViewBag.CommissionList = CommissionsToDisplay;
                }
            }
            else
            {
                ViewBag.CommissionList = null;
            }

            return View();
        }

        public ActionResult FirstQuery()
        {
            ViewBag.TotalAmount = "";
            return View();
        }

        [HttpPost]
        public ActionResult FirstQuery(FirstQueryModel model)
        {
            string totalAmount = "";
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["EntityConnection"].ConnectionString;
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("TotalAmountOfPaidCommission", connection);
                    SqlParameter code1 = new SqlParameter("@DealerId", model.DealerId);
                    SqlParameter code2 = new SqlParameter("@EndPeriodeDate", model.EndPeriodeDate);
                    SqlParameter code3 = new SqlParameter("@StartPeriodeDate", model.StartPeriodeDate);
                    cmd.Parameters.Add(code1);
                    cmd.Parameters.Add(code2);
                    cmd.Parameters.Add(code3);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        var TotalAmount = dr["numberOfPaidCommissions"];
                        totalAmount = TotalAmount.ToString();
                    }
                    dr.Close();

                }
                ViewBag.TotalAmount = totalAmount;
            }

            if (string.IsNullOrEmpty(ViewBag.TotalAmount))
            {
                ViewBag.TotalAmount = "-1";
            }
            return View();
        }

        public ActionResult SecondQuery()
        {
            ViewBag.Average = "";
            return View();
        }

        [HttpPost]
        public ActionResult SecondQuery(SecondQueryModel model)
        {
            string average = "";
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["EntityConnection"].ConnectionString;
                connection.Open();
                SqlCommand cmd = new SqlCommand("AveragePeriodFromVerifiedToPaid", connection);
                SqlParameter code1 = new SqlParameter("@DealerId", model.DealerId);
                cmd.Parameters.Add(code1);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    var Average = dr["average"];
                    average = Average.ToString();
                }
                dr.Close();

            }

            ViewBag.Average = average;
            if (string.IsNullOrEmpty(ViewBag.Average))
            {
                ViewBag.Average = "-1";
            }
            return View();
        }

        public ActionResult ThirdQuery()
        {
            string numMaxOfCommissions = "";
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["EntityConnection"].ConnectionString;
                connection.Open();
                SqlCommand cmd = new SqlCommand("CalculateMaxCommisionsInCreatedStatus", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    var NumMaxOfCommissions = dr[""];
                    numMaxOfCommissions = NumMaxOfCommissions.ToString();
                }
                dr.Close();

            }

            ViewBag.NumMaxOfCommissions = numMaxOfCommissions;
            if (string.IsNullOrEmpty(ViewBag.NumMaxOfCommissions))
            {
                ViewBag.NumMaxOfCommissions = "-1";
            }
            return View();
        }
    }
}
