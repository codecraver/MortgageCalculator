using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MortgageCalculator.Dto;

namespace MortgageCalculator.Api.Repos
{
    public interface IMortgageRepo
    {
        List<Mortgage> GetAllMortgages();
    }

    public class MortgageRepo : IMortgageRepo
    {
        public List<Mortgage> GetAllMortgages()
        {
            using (var context = new MortgageData.MortgageDataContext())
            {
                var mortgages = context.Mortgages.ToList();
                List<Mortgage> result = new List<Mortgage>();
                foreach (var mortgage in mortgages)
                {
                    result.Add(new Mortgage()
                        {
                            MortgageId = mortgage.MortgageId,
                            Name = mortgage.Name,
                            InterestRepayment = (InterestRepayment) (int)mortgage.InterestRepayment,
                            EffectiveStartDate = mortgage.EffectiveStartDate,
                            EffectiveEndDate = mortgage.EffectiveEndDate,
                            MortgageType = (MortgageType) (int)mortgage.MortgageType,
                            CancellationFee = mortgage.CancellationFee,
                            EstablishmentFee = mortgage.CancellationFee,
                            InterestRate = mortgage.InterestRate
                           //doesn't exist the property in the class inside the dll
                           //TermsInMonths = mortgage.TermsInMonths
                        }
                    );
                }
                return result;
            }
        }
    }
}