using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BitlyDotNet
{
    public class BitlyAPIClient
    {
        private const string BASEAPIURL = "https://api-ssl.bitly.com/v4";
        private const string BEARER_AUTH_SCHEME = "Bearer";
        private const string MIME_TYPE_APP_JSON = "application/json"; // This exists in System.Net.Mime.MediaTypeNames but only in netstandard2.1/netcoreapp3.x

        private static readonly string GROUPS_URL = $"{BASEAPIURL}/groups";
        private static readonly string SHORTEN_URL = $"{BASEAPIURL}/shorten";

        private readonly HttpClient httpClient;
        private readonly string accessToken;

        public BitlyAPIClient(HttpClient httpClient, string accessToken)
        {
            this.httpClient = httpClient;
            this.accessToken = accessToken;
        }

        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            string groupsJson;

            using (var getGroupsRequest = new HttpRequestMessage(HttpMethod.Get, GROUPS_URL))
            {
                getGroupsRequest.Headers.Authorization = new AuthenticationHeaderValue(BEARER_AUTH_SCHEME, accessToken);

                HttpResponseMessage res = await httpClient.SendAsync(getGroupsRequest).ConfigureAwait(false);
                res.EnsureSuccessStatusCode();

                groupsJson = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            GroupsResponse response = JsonConvert.DeserializeObject<GroupsResponse>(groupsJson);

            return response.Groups;
        }

        public async Task<string> ShortenAsync(string url, string groupGuid)
        {
            string shortenJson;
            string shortenData = JsonConvert.SerializeObject(new ShortenRequest { LongUrl = url, GroupGuid = groupGuid });

            using (var shortenRequest = new HttpRequestMessage(HttpMethod.Post, SHORTEN_URL))
            {
                shortenRequest.Headers.Authorization = new AuthenticationHeaderValue(BEARER_AUTH_SCHEME, accessToken);
                shortenRequest.Content = new StringContent(shortenData, Encoding.UTF8, MIME_TYPE_APP_JSON);

                HttpResponseMessage res = await httpClient.SendAsync(shortenRequest).ConfigureAwait(false);
                res.EnsureSuccessStatusCode();

                shortenJson = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            ShortenResponse shortenResponse = JsonConvert.DeserializeObject<ShortenResponse>(shortenJson);

            return shortenResponse.Link;
        }
    }
}
