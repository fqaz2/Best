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
        IEnumerable<Campaing> GetCampaingsByUserId(string user_id);
        Campaing GetCampaingByIdForUser(string user_id, string campaing_id);
        Campaing GetCampaingById(string campaing_id);
        Task<bool> Create(Campaing campaing);
        Task<bool> Update(Campaing campaing);
        Task<bool> Delete(Campaing campaing);
    }
}
