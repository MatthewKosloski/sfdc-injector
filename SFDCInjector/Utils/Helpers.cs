using System;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Generic;
using System.Reflection;
using SFDCInjector.Utils;

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

        /// <summary>
        /// Using reflection, dynamically makes a call to a generic method.
        /// <param name="methodName">The name of the generic method on `classType`.</param>
        /// <param name="classType">The name of the class to which the method belongs.</param>
        /// <param name="typeParameters">The types that are passed into the method as type params.</param>
        /// <param name="bindingAttrs">Reflection binding flags that describe how to search for the method.</param>
        /// </summary>
        public static MethodInfo MakeGenericMethod(string methodName, Type classType, 
        Type[] typeParameters, BindingFlags bindingAttrs = BindingFlags.NonPublic | BindingFlags.Static)
        {
            MethodInfo miMethod = classType.GetMethod(methodName, bindingAttrs);
            MethodInfo miMethodConstructed = miMethod.MakeGenericMethod(typeParameters);     
            return miMethodConstructed;
        }

    }
}