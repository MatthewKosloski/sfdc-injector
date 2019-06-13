using NUnit.Framework;
using SFDCInjector.Utils;
using System.Reflection;
using System;

namespace SFDCInjector.Tests.Utils
{

    using SystemStringList = System.Collections.Generic.List<string>;

    public class MakeGenericMethodTestClass
    {
        private static string GenericMethod<T>(T toDisplay)
        {
            return $"Here it is: {toDisplay}";
        } 
    }

    /// <summary>
    /// Tests for SFDCInjector's Helpers class.
    /// </summary>
    [TestFixture]
    public class HelpersTest
    {

        /// <summary>
        /// Tests that the method returns true for a list that contains
        /// one or more elements that are null, empty, or whitespace.
        /// </summary>
        [Test]
        public void HasNullOrWhiteSpace_ListsWithNullOrWhiteSpaceElements_ShouldReturnTrue() {
            System.Collections.Generic.List<SystemStringList> lists 
            = new System.Collections.Generic.List<SystemStringList>{
                new SystemStringList{"alpha", "beta", null},
                new SystemStringList{"theta", "delta", ""},
                new SystemStringList{"epsilon", "lambda", " "},
                new SystemStringList{"epsilon", "lambda", "\t"},
                new SystemStringList{"epsilon", "lambda", "\r"},
                new SystemStringList{"epsilon", "lambda", "\n"},
                new SystemStringList{"sigma", "pi", "    "}
            };

            foreach(SystemStringList list in lists)
                Assert.That(Helpers.HasNullOrWhiteSpace(list), Is.True);
        }
        
        /// <summary>
        /// Tests that the method returns false for a list that has
        /// no elements that are null, empty, or whitespace.
        /// </summary>
        [Test]
        public void HasNullOrWhiteSpace_ListsWithNonNullOrWhiteSpaceElements_ShouldReturnFalse() {
            System.Collections.Generic.List<SystemStringList> lists 
            = new System.Collections.Generic.List<SystemStringList>{
                new SystemStringList{"alpha", "beta", "sigma"},
                new SystemStringList{" gamma ", " delta", "epsilon"}
            };

            foreach(SystemStringList list in lists)
                Assert.That(Helpers.HasNullOrWhiteSpace(list), Is.False);
        }

        /// <summary>
        /// Tests that the original values are returned because
        /// they are all empty or white space.
        /// </summary>
        [Test]
        public void KeepOriginalIfEmptyReplacement_EmptyReplacements_ShouldReturnOriginals(
            [Values("lorem ipsum")] string original, 
            [Values(null, "", " ", "   ")] string replacement)
        {
            string expected = original;
            string actual = Helpers.KeepOriginalIfEmptyReplacement(original, replacement);
            Assert.That(expected, Is.EqualTo(actual));
        }

        /// <summary>
        /// Tests that the replacement values are returned because
        /// they are non-empty strings.
        /// </summary>
        [Test]
        public void KeepOriginalIfEmptyReplacement_NonEmptyReplacements_ShouldReturnReplacements(
            [Values("lorem ipsum")] string original, 
            [Values("dolor", " sit", "amet ")] string replacement)
        {
            string expected = replacement;
            string actual = Helpers.KeepOriginalIfEmptyReplacement(original, replacement);
            Assert.That(expected, Is.EqualTo(actual));
        }

        /// <summary>
        /// Tests that the original values are returned because
        /// they are all empty or white space.
        /// </summary>
        [Test]
        public void KeepOriginalIfEmptyReplacementDoubleOverload_EmptyReplacements_ShouldReturnOriginals(
            [Values(-999, -1, 0, 1, 999, 3.14, 2.178)] double original, 
            [Values(null, "", " ", "   ")] string replacement)
        {
            double expected = original;
            double actual = Helpers.KeepOriginalIfEmptyReplacement(original, replacement);
            Assert.That(expected, Is.EqualTo(actual));
        }

        /// <summary>
        /// Tests that the replacement values are returned because
        /// they are non-empty strings that can be parsed to double.
        /// </summary>
        [Test]
        public void KeepOriginalIfEmptyReplacementDoubleOverload_NonEmptyParsableReplacements_ShouldReturnReplacements(
            [Values(-999, -1, 0, 1, 999, 3.14, 2.178)] double original, 
            [Values("-999", " -999", "-1", "0", "1", "999", "3.14", "2.178", "2.178 ")] string replacement)
        {
            double expected = Conversions.StringToDouble(replacement);
            double actual = Helpers.KeepOriginalIfEmptyReplacement(original, replacement);
            Assert.That(expected, Is.EqualTo(actual));
        }


        /// <summary>
        /// Tests that the MakeGenericMethod works.  Since this is essentially a wrapper for
        /// MethodInfo.MakeGenericMethod, there is not a lot to test for.
        /// </summary>
        [Test]
        public void MakeGenericMethod_ShouldWork()
        {
            MethodInfo method = Helpers.MakeGenericMethod("GenericMethod",
            typeof(MakeGenericMethodTestClass), new Type[] {typeof(int)});

            var actual = (string) method.Invoke(null, new object[] {42});
            var expected = "Here it is: 42";

            Assert.That(expected, Is.EqualTo(actual));
        }
    }
}