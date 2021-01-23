using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Facade.Interface
{
    public interface ILookupFacade
    {
        List<LookupTableViewModel> FindLookupByTableNames(string TableNames);
    }
}
