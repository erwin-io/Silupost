using System;
using System.Collections.Generic;
using System.Text;

namespace SilupostMobileApp.BindingModels
{
    public class LegalEntityAddressBindingModel
    {
        public string Address { get; set; }
    }
    public class CreateLegalEntityAddressBindingModel : LegalEntityAddressBindingModel
    {
        public string LegalEntityId { get; set; }
    }
    public class UpdateLegalEntityAddressBindingModel : LegalEntityAddressBindingModel
    {
        public string LegalEntityAddressId { get; set; }
    }
}
