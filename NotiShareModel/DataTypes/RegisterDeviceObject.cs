using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NotiShareModel.DataTypes
{
    public class RegisterDeviceObject
    {
        
        [JsonProperty(PropertyName = "userDeviceId")]
        public int UserDeviceId { get; set; }

        [JsonProperty(PropertyName = "deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty(PropertyName = "deviceType")]
        public int DeviceType { get; set; }
    }
}
