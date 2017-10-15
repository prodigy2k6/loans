using System;

namespace LoanQuoter.DTO
{
    public class MonthlyQuote : Quote
    {
        public decimal MonthlyRate { get; set; }

        public decimal CompoundedMonthlyRate { get; set; }

        private const int NumberOfMonths = 36;

        public MonthlyQuote(Quote aQuote)
        {
            Lender = aQuote.Lender;
            Rate = aQuote.Rate;
            Available = aQuote.Available;

            MonthlyRate = CalculateMonthlyRate(Rate);
        }

        public decimal CalculateMonthlyRate(decimal rate)
        {
            var monthlyRate = Math.Pow((double)(1 + rate), (1 / 12)) - 1;

            return (decimal) monthlyRate;
        }

        public decimal CalculateCompoundedRate(decimal rate)
        {
            var compoundedRate = Math.Pow((double)rate, NumberOfMonths);

            return (decimal) compoundedRate;
        }
    }
}
