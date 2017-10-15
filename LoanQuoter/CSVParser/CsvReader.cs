using System;
using LoanQuoter.Exceptions;
using LoanQuoter.DTO;
using FileHelpers;
using NLog;

namespace LoanQuoter.CSVParser
{
    public interface ICsvReader
    {
        Quote[] GetQuotes(string path);
    }

    public class CsvReader : ICsvReader
    {
        private IFileHelperEngine<Quote> _engine;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger(); 

        public CsvReader(IFileHelperEngine<Quote> engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        public Quote[] GetQuotes(string filePath)
        {
            _logger.Info($"Parsing file {filePath}");

            Quote[] results = new Quote[0];

            try
            {
                results = _engine.ReadFile(filePath);
            }
            catch (Exception ex)
            {
                throw new CsvParserException(ex.Message);
            }

            _logger.Info($"{results.Length} Records read from file");

            return results;
        }
    }
}