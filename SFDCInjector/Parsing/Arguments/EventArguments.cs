using System.Collections.Generic;

namespace SFDCInjector.Parsing.Arguments
{
    public class EventArguments
    {

        private string _EventClassName;
        private string _EventFieldsClassName;

        public string EventClassName {
            get => _EventClassName;
            set {
                _EventClassName = value;
                _EventFieldsClassName = $"{value}Fields";
            }
        }

        public string EventFieldsClassName {
            get => _EventFieldsClassName;
        }

        public List<object> EventFieldsPropValues { get; set; }
    }
}