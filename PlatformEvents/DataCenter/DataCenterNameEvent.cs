namespace SFDCInjector.PlatformEvents.DataCenter
{
  
    public class DataCenterNameEvent: IPlatformEvent<DataCenterNameEventFields> 
    {

        public DataCenterNameEventFields Fields { get; set; }

        private string _API_NAME;

        public string API_NAME
        {
            get
            {
                return _API_NAME;
            }
        }

        public DataCenterNameEvent()
        {
            this._API_NAME = "Data_Center_Name__e";
        }
    }
}