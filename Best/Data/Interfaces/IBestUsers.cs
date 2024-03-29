﻿using Best.Areas.Identity.Data;
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
        Task Delete(String bestUserId);
        Task Block(string bestUserId);
        Task AddRole(string bestUserId, string roleId);
    }
}
