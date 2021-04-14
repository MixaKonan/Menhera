using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Menhera.Extensions;

namespace Menhera.Authentification
{
    public class Authenticator
    {
        
        public string GetHashString(string password)
        {
            return new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(password)).GetString();
        }
        
        public async Task<string> GetHashStringAsync(string password)
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