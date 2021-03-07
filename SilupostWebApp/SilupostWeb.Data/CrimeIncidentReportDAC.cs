using SilupostWeb.Data.Core;
using SilupostWeb.Data.Interface;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;
using System.Data;
using System;
using Dapper;
using System.Linq;
using System.Globalization;

namespace SilupostWeb.Data
{
    public class CrimeIncidentReportDAC : RepositoryBase<CrimeIncidentReportModel>, ICrimeIncidentReportRepositoryDAC
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public CrimeIncidentReportDAC(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public override string Add(CrimeIncidentReportModel model)
        {
            try
            {
                var id = Convert.ToString(_dBConnection.ExecuteScalar("usp_crimeincidentreport_add", new
                {
                    model.CrimeIncidentCategory.CrimeIncidentCategoryId,
                    PostedBySystemUserId = model.PostedBySystemUser.SystemUserId,
                    model.DateReported,
                    model.PossibleDate,
                    model.PossibleTime,
                    model.Description,
                    model.GeoStreet,
                    model.GeoDistrict,
                    model.GeoCityMun,
                    model.GeoProvince,
                    model.GeoCountry,
                    model.GeoTrackerLatitude,
                    model.GeoTrackerLongitude,
                    model.IsReviewActionEnable,
                    model.IsReviewCommentEnable,
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

        public override CrimeIncidentReportModel Find(string id)
        {
            try
            {
                var lookupCrimeIncidentReportMedia = new Dictionary<string, CrimeIncidentReportMediaModel>();
                using (var result = _dBConnection.QueryMultiple("usp_crimeincidentreport_getByID", new
                {
                    CrimeIncidentReportId = id,
                }, commandType: CommandType.StoredProcedure))
                {
                    var model = result.Read<CrimeIncidentReportModel>().FirstOrDefault();
                    if(model != null)
                    {
                        model.CrimeIncidentCategory = result.Read<CrimeIncidentCategoryModel>().FirstOrDefault();
                        model.CrimeIncidentCategory.CrimeIncidentType = result.Read<CrimeIncidentTypeModel>().FirstOrDefault();
                        model.PostedBySystemUser = result.Read<SystemUserModel>().FirstOrDefault();
                        if (model.PostedBySystemUser.LegalEntity == null)
                            model.PostedBySystemUser.LegalEntity = new LegalEntityModel();
                        model.PostedBySystemUser.LegalEntity = result.Read<LegalEntityModel>().FirstOrDefault();
                        model.ApprovalStatus = result.Read<EntityApprovalStatusModel>().FirstOrDefault();
                        //model.CrimeIncidentReportMedia = result.Read<CrimeIncidentReportMediaModel>().ToList();
                        result.Read<CrimeIncidentReportMediaModel, DocReportMediaTypeModel, FileModel, CrimeIncidentReportModel, CrimeIncidentReportMediaModel>((cirm, drmt, f, cir) =>
                        {
                            CrimeIncidentReportMediaModel crimeIncidentReportMediaModel;
                            if (!lookupCrimeIncidentReportMedia.TryGetValue(cirm.CrimeIncidentReportMediaId, out crimeIncidentReportMediaModel))
                                lookupCrimeIncidentReportMedia.Add(cirm.CrimeIncidentReportMediaId, crimeIncidentReportMediaModel = cirm);
                            crimeIncidentReportMediaModel.DocReportMediaType = drmt;
                            crimeIncidentReportMediaModel.File = f;
                            crimeIncidentReportMediaModel.CrimeIncidentReport = cir;
                            return crimeIncidentReportMediaModel;
                        }, splitOn: "CrimeIncidentReportMediaId,DocReportMediaTypeId,FileId,CrimeIncidentReportId").ToList();
                        if (model.CrimeIncidentReportMedia == null)
                            model.CrimeIncidentReportMedia = new List<CrimeIncidentReportMediaModel>();
                        model.CrimeIncidentReportMedia.AddRange(lookupCrimeIncidentReportMedia.Values);
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

        public override List<CrimeIncidentReportModel> GetAll() => throw new NotImplementedException();

        public List<CrimeIncidentReportModel> GetPage(string Search,
                                               bool IsAdvanceSearchMode,
                                               long ApprovalStatusId,
                                               string CrimeIncidentReportId,
                                               string CrimeIncidentCategoryName,
                                               string PostedByFullName,
                                               DateTime DateReportedFrom,
                                               DateTime DateReportedTo,
                                               DateTime PossibleDateFrom,
                                               DateTime PossibleDateTo,
                                               string PossibleTimeFrom,
                                               string PossibleTimeTo,
                                               string Description,
                                               string GeoStreet,
                                               string GeoDistrict,
                                               string GeoCityMun,
                                               string GeoProvince,
                                               string GeoCountry,
                                               int PageNo,
                                               int PageSize,
                                               string OrderColumn,
                                               string OrderDir)
        {
            var results = new List<CrimeIncidentReportModel>();
            try
            {
                var lookup = new Dictionary<string, CrimeIncidentReportModel>();

                _dBConnection.Query("usp_crimeincidentreport_getPaged",
                new[]
                {
                    typeof(CrimeIncidentReportModel),
                    typeof(CrimeIncidentCategoryModel),
                    typeof(SystemUserModel),
                    typeof(LegalEntityModel),
                    typeof(EntityApprovalStatusModel),
                    typeof(PageResultsModel),
                }, obj =>
                {
                    CrimeIncidentReportModel cir = obj[0] as CrimeIncidentReportModel;
                    CrimeIncidentCategoryModel cic = obj[1] as CrimeIncidentCategoryModel;
                    SystemUserModel p = obj[2] as SystemUserModel;
                    LegalEntityModel le = obj[3] as LegalEntityModel;
                    EntityApprovalStatusModel eas = obj[4] as EntityApprovalStatusModel;
                    PageResultsModel pr = obj[5] as PageResultsModel;
                    CrimeIncidentReportModel model;
                    if (!lookup.TryGetValue(cir.CrimeIncidentReportId, out model))
                        lookup.Add(cir.CrimeIncidentReportId, model = cir);

                    if (model.CrimeIncidentCategory == null)
                        model.CrimeIncidentCategory = new CrimeIncidentCategoryModel();
                    if (model.PostedBySystemUser == null)
                        model.PostedBySystemUser = new SystemUserModel();
                    if (model.PostedBySystemUser.LegalEntity == null)
                        model.PostedBySystemUser.LegalEntity = new LegalEntityModel();
                    if (model.ApprovalStatus == null)
                        model.ApprovalStatus = new EntityApprovalStatusModel();
                    if (model.PageResult == null)
                        model.PageResult = new PageResultsModel();
                    model.CrimeIncidentCategory = cic;
                    model.PostedBySystemUser = p;
                    model.PostedBySystemUser.LegalEntity = le;
                    model.ApprovalStatus = eas;
                    model.PageResult = pr;
                    return model;
                },
                new
                {
                    Search = Search,
                    IsAdvanceSearchMode = IsAdvanceSearchMode,
                    ApprovalStatusId = ApprovalStatusId,
                    CrimeIncidentReportId = CrimeIncidentReportId,
                    CrimeIncidentCategoryName = CrimeIncidentCategoryName,
                    PostedByFullName = PostedByFullName,
                    DateReportedFrom = DateReportedFrom,
                    DateReportedTo = DateReportedTo,
                    PossibleDateFrom = PossibleDateFrom,
                    PossibleDateTo = PossibleDateTo,
                    PossibleTimeFrom = PossibleTimeFrom,
                    PossibleTimeTo = PossibleTimeTo,
                    Description = Description,
                    GeoStreet = GeoStreet,
                    GeoDistrict = GeoDistrict,
                    GeoCityMun = GeoCityMun,
                    GeoProvince = GeoProvince,
                    GeoCountry = GeoCountry,
                    PageNo = PageNo,
                    PageSize = PageSize,
                    OrderColumn = OrderColumn,
                    OrderDir = OrderDir
                }, splitOn: "CrimeIncidentReportId,CrimeIncidentCategoryId,SystemUserId,LegalEntityId,ApprovalStatusId,TotalRows", commandType: CommandType.StoredProcedure).ToList();
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

        public List<CrimeIncidentReportModel> GetByTracker(string TrackerRadiusInKM,
                                               string TrackerPointLatitude,
                                               string TrackerPointLongitude,
                                               long ApprovalStatusId,
                                               string CrimeIncidentCategoryIds,
                                               DateTime DateReportedFrom,
                                               DateTime DateReportedTo,
                                               DateTime PossibleDateFrom,
                                               DateTime PossibleDateTo,
                                               string PossibleTimeFrom,
                                               string PossibleTimeTo)
        {
            var results = new List<CrimeIncidentReportModel>();
            try
            {
                var lookup = new Dictionary<string, CrimeIncidentReportModel>();

                _dBConnection.Query("usp_crimeincidentreport_getByTracker",
                new[]
                {
                    typeof(CrimeIncidentReportModel),
                    typeof(CrimeIncidentCategoryModel),
                    typeof(SystemUserModel),
                    typeof(LegalEntityModel),
                    typeof(EntityApprovalStatusModel),
                    typeof(PageResultsModel),
                }, obj =>
                {
                    CrimeIncidentReportModel cir = obj[0] as CrimeIncidentReportModel;
                    CrimeIncidentCategoryModel cic = obj[1] as CrimeIncidentCategoryModel;
                    SystemUserModel p = obj[2] as SystemUserModel;
                    LegalEntityModel le = obj[3] as LegalEntityModel;
                    EntityApprovalStatusModel eas = obj[4] as EntityApprovalStatusModel;
                    PageResultsModel pr = obj[5] as PageResultsModel;
                    CrimeIncidentReportModel model;
                    if (!lookup.TryGetValue(cir.CrimeIncidentReportId, out model))
                        lookup.Add(cir.CrimeIncidentReportId, model = cir);

                    if (model.CrimeIncidentCategory == null)
                        model.CrimeIncidentCategory = new CrimeIncidentCategoryModel();
                    if (model.PostedBySystemUser == null)
                        model.PostedBySystemUser = new SystemUserModel();
                    if (model.PostedBySystemUser.LegalEntity == null)
                        model.PostedBySystemUser.LegalEntity = new LegalEntityModel();
                    if (model.ApprovalStatus == null)
                        model.ApprovalStatus = new EntityApprovalStatusModel();
                    if (model.PageResult == null)
                        model.PageResult = new PageResultsModel();
                    model.CrimeIncidentCategory = cic;
                    model.PostedBySystemUser = p;
                    model.PostedBySystemUser.LegalEntity = le;
                    model.ApprovalStatus = eas;
                    model.PageResult = pr;
                    return model;
                },
                new
                {
                    TrackerRadiusInKM = TrackerRadiusInKM,
                    TrackerPointLatitude = TrackerPointLatitude,
                    TrackerPointLongitude = TrackerPointLongitude,
                    ApprovalStatusId = ApprovalStatusId,
                    CrimeIncidentCategoryIds = CrimeIncidentCategoryIds,
                    DateReportedFrom = DateReportedFrom,
                    DateReportedTo = DateReportedTo,
                    PossibleDateFrom = PossibleDateFrom,
                    PossibleDateTo = PossibleDateTo,
                    PossibleTimeFrom = PossibleTimeFrom,
                    PossibleTimeTo = PossibleTimeTo,
                }, splitOn: "CrimeIncidentReportId,CrimeIncidentCategoryId,SystemUserId,LegalEntityId,ApprovalStatusId,TotalRows", commandType: CommandType.StoredProcedure).ToList();
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
        public List<CrimeIncidentReportModel> GetPageByPostedBySystemUserId(string PostedBySystemUserId, int PageNo, int PageSize)
        {
            var results = new List<CrimeIncidentReportModel>();
            try
            {
                var lookup = new Dictionary<string, CrimeIncidentReportModel>();

                _dBConnection.Query("usp_crimeincidentreport_getPagedByPostedBySystemUserId",
                new[]
                {
                    typeof(CrimeIncidentReportModel),
                    typeof(CrimeIncidentCategoryModel),
                    typeof(SystemUserModel),
                    typeof(FileModel),
                    typeof(LegalEntityModel),
                    typeof(EntityApprovalStatusModel),
                    typeof(PageResultsModel),
                }, obj =>
                {
                    CrimeIncidentReportModel cir = obj[0] as CrimeIncidentReportModel;
                    CrimeIncidentCategoryModel cic = obj[1] as CrimeIncidentCategoryModel;
                    SystemUserModel p = obj[2] as SystemUserModel;
                    FileModel suf = obj[3] as FileModel;
                    LegalEntityModel le = obj[4] as LegalEntityModel;
                    EntityApprovalStatusModel eas = obj[5] as EntityApprovalStatusModel;
                    PageResultsModel pr = obj[6] as PageResultsModel;
                    CrimeIncidentReportModel model;
                    if (!lookup.TryGetValue(cir.CrimeIncidentReportId, out model))
                        lookup.Add(cir.CrimeIncidentReportId, model = cir);

                    if (model.CrimeIncidentCategory == null)
                        model.CrimeIncidentCategory = new CrimeIncidentCategoryModel();
                    if (model.PostedBySystemUser == null)
                        model.PostedBySystemUser = new SystemUserModel();
                    if (model.PostedBySystemUser.ProfilePicture == null)
                        model.PostedBySystemUser.ProfilePicture = new FileModel();
                    if (model.PostedBySystemUser.LegalEntity == null)
                        model.PostedBySystemUser.LegalEntity = new LegalEntityModel();
                    if (model.ApprovalStatus == null)
                        model.ApprovalStatus = new EntityApprovalStatusModel();
                    if (model.PageResult == null)
                        model.PageResult = new PageResultsModel();
                    model.CrimeIncidentCategory = cic;
                    model.PostedBySystemUser = p;
                    model.PostedBySystemUser.ProfilePicture = suf;
                    model.PostedBySystemUser.LegalEntity = le;
                    model.ApprovalStatus = eas;
                    model.PageResult = pr;
                    return model;
                },
                new
                {
                    PostedBySystemUserId = PostedBySystemUserId,
                    PageNo = PageNo,
                    PageSize = PageSize
                }, splitOn: "CrimeIncidentReportId,CrimeIncidentCategoryId,SystemUserId,FileId,LegalEntityId,ApprovalStatusId,TotalRows", commandType: CommandType.StoredProcedure).ToList();
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
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_crimeincidentreport_delete", new
                {
                    CrimeIncidentReportId = id,
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

        public override bool Update(CrimeIncidentReportModel model) => throw new NotImplementedException();

        public bool Update(CrimeIncidentReportModel model, string LastUpdatedBy)
        {
            bool success = false;
            try
            {
                int affectedRows = 0;
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_crimeincidentreport_update", new
                {
                    model.CrimeIncidentReportId,
                    model.CrimeIncidentCategory.CrimeIncidentCategoryId,
                    model.PossibleDate,
                    model.PossibleTime,
                    model.Description,
                    model.GeoStreet,
                    model.GeoDistrict,
                    model.GeoCityMun,
                    model.GeoProvince,
                    model.GeoCountry,
                    model.GeoTrackerLatitude,
                    model.GeoTrackerLongitude,
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

        public bool UpdateStatus(CrimeIncidentReportModel model, string LastUpdatedBy)
        {
            bool success = false;
            try
            {
                int affectedRows = 0;
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_crimeincidentreport_updateStatus", new
                {
                    model.CrimeIncidentReportId,
                    model.ApprovalStatus.ApprovalStatusId,
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
    }
}
