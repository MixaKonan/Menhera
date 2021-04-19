using System;
using System.Collections.Generic;
using System.Linq;
using Menhera.Database;
using Menhera.Models;

namespace Menhera.Classes.Anon
{
    public static class IpCheck
    {
        public static bool UserIsBanned(MenherachanContext db, string ipHash)
        {
            try
            {
                var ban = db.Ban.First(b => b.AnonIpHash == ipHash);

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

        public static bool UserIsOp(Thread thread, string ipHash)
        {
            return thread.OpIpHash == ipHash;
        }
    }
}