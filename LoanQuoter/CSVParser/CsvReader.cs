using System;
using System.Collections.Generic;
using LoanQuoter.DTO;
using FileHelpers;
using NLog;

namespace LoanQuoter.CSVParser
{
    public class CsvReader
    {
        private IFileHelperEngine<Quote> _engine;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger(); 

        public CsvReader(IFileHelperEngine<Quote> engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        public IEnumerable<Quote> GetQuotes(string filePath)
        {
            _logger.Info($"Parsing file {filePath}");
            Quote[] results = new Quote[0];
            try
            {
                results = _engine.ReadFile(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to parse csv file due to error '{ex.Message}'");
            }
            _logger.Info($"{results.Length} Records read from file");

            return results;
        }
    }
}
