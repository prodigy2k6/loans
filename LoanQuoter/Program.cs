using System;
using System.IO;
using System.Reflection;
using LoanQuoter.CSVParser;
using LoanQuoter.DTO;
using LoanQuoter.Quoter;
using FileHelpers;
using System.Collections.Generic;
using System.Linq;


namespace LoanQuoter
{
    public class Program
    {
        private static ICsvReader csvReader = new CsvReader(new FileHelperEngine<Quote>());
       

        static void Main(string[] args)
        {
            if (ValidateInput(args))
            {
                var quotes = csvReader.GetQuotes(args[0]);

                var monthlyQuotes = new List<MonthlyQuote>();
                
                foreach(var quote in quotes)
                {
                    monthlyQuotes.Add(new MonthlyQuote(quote));
                }

                var interestCalculator = new InterestCalculator(monthlyQuotes);

                var loanRequest = Decimal.Parse(args[1]);

                if (!interestCalculator.MoneyAvailable(loanRequest))
                {
                    Console.WriteLine("Market does not have sufficient funds to satisfy loan");
                }
                else
                {
                    var interest = interestCalculator.CalculateMoneyBorrowed(loanRequest);

                    var yearlyRate = interestCalculator.CalculateYearlyRate(loanRequest, interest);

                    var monthlyPayment = interestCalculator.CalculateMonthlyRepayment(loanRequest + interest);

                    Console.WriteLine($"Requested Amount: £{loanRequest}");
                    Console.WriteLine($"Rate: {yearlyRate.ToString("N1")}%");
                    Console.WriteLine($"Monthly Repayment: £{monthlyPayment.ToString("N2")}");
                    Console.WriteLine($"Total Repayment: £{ (loanRequest + interest).ToString("N2")}");

                }
            }

        }

        static bool ValidateInput(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Please enter file name and loan amount i.e 'quote.exe market.csv 1500'");
                return false;
            }

            if (!Int32.TryParse(args[1], out int loan))
            {
                Console.WriteLine($"Please enter loan amount as integer such as 1000 and not {args[1]}");
                return false;
            }

            if (loan % 100 != 0)
            {
                Console.WriteLine($"Please enter loan amount in increments of 100 such as 1000 and not {args[1]}");
                return false;
            }

            if (loan < 1000 || loan > 15000)
            {
                Console.WriteLine($"Please enter loan amount in range between 1000 and 15000 and not amount {args[1]} as entered");
                return false;
            }

            var pathCurrentLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var fileLocation = Path.Combine(pathCurrentLocation, args[0]);

            if (!File.Exists(fileLocation))
            {
                Console.WriteLine($"File {fileLocation} does not exist. Please enter valid file");
                return false;
            }

            return true;

        }
    }
}
