using System.IO;
using System.Reflection;
using NUnit.Framework;
using LoanQuoter.CSVParser;
using LoanQuoter.DTO;
using LoanQuoter.Exceptions;
using FileHelpers;
using FluentAssertions;


namespace LoanQuoterTest
{
    [TestFixture]
    public class CsvReaderTests
    {
        [Test]
        public void ParseFile_QuotesLoadedSuccessfully()
        {
            var csvReader = new CsvReader(new FileHelperEngine<Quote>());

            var pathLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var result = csvReader.GetQuotes(Path.Combine(pathLocation,"TestData\\quote1.csv"));

            result.Length.Should().Be(7);
            result[0].Lender.Should().Be("Bob");
            result[0].Rate.Should().Be(0.075m);
            result[0].Available.Should().Be(640);
        }

        [Test]
        public void ParseFile_QuotesLoadedFailed()
        {
            var csvReader = new CsvReader(new FileHelperEngine<Quote>());

            var pathLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Assert.Throws<CsvParserException>(() => csvReader.GetQuotes(Path.Combine(pathLocation, "TestData\\badquote.csv")));
        }
    }
}
