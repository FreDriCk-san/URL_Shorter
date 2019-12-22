using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using FluentAssertions;
using System.Linq;

namespace BitlyDotNet.Tests
{
    [TestClass]
    public class GroupsTests
    {
        [TestMethod]
        public async Task WhenGettingGroupsShouldReturnResults()
        {
            var client = new BitlyAPIClient(Http.MockClient, "FakeAccessToken");
            IEnumerable<Group> groups = await client.GetGroupsAsync().ConfigureAwait(false);

            groups.Should().NotBeNull();
            groups.Should().NotBeEmpty();
            groups.Should().NotContainNulls();
            groups.Should().OnlyContain(group => !string.IsNullOrWhiteSpace(group.GUID));
        }
    }
}
