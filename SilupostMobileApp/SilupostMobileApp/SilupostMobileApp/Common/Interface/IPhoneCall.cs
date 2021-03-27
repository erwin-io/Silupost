using SilupostMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SilupostMobileApp.Common.Interface
{
    public interface IPhoneCall
    {
        Task Call(string ContactNumber);
        Task<IList<PhoneCallLogsModel>> GetCallLogsByNumber(string contactNumber);
        Task<bool> ClearCallLogByNumber(string contactNumber);
    }
}
