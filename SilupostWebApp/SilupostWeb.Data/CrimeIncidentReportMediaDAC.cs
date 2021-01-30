using SilupostWeb.Data.Core;
using SilupostWeb.Data.Interface;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;
using System.Data;
using System;
using Dapper;
using System.Linq;

namespace SilupostWeb.Data
{
    public class CrimeIncidentReportMediaDAC : RepositoryBase<CrimeIncidentReportMediaModel>, ICrimeIncidentReportMediaRepositoryDAC
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public CrimeIncidentReportMediaDAC(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public override string Add(CrimeIncidentReportMediaModel model)
        {
            try
            {
                var id = Convert.ToString(_dBConnection.ExecuteScalar("usp_crimeincidentreportmedia_add", new
                {
                    model.DocReportMediaType.DocReportMediaTypeId,
                    model.File.FileId,
                    model.CrimeIncidentReport.CrimeIncidentReportId,
                    model.Caption,
                }, commandType: CommandType.StoredProcedure));

                if (id.Contains("Error"))
                    throw new Exception(id);

                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override CrimeIncidentReportMediaModel Find(string id)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("usp_crimeincidentreportmedia_getByID", new
                {
                    CrimeIncidentReportMediaId = id,
                }, commandType: CommandType.StoredProcedure))
                {
                    var model = result.Read<CrimeIncidentReportMediaModel>().FirstOrDefault();
                    if(model != null)
                    {
                        model.DocReportMediaType = result.Read<DocReportMediaTypeModel>().FirstOrDefault();
                        model.File = result.Read<FileModel>().FirstOrDefault();
                        model.CrimeIncidentReport = result.Read<CrimeIncidentReportModel>().FirstOrDefault();
                        model.EntityStatus = result.Read<EntityStatusModel>().FirstOrDefault();
                    }

                    return model;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<CrimeIncidentReportMediaModel> GetAll() => throw new NotImplementedException();

        public List<CrimeIncidentReportMediaModel> FindByCrimeIncidentReportId(string CrimeIncidentReportId)
        {
            var results = new List<CrimeIncidentReportMediaModel>();
            try
            {
                var lookup = new Dictionary<string, CrimeIncidentReportMediaModel>();

                _dBConnection.Query("usp_crimeincidentreportmedia_getByCrimeIncidentReportId",
                new[]
                {
                    typeof(CrimeIncidentReportMediaModel),
                    typeof(DocReportMediaTypeModel),
                    typeof(FileModel),
                    typeof(CrimeIncidentReportModel),
                }, obj =>
                {
                    CrimeIncidentReportMediaModel cit = obj[0] as CrimeIncidentReportMediaModel;
                    DocReportMediaTypeModel drmt = obj[1] as DocReportMediaTypeModel;
                    FileModel f = obj[2] as FileModel;
                    CrimeIncidentReportModel cir = obj[3] as CrimeIncidentReportModel;
                    CrimeIncidentReportMediaModel model;
                    if (!lookup.TryGetValue(cit.CrimeIncidentReportMediaId, out model))
                        lookup.Add(cit.CrimeIncidentReportMediaId, model = cit);
                    if (model.DocReportMediaType == null)
                        model.DocReportMediaType = new DocReportMediaTypeModel();
                    if (model.File == null)
                        model.File = new FileModel();
                    if (model.CrimeIncidentReport == null)
                        model.CrimeIncidentReport = new CrimeIncidentReportModel();
                    if (model.PageResult == null)
                        model.PageResult = new PageResultsModel();
                    model.DocReportMediaType = drmt;
                    model.File = f;
                    model.CrimeIncidentReport = cir;
                    return model;
                },
                new
                {
                    CrimeIncidentReportId = CrimeIncidentReportId,
                }, splitOn: "CrimeIncidentReportMediaId,DocReportMediaTypeId,FileId,CrimeIncidentReportId", commandType: CommandType.StoredProcedure).ToList();
                if (lookup.Values.Any())
                {
                    results.AddRange(lookup.Values);
                }
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Remove(string id)
        {
            bool success = false;
            try
            {
                int affectedRows = 0;
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_crimeincidentreportmedia_delete", new
                {
                    CrimeIncidentReportMediaId = id
                }, commandType: CommandType.StoredProcedure));

                if (result.Contains("Error"))
                    throw new Exception(result);

                affectedRows = Convert.ToInt32(result);
                success = affectedRows > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return success;
        }
        public override bool Update(CrimeIncidentReportMediaModel model)
        {
            bool success = false;
            try
            {
                int affectedRows = 0;
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_crimeincidentreportmedia_update", new
                {
                    model.CrimeIncidentReportMediaId,
                    model.Caption
                }, commandType: CommandType.StoredProcedure));

                if (result.Contains("Error"))
                    throw new Exception(result);

                affectedRows = Convert.ToInt32(result);
                success = affectedRows > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return success;
        }
    }
}
