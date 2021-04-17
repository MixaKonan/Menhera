using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Menhera.Extensions;

namespace Menhera.Authentication
{
    public static class Authenticator
    {
        public static async Task<string> GetHashStringAsync(string password)
        {
            var hashString = string.Empty;
            
            await Task.Run(() =>
            {
                hashString = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(password)).GetString();
            });

            return hashString;
        }
    }
}