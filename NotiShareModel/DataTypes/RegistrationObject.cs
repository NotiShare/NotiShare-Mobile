using Newtonsoft.Json;

namespace NotiShareModel.DataTypes
{
    public class RegistrationObject
    {
         
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }  

        [JsonProperty(PropertyName = "surname")]
        public string Surname { get; set; }


        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "passwordHash")]
        public string PasswordHash { get; set; }
    }
}
