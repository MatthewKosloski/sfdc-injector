using NUnit.Framework;
using SFDCInjector.Utils;
using System;

namespace SFDCInjector.Tests.Utils
{

    using SystemStringList = System.Collections.Generic.List<string>;

    /// <summary>
    /// Tests for SFDCInjector's Helpers class.
    /// </summary>
    [TestFixture]
    public class HelpersTest
    {

        /// <summary>
        /// Tests the IsTrimmedStringEmpty method.
        /// </summary>
        [Test]
        [TestCase(null, ExpectedResult = true)]
        [TestCase("", ExpectedResult = true)]
        [TestCase(" ", ExpectedResult = true)]
        [TestCase("  ", ExpectedResult = true)]
        [TestCase("foo", ExpectedResult = false)]
        [TestCase("hello world", ExpectedResult = false)]  
        [TestCase("   hello world   ", ExpectedResult = false)]  
        public bool IsTrimmedStringEmptyTest(string str)
        {
            return Helpers.IsTrimmedStringEmpty(str);
        }

        /// <summary>
        /// Tests the HasEmptyTrimmedString method.
        /// </summary>
        [Test]
        public void HasEmptyTrimmedStringTest()
        {
            // HasEmptyTrimmedString should return true for each of these lists
            System.Collections.Generic.List<SystemStringList> isTrueLists 
            = new System.Collections.Generic.List<SystemStringList>{
                new SystemStringList{"alpha", "beta", null},
                new SystemStringList{"theta", "delta", ""},
                new SystemStringList{"epsilon", "lambda", " "},
                new SystemStringList{"sigma", "pi", "    "}
            };

            // HasEmptyTrimmedString should return false for each of these lists
            System.Collections.Generic.List<SystemStringList> isFalseLists 
            = new System.Collections.Generic.List<SystemStringList>{
                new SystemStringList{"alpha", "beta", "sigma"}
            };
            
            foreach(System.Collections.Generic.List<string> list in isTrueLists)
                Assert.That(Helpers.HasEmptyTrimmedString(list), Is.True);
            
            foreach(System.Collections.Generic.List<string> list in isFalseLists)
                Assert.That(Helpers.HasEmptyTrimmedString(list), Is.False);
        }

        /// <summary>
        /// Tests the KeepOriginalIfEmptyReplacement method.
        /// </summary>
        [Test]
        [TestCase("lorem ipsum", null, ExpectedResult = "lorem ipsum")]  
        [TestCase("lorem ipsum", "", ExpectedResult = "lorem ipsum")]
        [TestCase("lorem ipsum", " ", ExpectedResult = "lorem ipsum")]    
        [TestCase("lorem ipsum", "     ", ExpectedResult = "lorem ipsum")] 
        [TestCase("lorem ipsum", "dolor", ExpectedResult = "dolor")] 
        [TestCase("lorem ipsum", "sit amet", ExpectedResult = "sit amet")]
        public string KeepOriginalIfEmptyReplacementTest(string original, string replacement)
        {
            return Helpers.KeepOriginalIfEmptyReplacement(original, replacement);
        }

        /// <summary>
        /// Tests the KeepOriginalIfEmptyReplacement method.
        /// For these tests, the original values are kept because
        /// the replacements are all considered empty.
        /// </summary>
        [Test]
        public void KeepOriginalIfEmptyReplacementTest1(
            [Values(-999, -1, 0, 1, 999, 3.14, 2.178)] double original, 
            [Values(null, "", " ", "   ")] string replacement)
        {
            double expected = original;
            double actual = Helpers.KeepOriginalIfEmptyReplacement(original, replacement);
            Assert.That(expected, Is.EqualTo(actual));
        }

        /// <summary>
        /// Tests the KeepOriginalIfEmptyReplacement method.
        /// For these tests, the replacement values are returned because
        /// they are non-empty strings that can be parsed to double.
        /// </summary>
        [Test]
        public void KeepOriginalIfEmptyReplacementTest2(
            [Values(-999, -1, 0, 1, 999, 3.14, 2.178)] double original, 
            [Values("-999", " -999", "-1", "0", "1", "999", "3.14", "2.178", "2.178 ")] string replacement)
        {
            double expected = Conversions.StringToDouble(replacement);
            double actual = Helpers.KeepOriginalIfEmptyReplacement(original, replacement);
            Assert.That(expected, Is.EqualTo(actual));
        }
    }
}