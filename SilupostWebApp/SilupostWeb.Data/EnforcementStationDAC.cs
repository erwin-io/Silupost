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
    public class EnforcementStationDAC : RepositoryBase<EnforcementStationModel>, IEnforcementStationRepositoryDAC
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public EnforcementStationDAC(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public override string Add(EnforcementStationModel model)
        {
            try
            {
                var id = Convert.ToString(_dBConnection.ExecuteScalar("usp_enforcementstation_add", new
                {
                    model.EnforcementStationName,
                    model.EnforcementStationGuestCode,
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

        public override EnforcementStationModel Find(string id)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("usp_enforcementstation_getByID", new
                {
                    EnforcementStationId = id,
                }, commandType: CommandType.StoredProcedure))
                {
                    var model = result.Read<EnforcementStationModel>().FirstOrDefault();
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

        public EnforcementStationModel FindByGuestCode(string EnforcementStationGuestCode)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("usp_enforcementstation_getByGuestCode", new
                {
                    EnforcementStationGuestCode = EnforcementStationGuestCode,
                }, commandType: CommandType.StoredProcedure))
                {
                    var model = result.Read<EnforcementStationModel>().FirstOrDefault();
                    if (model != null)
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


        public override List<EnforcementStationModel> GetAll() => throw new NotImplementedException();

        public List<EnforcementStationModel> GetPage(string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir)
        {
            var results = new List<EnforcementStationModel>();
            try
            {
                var lookup = new Dictionary<string, EnforcementStationModel>();

                _dBConnection.Query("usp_enforcementstation_getPaged",
                new[]
                {
                    typeof(EnforcementStationModel),
                    typeof(FileModel),
                    typeof(PageResultsModel),
                }, obj =>
                {
                    EnforcementStationModel cit = obj[0] as EnforcementStationModel;
                    FileModel f = obj[1] as FileModel;
                    PageResultsModel pr = obj[2] as PageResultsModel;
                    EnforcementStationModel model;
                    if (!lookup.TryGetValue(cit.EnforcementStationId, out model))
                        lookup.Add(cit.EnforcementStationId, model = cit);
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
                }, splitOn: "EnforcementStationId,FileId,TotalRows", commandType: CommandType.StoredProcedure).ToList();
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
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_enforcementstation_delete", new
                {
                    EnforcementStationId = id,
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

        public override bool Update(EnforcementStationModel model)
        {
            bool success = false;
            try
            {
                int affectedRows = 0;
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_enforcementstation_update", new
                {
                    model.EnforcementStationId,
                    model.EnforcementStationName,
                    model.EnforcementStationGuestCode,
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
