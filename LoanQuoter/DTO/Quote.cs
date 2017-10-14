using System;
using FileHelpers;

namespace LoanQuoter.DTO
{
    [DelimitedRecord(",")]
    [IgnoreFirst]
    public class Quote
    {
        public string Lender { get; set; }
        public decimal Rate { get; set; }
        public decimal Available { get; set; }
    }
}
