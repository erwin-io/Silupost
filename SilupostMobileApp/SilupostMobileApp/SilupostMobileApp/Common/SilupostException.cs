using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace SilupostMobileApp.Common
{
    public class SilupostServiceException : Exception
    {
        public string ExceptionMessage { get; set; }
        public SilupostServiceExceptionTypeEnums Type { get; set; }
        public SilupostServiceException(string exception)
        {
            MessagingCenter.Send(this, "Logout");
        }
    }
    public static class SilupostExceptionLogger
    {
        public static void GetError(Exception exception)
        {
            // Get stack trace for the exception with source file information
            var st = new StackTrace(exception, true);
            // Get the top stack frame
            var frame = st.GetFrame(0);
            // Get the line number from the stack frame
            var line = frame.GetFileLineNumber();
            //SilupostPopMessage.ShowToastMessage(string.Format("Error at line {0}, Message: {1}", line, exception.Message));
            if(!exception.Message.ToLower().Contains("object reference"))
                SilupostPopMessage.ShowToastMessage(exception.Message);
        }
        public static void GetError(Exception exception, string CustomMessage)
        {
            // Get stack trace for the exception with source file information
            var st = new StackTrace(exception, true);
            // Get the top stack frame
            var frame = st.GetFrame(0);
            // Get the line number from the stack frame
            var line = frame.GetFileLineNumber();
            //SilupostPopMessage.ShowToastMessage(string.Format("Error at line {0}, Message: {1}", line, CustomMessage));
            SilupostPopMessage.ShowToastMessage(CustomMessage);
        }
    }
}
