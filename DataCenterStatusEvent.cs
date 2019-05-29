using System.Runtime.Serialization;

namespace SFDCInjector
{
  
    public class DataCenterStatusEvent<DataCenterStatusEventFields>: IPlatformEvent<DataCenterStatusEventFields> {

        public DataCenterStatusEventFields Fields { get; set; }

        private string _API_NAME;

        public string API_NAME
        {
            get
            {
                return _API_NAME;
            }
        }

        public DataCenterStatusEvent()
        {
            this._API_NAME = "Data_Center_Status__e";
        }
    }
}