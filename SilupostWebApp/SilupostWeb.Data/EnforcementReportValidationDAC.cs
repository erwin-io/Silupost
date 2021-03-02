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
    public class EnforcementReportValidationDAC : RepositoryBase<EnforcementReportValidationModel>, IEnforcementReportValidationRepositoryDAC
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public EnforcementReportValidationDAC(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public override string Add(EnforcementReportValidationModel model)
        {
            try
            {
                var id = Convert.ToString(_dBConnection.ExecuteScalar("usp_enforcementreportvalidation_add", new
                {
                    model.CrimeIncidentReport.CrimeIncidentReportId,
                    model.EnforcementUnit.EnforcementUnitId,
                    model.ReportNotes,
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

        public override EnforcementReportValidationModel Find(string id)
        {
            try
            {
                var lookupCrimeIncidentReportMedia = new Dictionary<string, CrimeIncidentReportMediaModel>();
                using (var result = _dBConnection.QueryMultiple("usp_enforcementreportvalidation_getByID", new
                {
                    EnforcementReportValidationId = id,
                }, commandType: CommandType.StoredProcedure))
                {
                    var model = result.Read<EnforcementReportValidationModel>().FirstOrDefault();
                    if(model != null)
                    {
                        model.EnforcementUnit = result.Read<EnforcementUnitModel>().FirstOrDefault();
                        if (model.EnforcementUnit != null)
                        {
                            model.EnforcementUnit.EnforcementType = result.Read<EnforcementTypeModel>().FirstOrDefault();
                            model.EnforcementUnit.EnforcementStation = result.Read<EnforcementStationModel>().FirstOrDefault();
                            model.EnforcementUnit.ProfilePicture = result.Read<FileModel>().FirstOrDefault();
                            model.EnforcementUnit.LegalEntity = result.Read<LegalEntityModel>().FirstOrDefault();
                            model.EnforcementUnit.LegalEntity.Gender = result.Read<EntityGenderModel>().FirstOrDefault();
                            model.EnforcementUnit.SystemRecordManager = result.Read<SystemRecordManagerModel>().FirstOrDefault();
                            model.EnforcementUnit.EntityStatus = result.Read<EntityStatusModel>().FirstOrDefault();
                        }
                        model.CrimeIncidentReport = result.Read<CrimeIncidentReportModel>().FirstOrDefault();
                        if(model.CrimeIncidentReport != null)
                        {
                            model.CrimeIncidentReport.CrimeIncidentCategory = result.Read<CrimeIncidentCategoryModel>().FirstOrDefault();
                            model.CrimeIncidentReport.CrimeIncidentCategory.CrimeIncidentType = result.Read<CrimeIncidentTypeModel>().FirstOrDefault();
                            model.CrimeIncidentReport.PostedBySystemUser = result.Read<SystemUserModel>().FirstOrDefault();
                            if (model.CrimeIncidentReport.PostedBySystemUser.LegalEntity == null)
                                model.CrimeIncidentReport.PostedBySystemUser.LegalEntity = new LegalEntityModel();
                            model.CrimeIncidentReport.PostedBySystemUser.LegalEntity = result.Read<LegalEntityModel>().FirstOrDefault();
                            model.CrimeIncidentReport.ApprovalStatus = result.Read<EntityApprovalStatusModel>().FirstOrDefault();
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
                            if (model.CrimeIncidentReport.CrimeIncidentReportMedia == null)
                                model.CrimeIncidentReport.CrimeIncidentReportMedia = new List<CrimeIncidentReportMediaModel>();
                            model.CrimeIncidentReport.CrimeIncidentReportMedia.AddRange(lookupCrimeIncidentReportMedia.Values);
                            model.SystemRecordManager = result.Read<SystemRecordManagerModel>().FirstOrDefault();
                            model.EntityStatus = result.Read<EntityStatusModel>().FirstOrDefault();
                        }
                        model.ReportValidationStatus = result.Read<ReportValidationStatusModel>().FirstOrDefault();
                    }

                    return model;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<EnforcementReportValidationModel> GetAll() => throw new NotImplementedException();

        public List<EnforcementReportValidationModel> GetTablePageByCrimeIncidentReportId(string Search,
                                               bool IsAdvanceSearchMode,
                                               string CrimeIncidentReportId,
                                               string EnforcementUnitName,
                                               DateTime DateSubmittedFrom,
                                               DateTime DateSubmittedTo, 
                                               string ReportValidationStatusId,
                                               int PageNo,
                                               int PageSize,
                                               string OrderColumn,
                                               string OrderDir)
        {
            var results = new List<EnforcementReportValidationModel>();
            try
            {
                var lookup = new Dictionary<string, EnforcementReportValidationModel>();

                _dBConnection.Query("usp_enforcementreportvalidation_getPagedByCrimeIncidentReportId",
                new[]
                {
                    typeof(EnforcementReportValidationModel),
                    typeof(CrimeIncidentReportModel),
                    typeof(CrimeIncidentCategoryModel),
                    typeof(EnforcementUnitModel),
                    typeof(LegalEntityModel),
                    typeof(ReportValidationStatusModel),
                    typeof(PageResultsModel),
                }, obj =>
                {
                    EnforcementReportValidationModel erv = obj[0] as EnforcementReportValidationModel;
                    CrimeIncidentReportModel cir = obj[1] as CrimeIncidentReportModel;
                    CrimeIncidentCategoryModel cic = obj[2] as CrimeIncidentCategoryModel;
                    EnforcementUnitModel eu = obj[3] as EnforcementUnitModel;
                    LegalEntityModel eule = obj[4] as LegalEntityModel;
                    ReportValidationStatusModel rvs = obj[5] as ReportValidationStatusModel;
                    PageResultsModel pr = obj[6] as PageResultsModel;
                    EnforcementReportValidationModel model;
                    if (!lookup.TryGetValue(erv.EnforcementReportValidationId, out model))
                        lookup.Add(erv.EnforcementReportValidationId, model = erv);

                    if (model.CrimeIncidentReport == null)
                        model.CrimeIncidentReport = new CrimeIncidentReportModel();
                    if (model.CrimeIncidentReport.CrimeIncidentCategory == null)
                        model.CrimeIncidentReport.CrimeIncidentCategory = new CrimeIncidentCategoryModel();
                    if (model.EnforcementUnit == null)
                        model.EnforcementUnit = new EnforcementUnitModel();
                    if (model.EnforcementUnit.LegalEntity == null)
                        model.EnforcementUnit.LegalEntity = new LegalEntityModel();
                    if (model.ReportValidationStatus == null)
                        model.ReportValidationStatus = new ReportValidationStatusModel();
                    if (model.PageResult == null)
                        model.PageResult = new PageResultsModel();
                    model.CrimeIncidentReport = cir;
                    model.CrimeIncidentReport.CrimeIncidentCategory = cic;
                    model.EnforcementUnit = eu;
                    model.EnforcementUnit.LegalEntity = eule;
                    model.ReportValidationStatus = rvs;
                    model.PageResult = pr;
                    return model;
                },
                new
                {
                    Search = Search,
                    IsAdvanceSearchMode = IsAdvanceSearchMode,
                    CrimeIncidentReportId = CrimeIncidentReportId,
                    EnforcementUnitName = EnforcementUnitName,
                    DateSubmittedFrom = DateSubmittedFrom,
                    DateSubmittedTo = DateSubmittedTo,
                    ReportValidationStatusId = ReportValidationStatusId,
                    PageNo = PageNo,
                    PageSize = PageSize,
                    OrderColumn = OrderColumn,
                    OrderDir = OrderDir
                }, splitOn: "EnforcementReportValidationId,CrimeIncidentReportId,CrimeIncidentCategoryId,EnforcementUnitId,LegalEntityId,ReportValidationStatusId,TotalRows", commandType: CommandType.StoredProcedure).ToList();
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

        public List<EnforcementReportValidationModel> GetPageByEnforcementSystemUserId(string Search,
                                               bool IsAdvanceSearchMode,
                                               string SystemUserId,
                                               string CrimeIncidentCategoryName,
                                               DateTime DateSubmittedFrom,
                                               DateTime DateSubmittedTo,
                                               string ReportValidationStatusId,
                                               int PageNo,
                                               int PageSize,
                                               string OrderColumn,
                                               string OrderDir)
        {
            var results = new List<EnforcementReportValidationModel>();
            try
            {
                var lookup = new Dictionary<string, EnforcementReportValidationModel>();

                _dBConnection.Query("usp_enforcementreportvalidation_getPagedBySystemUserId",
                new[]
                {
                    typeof(EnforcementReportValidationModel),
                    typeof(CrimeIncidentReportModel),
                    typeof(CrimeIncidentCategoryModel),
                    typeof(ReportValidationStatusModel),
                    typeof(PageResultsModel),
                }, obj =>
                {
                    EnforcementReportValidationModel erv = obj[0] as EnforcementReportValidationModel;
                    CrimeIncidentReportModel cir = obj[1] as CrimeIncidentReportModel;
                    CrimeIncidentCategoryModel cic = obj[2] as CrimeIncidentCategoryModel;
                    ReportValidationStatusModel rvs = obj[3] as ReportValidationStatusModel;
                    PageResultsModel pr = obj[4] as PageResultsModel;
                    EnforcementReportValidationModel model;
                    if (!lookup.TryGetValue(erv.EnforcementReportValidationId, out model))
                        lookup.Add(erv.EnforcementReportValidationId, model = erv);

                    if (model.CrimeIncidentReport == null)
                        model.CrimeIncidentReport = new CrimeIncidentReportModel();
                    if (model.CrimeIncidentReport.CrimeIncidentCategory == null)
                        model.CrimeIncidentReport.CrimeIncidentCategory = new CrimeIncidentCategoryModel();
                    if (model.ReportValidationStatus == null)
                        model.ReportValidationStatus = new ReportValidationStatusModel();
                    if (model.PageResult == null)
                        model.PageResult = new PageResultsModel();
                    model.CrimeIncidentReport = cir;
                    model.CrimeIncidentReport.CrimeIncidentCategory = cic;
                    model.ReportValidationStatus = rvs;
                    model.PageResult = pr;
                    return model;
                },
                new
                {
                    Search = Search,
                    IsAdvanceSearchMode = IsAdvanceSearchMode,
                    SystemUserId = SystemUserId,
                    CrimeIncidentCategoryName = CrimeIncidentCategoryName,
                    DateSubmittedFrom = DateSubmittedFrom,
                    DateSubmittedTo = DateSubmittedTo,
                    ReportValidationStatusId = ReportValidationStatusId,
                    PageNo = PageNo,
                    PageSize = PageSize,
                    OrderColumn = OrderColumn,
                    OrderDir = OrderDir
                }, splitOn: "EnforcementReportValidationId,CrimeIncidentReportId,CrimeIncidentCategoryId,ReportValidationStatusId,TotalRows", commandType: CommandType.StoredProcedure).ToList();
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
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_enforcementreportvalidation_delete", new
                {
                    EnforcementReportValidationId = id,
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

        public override bool Update(EnforcementReportValidationModel model)
        {
            bool success = false;
            try
            {
                int affectedRows = 0;
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_enforcementreportvalidation_update", new
                {
                    model.EnforcementReportValidationId,
                    model.EnforcementUnit.EnforcementUnitId,
                    model.ReportNotes,
                    model.ValidationNotes,
                    model.ReportValidationStatus.ReportValidationStatusId,
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
