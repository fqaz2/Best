using Best.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Interfaces
{
    public interface ITopics
    {
        IEnumerable<Topic> GetTopics { get; }
        Topic GetTopicById(string topic_id);
    }
}
