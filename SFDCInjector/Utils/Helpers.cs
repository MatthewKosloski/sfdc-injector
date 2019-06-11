using System;
using System.Diagnostics;
using System.Configuration;
using SFDCInjector.Utils;
using System.Collections.Generic;

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
        /// Returns a boolean indicating if a list of strings 
        /// contains one or more elements that are either null, empty, or whitespace.
        /// </summary>
        /// <example>
        /// <code>
        /// HasNullOrWhiteSpace(new List&lt;string&gt;{"a", "b", null}); // True
        /// HasNullOrWhiteSpace(new List&lt;string&gt;{"a", "b", ""}); // True
        /// HasNullOrWhiteSpace(new List&lt;string&gt;{"a", "b", " "}); // True
        /// HasNullOrWhiteSpace(new List&lt;string&gt;{"a", "b", "c"}); // False
        /// </code>
        /// </example>
        public static bool HasNullOrWhiteSpace(List<string> strs)
        {
            return strs.Exists(str => String.IsNullOrWhiteSpace(str));
        }

        /// <summary>
        /// Returns `original` if `replacement`, after being trimmed, 
        /// is Empty; otherwise returns `replacement`.
        /// <example>
        /// <code>
        /// KeepOriginalIfEmptyReplacement("i am original", null); // "i am original"
        /// KeepOriginalIfEmptyReplacement("i am original", ""); // "i am original"
        /// KeepOriginalIfEmptyReplacement("i am original", " "); // "i am original"
        /// KeepOriginalIfEmptyReplacement("i am original", "i am the replacement"); // "i am the replacement"
        /// </code>  
        /// </example>
        /// </summary>
        public static string KeepOriginalIfEmptyReplacement(string original, string replacement)
        {
            return String.IsNullOrWhiteSpace(replacement) ? original : replacement;
        }

        /// <summary>
        /// Returns `original` if `replacement`, after being trimmed, 
        /// is Empty; otherwise returns `replacement`.
        /// <example>
        /// <code>
        /// KeepOriginalIfEmptyReplacement(3.14, null); // 3.14
        /// KeepOriginalIfEmptyReplacement(3.14, ""); // 3.14
        /// KeepOriginalIfEmptyReplacement(3.14, " "); // 3.14
        /// KeepOriginalIfEmptyReplacement(3.14, "2.178"); // 2.178
        /// </code>  
        /// </example>
        /// </summary>
        public static double KeepOriginalIfEmptyReplacement(double original, string replacement)
        {            
            return String.IsNullOrWhiteSpace(replacement) 
                ? original 
                : Conversions.StringToDouble(replacement);
        }

    }
}