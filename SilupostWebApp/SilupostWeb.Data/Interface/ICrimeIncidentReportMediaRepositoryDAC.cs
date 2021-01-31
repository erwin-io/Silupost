using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface ICrimeIncidentReportMediaRepositoryDAC : IRepository<CrimeIncidentReportMediaModel>
    {
        List<CrimeIncidentReportMediaModel> FindByCrimeIncidentReportId(string CrimeIncidentReportId);
    }
}
