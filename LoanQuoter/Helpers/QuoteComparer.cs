using System.Collections.Generic;
using LoanQuoter.DTO;

namespace LoanQuoter.Helpers
{
    public class QuoteComparer:  IComparer<MonthlyQuote>
    {
        public int Compare(MonthlyQuote a, MonthlyQuote b)
        {
            if (a.CompoundedMonthlyRate == b.CompoundedMonthlyRate)
                return 0;

            if (a.CompoundedMonthlyRate < b.CompoundedMonthlyRate)
                return -1;

            return 1;
        }
    }
}
