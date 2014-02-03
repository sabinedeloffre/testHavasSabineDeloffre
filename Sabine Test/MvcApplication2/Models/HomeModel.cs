using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class MyDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d = Convert.ToDateTime(value);
            //Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.
            DateTime min = new DateTime(1800, 1, 1);
            DateTime max = new DateTime(3000, 1, 1);
            bool result = (d > min) && (d < max);
            return result;
        }
    }

    public class MyNullableDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                DateTime d = Convert.ToDateTime(value);
                //Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.
                DateTime min = new DateTime(1800, 1, 1);
                DateTime max = new DateTime(3000, 1, 1);
                bool result = (d > min) && (d < max);
                return result;
            }
            else
            {
                return true;
            }
        }
    }

    public class FirstQueryModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid Dealer Id (integer)")]
        [Display(Name = "Dealer Id")]
        public int DealerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [MyDate(ErrorMessage = "Please enter a valid date.")]
        [Display(Name = "From Date")]
        public DateTime StartPeriodeDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [MyDate(ErrorMessage = "Please enter a valid date.")]
        [Display(Name = "To Date")]
        public DateTime EndPeriodeDate { get; set; } 
    }

    public class SecondQueryModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid Dealer Id (integer)")]
        [Display(Name = "Dealer Id")]
        public int DealerId { get; set; }
    }

    public class CommissionFilterModel
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [MyNullableDate(ErrorMessage = "Please enter a valid date.")]
        [Display(Name = "Min Date created")]
        public Nullable<DateTime> StartPeriodeDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [MyNullableDate(ErrorMessage = "Please enter a valid date.")]
        [Display(Name = "Max Date created")]
        public Nullable<DateTime> EndPeriodeDate { get; set; }

        [Range(1, 6, ErrorMessage = "Please enter valid Payment Status Id (integer between 1 and 6).")]
        [Display(Name = "Payment Status Id")]
        public Nullable<int> PaymentStatusId { get; set; }
    }
}