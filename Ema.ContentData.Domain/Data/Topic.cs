using System;
namespace Ema.ContentData.Domain.Data
{
    public class Topic
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Category { get; set; }
        public string Topic_Detail { get; set; }
        public bool Liked { get; set; }
        public int[] Releated { get; set; }
    }
}
