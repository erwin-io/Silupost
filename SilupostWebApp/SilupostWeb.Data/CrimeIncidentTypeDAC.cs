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
    public class CrimeIncidentTypeDAC : RepositoryBase<CrimeIncidentTypeModel>, ICrimeIncidentTypeRepositoryDAC
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public CrimeIncidentTypeDAC(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public override string Add(CrimeIncidentTypeModel model)
        {
            try
            {
                var id = Convert.ToString(_dBConnection.ExecuteScalar("usp_crimeincidenttype_add", new
                {
                    model.CrimeIncidentTypeName,
                    model.CrimeIncidentTypeDescription,
                    IconFileId = model.IconFile.FileId,
                    model.SystemRecordManager.CreatedBy,
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

        public override CrimeIncidentTypeModel Find(string id)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("usp_crimeincidenttype_getByID", new
                {
                    CrimeIncidentTypeId = id,
                }, commandType: CommandType.StoredProcedure))
                {
                    var model = result.Read<CrimeIncidentTypeModel>().FirstOrDefault();
                    if(model != null)
                    {
                        model.IconFile = result.Read<FileModel>().FirstOrDefault();
                        model.SystemRecordManager = result.Read<SystemRecordManagerModel>().FirstOrDefault();
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

        public override List<CrimeIncidentTypeModel> GetAll() => throw new NotImplementedException();

        public List<CrimeIncidentTypeModel> GetPage(string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir)
        {
            var results = new List<CrimeIncidentTypeModel>();
            try
            {
                var lookup = new Dictionary<string, CrimeIncidentTypeModel>();

                _dBConnection.Query("usp_crimeincidenttype_getPaged",
                new[]
                {
                    typeof(CrimeIncidentTypeModel),
                    typeof(FileModel),
                    typeof(PageResultsModel),
                }, obj =>
                {
                    CrimeIncidentTypeModel cit = obj[0] as CrimeIncidentTypeModel;
                    FileModel f = obj[1] as FileModel;
                    PageResultsModel pr = obj[2] as PageResultsModel;
                    CrimeIncidentTypeModel model;
                    if (!lookup.TryGetValue(cit.CrimeIncidentTypeId, out model))
                        lookup.Add(cit.CrimeIncidentTypeId, model = cit);
                    cit.IconFile = f;
                    cit.PageResult = pr;
                    return model;
                },
                new
                {
                    Search = Search,
                    PageNo = PageNo,
                    PageSize = PageSize,
                    OrderColumn = OrderColumn,
                    OrderDir = OrderDir
                }, splitOn: "CrimeIncidentTypeId,FileId,TotalRows", commandType: CommandType.StoredProcedure).ToList();
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

        public override bool Remove(string id) => throw new NotImplementedException();

        public bool Remove(string id, string LastUpdatedBy)
        {
            bool success = false;
            try
            {
                int affectedRows = 0;
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_crimeincidenttype_delete", new
                {
                    CrimeIncidentTypeId = id,
                    LastUpdatedBy = LastUpdatedBy
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

        public override bool Update(CrimeIncidentTypeModel model)
        {
            bool success = false;
            try
            {
                int affectedRows = 0;
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_crimeincidenttype_update", new
                {
                    model.CrimeIncidentTypeId,
                    model.CrimeIncidentTypeName,
                    model.CrimeIncidentTypeDescription,
                    model.SystemRecordManager.LastUpdatedBy
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
