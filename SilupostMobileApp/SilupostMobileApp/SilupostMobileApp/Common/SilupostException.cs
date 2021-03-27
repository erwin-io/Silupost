using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SilupostMobileApp.Common
{
    public class SilupostServiceException : Exception
    {
        public string ExceptionMessage { get; set; }
        public SilupostServiceExceptionTypeEnums Type { get; set; }
        public SilupostServiceException(string exception)
        {

            if (exception.Contains("No record"))
            {
                ExceptionMessage = SilupostMessage.NO_RECORDS_FOUND;
                Type = SilupostServiceExceptionTypeEnums.NOT_FOUND;
            }
            else if(exception.Contains("Problem occurs"))
            {
                ExceptionMessage = SilupostMessage.SERVER_ERROR;
                Type = SilupostServiceExceptionTypeEnums.SERVER_ERROR;
            }
            else
            {
                ExceptionMessage = SilupostMessage.APP_ERROR;
                Type = SilupostServiceExceptionTypeEnums.APP_ERROR;
            }
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
            //CrossToastPopUp.Current.ShowToastMessage(string.Format("Error at line {0}, Message: {1}", line, exception.Message));
            CrossToastPopUp.Current.ShowToastMessage(exception.Message);
        }
        public static void GetError(Exception exception, string CustomMessage)
        {
            // Get stack trace for the exception with source file information
            var st = new StackTrace(exception, true);
            // Get the top stack frame
            var frame = st.GetFrame(0);
            // Get the line number from the stack frame
            var line = frame.GetFileLineNumber();
            //CrossToastPopUp.Current.ShowToastMessage(string.Format("Error at line {0}, Message: {1}", line, CustomMessage));
            CrossToastPopUp.Current.ShowToastMessage(CustomMessage);
        }
    }
}
