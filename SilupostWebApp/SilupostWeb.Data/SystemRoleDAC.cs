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
    public class SystemRoleDAC : RepositoryBase<SystemRoleModel>, ISystemRoleRepositoryDAC
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public SystemRoleDAC(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public override string Add(SystemRoleModel model)
        {
            try
            {
                var id = Convert.ToString(_dBConnection.ExecuteScalar("usp_systemrole_add", new
                {
                    model.Name,
                    model.Location.LocationId,
                    CreatedBy = model.CreatedBy.SystemUserId,
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

        public override SystemRoleModel Find(string id)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("usp_systemrole_getByID", new
                {
                    SystemRoleId = id,
                }, commandType: CommandType.StoredProcedure))
                {
                    var model = result.Read<SystemRoleModel>().FirstOrDefault();
                    if(model != null)
                    {
                        model.Location = result.Read<LocationModel>().FirstOrDefault();
                        model.CreatedBy = result.Read<SystemRecordManagerModel>().FirstOrDefault();
                        model.UpdatedBy = result.Read<SystemRecordManagerModel>().FirstOrDefault();
                    }

                    return model;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<SystemRoleModel> GetAll() => throw new NotImplementedException();

        public PageResultsModel<SystemRoleModel> GetPage(string SystemRoleId, string Name, string CreatedAt, string UpdatedAt, int PageNo, int PageSize, long LocationId)
        {
            var results = new PageResultsModel<SystemRoleModel>();
            results.Items = new List<SystemRoleModel>();
            try
            {
                var lookup = new Dictionary<string, SystemRoleModel>();

                _dBConnection.Query("usp_systemrole_getPaged",
                new[]
                {
                    typeof(SystemRoleModel),
                    typeof(LocationModel),
                    typeof(SystemRecordManagerModel),
                    typeof(SystemRecordManagerModel),
                    typeof(int),
                }, obj =>
                {
                    SystemRoleModel sr = obj[0] as SystemRoleModel;
                    LocationModel l = obj[1] as LocationModel;
                    SystemRecordManagerModel cb = obj[2] as SystemRecordManagerModel;
                    SystemRecordManagerModel ub = obj[3] as SystemRecordManagerModel;
                    int t = 0;
                    int.TryParse(obj[4].ToString(), out t);
                    SystemRoleModel model;
                    if (!lookup.TryGetValue(sr.SystemRoleId, out model))
                        lookup.Add(sr.SystemRoleId, model = sr);
                    if (model.Location == null)
                        model.Location = new LocationModel();
                    if (model.CreatedBy == null)
                        model.CreatedBy = new SystemRecordManagerModel();
                    if (model.UpdatedBy == null)
                        model.UpdatedBy = new SystemRecordManagerModel();
                    model.Location = l;
                    model.CreatedBy = cb;
                    model.UpdatedBy = ub;
                    results.TotalCount = t;
                    return model;
                },
                new
                {
                    SystemRoleId = SystemRoleId,
                    Name = Name,
                    CreatedAt = CreatedAt,
                    UpdatedAt = UpdatedAt,
                    PageNo = PageNo,
                    PageSize = PageSize,
                    LocationId = LocationId
                }, splitOn: "SystemRoleId,LocationId,SystemUserId,SystemUserId,TotalRows", commandType: CommandType.StoredProcedure).ToList();
                if (lookup.Values.Any())
                {
                    results.Items = lookup.Values;
                }
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Remove(string id) => throw new NotImplementedException();

        public bool Remove(string id, string UpdatedBy)
        {
            bool success = false;
            try
            {
                int affectedRows = 0;
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_systemrole_delete", new
                {
                    SystemRoleId = id,
                    UpdatedBy = UpdatedBy
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

        public override bool Update(SystemRoleModel model)
        {
            bool success = false;
            try
            {
                int affectedRows = 0;
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_systemrole_update", new
                {
                    model.SystemRoleId,
                    model.Name,
                    UpdatedBy = model.UpdatedBy.SystemUserId
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
