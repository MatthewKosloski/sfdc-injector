using SFDCInjector.PlatformEvents;
using SFDCInjector.Attributes;
using System.Diagnostics;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace SFDCInjector
{
    /// <summary>
    /// Facilitates the dynamic creation of Platform Events via Reflection.
    /// </summary>
    public static class EventCreator
    {
        private static readonly string _GlobalEventNamespace = "SFDCInjector.PlatformEvents";

        /// <summary>
        /// Dynamically creates an instance of an event with Null Fields.
        /// <param name="eventClassName">The name of the event class, including namespaces.</param>
        /// <param name="eventFieldsClassName">The name of the event fields class, including namespaces.</param>
        /// </summary>
        public static IPlatformEvent<TEventFields> CreateEventInstance<TEventFields>(
            string eventClassName, string eventFieldsClassName) where TEventFields : IPlatformEventFields
        {
            Type eventType = Type.GetType(eventClassName);
            Type eventFieldsType = Type.GetType(eventFieldsClassName);
            var eventInstance = (IPlatformEvent<TEventFields>) Activator.CreateInstance(eventType);
            var eventFieldsInstance = (TEventFields) Activator.CreateInstance(eventFieldsType);
            eventInstance.Fields = eventFieldsInstance;
            return eventInstance;
        }

        /// <summary>
        /// Using Reflection, creates and returns an instance of `eventClassName` as a dynamic type.
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

            eventClassName = $"{_GlobalEventNamespace}.{eventClassName}";
            eventFieldsClassName = $"{_GlobalEventNamespace}.{eventFieldsClassName}";

            try
            {
                Type eventFieldsType = Type.GetType(eventFieldsClassName);
                Type classType = typeof(EventCreator);

                MethodInfo miCreateEventInstance = classType.GetMethod("CreateEventInstance");
                MethodInfo miCreateEventInstanceConstructed = miCreateEventInstance.MakeGenericMethod(eventFieldsType);
                
                MethodInfo miGetEventCliProperties = classType.GetMethod("GetEventCliProperties");
                MethodInfo miGetEventCliPropertiesConstructed = miGetEventCliProperties.MakeGenericMethod(eventFieldsType);

                evt = miCreateEventInstanceConstructed.Invoke(null, new object[] {
                    eventClassName, eventFieldsClassName});
                
                var evtFieldsPropNames = (string[]) miGetEventCliPropertiesConstructed.Invoke(null, 
                    new object[]{});

                for(int i = 0; i < evtFieldsPropNames.Length; i++)
                {
                    evt.Fields = EventCreator.SetEventFieldsProperty(evt.Fields, 
                    evtFieldsPropNames[i], eventFieldsPropValues[i]);
                }

                
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"{e.GetType()}: Could not make generic method because" + 
                " the type parameter supplied does not exist.");
                Console.WriteLine(new StackTrace(true).ToString());
            }
            return evt;
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
        public static string[] GetEventCliProperties<TEventFields>() 
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
        public static TEventFields SetEventFieldsProperty<TEventFields>(
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