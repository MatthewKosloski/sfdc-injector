using System.Collections.Generic;

namespace SFDCInjector
{
    public interface IPlatformEvent<IPlatformEventFields>
    {
        string API_NAME { get; }

        IPlatformEventFields Fields { get; set; }
    }
}