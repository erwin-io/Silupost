
using SilupostWeb.Mapping;
using SilupostWeb.Data.Entity;
using SilupostWeb.Data.Interface;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using SilupostWeb.Facade.Interface;
using System;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;
using System.IO;

namespace SilupostWeb.Facade
{
    public class CrimeIncidentReportFacade : ICrimeIncidentReportFacade
    {
        private readonly ICrimeIncidentReportRepositoryDAC _crimeIncidentReportRepositoryDAC;
        private readonly ICrimeIncidentReportMediaRepositoryDAC _crimeIncidentReportMediaRepositoryDAC;
        private readonly IFileRepositoryRepositoryDAC _fileRepositoryDAC;

        #region CONSTRUCTORS
        public CrimeIncidentReportFacade(ICrimeIncidentReportRepositoryDAC crimeIncidentReportRepositoryDAC, ICrimeIncidentReportMediaRepositoryDAC crimeIncidentReportMediaRepositoryDAC, IFileRepositoryRepositoryDAC fileRepositoryDAC)
        {
            _crimeIncidentReportRepositoryDAC = crimeIncidentReportRepositoryDAC ?? throw new ArgumentNullException(nameof(crimeIncidentReportRepositoryDAC));
            _crimeIncidentReportMediaRepositoryDAC = crimeIncidentReportMediaRepositoryDAC ?? throw new ArgumentNullException(nameof(crimeIncidentReportMediaRepositoryDAC));
            _fileRepositoryDAC = fileRepositoryDAC ?? throw new ArgumentNullException(nameof(fileRepositoryDAC));
        }
        #endregion
        public string Add(CreateCrimeIncidentReportBindingModel model, string CreatedBy)
        {
            try
            {
                var id = string.Empty;
                using (var scope = new TransactionScope())
                {
                    var addReportModel = AutoMapperHelper<CreateCrimeIncidentReportBindingModel, CrimeIncidentReportModel>.Map(model);
                    addReportModel.SystemRecordManager.CreatedBy = CreatedBy;
                    id = _crimeIncidentReportRepositoryDAC.Add(addReportModel);
                    if (string.IsNullOrEmpty(id))
                        throw new Exception("Error Saving Crime Incident Report");
                    if (model.CrimeIncidentReportMedia == null)
                        model.CrimeIncidentReportMedia = new List<NewCrimeIncidentReportMediaBindingModel>();
                    foreach (var media in model.CrimeIncidentReportMedia)
                    {
                        //Start Saving file
                        var addMedia = AutoMapperHelper<NewCrimeIncidentReportMediaBindingModel, CrimeIncidentReportMediaModel>.Map(media);
                        addMedia.CrimeIncidentReport = addReportModel;
                        addMedia.CrimeIncidentReport.CrimeIncidentReportId = id;
                        addMedia.File.SystemRecordManager = addReportModel.SystemRecordManager;
                        var fileId = _fileRepositoryDAC.Add(addMedia.File);
                        if (string.IsNullOrEmpty(fileId))
                            throw new Exception("Error Saving Crime Incident Report Media File");

                        //End Saving file
                        //start store file directory
                        if (File.Exists(media.File.FileName))
                        {
                            File.Delete(media.File.FileName);
                        }
                        using (var fs = new FileStream(media.File.FileName, FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(media.File.FileContent, 0, media.File.FileContent.Length);
                        }
                        //end store file directory
                        addMedia.File.FileId = fileId;

                        var crimeIncidentReportMediaId = _crimeIncidentReportMediaRepositoryDAC.Add(addMedia);
                        if (string.IsNullOrEmpty(crimeIncidentReportMediaId))
                        {
                            throw new Exception("Error Saving Crime Incident Report Media");
                        }
                    }
                    scope.Complete();
                }
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CrimeIncidentReportViewModel Find(string id)
        {
            var result = AutoMapperHelper<CrimeIncidentReportModel, CrimeIncidentReportViewModel>.Map(_crimeIncidentReportRepositoryDAC.Find(id));
            foreach(var media in result.CrimeIncidentReportMedia)
            {
                if (media.File != null && File.Exists(media.File.FileName))
                    media.File.FileContent = System.IO.File.ReadAllBytes(media.File.FileName);
            }
            return result;
        }

        public CrimeIncidentReportViewModel Find(string id, bool GetMediaFiles)
        {
            var result = AutoMapperHelper<CrimeIncidentReportModel, CrimeIncidentReportViewModel>.Map(_crimeIncidentReportRepositoryDAC.Find(id));
            if (GetMediaFiles)
            {
                foreach (var media in result.CrimeIncidentReportMedia)
                {
                    if (media.File != null && File.Exists(media.File.FileName))
                        media.File.FileContent = System.IO.File.ReadAllBytes(media.File.FileName);
                }
            }
            return result;
        }

        public PageResultsViewModel<CrimeIncidentReportViewModel> GetPage(string Search,
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
            var result = new PageResultsViewModel<CrimeIncidentReportViewModel>();
            var data = _crimeIncidentReportRepositoryDAC.GetPage(Search, 
                                                                 IsAdvanceSearchMode,
                                                                 ApprovalStatusId,
                                                                 CrimeIncidentReportId,
                                                                 CrimeIncidentCategoryName,
                                                                 PostedByFullName,
                                                                 DateReportedFrom,
                                                                 DateReportedTo,
                                                                 PossibleDateFrom,
                                                                 PossibleDateTo,
                                                                 PossibleTimeFrom,
                                                                 PossibleTimeTo,
                                                                 Description,
                                                                 GeoStreet,
                                                                 GeoDistrict,
                                                                 GeoCityMun,
                                                                 GeoProvince,
                                                                 GeoCountry,
                                                                 PageNo, 
                                                                 PageSize, 
                                                                 OrderColumn, 
                                                                 OrderDir);
            result.Items = AutoMapperHelper<CrimeIncidentReportModel, CrimeIncidentReportViewModel>.MapList(data);
            result.TotalRows = data.Count > 0 ? data.FirstOrDefault().PageResult.TotalRows: 0;
            return result;
        } 

        public PageResultsViewModel<CrimeIncidentReportViewModel> GetPageByPostedBySystemUserId(string PostedBySystemUserId, int PageNo, int PageSize)
        {
            var result = new PageResultsViewModel<CrimeIncidentReportViewModel>();
            var data = _crimeIncidentReportRepositoryDAC.GetPageByPostedBySystemUserId(PostedBySystemUserId, PageNo, PageSize);
            result.Items = AutoMapperHelper<CrimeIncidentReportModel, CrimeIncidentReportViewModel>.MapList(data);
            foreach (var item in result.Items)
            {
                if (item.PostedBySystemUser.ProfilePicture != null && File.Exists(item.PostedBySystemUser.ProfilePicture.FileName))
                    item.PostedBySystemUser.ProfilePicture.FileContent = System.IO.File.ReadAllBytes(item.PostedBySystemUser.ProfilePicture.FileName);
            }
            result.TotalRows = data.Count > 0 ? data.FirstOrDefault().PageResult.TotalRows : 0;
            return result;
        }
        public bool Remove(string id, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                success = _crimeIncidentReportRepositoryDAC.Remove(id, LastUpdatedBy);
                if(success)
                    scope.Complete();
            }
            return success;
        }

        public bool Update(UpdateCrimeIncidentReportBindingModel model, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                var updateModel = AutoMapperHelper<UpdateCrimeIncidentReportBindingModel, CrimeIncidentReportModel>.Map(model);
                updateModel.SystemRecordManager.LastUpdatedBy = LastUpdatedBy;
                success = _crimeIncidentReportRepositoryDAC.Update(updateModel, LastUpdatedBy);
                if (success)
                    scope.Complete();
            }
            return success;
        }

        public bool UpdateStatus(UpdateCrimeIncidentReportStatusBindingModel model, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                var updateModel = AutoMapperHelper<UpdateCrimeIncidentReportStatusBindingModel, CrimeIncidentReportModel>.Map(model);
                updateModel.SystemRecordManager.LastUpdatedBy = LastUpdatedBy;
                success = _crimeIncidentReportRepositoryDAC.UpdateStatus(updateModel, LastUpdatedBy);
                if (success)
                    scope.Complete();
            }
            return success;
        }
    }
}
