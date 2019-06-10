using NUnit.Framework;
using SFDCInjector.Utils;

namespace SFDCInjector.Tests.Utils
{
    /// <summary>
    /// Tests for SFDCInjector's Conversions class.
    /// </summary>
    [TestFixture]
    public class ConversionsTest
    {

        /// <summary>
        /// Tests the StringToDouble method.
        /// </summary>
        [Test]
        [TestCase("-1.5", ExpectedResult = -1.5)]
        [TestCase("-1", ExpectedResult = -1)]
        [TestCase("-1.0", ExpectedResult = -1.0)]
        [TestCase("0", ExpectedResult = 0)]
        [TestCase("1", ExpectedResult = 1)]
        [TestCase("1.0", ExpectedResult = 1.0)]
        [TestCase("1.5", ExpectedResult = 1.5)]
        [TestCase(" 2", ExpectedResult = 2.0)]
        [TestCase("2 ", ExpectedResult = 2.0)]
        [TestCase(" 2 ", ExpectedResult = 2.0)]
        public double StringToDoubleTest(string str)
        {
            return Conversions.StringToDouble(str);
        }

    }
}