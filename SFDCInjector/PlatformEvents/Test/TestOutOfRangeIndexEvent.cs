namespace SFDCInjector.PlatformEvents.Test
{
    /// <summary>
    /// A Platform Event used to test the index property
    /// on the CommandLineArgumentIndex attribute.
    /// </summary>
    public class TestOutOfRangeIndexEvent: IPlatformEvent<TestOutOfRangeIndexEventFields> 
        {
            public TestOutOfRangeIndexEventFields Fields { get; set; }
            public string ApiName { get; set; }
        }
}