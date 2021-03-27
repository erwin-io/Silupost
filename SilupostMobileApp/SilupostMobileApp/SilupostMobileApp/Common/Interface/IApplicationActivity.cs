using System;
using System.Collections.Generic;
using System.Text;

namespace SilupostMobileApp.Common.Interface
{
    public interface IApplicationActivity
    {
        void CloseApplication();
        void HideStatusBar();
        void ShowStatusBar();
    }
}
