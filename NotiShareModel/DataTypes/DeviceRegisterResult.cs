using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NotiShareModel.DataTypes
{
    public class DeviceRegisterResult
    {
        [JsonProperty(PropertyName = "idUserDevice")]
        public string UserDeviceDbId { get; set; }


        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
