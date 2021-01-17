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
    public class SystemUserDAC : RepositoryBase<SystemUserModel>, ISystemUserRepository
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public SystemUserDAC(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public override string Add(SystemUserModel model)
        {
            try
            {
                var id = Convert.ToString(_dBConnection.ExecuteScalar("usp_systemuser_add", new
                {
                    model.SystemUserType.SystemUserTypeId,
                    model.LegalEntity.LegalEntityId,
                    ProfilePictureFile = model.ProfilePicture.FileId,
                    model.UserName,
                    model.Password,
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

        public override SystemUserModel Find(string id)
        {
            try
            {
                var lookupSystemWebAdminUserRoles = new Dictionary<string, SystemWebAdminUserRolesModel>();
                using (var result = _dBConnection.QueryMultiple("usp_systemuser_getByID", new
                {
                    SystemUserId = id
                }, commandType: CommandType.StoredProcedure))
                {
                    var model = result.Read<SystemUserModel>().FirstOrDefault();
                    if (model != null)
                    {
                        model.SystemUserType = result.Read<SystemUserTypeModel>().FirstOrDefault();
                        model.LegalEntity = result.Read<LegalEntityModel>().FirstOrDefault();
                        model.LegalEntity.Gender = result.Read<EntityGenderModel>().FirstOrDefault();
                        result.Read<SystemWebAdminUserRolesModel, SystemWebAdminRoleModel, SystemWebAdminUserRolesModel>((swaur, swar) =>
                        {
                            SystemWebAdminUserRolesModel systemWebAdminUserRolesModel;
                            if (!lookupSystemWebAdminUserRoles.TryGetValue(swaur.SystemWebAdminUserRoleId, out systemWebAdminUserRolesModel))
                                lookupSystemWebAdminUserRoles.Add(swaur.SystemWebAdminUserRoleId, systemWebAdminUserRolesModel = swaur);
                            systemWebAdminUserRolesModel.SystemWebAdminRole = swar;
                            return systemWebAdminUserRolesModel;
                        }, splitOn: "SystemWebAdminUserRoleId,SystemWebAdminRoleId").ToList();
                        if (model.SystemWebAdminUserRoles == null)
                            model.SystemWebAdminUserRoles = new List<SystemWebAdminUserRolesModel>();
                        model.SystemWebAdminUserRoles.AddRange(lookupSystemWebAdminUserRoles.Values);
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

        public SystemUserModel Find(string Username, string Password)
        {
            try
            {
                var lookupSystemWebAdminUserRoles = new Dictionary<string, SystemWebAdminUserRolesModel>();
                using (var result = _dBConnection.QueryMultiple("usp_systemuser_getByCredentials", new
                {
                    Username = Username,
                    Password = Password,
                }, commandType: CommandType.StoredProcedure))
                {
                    var model = result.Read<SystemUserModel>().FirstOrDefault();
                    if(model != null)
                    {
                        model.SystemUserType = result.Read<SystemUserTypeModel>().FirstOrDefault();
                        model.LegalEntity = result.Read<LegalEntityModel>().FirstOrDefault();
                        model.LegalEntity.Gender = result.Read<EntityGenderModel>().FirstOrDefault();
                        result.Read<SystemWebAdminUserRolesModel, SystemWebAdminRoleModel, SystemWebAdminUserRolesModel>((swaur, swar) =>
                        {
                            SystemWebAdminUserRolesModel systemWebAdminUserRolesModel;
                            if (!lookupSystemWebAdminUserRoles.TryGetValue(swaur.SystemWebAdminUserRoleId, out systemWebAdminUserRolesModel))
                                lookupSystemWebAdminUserRoles.Add(swaur.SystemWebAdminUserRoleId, systemWebAdminUserRolesModel = swaur);
                            systemWebAdminUserRolesModel.SystemWebAdminRole = swar;
                            return systemWebAdminUserRolesModel;
                        }, splitOn: "SystemWebAdminUserRoleId,SystemWebAdminRoleId").ToList();
                        if (model.SystemWebAdminUserRoles == null)
                            model.SystemWebAdminUserRoles = new List<SystemWebAdminUserRolesModel>();
                        model.SystemWebAdminUserRoles.AddRange(lookupSystemWebAdminUserRoles.Values);
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
        public override List<SystemUserModel> GetAll() => throw new NotImplementedException();

        public List<SystemUserModel> GetPage(string Search, long SystemUserType, long PageNo, long PageSize, string OrderColumn, string OrderDir)
        {
            var results = new List<SystemUserModel>();
            try
            {
                var lookup = new Dictionary<string, SystemUserModel>();

                _dBConnection.Query("usp_systemuser_getPaged",
                new[]
                {
                    typeof(SystemUserModel),
                    typeof(SystemUserTypeModel),
                    typeof(LegalEntityModel),
                    typeof(EntityGenderModel),
                    typeof(SystemWebAdminUserRolesModel),
                    typeof(SystemWebAdminRoleModel),
                    typeof(PageResultsModel),
                }, obj =>
                {
                    SystemUserModel su = obj[0] as SystemUserModel;
                    SystemUserTypeModel sut = obj[1] as SystemUserTypeModel;
                    LegalEntityModel le = obj[2] as LegalEntityModel;
                    EntityGenderModel eg = obj[3] as EntityGenderModel;
                    SystemWebAdminUserRolesModel swaur = obj[4] as SystemWebAdminUserRolesModel;
                    SystemWebAdminRoleModel swar = obj[5] as SystemWebAdminRoleModel;
                    PageResultsModel pr = obj[6] as PageResultsModel;

                    SystemUserModel model;
                    if (!lookup.TryGetValue(su.SystemUserId, out model))
                        lookup.Add(su.SystemUserId, model = su);
                    if (model.SystemUserType == null)
                        model.SystemUserType = new SystemUserTypeModel();
                    if (model.LegalEntity == null)
                        model.LegalEntity = new LegalEntityModel();
                    if (model.LegalEntity.Gender == null)
                        model.LegalEntity.Gender = new EntityGenderModel();
                    if (model.SystemWebAdminUserRoles == null)
                        model.SystemWebAdminUserRoles = new List<SystemWebAdminUserRolesModel>();
                    if (model.PageResult == null)
                        model.PageResult = new PageResultsModel();
                    model.SystemUserType = sut;
                    model.LegalEntity = le;
                    model.LegalEntity.Gender = eg;
                    if(swaur != null)
                    {
                        if (swaur.SystemWebAdminRole == null)
                            swaur.SystemWebAdminRole = new SystemWebAdminRoleModel();
                        swaur.SystemWebAdminRole = swar;
                        model.SystemWebAdminUserRoles.Add(swaur);
                    }
                    model.PageResult = pr;
                    return model;
                },
                new
                {
                    Search = Search,
                    SystemUserType = SystemUserType,
                    PageNo = PageNo,
                    PageSize = PageSize,
                    OrderColumn = OrderColumn,
                    OrderDir = OrderDir
                }, splitOn: "SystemUserId,SystemUserTypeId,LegalEntityId,GenderId,SystemWebAdminUserRoleId,SystemWebAdminRoleId,TotalRows", commandType: CommandType.StoredProcedure).ToList();
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
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_systemuser_delete", new
                {
                    SystemUserId = id,
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

        public override bool Update(SystemUserModel model)
        {
            bool success = false;
            try
            {
                int affectedRows = 0;
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_systemuser_update", new
                {
                    model.SystemUserId,
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
