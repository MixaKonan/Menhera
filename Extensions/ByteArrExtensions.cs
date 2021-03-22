using System.Text;

namespace Menhera.Extensions
{
    public static class ByteArrExtensions
    {
        public static string BytesToString(this byte[] arr)
        {
            var stringBuilder = new StringBuilder();
            
            foreach (var b in arr)
            {
                stringBuilder.AppendFormat("{0:X2}", b);
            }

            return stringBuilder.ToString();
        }
    }
}