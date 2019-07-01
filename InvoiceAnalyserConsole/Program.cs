using System;
using System.IO;
using System.Reflection;
using InvoiceAnalyserLibrary;

namespace InvoiceAnalyserConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteProgram(args);

        }

        private static void ExecuteProgram(string[] args = null)
        {
            Console.WriteLine("Hello! \n\nBefore we begin, please make sure all invoices are downloaded and share the same root folder folder e.g. downloads.\n");
            FileHandler handler = OpenFileHandler();
            Console.Clear();
            Console.WriteLine("The application will now try to rename and organise your invoices.\nIf you have already completed this step, press y to continue to analysis.");
            ConfirmationPrompt();
            handler.OrganiseFiles();
            Console.Clear();
            Console.WriteLine("\nOrganisation successful!\n");

            Console.WriteLine("I will now attempt to analyse your invoices.");

            ConfirmationPrompt();
            InvoiceAnalysis analyser = new InvoiceAnalysis(handler.InvoiceFiles());
            do
            {
                Console.Clear();
                var dateRange = DateEnterPrompt();
                WriteBar();
                IInvoice[] targets;
                if (dateRange.Item1 == null || dateRange.Item2 == null)
                {
                    targets = null;
                }
                else
                    targets = Array.FindAll(analyser.Invoices, x => (x.Date >= dateRange.Item1 && x.Date <= dateRange.Item2));

                RunAnalytics(analyser, targets);
                WriteBar();
                Console.ReadKey();
            }
            while (AnotherDate());
        }

        private static bool AnotherDate()
        {
            Console.WriteLine("Would you like to try another date?\n Yes=Y, No=N");
            var answer = Console.ReadKey();
            Console.WriteLine();

            if (answer.Key != ConsoleKey.Y && answer.Key != ConsoleKey.N)
            {
                Console.WriteLine("Sorry, I didn't catch that..");
                AnotherDate();
            }

            if (answer.Key == ConsoleKey.Y)
                return true;
            else
                return false;
        }

        private static void WriteBar()
        {
            Console.WriteLine("------------------------------------------------------------------------------------------------------------");        
        }

        private static void RunAnalytics(InvoiceAnalysis analyser, IInvoice[] invoices)
        {
 
            Console.WriteLine("*FIGURES*\n");
            if(invoices==null)
                Console.WriteLine($"Invoices Count: {analyser.Invoices.Length}");
            else
                Console.WriteLine($"Invoices Count: {invoices.Length}");

            Console.WriteLine($"Total: £{analyser.Total(invoices)} \nOrders Delivered: {analyser.OrdersDelivered(invoices)} \nDrop Fees: £{analyser.DropFees(invoices)} \nTips: £{analyser.Tips(invoices)} " +
                $"\nAdjustments: £{analyser.Adjustments(invoices)} \nTransaction Fees: £{analyser.TransactionFees(invoices)} \nHours Worked: {analyser.HoursWorked(invoices)}h\n");
            WriteBar();
            Console.WriteLine("*AVERAGES*\n");
            Console.WriteLine($"Average Total: £{analyser.AverageTotal(invoices)} \nAverage Orders Delivered: {analyser.AverageOrdersPerInvoice(invoices)} " +
                $"\nAverage Drop Fees: £{analyser.AverageDropFees(invoices)} \nAverage Tips: £{analyser.AverageTips(invoices)} \nAverage Hours Worked: {analyser.AverageHoursWorked(invoices)}h\n");
            WriteBar();
            Console.WriteLine("*STATS*\n");
            Console.WriteLine($"Hourly Earnings: £{analyser.HourlyEarnings(invoices)} p/h \nOrders per Hour: {analyser.OrdersPerHour(invoices)} p/h \nFee per Order: £{analyser.AverageOrderFee(invoices)} \n" +
                $"Tips per Order: £{analyser.TipPerOrder(invoices)} \nOrders per £1 Tip: {analyser.OrdersPerTip(invoices)}\n");

            
        }

        private static (DateTime?,DateTime?) DateEnterPrompt()
        {
            DateTime start = new DateTime(), end = new DateTime();
            Console.WriteLine();
            Console.WriteLine("Please enter a date range to analyse by pressing 'd', or press 'a' for all. \nNote, all may show figures from differing tax years.");
            var answer = Console.ReadKey();
            Console.WriteLine();
            if (answer.Key != ConsoleKey.D && answer.Key != ConsoleKey.A)
            {
                Console.WriteLine("Sorry, I didn't quite catch that..");
                DateEnterPrompt();
            }
            else if (answer.Key == ConsoleKey.A)
                return (null,null);

            else
            {
                Console.WriteLine();
                Console.WriteLine("Time to enter a date range. Note that UK tax years are from 6th April, to the 5th April the following year.");
                Console.WriteLine("Please format as yyyy-mm-dd, e.g. 2018-04-20\n");
                Console.Write("Start date: ");
                start = GetDate("Start");
                Console.Write("End date: ");
                end = GetDate("End");
            }
            return (start, end);
        }

        private static DateTime GetDate(string dateName)
        {
            DateTime result = new DateTime();
            if (!DateTime.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Error! Incorrect date input, please make sure the date is formatted correctly\n");
                Console.Write($"{dateName} date: ");
                result = GetDate(dateName);
            }

            return result;
        }

        private static FileHandler OpenFileHandler()
        {
            Console.WriteLine("Please now enter the full directory path of the root folder which contains all your invoices, e.g.");
            Console.WriteLine(@"On Windows, 'C:\Users\{username}\Downloads'");
            Console.WriteLine(@"On MacOS, '/Users/{username}/Downloads/'");
            Console.WriteLine(@"On Linux, '/home/{username}/Downloads/'");
            Console.WriteLine();

            return new FileHandler(GetRootDirectoryInput());
        }

        private static string GetRootDirectoryInput()
        {
            var pathInput = Console.ReadLine();
            if (!Directory.Exists(pathInput))
            {
                Console.WriteLine("The directory path you entered does not exist, please check and try again.\n");
                GetRootDirectoryInput();
            }

            return pathInput;
        }

        private static void ConfirmationPrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Would you like to continue?\nYes=Y, No=N");
            var keyPress = Console.ReadKey();
            Console.WriteLine();
            if (keyPress.Key != ConsoleKey.Y && keyPress.Key != ConsoleKey.N)
            {
                Console.WriteLine("Sorry, I didn't quite catch that..");
                ConfirmationPrompt();
            }
            else if (keyPress.Key == ConsoleKey.Y)
                return;

            else
            {
                Console.WriteLine("Ok, restarting application.");
                System.Threading.Thread.Sleep(1500);
                Console.Clear();
                ExecuteProgram();
            }

        }
    }
}
