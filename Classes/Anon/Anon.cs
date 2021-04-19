namespace Menhera.Classes.Anon
{
    public class Anon
    {
        public string IpHash { get; }
        public bool IsBanned { get; }
        public Anon(string ipHash, bool isBanned)
        {
            IpHash = ipHash;
            IsBanned = isBanned;
        }
    }
}