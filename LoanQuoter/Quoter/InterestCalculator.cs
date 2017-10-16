using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using LoanQuoter.DTO;
using NLog;

namespace LoanQuoter.Quoter
{
    public interface IInterestCalculator
    {
        bool MoneyAvailable(decimal available);

        decimal CalculateMoneyBorrowed(decimal principal);
        
        decimal CalculateYearlyRate(decimal principal, decimal interest);

        double CalculateMonthlyRepayment(decimal amountToRepay);

    }

    public class InterestCalculator : IInterestCalculator
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        internal List<MonthlyQuote> LoanQuotes { get; set; }

        public double months = Double.Parse(ConfigurationManager.AppSettings["loanterm"]);

        public InterestCalculator() { }

        public InterestCalculator (List<MonthlyQuote> quotes)
        {
            //Order loans by lowest rate first
            LoanQuotes = quotes.OrderBy(x => x.CompoundedMonthlyRate).ToList();
        }

        /// <summary>
        /// Checks if enough money is available
        /// </summary>
        /// <param name="available"></param>
        /// <returns></returns>
        public bool MoneyAvailable (decimal available)
        {
            return LoanQuotes.Sum(x => x.Available) >= available;
        }

        /// <summary>
        /// Calculates interest for the loan
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public decimal CalculateMoneyBorrowed (decimal principal)
        {
            var interest = 0.0m;
            var remainingAmount = principal;

            foreach(var quote in LoanQuotes.Where(x => x.Available > 0 && x.CompoundedMonthlyRate != 0))
            {
                if (remainingAmount <= 0)
                {
                    _logger.Info("Remaining amount is 0");
                    break;
                }

                if (quote.Available >= remainingAmount)
                {
                    interest += remainingAmount * quote.CompoundedMonthlyRate;
                    _logger.Info($"Quote from {quote.Lender} has {quote.Available} amount available at {quote.CompoundedMonthlyRate} compounded rate");
                    _logger.Info($"Currently we only need to allocate {remainingAmount} so this is the last quote to inlcude.");
                    _logger.Info($"Interest is {interest}");
                    break;
                }
                else
                {
                    _logger.Info($"Quote from {quote.Lender} has {quote.Available} amount available at {quote.CompoundedMonthlyRate} compounded rate");
                    _logger.Info($"Interest is currently {interest}");
                    interest += quote.Available * quote.CompoundedMonthlyRate;
                    remainingAmount -= quote.Available;
                    _logger.Info($"Currently we need to borrow {remainingAmount}.");
                    _logger.Info($"Interest is now {interest}");
                }
            }

            return interest;
        }

        /// <summary>
        /// Calculates yearly rate for interest
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="interest"></param>
        /// <returns></returns>
        public decimal CalculateYearlyRate (decimal principal, decimal interest )
        {
            var proportionOfInterest = interest / principal;

            var yearlyRate = Math.Pow((double)(proportionOfInterest + 1), (1.0 / (months / 12.0))) - 1;

            return (decimal)yearlyRate * 100;
        }

        /// <summary>
        /// Calculate monthly repayment rate
        /// </summary>
        /// <param name="amountToRepay"></param>
        /// <returns></returns>
        public double CalculateMonthlyRepayment(decimal amountToRepay) => ((double)amountToRepay) / months;

    }
}
