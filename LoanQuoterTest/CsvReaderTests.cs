using System;
using NUnit.Framework;
using LoanQuoter.CSVParser;
using LoanQuoter.DTO;
using FileHelpers;

namespace LoanQuoterTest
{
    [TestFixture]
    public class CsvReaderTests
    {
        [Test]
        public void ParseFile_QuotesLoadedSuccessfully()
        {
            var csvReader = new CsvReader(new FileHelperEngine<Quote>());

            var result = csvReader.GetQuotes("TestData\\quote1.csv");
        }
    }
}
