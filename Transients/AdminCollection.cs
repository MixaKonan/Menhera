using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Menhera.Database;
using Menhera.Intefaces;
using Menhera.Models;

namespace Menhera.Transients
{
    public class AdminCollection : IAdminCollection
    {
        public IEnumerable<Admin> Admins { get; set; }
        
        private readonly MenherachanContext _db = new MenherachanContext();
        
        public AdminCollection()
        {
            Admins = _db.Admin.Select(admin => admin).ToList();
        }
        
    }
}