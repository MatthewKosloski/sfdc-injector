using System;
using System.Diagnostics;

namespace SFDCInjector.Utils
{
    public class Convertions
    {
        public static double StringToDouble(string str, 
        string formatErr = "Unable to parse string to double due to malformed argument.")
        {
            double temp = 0.0;

            try
            {
                temp = double.Parse(str);
            }
            catch(FormatException)
            {
                Console.WriteLine(formatErr);
                Console.WriteLine(new StackTrace(true).ToString());
            }
            catch(ArgumentNullException)
            {
                Console.WriteLine("Argument cannot be null.");
                Console.WriteLine(new StackTrace(true).ToString());
            }
            catch(OverflowException)
            {
                Console.WriteLine("Argument cannot fit inside type double.");
                Console.WriteLine(new StackTrace(true).ToString());
            }

            return temp;
        }
    }
}