using System;

namespace LoanQuoter.Exceptions
{
    public class CsvParserException : Exception
    {
        public CsvParserException()
        { }

        public CsvParserException(string message) : base(message)
        { }

        public CsvParserException(string message, Exception baseException)
            : base(message, baseException)
        { }
    }
}
