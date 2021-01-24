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
    public class EnforcementTypeDAC : RepositoryBase<EnforcementTypeModel>, IEnforcementTypeRepositoryDAC
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public EnforcementTypeDAC(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public override string Add(EnforcementTypeModel model)
        {
            try
            {
                var id = Convert.ToString(_dBConnection.ExecuteScalar("uspe_enforcementtyp_add", new
                {
                    model.EnforcementTypeName,
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

        public override EnforcementTypeModel Find(string id)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("usp_enforcementtype_getByID", new
                {
                    EnforcementTypeId = id,
                }, commandType: CommandType.StoredProcedure))
                {
                    var model = result.Read<EnforcementTypeModel>().FirstOrDefault();
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

        public override List<EnforcementTypeModel> GetAll() => throw new NotImplementedException();

        public List<EnforcementTypeModel> GetPage(string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir)
        {
            var results = new List<EnforcementTypeModel>();
            try
            {
                var lookup = new Dictionary<string, EnforcementTypeModel>();

                _dBConnection.Query("usp_enforcementtype_getPaged",
                new[]
                {
                    typeof(EnforcementTypeModel),
                    typeof(FileModel),
                    typeof(PageResultsModel),
                }, obj =>
                {
                    EnforcementTypeModel cit = obj[0] as EnforcementTypeModel;
                    FileModel f = obj[1] as FileModel;
                    PageResultsModel pr = obj[2] as PageResultsModel;
                    EnforcementTypeModel model;
                    if (!lookup.TryGetValue(cit.EnforcementTypeId, out model))
                        lookup.Add(cit.EnforcementTypeId, model = cit);
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
                }, splitOn: "EnforcementTypeId,FileId,TotalRows", commandType: CommandType.StoredProcedure).ToList();
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
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_enforcementtype_delete", new
                {
                    EnforcementTypeId = id,
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

        public override bool Update(EnforcementTypeModel model)
        {
            bool success = false;
            try
            {
                int affectedRows = 0;
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_enforcementtype_update", new
                {
                    model.EnforcementTypeId,
                    model.EnforcementTypeName,
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
