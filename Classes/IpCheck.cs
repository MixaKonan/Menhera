using System;
using System.Collections.Generic;
using System.Linq;
using Menhera.Database;
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

        public static bool UserIsBanned(MenherachanContext db, string ipHash)
        {
            try
            {
                var ban = db.Ban.Last(b => b.AnonIpHash == ipHash);

                if (ban == null)
                {
                    return false;
                }

                return ban.Term > DateTimeOffset.Now.ToUnixTimeSeconds();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}