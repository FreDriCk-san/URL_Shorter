using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitlyDotNet.Tests
{
    [TestClass]
    public class URLShorteningTests
    {
        private const string Url = "https://www.google.com/search?q=bitly+url+short+dot+net";

        [TestMethod]
        public async Task WhenShorteningURLShouldGetLinkAsync()
        {
            var client = new BitlyAPIClient(Http.MockClient, "FakeAccessToken");
            IEnumerable<Group> groups = await client.GetGroupsAsync().ConfigureAwait(false);
            string groupGuid = groups?.FirstOrDefault()?.GUID;

            string shortUrl = await client.ShortenAsync(Url, groupGuid).ConfigureAwait(false);
            shortUrl.Should().NotBeNullOrWhiteSpace();
            shortUrl.Should().NotBeSameAs(Url);
        }
    }
}
