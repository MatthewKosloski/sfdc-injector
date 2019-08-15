namespace SFDCInjector.PlatformEvents.DataCenter
{
  
    public class DataCenterNameEvent: IPlatformEvent<DataCenterNameEventFields> 
    {

        public DataCenterNameEventFields Fields { get; set; }

        private string _ApiName;

        public string ApiName { get => _ApiName; }

        public DataCenterNameEvent()
        {
            this._ApiName = "Data_Center_Name__e";
        }
    }
}