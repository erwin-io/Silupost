
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
    public class CrimeIncidentReportMediaFacade : ICrimeIncidentReportMediaFacade
    {
        private readonly ICrimeIncidentReportMediaRepositoryDAC _crimeIncidentReportMediaRepositoryDAC;
        private readonly IFileRepositoryRepositoryDAC _fileRepositoryDAC;

        #region CONSTRUCTORS
        public CrimeIncidentReportMediaFacade(ICrimeIncidentReportMediaRepositoryDAC crimeIncidentReportMediaRepositoryDAC, IFileRepositoryRepositoryDAC fileRepositoryDAC)
        {
            _crimeIncidentReportMediaRepositoryDAC = crimeIncidentReportMediaRepositoryDAC ?? throw new ArgumentNullException(nameof(crimeIncidentReportMediaRepositoryDAC));
            _fileRepositoryDAC = fileRepositoryDAC ?? throw new ArgumentNullException(nameof(fileRepositoryDAC));
        }
        #endregion

        public string Add(CreateCrimeIncidentReportMediaBindingModel model, string CreatedBy)
        {
            try
            {
                var id = string.Empty;
                using (var scope = new TransactionScope())
                {
                    var addModel = AutoMapperHelper<CreateCrimeIncidentReportMediaBindingModel, CrimeIncidentReportMediaModel>.Map(model);
                    //Start Saving file
                    addModel.File.SystemRecordManager.CreatedBy = CreatedBy;
                    var fileId = _fileRepositoryDAC.Add(addModel.File);
                    if (string.IsNullOrEmpty(fileId))
                        throw new Exception("Error Saving Crime Incident Report Media File");
                    //End Saving file

                    //start store file directory
                    if (File.Exists(addModel.File.FileName))
                    {
                        File.Delete(addModel.File.FileName);
                    }
                    using (var fs = new FileStream(addModel.File.FileName, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(addModel.File.FileContent, 0, addModel.File.FileContent.Length);
                    }
                    //end store file directory

                    addModel.File.FileId = fileId;
                    id = _crimeIncidentReportMediaRepositoryDAC.Add(addModel);
                    if (string.IsNullOrEmpty(id))
                        throw new Exception("Error Saving Crime Incident Report Media");
                    scope.Complete();
                }
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<CrimeIncidentReportMediaViewModel> FindByCrimeIncidentReportId(string CrimeIncidentReportId) => AutoMapperHelper<CrimeIncidentReportMediaModel, CrimeIncidentReportMediaViewModel>.MapList(_crimeIncidentReportMediaRepositoryDAC.FindByCrimeIncidentReportId(CrimeIncidentReportId)).ToList();
        public List<CrimeIncidentReportMediaViewModel> FindByCrimeIncidentReportId(string CrimeIncidentReportId)
        {
            var result = AutoMapperHelper<CrimeIncidentReportMediaModel, CrimeIncidentReportMediaViewModel>.MapList(_crimeIncidentReportMediaRepositoryDAC.FindByCrimeIncidentReportId(CrimeIncidentReportId)).ToList();
            foreach(var media in result)
            {
                if(media.File != null && File.Exists(media.File.FileName))
                    media.File.FileContent = System.IO.File.ReadAllBytes(media.File.FileName);
            }
            return result;
        }
        public CrimeIncidentReportMediaViewModel Find(string id) => AutoMapperHelper<CrimeIncidentReportMediaModel, CrimeIncidentReportMediaViewModel>.Map(_crimeIncidentReportMediaRepositoryDAC.Find(id));

        public bool Remove(string id)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                success = _crimeIncidentReportMediaRepositoryDAC.Remove(id);
                if (success)
                    scope.Complete();
            }
            return success;
        }

        public bool Update(UpdateCrimeIncidentReportMediaBindingModel model, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                var updateModel = AutoMapperHelper<UpdateCrimeIncidentReportMediaBindingModel, CrimeIncidentReportMediaModel>.Map(model);
                success = _crimeIncidentReportMediaRepositoryDAC.Update(updateModel);
                if (!success)
                    throw new Exception("Error Updating Crime Incident Report Media");

                //Start Saving file
                updateModel.File.SystemRecordManager.LastUpdatedBy = LastUpdatedBy;
                success = _fileRepositoryDAC.Update(updateModel.File);
                if (!success)
                    throw new Exception("Error Updating Crime Incident Report Media File");
                //End Saving file

                //start store file directory
                if (File.Exists(updateModel.File.FileName))
                {
                    File.Delete(updateModel.File.FileName);
                }
                using (var fs = new FileStream(updateModel.File.FileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(updateModel.File.FileContent, 0, updateModel.File.FileContent.Length);
                }
                //end store file directory

                scope.Complete();
            }
            return success;
        }
    }
}
