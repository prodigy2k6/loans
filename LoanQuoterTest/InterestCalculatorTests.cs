using System.Collections.Generic;
using NUnit.Framework;
using LoanQuoter.DTO;
using LoanQuoter.Quoter;
using FluentAssertions;

namespace LoanQuoterTest
{
    [TestFixture]
    public class InterestCalculatorTests
    {

        [TestCase(1500,53.5)]
        [TestCase(1000, 19.5)]
        [TestCase(200, 3.0)]
        [TestCase(900, 13.5)]
        public void AllocateLoans_LoansAllocatedSuccessfuly(decimal principal, decimal interest)
        {
            var quotes = new List<MonthlyQuote>
            {
                new MonthlyQuote{Lender= "Bob", CompoundedMonthlyRate = 0.21m, Available = 500},
                new MonthlyQuote{Lender= "Jane", CompoundedMonthlyRate = 0.015m, Available = 900},
                new MonthlyQuote{Lender= "Susie", CompoundedMonthlyRate = 0.27m, Available = 500},
                new MonthlyQuote{Lender= "Alan", CompoundedMonthlyRate = 0.06m, Available = 200},
                new MonthlyQuote{Lender= "Bob", CompoundedMonthlyRate = 0.07m, Available = 1000},
            };

            var interestCalculator = new InterestCalculator(quotes);

            var calcInterest = interestCalculator.CalculateMoneyBorrowed(principal);

            calcInterest.Should().Be(interest);
        }

        [TestCase(1000, 100, 3.228)]
        [TestCase(1500, 200, 4.26)]
        public void CalculateYearlyRate(decimal princial, decimal interest, decimal rate)
        {
            var intCalculator = new InterestCalculator();

            var calcRate = intCalculator.CalculateYearlyRate(princial, interest);

            calcRate.Should().BeApproximately(rate,0.001m);
        }

    }
}
