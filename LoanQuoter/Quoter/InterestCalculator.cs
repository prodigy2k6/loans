using System;
using System.Collections.Generic;
using System.Linq;
using LoanQuoter.DTO;
using NLog;

namespace LoanQuoter.Quoter
{
    public class InterestCalculator
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        internal List<MonthlyQuote> LoanQuotes { get; set; }

        public const double months = 36.0;

        public InterestCalculator() { }

        public InterestCalculator (List<MonthlyQuote> quotes)
        {
            LoanQuotes = quotes;

            LoanQuotes = LoanQuotes.OrderBy(x => x.CompoundedMonthlyRate).ToList();
        }

        public bool MoneyAvailable (decimal available)
        {
            return LoanQuotes.Sum(x => x.Available) >= available;
        }

        public decimal CalculateMoneyBorrowed (decimal principal)
        {
            var interest = 0.0m;
            var remainingAmount = principal;

            foreach(var quote in LoanQuotes)
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

        internal decimal CalculateYearlyRate (decimal principal, decimal interest )
        {
            var proportionOfInterest = interest / principal;

            var yearlyRate = Math.Pow((double)(proportionOfInterest + 1), (1.0 / (months / 12.0))) - 1;

            return (decimal)yearlyRate;
        }

        internal double CalculateMonthlyRepayment(decimal amountToRepay)
        {
            return ((double)amountToRepay) / months;
        }



    }
}
