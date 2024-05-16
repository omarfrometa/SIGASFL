using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGASFL.Models;

namespace SIGASFL.Services.Interface
{
    public interface ITokenManager
    {
        Task<ClientResponse<string>> GenerateToken(string secret, double expirationMinutes, Dictionary<string, string> claims);
        Task<ClientResponse<Dictionary<string, string>>> ValidateToken(string token, string secret);
    }
}
