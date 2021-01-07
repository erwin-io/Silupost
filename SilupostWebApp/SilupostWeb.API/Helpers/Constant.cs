﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSWeb.POS.API.Helpers
{
    public static class Messages
    {
        public const string Created = "Successfully created.";
        public const string Updated = "Successfully updated.";
        public const string Removed = "Successfully removed.";
        public const string NoRecord = "No record found.";
        public const string NoRecords = "No records found.";
        public const string Failed = "Tansaction failed. {0}";
        public const string NoUpdate = "No records updated.";
        public const string ServerError = "Problem occurs while proccessing your request. Please contact Developer.";

        public const string InvalidId = "Invalid {0} id.";
        public const string Error = "Error";
        public const string SyncSuccess = "Data has been successfully synced.";
        public const string SyncError = "An error occurred during syncing.";
    }
}