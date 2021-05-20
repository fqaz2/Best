using Best.Areas.Identity.Data;
using Best.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Interfaces
{
    public interface ICampaigns
    {
        IEnumerable<Campaign> GetCampaigns { get; }
        IEnumerable<Campaign> GetCampaignsByUserId(string user_id);
        Campaign GetCampaignByIdForUser(string user_id, string Campaign_id);
        Campaign GetCampaignById(string Campaign_id);
        Task Create(Campaign campaign);
        Task Update(Campaign campaign);
        Task Delete(string campaignId);
        Task<double> Rating(string campaign_id);
    }
}
