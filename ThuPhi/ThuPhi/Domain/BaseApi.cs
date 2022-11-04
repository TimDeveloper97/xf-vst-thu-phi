using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ThuPhi.Services;
using Xamarin.Forms;

namespace ThuPhi.Domain
{
    class Api
    {
        public const string Url = "http://thuphi.vst.edu.vn/api/common";
        public const string Login = "account/login";
        public const string UpdateDotThu = "nguoithu/updatedotthu";
    }

    class Service<R, S> : Api
    {
        public static async Task<R> Post(string api, S model)
        {
            var httpClient = new HttpClient();
            var bm = new BaseModel
            {
                Url = api,
                Value = model,
            };

            var json = JsonConvert.SerializeObject(bm);
            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var response = await httpClient.PostAsync(Url, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jBaseModel = JsonConvert.DeserializeObject<BaseModel>(content);
                    var jObj = JsonConvert.DeserializeObject<R>(jBaseModel.Value.ToString());

                    return jObj;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error: fail to call api");
            }

            return default(R);
        }

        public static async Task<List<R>> Posts(string api, S model)
        {
            var httpClient = new HttpClient();
            var bm = new BaseModel
            {
                Url = api,
                Value = model,
            };

            var json = JsonConvert.SerializeObject(bm);
            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var response = await httpClient.PostAsync(Url, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jBaseModel = JsonConvert.DeserializeObject<BaseModel>(content);

                    if (jBaseModel.Code != 0)
                    {
                        DependencyService.Get<IMessage>().ShortAlert(jBaseModel.Message);
                    }
                    else
                    {
                        var jvalue = JsonConvert.SerializeObject(jBaseModel.Value);
                        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jvalue);
                        var result = new List<R>();
                        foreach (var kv in dict)
                        {
                            var r = JsonConvert.DeserializeObject<R>(kv.Value);
                            result.Add(r);
                        }

                        return result;
                    }
                    
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error: fail to call api");
            }

            return default(List<R>);
        }

        public static async Task<List<R>> Posts(BaseModel bm)
        {
            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(bm);
            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var response = await httpClient.PostAsync(Url, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jBaseModel = JsonConvert.DeserializeObject<BaseModel>(content);

                    if (jBaseModel.Code != 0)
                    {
                        DependencyService.Get<IMessage>().ShortAlert(jBaseModel.Message);
                    }
                    else
                    {
                        var jvalue = JsonConvert.SerializeObject(jBaseModel.Value);
                        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jvalue);
                        var result = new List<R>();
                        foreach (var kv in dict)
                        {
                            var r = JsonConvert.DeserializeObject<R>(kv.Value);
                            result.Add(r);
                        }

                        return result;
                    }

                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error: fail to call api");
            }

            return default(List<R>);
        }
    }
}
