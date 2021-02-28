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
    public class CrimeIncidentCategoryDAC : RepositoryBase<CrimeIncidentCategoryModel>, ICrimeIncidentCategoryRepositoryDAC
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public CrimeIncidentCategoryDAC(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public override string Add(CrimeIncidentCategoryModel model)
        {
            try
            {
                var id = Convert.ToString(_dBConnection.ExecuteScalar("usp_crimeincidentcategory_add", new
                {
                    model.CrimeIncidentType.CrimeIncidentTypeId,
                    model.CrimeIncidentCategoryName,
                    model.CrimeIncidentCategoryDescription,
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

        public override CrimeIncidentCategoryModel Find(string id)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("usp_crimeincidentcategory_getByID", new
                {
                    CrimeIncidentCategoryId = id,
                }, commandType: CommandType.StoredProcedure))
                {
                    var model = result.Read<CrimeIncidentCategoryModel>().FirstOrDefault();
                    if(model != null)
                    {
                        model.CrimeIncidentType = result.Read<CrimeIncidentTypeModel>().FirstOrDefault();
                        model.CrimeIncidentType.IconFile = result.Read<FileModel>().FirstOrDefault();
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

        public override List<CrimeIncidentCategoryModel> GetAll()
        {
            var results = new List<CrimeIncidentCategoryModel>();
            try
            {
                var lookup = new Dictionary<string, CrimeIncidentCategoryModel>();

                _dBConnection.Query("usp_crimeincidentcategory_getAll",
                new[]
                {
                    typeof(CrimeIncidentCategoryModel),
                    typeof(CrimeIncidentTypeModel),
                    typeof(FileModel),
                }, obj =>
                {
                    CrimeIncidentCategoryModel cic = obj[0] as CrimeIncidentCategoryModel;
                    CrimeIncidentTypeModel cit = obj[1] as CrimeIncidentTypeModel;
                    FileModel citf = obj[2] as FileModel;
                    CrimeIncidentCategoryModel model;
                    if (!lookup.TryGetValue(cic.CrimeIncidentCategoryId, out model))
                        lookup.Add(cic.CrimeIncidentCategoryId, model = cic);
                    if (model.CrimeIncidentType == null)
                        model.CrimeIncidentType = new CrimeIncidentTypeModel();
                    if (model.CrimeIncidentType.IconFile == null)
                        model.CrimeIncidentType.IconFile = new FileModel();
                    model.CrimeIncidentType = cit;
                    model.CrimeIncidentType.IconFile = citf;
                    return model;
                }, splitOn: "CrimeIncidentCategoryId,CrimeIncidentTypeId,FileId", commandType: CommandType.StoredProcedure).ToList();
                if (lookup.Values.Any())
                {
                    results.AddRange(lookup.Values);
                }
                return results;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<CrimeIncidentCategoryModel> GetPage(string CrimeIncidentTypeId, string Search,  int PageNo, int PageSize, string OrderColumn, string OrderDir)
        {
            var results = new List<CrimeIncidentCategoryModel>();
            try
            {
                var lookup = new Dictionary<string, CrimeIncidentCategoryModel>();

                _dBConnection.Query("usp_crimeincidentcategory_getPaged",
                new[]
                {
                    typeof(CrimeIncidentCategoryModel),
                    typeof(PageResultsModel),
                }, obj =>
                {
                    CrimeIncidentCategoryModel cic = obj[0] as CrimeIncidentCategoryModel;
                    PageResultsModel pr = obj[1] as PageResultsModel;
                    CrimeIncidentCategoryModel model;
                    if (!lookup.TryGetValue(cic.CrimeIncidentCategoryId, out model))
                        lookup.Add(cic.CrimeIncidentCategoryId, model = cic);
                    cic.PageResult = pr;
                    return model;
                },
                new
                {
                    Search = Search,
                    CrimeIncidentTypeId = CrimeIncidentTypeId,
                    PageNo = PageNo,
                    PageSize = PageSize,
                    OrderColumn = OrderColumn,
                    OrderDir = OrderDir
                }, splitOn: "CrimeIncidentCategoryId,TotalRows", commandType: CommandType.StoredProcedure).ToList();
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
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_crimeincidentcategory_delete", new
                {
                    CrimeIncidentCategoryId = id,
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

        public override bool Update(CrimeIncidentCategoryModel model)
        {
            bool success = false;
            try
            {
                int affectedRows = 0;
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_crimeincidentcategory_update", new
                {
                    model.CrimeIncidentCategoryId,
                    model.CrimeIncidentType.CrimeIncidentTypeId,
                    model.CrimeIncidentCategoryName,
                    model.CrimeIncidentCategoryDescription,
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
