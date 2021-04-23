using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Models
{
    public class Topic
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<Campaing> Campaings { get; }
    }
}
