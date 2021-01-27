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
    public class EnforcementUnitDAC : RepositoryBase<EnforcementUnitModel>, IEnforcementUnitRepositoryDAC
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public EnforcementUnitDAC(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public override string Add(EnforcementUnitModel model)
        {
            try
            {
                var id = Convert.ToString(_dBConnection.ExecuteScalar("usp_enforcementunit_add", new
                {
                    model.EnforcementType.EnforcementTypeId,
                    model.EnforcementStation.EnforcementStationId,
                    model.LegalEntity.LegalEntityId,
                    ProfilePictureFile = model.ProfilePicture.FileId,
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

        public override EnforcementUnitModel Find(string id)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("usp_enforcementunit_getByID", new
                {
                    EnforcementUnitId = id
                }, commandType: CommandType.StoredProcedure))
                {
                    var model = result.Read<EnforcementUnitModel>().FirstOrDefault();
                    if (model != null)
                    {
                        model.EnforcementType = result.Read<EnforcementTypeModel>().FirstOrDefault();
                        model.EnforcementStation = result.Read<EnforcementStationModel>().FirstOrDefault();
                        model.ProfilePicture = result.Read<FileModel>().FirstOrDefault();
                        model.LegalEntity = result.Read<LegalEntityModel>().FirstOrDefault();
                        model.LegalEntity.Gender = result.Read<EntityGenderModel>().FirstOrDefault();
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

        public override List<EnforcementUnitModel> GetAll() => throw new NotImplementedException();

        public List<EnforcementUnitModel> GetPage(string Search, long PageNo, long PageSize, string OrderColumn, string OrderDir)
        {
            var results = new List<EnforcementUnitModel>();
            try
            {
                var lookup = new Dictionary<string, EnforcementUnitModel>();

                _dBConnection.Query("usp_enforcementunit_getPaged",
                new[]
                {
                    typeof(EnforcementUnitModel),
                    typeof(EnforcementTypeModel),
                    typeof(EnforcementStationModel),
                    typeof(LegalEntityModel),
                    typeof(EntityGenderModel),
                    typeof(FileModel),
                    typeof(PageResultsModel),
                }, obj =>
                {
                    EnforcementUnitModel eu = obj[0] as EnforcementUnitModel;
                    EnforcementTypeModel et = obj[1] as EnforcementTypeModel;
                    EnforcementStationModel es = obj[2] as EnforcementStationModel;
                    LegalEntityModel le = obj[3] as LegalEntityModel;
                    EntityGenderModel eg = obj[4] as EntityGenderModel;
                    FileModel f = obj[5] as FileModel;
                    PageResultsModel pr = obj[6] as PageResultsModel;

                    EnforcementUnitModel model;
                    if (!lookup.TryGetValue(eu.EnforcementUnitId, out model))
                        lookup.Add(eu.EnforcementUnitId, model = eu);
                    if (model.EnforcementType == null)
                        model.EnforcementType = new EnforcementTypeModel();
                    if (model.EnforcementStation == null)
                        model.EnforcementStation = new EnforcementStationModel();
                    if (model.LegalEntity == null)
                        model.LegalEntity = new LegalEntityModel();
                    if (model.LegalEntity.Gender == null)
                        model.LegalEntity.Gender = new EntityGenderModel();
                    if (model.ProfilePicture == null)
                        model.ProfilePicture = new FileModel();
                    if (model.PageResult == null)
                        model.PageResult = new PageResultsModel();
                    model.EnforcementType = et;
                    model.EnforcementStation = es;
                    model.LegalEntity = le;
                    model.LegalEntity.Gender = eg;
                    model.ProfilePicture = f;
                    model.PageResult = pr;
                    return model;
                },
                new
                {
                    Search = Search,
                    PageNo = PageNo,
                    PageSize = PageSize,
                    OrderColumn = OrderColumn,
                    OrderDir = OrderDir
                }, splitOn: "EnforcementUnitId,EnforcementTypeId,EnforcementStationId,LegalEntityId,GenderId,FileId,TotalRows", commandType: CommandType.StoredProcedure).ToList();
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
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_enforcementunit_delete", new
                {
                    EnforcementUnitId = id,
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

        public override bool Update(EnforcementUnitModel model)
        {
            bool success = false;
            try
            {
                int affectedRows = 0;
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_enforcementunit_update", new
                {
                    model.EnforcementUnitId,
                    model.EnforcementType.EnforcementTypeId,
                    model.EnforcementStation.EnforcementStationId,
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
