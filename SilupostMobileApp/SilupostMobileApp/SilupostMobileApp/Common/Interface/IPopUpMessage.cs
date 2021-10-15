using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SilupostMobileApp.Common.Interface
{
    public interface IPopUpMessage
    {
        void ShowToast(string message);
        void ShowSnackbar(string message);
    }
}