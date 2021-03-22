using System.Collections;

namespace Menhera.Intefaces
{
    public interface IHashComparator : IComparer
    {
        public bool CompareStringHashes(string h1, string h2);
        public bool CompareByteArrHashes(byte[] h1, byte[] h2);
    }
}