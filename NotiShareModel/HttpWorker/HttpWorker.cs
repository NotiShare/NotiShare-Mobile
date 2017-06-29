using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NotiShareModel.DataTypes;

namespace NotiShareModel.HttpWorker
{
    public sealed class HttpWorker
    {

        private static readonly Lazy<HttpWorker> singleton = new Lazy<HttpWorker>(() => new HttpWorker());

        private HttpClient client;

        public static HttpWorker Instance { get { return singleton.Value; } }
        private const int Timeout = 10;

        private const string DefaultUrl = "http://192.168.100.6:3030";

        private HttpWorker()
        {
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(Timeout);
        }



        public async Task<string> RegisterUser(RegistrationObject registration)
        {
            return await PostRequest(registration, $"{DefaultUrl}/register");
        }


        public async Task<string> Login(LoginObject loginObject)
        {
            return await PostRequest(loginObject, $"{DefaultUrl}/login");
        }


        public async Task<DeviceRegisterResult> RegisterDevice(RegisterDeviceObject device)
        {
            var resultString = await PostRequest(device, $"{DefaultUrl}/registerDevice").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<DeviceRegisterResult>(resultString);
        }

        private async Task<string> PostRequest<T>(T objectToSend, string url)
        {
            string result;
            var jsonObject = JsonConvert.SerializeObject(objectToSend);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            try
            {
                using (var request = await client.PostAsync(url, content).ConfigureAwait(false))
                {
                    using (var resultContent = request.Content)
                    {
                        result = await resultContent.ReadAsStringAsync();
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                result = "Timeout";
            }
            catch (WebException ex)
            {
                result = nameof(WebException);
            }
            catch (HttpRequestException ex)
            {
                result = nameof(HttpRequestException);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    }
}
