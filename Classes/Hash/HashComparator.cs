using Menhera.Intefaces;

namespace Menhera.Classes.Hash
{
    public static class HashComparator
    {
        public static bool CompareStringHashes(string h1, string h2)
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

        public static bool CompareByteArrHashes(byte[] h1, byte[] h2)
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