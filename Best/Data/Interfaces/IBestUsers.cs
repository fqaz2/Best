using Best.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Best.Data.Interfaces
{
    public interface IBestUsers
    {
        IEnumerable<BestUser> GetUsers { get; }
        BestUser GetUserById(string user_id);
        Task<int> Delete(BestUser bestUser);
    }
}
