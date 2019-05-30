namespace SFDCInjector.PlatformEvents
{
  
    public class DataCenterStatus: IPlatformEvent<DataCenterStatusFields> 
    {

        public DataCenterStatusFields Fields { get; set; }

        private string _API_NAME;

        public string API_NAME
        {
            get
            {
                return _API_NAME;
            }
        }

        public DataCenterStatus()
        {
            this._API_NAME = "Data_Center_Status__e";
        }
    }
}