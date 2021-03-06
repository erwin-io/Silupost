﻿using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface ILegalEntityAddressRepositoryDAC : IRepository<LegalEntityAddressModel>
    {
        List<LegalEntityAddressModel> FindBySystemUserId(string SystemUserId);
        List<LegalEntityAddressModel> FindByLegalEntityId(string LegalEntityId);
    }
}