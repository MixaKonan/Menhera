using System.Collections.Generic;

namespace Menhera.Models
{
    public class ThreadViewModel
    {
        public List<ThreadPostLastThreePosts> Model { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}