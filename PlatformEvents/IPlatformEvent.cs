namespace SFDCInjector.PlatformEvents
{
    public interface IPlatformEvent<TFields> where TFields : IPlatformEventFields
    {
        string ApiName { get; }

        TFields Fields { get; set; }
    }
}