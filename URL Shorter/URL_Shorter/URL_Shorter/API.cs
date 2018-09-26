using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace URL_Shorter
{
    public class API
    {

        private class Info
        {
            public string long_url { get; set; }
            public string group_guid { get; set; }
        }


        // Get the guid of the group which Bitlink would belong to
        private async static Task<Info> GetGroupGuidAsync()
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://api-ssl.bitly.com/v4/groups");
                request.Method = "GET";
                request.Host = "api-ssl.bitly.com";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization: Bearer " + Resources.AccessToken);

                var rawJSON = await GetAsync(request);

                var json = JObject.Parse(rawJSON);

                var info = new Info
                {
                    group_guid = (string)json["groups"][0]["guid"]
                };

                return info;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Short Error: " + exception.Message);
            }

            return null;
        }


        // Main Task for URL shorting
        public async static Task<string> ShorterAsync(string url)
        {
            try
            {
                var info = await GetGroupGuidAsync();
                info.long_url = url;

                string infoJSON = JsonConvert.SerializeObject(info);

                var jsonByte = Encoding.UTF8.GetBytes(infoJSON);

                var request = (HttpWebRequest)WebRequest.Create("https://api-ssl.bitly.com/v4/shorten");
                request.Method = "POST";
                request.Host = "api-ssl.bitly.com";
                request.ContentType = "application/json";
                request.ContentLength = jsonByte.Length;
                request.Headers.Add("Authorization: Bearer " + Resources.AccessToken);


                using (var stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(infoJSON);
                    stream.Flush();
                    stream.Close();
                }


                var rawJSON = await GetAsync(request);

                var json = JObject.Parse(rawJSON);

                string link = (string)json["link"];

                return link;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Shorter Error: " + exception.Message);
            }

            return null;
        }


        // Get JSON from server
        private async static Task<string> GetAsync(HttpWebRequest request)
        {
            try
            {
                var task = request.GetResponseAsync();

                var response = (HttpWebResponse)await task;

                string rawJSON = null;
                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    rawJSON = reader.ReadToEnd();
                }

                return rawJSON;
            }
            catch (Exception exception)
            {
                Console.WriteLine("GetAsync Error: " + exception.Message);
            }

            return null;
        }
    }
}
