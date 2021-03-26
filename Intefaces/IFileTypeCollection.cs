using System.Collections.Generic;

namespace Menhera.Intefaces
{
    public interface IFileTypeCollection
    {
        public IEnumerable<string> FileTypes { get; set; }
    }
}