using System;
using System.Collections.Generic;
using System.Text;

namespace SilupostMobileApp.Common.Interface
{
    public interface ICoreFunctions
    {
        void ShowMessage(string message);
        void StartService();
        void StopService();
    }
}
