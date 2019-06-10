using System;

namespace SFDCInjector.Attributes
{
    /// <summary>
    /// A custom attribute that is to be applied to
    /// properties of a class that implements IPlatformEventFields.
    /// </summary>
    /// <remarks>
    /// Below is an example of its usage.  For instance, 
    /// the first CLI argument following the event name is set to `SomeEventField`.
    /// </remarks>
    /// <example>
    /// <code>
    /// public class MyEventFields : IPlatformEventFields
    /// {
    ///     [CommandLineArgumentIndex(0)]
    ///     public string SomeEventField { get; set; }
    ///     [CommandLineArgumentIndex(1)]
    ///     public string AnotherEventField { get; set; }
    /// }
    /// </code>
    /// </example>
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]  
    public class CommandLineArgumentIndexAttribute : System.Attribute  
    {  
        private int _index;

        public int Index { get => _index; }
    
        public CommandLineArgumentIndexAttribute(int index)  
        {  
            this._index = index;
        }  
    }  
}