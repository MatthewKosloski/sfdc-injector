using SFDCInjector.PlatformEvents;
using SFDCInjector.Attributes;
using SFDCInjector.Exceptions;
using SFDCInjector.Utils;
using System.Diagnostics;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace SFDCInjector.Core
{
    /// <summary>
    /// Facilitates the dynamic creation of Platform Events via Reflection.
    /// </summary>
    public static class EventCreator
    {

        public static string GlobalEventNamespace { get; set; } = "SFDCInjector.PlatformEvents";

        /// <summary>
        /// Using Reflection, creates and returns an instance of `eventClassName` as a dynamic type.
        /// <exception cref="SFDCInjector.Exceptions.UnknownPlatformEventException"></exception>
        /// <exception cref="SFDCInjector.Exceptions.UnknownPlatformEventFieldsException"></exception>
        /// </summary>
        /// <example>
        /// <code>
        /// CreateEvent("DataCenter.DataCenterNameEvent", "DataCenter.DataCenterNameEventFields", new List&lt;object&gt;{"foo", "123"});
        /// </code>
        /// </example>
        public static dynamic CreateEvent(string eventClassName, string eventFieldsClassName, 
        List<object> eventFieldsPropValues)
        {
            dynamic evt = null;

            eventClassName = $"{GlobalEventNamespace}.{eventClassName}";
            eventFieldsClassName = $"{GlobalEventNamespace}.{eventFieldsClassName}";

            Type eventType = Type.GetType(eventClassName);
            Type eventFieldsType = Type.GetType(eventFieldsClassName);

            bool isUnknownEventType = eventType == null;
            bool isUnknownEventFieldsType = eventFieldsType == null;

            if(isUnknownEventType)
            {
                throw new UnknownPlatformEventException("Unable to create the event because the type " + 
                "of the event class is unknown.  Make sure the string supplied to eventClassName resolves " + 
                $"to a known type under the namespace {GlobalEventNamespace}.");
            }

            if(isUnknownEventFieldsType)
            {
                throw new UnknownPlatformEventFieldsException("Unable to create the event because the type " + 
                "of the event fields class is unknown.  Make sure the string supplied to eventFieldsClassName " + 
                $"resolves to a known type under the namespace {GlobalEventNamespace}.");
            }

            Type classType = typeof(EventCreator);
            Type[] typeParameters = new Type[]{eventFieldsType};

            MethodInfo createEventInstance = Helpers.MakeGenericMethod("CreateEventInstance", 
            classType, typeParameters);

            MethodInfo setEventFieldsProperties = Helpers.MakeGenericMethod("SetEventFieldsProperties", 
            classType, typeParameters);
            
            evt = createEventInstance.Invoke(null, new object[] {eventType, eventFieldsType});

            setEventFieldsProperties.Invoke(null, new object[] {
                evt.Fields, eventFieldsPropValues});

            return evt;
        }

        /// <summary>
        /// Uses reflection to dynamically set the values of each of the event fields's properties.
        /// </summary>
        private static void SetEventFieldsProperties<TEventFields>(TEventFields eventFields, 
        List<object> eventFieldsPropValues) where TEventFields : IPlatformEventFields
        {
            Type eventFieldsType = typeof(TEventFields);
            Type classType = typeof(EventCreator);

            MethodInfo getEventCliProperties = Helpers.MakeGenericMethod("GetEventCliProperties", 
                classType, new Type[]{eventFieldsType});

            var evtFieldsPropNames = (string[]) getEventCliProperties.Invoke(null, new object[]{});

            for(int i = 0; i < evtFieldsPropNames.Length; i++)
            {
                eventFields = EventCreator.SetEventFieldsProperty(eventFields, 
                evtFieldsPropNames[i], eventFieldsPropValues[i]);
            }  
        }

        /// <summary>
        /// Dynamically creates an instance of an event with Null Fields.
        /// <param name="eventType">The data type of the event.</param>
        /// <param name="eventFieldsType">The data type of the event fields.</param>
        /// </summary>
        private static IPlatformEvent<TEventFields> CreateEventInstance<TEventFields>(
            Type eventType, Type eventFieldsType) where TEventFields : IPlatformEventFields
        {
            var eventInstance = (IPlatformEvent<TEventFields>) Activator.CreateInstance(eventType);
            var eventFieldsInstance = (TEventFields) Activator.CreateInstance(eventFieldsType);
            eventInstance.Fields = eventFieldsInstance;
            return eventInstance;
        }

        /// <summary>
        /// Gets the property names of `TEventFields` that use 
        /// the `CommandLineArgumentIndexAttribute`.  The position of
        /// the property in the array corresponds to the integer supplied
        /// to the attribute.
        /// </summary>
        /// <remarks>
        /// Below is an example of its usage.
        /// </remarks>
        /// <example>
        /// <code>
        /// public class MyEventFields : IPlatformEventFields
        /// {
        ///     [CommandLineArgumentIndex(1)]
        ///     public string SomeEventField { get; set; }
        ///     [CommandLineArgumentIndex(0)]
        ///     public string AnotherEventField { get; set; }
        /// }
        /// ...
        /// string[] props = GetEventCliProperties&lt;MyEventFields&gt;(); 
        /// // {"AnotherEventField", "SomeEventField"}
        /// </code>
        /// </example>
        private static string[] GetEventCliProperties<TEventFields>() 
        where TEventFields : IPlatformEventFields
        {
            Type eventFieldsType = typeof(TEventFields);
            PropertyInfo[] properties = eventFieldsType.GetProperties();

            string[] eventCliProperties = new string[properties.Length];

            try
            {
                foreach(PropertyInfo property in properties)
                {
                    object[] attributes = property.GetCustomAttributes(true);
                    foreach(Attribute attribute in attributes)
                    {
                        bool isCliArgIndexAttribute = attribute.GetType() == 
                        typeof(CommandLineArgumentIndexAttribute);
                        if(isCliArgIndexAttribute)
                        {
                            var cliArgIndexAttribute = (CommandLineArgumentIndexAttribute) attribute;
                            eventCliProperties[cliArgIndexAttribute.Index] = property.Name;
                        }
                    }
                }
            }
            catch(IndexOutOfRangeException e)
            {
                Console.WriteLine($"{e.GetType()}: Failed to add one or more properties to the array. " + 
                "Ensure every occurence of CommandLineArgumentIndexAttribute has an integer that is less than " + 
                "or equal to the total number of properties in the class.");
                Console.WriteLine(new StackTrace(true).ToString());
            }

            return eventCliProperties;
        }

        /// <summary>
        /// Uses Reflection to dynamically set the value of property 
        /// `propName` on `evtFields` to `propValue`.
        /// </summary>
        /// <remarks>
        /// Below is an example of its usage.
        /// </remarks>
        /// <example>
        /// <code>
        /// MyEventFields evtFields = new MyEventFields();
        /// evtFields = SetEventFieldsProperty(evtFields, "MyProperty", 3.14);
        /// Console.WriteLine(evtFields.MyProperty); // 3.14
        /// </code>
        /// </example>
        private static TEventFields SetEventFieldsProperty<TEventFields>(
            TEventFields evtFields, string propName, object propValue)
        where TEventFields : IPlatformEventFields
        {
            Type evtType = typeof(TEventFields);
            PropertyInfo propInfo = evtType.GetProperty(propName);
            propInfo.SetValue(evtFields, propValue);
            return evtFields;
        }
    }
}