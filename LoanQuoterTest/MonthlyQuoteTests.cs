using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using LoanQuoter.DTO;
using FluentAssertions;

namespace LoanQuoterTest
{
    [TestFixture]
    public class MonthlyQuoteTests
    {
        [TestCase(0.15,0.0117)]
        [TestCase(0.05,0.00407)]
        public void CalculateMonthlyRate_CalcuationSuccessful(decimal input, decimal output)
        {
            var monthlyQuote = new MonthlyQuote();

            var monthlyRate = monthlyQuote.CalculateMonthlyRate(input);

            monthlyRate.Should().BeApproximately(output, 0.0001m);
        }
    }
}
