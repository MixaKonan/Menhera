using System.Text;

namespace Menhera.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string GetString(this byte[] array)
        {
            var stringBuilder = new StringBuilder();

            foreach (var b in array)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }

            return stringBuilder.ToString();
        }
    }
}