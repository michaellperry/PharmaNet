﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PharmaNet.Fulfillment.Presentation
{
    [DataContract]
    public class Shipment
    {
        [DataMember]
        public string TrackingNumber { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }
}