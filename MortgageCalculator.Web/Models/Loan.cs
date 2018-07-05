using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MortgageCalculator.Web.Models
{
    public class Loan
    {
        [Required(ErrorMessage ="Please enter the loan amount.")]
        [Range(100000.00, 1000000,ErrorMessage ="The loan amount ranges from Rs.100000.00 to Rs.1000000.00 only.")]
        public double LoanAmount { get; set; }

        [Required(ErrorMessage ="Please choose the loan type")]
        public  string Type { get; set; }

        [Required(ErrorMessage = "Please enter the tenure in years.")]
        [Range(1,10,ErrorMessage ="Please choose the tenure between 1 to 10 years.")]
        public int Tenure { get; set; }


        public double TotalRepayment { get; set; }

        public double TotalInterest { get; set; }

        public double  MonthlyPayment { get; set; }

        public bool HasQuote { get; set; }

        public double RateOfInterest
        {
            get
            {
                //Get the random interest rate between 4 to 10 for each search made by user.
                //eg; 4.12, 5,16,,,,etc
                return getInterestRate(4, 11);
            }
        }

        public List<SelectListItem> InterestTypes
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>
                {
                    new SelectListItem{Text = "Fixed", Value = "Fixed"},
                    new SelectListItem{Text = "Variable", Value = "Fixed"}
                };
               
                return items;
            }
        }

        private double getInterestRate(int min, int max)
        {
            Random random = new Random();
            double randomValue = random.Next(min, max) + random.NextDouble();

            return Math.Round(randomValue, 2);
        }
    }
}