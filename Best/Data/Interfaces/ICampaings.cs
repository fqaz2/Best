using Best.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Interfaces
{
    public interface ICampaings
    {
        IEnumerable<Campaing> GetCampaings { get; }
        Campaing GetCampaingById(string campaing_id);
        Task<bool> Add(Campaing Campaing);
    }
}
