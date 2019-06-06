using SFDCInjector.PlatformEvents;
using System.Diagnostics;
using System;
using System.Reflection;

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
        /// CreateEvent("DataCenter.DataCenterNameEvent", "DataCenter.DataCenterNameEventFields");
        /// </code>
        /// </example>
        public static dynamic CreateEvent(string eventClassName, string eventFieldsClassName)
        {
            dynamic evt = null;

            eventClassName = $"{_GlobalEventNamespace}.{eventClassName}";
            eventFieldsClassName = $"{_GlobalEventNamespace}.{eventFieldsClassName}";

            try
            {
                Type typeParameter = Type.GetType(eventFieldsClassName);
                MethodInfo mi = typeof(EventCreator).GetMethod("CreateEventInstance");
                MethodInfo miConstructed = mi.MakeGenericMethod(typeParameter);
                object[] methodArgs = {eventClassName, eventFieldsClassName};
                evt = miConstructed.Invoke(null, methodArgs);
                
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"{e.GetType()}: Could not make generic method because" + 
                " the type parameter supplied does not exist.");
                Console.WriteLine(new StackTrace(true).ToString());
            }
            return evt;
        }

    }
}