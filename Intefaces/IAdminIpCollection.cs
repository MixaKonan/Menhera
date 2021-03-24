using System.Collections.Generic;
using Menhera.Models;

namespace Menhera.Intefaces
{
    public interface IAdminCollection
    {
        public IEnumerable<Admin> Admins { get; set; }
    }
}