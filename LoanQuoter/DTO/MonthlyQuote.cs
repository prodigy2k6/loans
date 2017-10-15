using System;

namespace LoanQuoter.DTO
{
    public class MonthlyQuote : Quote
    {
        public decimal MonthlyRate { get; set; }

        public decimal CompoundedMonthlyRate { get; set; }

        private const int NumberOfMonths = 36;

        public MonthlyQuote() { }

        public MonthlyQuote(Quote aQuote)
        {
            Lender = aQuote.Lender;
            Rate = aQuote.Rate;
            Available = aQuote.Available;

            MonthlyRate = CalculateMonthlyRate(Rate);
            CompoundedMonthlyRate = CalculateCompoundedRate(MonthlyRate);
        }

        internal decimal CalculateMonthlyRate(decimal rate)
        {
            var monthlyRate = Math.Pow((double)(1 + rate), (1.0 / 12.0)) - 1;

            return (decimal) monthlyRate;
        }

        internal decimal CalculateCompoundedRate(decimal rate)
        {
            var compoundedRate = Math.Pow((double)rate, NumberOfMonths);

            return (decimal) compoundedRate;
        }
    }
}
