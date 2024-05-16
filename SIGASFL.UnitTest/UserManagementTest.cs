using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.UnitTest
{
    [TestClass]
    public class UserManagementTest
    {
        [TestMethod]
        public async void GetUsers()
        {
            var _httpClient = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7086/api/users");
            var res = await _httpClient.SendAsync(req);

            if (res.IsSuccessStatusCode)
            {
                var result = await res.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<dynamic>(result);
            }
        }
    }
}
