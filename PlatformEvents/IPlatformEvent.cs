namespace SFDCInjector.PlatformEvents
{
    public interface IPlatformEvent<TFields> where TFields : IPlatformEventFields
    {
        string API_NAME { get; }

        TFields Fields { get; set; }
    }
}