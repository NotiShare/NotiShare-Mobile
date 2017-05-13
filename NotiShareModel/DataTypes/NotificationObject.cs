using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NotiShareModel.DataTypes
{
    public class NotificationObject
    {

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "notificationText")]
        public string NotificationText { get; set; }


        [JsonProperty(PropertyName = "image")]
        public string ImageBase64 { get; set; }


    }
}
