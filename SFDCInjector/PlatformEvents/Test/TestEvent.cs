namespace SFDCInjector.PlatformEvents.Test
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