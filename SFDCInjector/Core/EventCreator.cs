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

        private static readonly string _GlobalEventNamespace;

        static EventCreator()
        {
            Type baseClassType = typeof(IPlatformEvent<IPlatformEventFields>);
            string[] baseClassNamespaces = baseClassType.Namespace.Split('.');
            _GlobalEventNamespace = $"{baseClassNamespaces[0]}.{baseClassNamespaces[1]}";
        }

        /// <summary>
        /// Returns a boolean indicating if Type is in the same namespace
        /// as IPlatformEvent, the interface of which all events implement.
        /// </summary>
        private static bool IsTypeInGlobalNamespace(Type type)
        {
            return type.Namespace.Contains(_GlobalEventNamespace);
        }

        /// <summary>
        /// Returns a boolean indicating if Type is null or is
        /// not in the global namespace.
        /// </summary>
        private static bool isUnknownType(Type type)
        {
            return type == null || !IsTypeInGlobalNamespace(type);
        }

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

            eventClassName = $"{_GlobalEventNamespace}.{eventClassName}";
            eventFieldsClassName = $"{_GlobalEventNamespace}.{eventFieldsClassName}";

            Type eventType = Type.GetType(eventClassName);
            Type eventFieldsType = Type.GetType(eventFieldsClassName);
            Type classType = typeof(EventCreator);
            Type[] typeParameters = new Type[]{eventFieldsType};

            bool isUnknownEventType = isUnknownType(eventType);
            bool isUnknownEventFieldsType = isUnknownType(eventFieldsType);

            if(isUnknownEventType)
            {
                throw new UnknownPlatformEventException("Unable to create the event " + 
                "because the type of the event class is unknown.  Make sure the " + 
                "event class name resolves to a known event type " + 
                $"in the {_GlobalEventNamespace} namespace.");
            }

            if(isUnknownEventFieldsType)
            {
                throw new UnknownPlatformEventFieldsException("Unable to create the event " + 
                "because the type of the event fields class is unknown.  Make sure the " + 
                "event fields class name resolves to a known event type " + 
                $"in the {_GlobalEventNamespace} namespace.");
            }

            // Reflection is used here because the data type of the event fields class
            // is only known at runtime.
            try
            {
                MethodInfo createEventInstance = Helpers.MakeGenericMethod("CreateEventInstance", 
                classType, typeParameters);

                MethodInfo setEventFieldsProperties = Helpers.MakeGenericMethod("SetEventFieldsProperties", 
                classType, typeParameters);
                
                evt = createEventInstance.Invoke(null, new object[] {eventType, eventFieldsType});

                setEventFieldsProperties.Invoke(null, new object[] {
                    evt.Fields, eventFieldsPropValues});
            }
            catch(TargetInvocationException e)
            {
                Type innerInnerExceptionType = e.InnerException.InnerException.GetType();
                bool innerInnerExceptionIsOutOfRangeException = innerInnerExceptionType 
                == typeof(IndexOutOfRangeException);

                if(innerInnerExceptionIsOutOfRangeException)
                {
                    throw new InvalidCommandLineArgumentIndexException("Unable to create the event because" + 
                    $"one or more CommandLineArgumentIndexAttributes in the {eventFieldsType.Name} class has " +
                    "an Index property that is out of range.  Make sure the Index property is an integer that " + 
                    "is greater than or equal to zero and less than the total number of properties.");
                }
            }

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