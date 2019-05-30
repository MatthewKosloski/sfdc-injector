using System.Collections.Generic;

namespace SFDCInjector
{
    public interface IPlatformEvent<TFields> where TFields : IPlatformEventFields
    {
        string API_NAME { get; }

        TFields Fields { get; set; }
    }
}