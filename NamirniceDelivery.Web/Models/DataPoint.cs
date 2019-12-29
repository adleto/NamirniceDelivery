using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.Models
{
    [DataContract]
    public class DataPoint
    {
        public DataPoint(string label, decimal y)
        {
            this.Label = label;
            this.Y = y;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string Label = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<decimal> Y = null;
    }
}
