using System;
using System.Diagnostics;
using System.Configuration;

namespace SFDCInjector.Utils
{
    /// <summary>
    /// Contains helper methods that are too general 
    /// to be in any other class.
    /// <remarks>
    /// This is a good place to but general-purpose, 
    /// reusable logic that is used in many parts of
    /// the program.
    /// </remarks>
    /// </summary>
    public static class Helpers
    {
        
        /// <summary>
        /// Returns a boolean indicating if one or more App.config values 
        /// associated with the provided keys is Null or Empty.
        /// </summary>
        // public static bool hasMissingSetting(string[] keys)
        // {

        //     var appSettings = ConfigurationManager.AppSettings;
        //     bool result = false;

        //     try
        //     {

        //     }
        //     catch(Con)

        //     return result;
        // }

        /// <summary>
        /// Returns a boolean indicating if the provided string argument,
        /// after being trimmed, is Empty.  
        /// </summary>
        /// <example>
        /// <code>
        /// IsTrimmedStringEmpty(null); // True
        /// IsTrimmedStringEmpty(""); // True
        /// IsTrimmedStringEmpty(" "); // True
        /// IsTrimmedStringEmpty("hello"); // False
        /// IsTrimmedStringEmpty(" hello "); // False
        /// </code>
        /// </example>
        public static bool IsTrimmedStringEmpty(string str)
        {
            bool result = true;
            try
            {
                result = String.IsNullOrEmpty(str.Trim());
            }
            catch(NullReferenceException)
            {
                Console.WriteLine("Argument cannot be null.");
                Console.WriteLine(new StackTrace(true).ToString());
            }
            return result;
        }

        /// <summary>
        /// Returns the original argument if replacement, after being trimmed, 
        /// is Empty; otherwise returns the replacement argument.  
        /// </summary>
        public static string KeepOriginalIfEmpty(string original, string replacement)
        {
            string result = "";

            try
            {
                result = IsTrimmedStringEmpty(replacement) ? original : replacement;
            }
            catch(NullReferenceException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Make sure the replacement argument is not null.");
                Console.WriteLine(new StackTrace(true).ToString());
            }

            return result;
        }

        // public static double KeepDefaultIfNoReplacement(double dfault, double replacement)
        // {

        // }

    }
}