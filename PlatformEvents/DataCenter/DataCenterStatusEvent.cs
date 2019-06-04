namespace SFDCInjector.PlatformEvents.DataCenter
{
  
    public class DataCenterStatusEvent: IPlatformEvent<DataCenterStatusEventFields> 
    {

        public DataCenterStatusEventFields Fields { get; set; }

        private string _ApiName;

        public string ApiName { get => _ApiName; }

        public DataCenterStatusEvent()
        {
            this._ApiName = "Data_Center_Status__e";
        }
    }
}