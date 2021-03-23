using Menhera.Intefaces;

namespace Menhera.Classes.Hash
{
    public class HashComparator : IHashComparator
    {
        public bool CompareStringHashes(string h1, string h2)
        {
            if (h1.Length != h2.Length)
            {
                return false;
            }

            for (var i = 0; i < h1.Length; i++)
            {
                if (h1[i] != h2[i])
                {
                    return false;
                }
            }

            return true;
        }

        public bool CompareByteArrHashes(byte[] h1, byte[] h2)
        {
            if (h1.Length != h2.Length)
            {
                return false;
            }

            for (var i = 0; i < h1.Length; i++)
            {
                if (h1[i] != h2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}