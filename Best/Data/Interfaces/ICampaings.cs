using Best.Areas.Identity.Data;
using Best.Data.Models;
using Microsoft.AspNetCore.Identity;
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
        Task<int> Create(Campaing campaing);
        Task Update(Campaing campaing);
        Task<int> Delete(Campaing campaing);
        Task<int> DeleteCampaingsByUserId(string user_id);
    }
}
