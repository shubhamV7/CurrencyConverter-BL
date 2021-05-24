using CurrencyConverter_BL_BLogic;
using System;

namespace ConsoleApp5Currency
{
    internal class Program
    {
        private static string _filePath = @".\CurrencyValues.txt";

        private static void Main(string[] args)
        {
            int noOfRates = 0;
            BLogicLayer bLayer = new BLogicLayer(_filePath);

            //Checking if rate list file already exist or not
            if (bLayer.CheckIfExist())
            {
                Console.WriteLine("Do you want to continue with the existing conversion rate list " +
                    "\n or create a new conversion rate list\n" +
                    " y - Continue with existing \n n - Create New ");

                char ch = InputChoice();
                if (ch == 'y')
                {
                    try
                    {
                        bLayer.GetRateList();
                    }
                    catch (FormatException fExc)
                    {
                        Console.WriteLine("Format Exception Occured (while parsing float value from file)\n" + fExc.Message);
                        Console.WriteLine("Creating rate list again ... \n");
                        noOfRates =  AddRateList(bLayer);
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Exception Occured \n" + exc.Message);
                        Console.WriteLine("Creating rate list again ... \n");
                        noOfRates = AddRateList(bLayer);
                    }
                }
                else
                {
                    noOfRates = AddRateList(bLayer);
                }
            }
            else
            {
                noOfRates = AddRateList(bLayer);
            }
            

            if(noOfRates > 0)
            {
                CalculateCurrency(bLayer);
            }
            else
            {
                Console.WriteLine("No record found for rate list, try changing the path!!");
                Console.WriteLine("Exiting....");
            }
        }

        /// <summary>
        /// Method to start calculating the currency conversions
        /// </summary>
        /// <param name="bLayer"> object of BLogicLayer</param>
        private static void CalculateCurrency(BLogicLayer bLayer)
        {
            double amount;
            string symbol;
            double resultAmount;

            //printing available currencies
            Console.WriteLine("\n\nAvailable Currency Symbols to use");
            int count = 1;
            foreach (var str in bLayer.GetCurrencySymbolList())
            {
                Console.WriteLine($"{count++}. {str}");
            }

            //Converting
            do
            {
                symbol = InputCurrencySymbol();
                amount = InputAmount();

                if (bLayer.CalculateConversion(symbol, amount, out resultAmount))
                {
                    Console.WriteLine("Converted Amount : {0:0.00} ", resultAmount);
                }
                else
                {
                    Console.WriteLine("Invalid Currency Symbol, try again");
                    continue;
                }

                Console.WriteLine("\n\nDo you want to use again ? (y/n)");
                char ch = InputChoice();
                if (ch == 'n')
                {
                    Console.WriteLine("\nExiting...");
                    break;
                }
            } while (true);
        }

        /// <summary>
        /// Method to add new Currency conversion rates
        /// </summary>
        /// <returns>integer - no. of rates added to list</returns>
        private static int AddRateList(BLogicLayer bLayer)
        {
            int count = 0;
            float rate;
            string symbol = string.Empty;

            Console.WriteLine("Enter Currency rates one by one (at least 5)");
            Console.WriteLine("Currency Rate contains two parts \n" +
                              " 1. Symbol for Currency ( for ex. USDINR - USD to INR)\n" +
                              " 2. Rate against in INR (for example - 1 USD = 73.32 INR)\n");

            while (true)
            {
                if (count >= 5)
                {
                    Console.WriteLine("\nYou have added 5 conversion rates in the list \nDo you want to add more ?(y/n)");
                    char ch = InputChoice();

                    if (ch == 'n')
                    {
                        break;
                    }
                }

                symbol = InputCurrencySymbol();
                rate = InputCurrencyRate();

                try
                {
                    if (count == 0)
                    {
                        //adding first value so to create new file passing true
                        bLayer.AddNewSymbolAndRate(symbol, rate, true);
                        Console.WriteLine($"Currency with Symbol : {symbol} and Rate : {rate} added successfully...\n");
                        ++count;
                    }
                    else
                    {
                        if (!(bLayer.AddNewSymbolAndRate(symbol, rate, false)))
                        {
                            Console.WriteLine($"Currency Symbol {symbol} already present, try different symbol : ");
                        }
                        else
                        {
                            Console.WriteLine($"Currency with Symbol : {symbol} and Rate : {rate} added successfully...\n");
                            ++count;
                        }
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine("Exception Occured : " + exc.Message);
                    break;
                }
            }
            return count;
        }

        /// <summary>
        /// Method to take and validate amount
        /// </summary>
        private static double InputAmount()
        {
            double amount;
            do
            {
                Console.Write("Enter Amount to convert : ");
                if (!double.TryParse(Console.ReadLine(), out amount))
                {
                    Console.WriteLine("\nInvalid value for amount (must be a double value) try again !!");
                }
                else
                {
                    break;
                }
            } while (true);

            return amount;
        }

        /// <summary>
        /// Method to take and validate currency rate
        /// </summary>
        /// <returns>float currency rate</returns>
        private static float InputCurrencyRate()
        {
            float rate;
            do
            {
                Console.Write("Enter Currency Rate (in INR) : ");
                if (!float.TryParse(Console.ReadLine(), out rate))
                {
                    Console.WriteLine("\nInvalid value for Rate (must be a integer|float value) try again !!");
                }
                else
                {
                    break;
                }
            } while (true);

            return rate;
        }

        /// <summary>
        /// Method to take and validate currency symbol
        /// </summary>
        /// <returns>string currency symbol</returns>
        private static string InputCurrencySymbol()
        {
            string currency;
            do
            {
                Console.Write("\nEnter Currency Symbol : ");
                currency = Console.ReadLine().Trim().ToUpper();
                if (string.IsNullOrEmpty(currency))
                {
                    Console.WriteLine("Must enter a valid currency symbol, try again ");
                }
                else
                {
                    break;
                }
            } while (true);

            return currency;
        }

        /// <summary>
        /// Method to take and validate choice inputs (y/n)
        /// </summary>
        /// <returns>char either 'y' or 'n'</returns>
        private static char InputChoice()
        {
            char ch;
            do
            {
                if (char.TryParse(Console.ReadLine(), out ch))
                {
                    if (char.ToLower(ch) == 'y' || char.ToLower(ch) == 'n')
                    {
                        break;
                    }
                    else
                    {
                        Console.Write("\nWrong Choice try again (y/n): ");
                    }
                }
                else
                {
                    Console.Write("\nWrong Choice try again (y/n): ");
                }
            } while (true);

            return char.ToLower(ch);
        }
    }
}