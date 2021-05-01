using Best.Data.Interfaces;
using Best.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Repository
{
    public class TopicRepository : ITopics
    {
        private readonly BestContent bestContent;
        public TopicRepository(BestContent bestContent)
        {
            this.bestContent = bestContent;
        }
        public IEnumerable<Topic> GetTopics => bestContent.Topic.Include(c => c.Campaings);

        public Topic GetTopicById(string topic_id) => bestContent.Topic.FirstOrDefault(t => t.Id == topic_id);
    }
}
