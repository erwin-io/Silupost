﻿using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface IEnforcementStationRepositoryDAC : IRepository<EnforcementStationModel>
    {
        EnforcementStationModel FindByGuestCode(string EnforcementStationGuestCode);
        List<EnforcementStationModel> GetPage(string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir);
        bool Remove(string id, string LastUpdatedBy);
    }
}
