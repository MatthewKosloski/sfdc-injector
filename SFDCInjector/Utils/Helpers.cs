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
        /// Returns a boolean indicating if `str`,
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
            if(str == null) str = "";
            return String.IsNullOrEmpty(str.Trim());
        }

        /// <summary>
        /// Loops through a list of strings and returns a boolean 
        /// indicating if the list contains a trimmed, empty string.
        /// </summary>
        /// <example>
        /// <code>
        /// HasEmptyTrimmedString(new List&lt;string&gt;{"a", "b", null}); // True
        /// HasEmptyTrimmedString(new List&lt;string&gt;{"a", "b", ""}); // True
        /// HasEmptyTrimmedString(new List&lt;string&gt;{"a", "b", " "}); // True
        /// HasEmptyTrimmedString(new List&lt;string&gt;{"a", "b", "c"}); // False
        /// </code>
        /// </example>
        public static bool HasEmptyTrimmedString(List<string> strs)
        {
            return strs.Exists(str => IsTrimmedStringEmpty(str));
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
            if(replacement == null) replacement = "";
            return IsTrimmedStringEmpty(replacement) ? original : replacement;
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
            if(replacement == null) replacement = "";
            
            return IsTrimmedStringEmpty(replacement) 
                ? original 
                : Conversions.StringToDouble(replacement);
        }

    }
}