using NUnit.Framework;
using System.Collections.Generic;
using SFDCInjector.Core;
using SFDCInjector.Exceptions;

namespace SFDCInjector.Tests.Core
{

    [TestFixture]
    public class EventCreatorTest
    {

        /// <summary>
        /// Tests that an exception is thrown if the string of the
        /// event class name does not resolve to a known type.
        /// </summary>
        [Test]
        public void CreateEvent_UnknownEventName_ShouldThrowUnknownPlatformEventException()
        {
            // There is no event with this name, so an exception is thrown.
            string eventClassName = "SomeClassNameOfAnEventThatDoesNotExist";

            string eventFieldsClassName = "Test.TestEventFields";
            List<object> eventFieldsPropValues = new List<object> {"string", 100, 103.5};

            Assert.That(() => {
                dynamic evt = EventCreator.CreateEvent(eventClassName, 
                eventFieldsClassName, eventFieldsPropValues);
            }, Throws.TypeOf<UnknownPlatformEventException>());
        }

        /// <summary>
        /// Tests that an exception is thrown if the string of the
        /// event fields class name does not resolve to a known type.
        /// </summary>
        [Test]
        public void CreateEvent_UnknownEventFieldsName_ShouldThrowUnknownPlatformEventFieldsException()
        {
            // There is no event field class with this name, so an exception is thrown.
            string eventFieldsClassName = "SomeNonExistentEventFieldsClassName";

            string eventClassName = "Test.TestEvent";
            List<object> eventFieldsPropValues = new List<object> {"Name", "DataCenterId"};

            Assert.That(() => {
                dynamic evt = EventCreator.CreateEvent(eventClassName, 
                eventFieldsClassName, eventFieldsPropValues);
            }, Throws.TypeOf<UnknownPlatformEventFieldsException>());
        }

        /// <summary>
        /// Tests that an exception is thrown if a CommandLineArgumentIndexAttribute
        /// on one or more properties of the field class has an Index property that
        /// is out of range.
        /// </summary>
        [Test]
        public void CreateEvent_OutOfRangeIndexAttribute_ShouldThrowInvalidCommandLineArgumentIndexException()
        {
            string eventClassName = "Test.TestOutOfRangeIndexEvent";
            string eventFieldsClassName = "Test.TestOutOfRangeIndexEventFields";
            List<object> eventFieldsPropValues = new List<object> {"Name", 42, 103.5};

            Assert.That(() => {
                dynamic evt = EventCreator.CreateEvent(eventClassName, 
                eventFieldsClassName, eventFieldsPropValues);
            }, Throws.TypeOf<InvalidCommandLineArgumentIndexException>());
        }
    }
}