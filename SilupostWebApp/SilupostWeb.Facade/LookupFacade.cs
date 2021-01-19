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
    public class LookupFacade : ILookupFacade
    {
        private readonly ILookupTableRepositoryDAC _lookupTableRepositoryDAC;

        #region CONSTRUCTORS
        public LookupFacade(ILookupTableRepositoryDAC lookupTableRepositoryDAC)
        {
            _lookupTableRepositoryDAC = lookupTableRepositoryDAC ?? throw new ArgumentNullException(nameof(lookupTableRepositoryDAC));
        }
        #endregion

        public List<LookupTableViewModel> FindLookupByTableNames(string TableNames) => AutoMapperHelper<LookupTableModel, LookupTableViewModel>.MapList(_lookupTableRepositoryDAC.FindLookupByTableNames(TableNames)).ToList();
    }
}
