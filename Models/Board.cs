using System.Collections.Generic;
using Newtonsoft.Json;

namespace Menhera.Models
{
    public class Board
    {
        [JsonProperty]
        public string Prefix { get; }
        
        [JsonProperty]
        public string Postfix { get;}
        
        [JsonProperty]
        public string Title { get; }
        
        [JsonProperty]
        public string Description { get; }
        
        [JsonProperty]
        public bool IsHidden { get;}
        
        [JsonProperty]
        public bool AnonHasNoName { get; }
        
        [JsonProperty]
        public bool NoSubject { get; }
        [JsonProperty]
        
        public bool NoFilesAllowed { get; }
        [JsonProperty]
        public int FileLimit { get; }
        
        [JsonProperty]
        public string AnonName { get; }

        public Board(string prefix, string postfix, string title, string description, int fileLimit = 4, string anonName = "", bool isHidden = false, bool anonHasNoName = false, bool noSubject = false, bool noFilesAllowed = false)
        {
            Prefix = prefix;
            Postfix = postfix;
            Title = title;
            Description = description;
            IsHidden = isHidden;
            AnonHasNoName = anonHasNoName;
            NoSubject = noSubject;
            NoFilesAllowed = noFilesAllowed;
            FileLimit = fileLimit;
            AnonName = anonName;
        }
    }
}