using System.Collections.Generic;
using Menhera.Models;

namespace Menhera.Classes
{
    public static class IpCheck
    {
        public static bool IpBelongsToAdmin(string ipHash, IEnumerable<Admin> admins, out Admin admin)
        {
            admin = new Admin();
            
            foreach (var _admin in admins)
            {
                if (ipHash == _admin.AdminIpHash)
                {
                    admin = _admin;
                    return true;
                }
            }


            return false;
        }
    }
}