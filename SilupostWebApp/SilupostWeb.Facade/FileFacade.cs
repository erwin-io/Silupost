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

namespace SilupostWeb.Facade
{
    public class FileFacade : IFileFacade
    {
        private readonly IFileRepositoryRepositoryDAC _fileRepositoryRepositoryDAC;

        #region CONSTRUCTORS
        public FileFacade(IFileRepositoryRepositoryDAC fileRepositoryRepositoryDAC)
        {
            _fileRepositoryRepositoryDAC = fileRepositoryRepositoryDAC ?? throw new ArgumentNullException(nameof(fileRepositoryRepositoryDAC));
        }
        #endregion

        public FileViewModel Find(string id) => AutoMapperHelper<FileModel, FileViewModel>.Map(_fileRepositoryRepositoryDAC.Find(id));
    }
}
