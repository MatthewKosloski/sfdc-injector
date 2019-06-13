using SFDCInjector.PlatformEvents;

namespace SFDCInjector.Tests.Core
{
    /// <summary>
    /// A Platform Event used in tests.
    /// </summary>
    public class TestEvent: IPlatformEvent<TestEventFields> 
        {
            public TestEventFields Fields { get; set; }
            public string ApiName { get; set; }
        }
}