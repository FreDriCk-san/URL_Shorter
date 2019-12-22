using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BitlyDotNet.Tests
{
    public static class Http
    {
        private static HttpClient client;

        public static HttpClient MockClient
        {
            get
            {
                if (client == null)
                {
                    var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

                    handlerMock
                       .Protected()
                       .Setup<Task<HttpResponseMessage>>(
                          "SendAsync",
                          ItExpr.Is<HttpRequestMessage>(x => x.RequestUri.AbsoluteUri.EndsWith("groups", StringComparison.OrdinalIgnoreCase)),
                          ItExpr.IsAny<CancellationToken>()
                       )
                       .ReturnsAsync(new HttpResponseMessage()
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent("{\"groups\":[{\"created\":\"2019-01-01T08:00:00+0000\",\"modified\":\"2019-01-01T08:00:00+0000\",\"bsds\":[],\"guid\":\"FakeGrpGuid\",\"organization_guid\":\"FakeOrgGuid\",\"name\":\"FakeOrg\",\"is_active\":true,\"role\":\"org-admin\",\"references\":{\"organization\":\"https://api-ssl.bitly.com/v4/organizations/FakeOrgGuid\"}}]}"),
                       })
                       .Verifiable();

                    handlerMock
                       .Protected()
                       .Setup<Task<HttpResponseMessage>>(
                          "SendAsync",
                          ItExpr.Is<HttpRequestMessage>(x => x.RequestUri.AbsoluteUri.EndsWith("shorten", StringComparison.OrdinalIgnoreCase)),
                          ItExpr.IsAny<CancellationToken>()
                       )
                       .ReturnsAsync((HttpRequestMessage msg, CancellationToken token) =>
                       {
                           // TODO BAD BAD BAD
                           string body = msg.Content.ReadAsStringAsync().Result;
                           var x = JObject.Parse(body);
                           return new HttpResponseMessage()
                           {
                               StatusCode = HttpStatusCode.OK,
                               Content = new StringContent($"{{\"created_at\":\"{DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz")}\",\"id\":\"bit.ly/FakeLnk\",\"link\":\"http://bit.ly/FakeLnk\",\"custom_bitlinks\":[],\"long_url\":\"{x["long_url"]}\",\"archived\":false,\"tags\":[],\"deeplinks\":[],\"references\":{{\"group\":\"https://api-ssl.bitly.com/v4/groups/FakeGrpGuid\"}}}}"),
                           };
                       })
                       .Verifiable();

                    client = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://MSTestV2.com/"), };
                }

                return client;
            }
        }
    }
}
