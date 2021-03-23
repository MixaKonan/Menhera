using System.Text;

namespace Menhera.Extensions
{
    public static class StringExtensions
    {
        public static byte[] GetByteArray(this string str)
        {
            Encoding enc = new UTF8Encoding();

            return enc.GetBytes(str);
        }
    }
}