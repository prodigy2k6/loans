using NUnit.Framework;
using LoanQuoter.DTO;
using FluentAssertions;

namespace LoanQuoterTest
{
    [TestFixture]
    public class MonthlyQuoteTests
    {
        [TestCase(0.15,0.0117)]
        [TestCase(0.05,0.0041)]
        public void CalculateMonthlyRate_CalcuationSuccessful(decimal input, decimal output)
        {
            var monthlyQuote = new MonthlyQuote();

            var monthlyRate = monthlyQuote.CalculateMonthlyRate(input);

            monthlyRate.Should().BeApproximately(output, 0.0001m);
        }

        [TestCase(0.05, 4.791)]
        [TestCase(0.08, 14.968)]
        [TestCase(0.01, 0.4307)]
        public void CalculateCompoundedRate_CalcuationSuccessful(decimal input, decimal output)
        {
            var monthlyQuote = new MonthlyQuote();

            var compRate = monthlyQuote.CalculateCompoundedRate(input);

            compRate.Should().BeApproximately(output, 0.001m);
        }
    }
}
