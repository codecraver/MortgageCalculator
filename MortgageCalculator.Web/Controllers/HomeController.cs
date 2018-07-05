using MortgageCalculator.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MortgageCalculator.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var loanVM = new Loan();
            return View(loanVM);
        }

        [HttpPost]
        public ActionResult Index(Loan model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            if (model.Type == "Fixed")
            {
                double principal = model.LoanAmount;
                //Monthly interest rate
                double r = model.RateOfInterest / 100 / 12;
                //Number of monthly payments
                double n = model.Tenure * -12;

                //The fixed monthly payment
                double c = (r / (1 - Math.Pow((1 + r), n)) * model.LoanAmount);
                model.MonthlyPayment = Math.Round(c, 2);
                //Total repayment over the life time of the loan
                model.TotalRepayment = Math.Round(model.Tenure * 12 * c, 2);
                //Total Amount paid in interest over the life time of the loan
                model.TotalInterest = Math.Round(model.TotalRepayment - model.LoanAmount, 2);
                model.HasQuote = true;

                return View(model);
            }
            if (model.Type == "Variable")
            {
                //Monthly interest rate
                double r = model.RateOfInterest / 100 / 12;
                //Number of months in the loans
                int n = model.Tenure * 12;
                //Calculate the power value
                double sample = Math.Pow(1 + r, n);
                //The Monthly payment 
                double c = (r * sample / (sample - 1)) * model.LoanAmount;
                model.MonthlyPayment = Math.Round(c,2);
                //Total repayment over the life time of the loan
                model.TotalRepayment = Math.Round(model.Tenure * 12 * c, 2);
                //Total Amount paid in interest over the life time of the loan
                model.TotalInterest = Math.Round(model.TotalRepayment - model.LoanAmount, 2);
                model.HasQuote = true;

                return View(model);
            }

            return View(model);
        }
        public ActionResult GetMortgages(string sortOrder)
        {
            IEnumerable<MortgageCalculator.Dto.Mortgage> Mortgages;

            try
            {
                Mortgages =   new Api.Controllers.MortgageController().Get();

                //Active Mortgages
                Mortgages = Mortgages.Where(m => m.EffectiveEndDate >= DateTime.Now);
                                    

                ViewBag.TypeSortParm = string.IsNullOrEmpty(sortOrder) ? "type-desc" : "";
                ViewBag.ROIParm = sortOrder == "Interest" ? "interest-desc" : "Interest";

                switch (sortOrder)
                {
                    case "type-desc":
                            Mortgages = Mortgages.OrderByDescending(m => m.MortgageType.ToString());
                        break;
                    case "Interest":
                            Mortgages = Mortgages.OrderBy(m => m.InterestRate);
                        break;
                    case "interest-desc":
                        Mortgages = Mortgages.OrderByDescending(m => m.InterestRate);
                        break;
                    default:
                        Mortgages = Mortgages.OrderBy(m => m.Name.ToString());
                        break;
                }
                return View("Mortgage", Mortgages);
            }
            catch (Exception ex)
            {

                throw;
            }
            return View();
        }
    }
}