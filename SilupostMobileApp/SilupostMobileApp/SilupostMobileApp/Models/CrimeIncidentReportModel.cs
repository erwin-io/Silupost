﻿using System;
using System.Collections.Generic;

namespace SilupostMobileApp.Models
{
    public class CrimeIncidentReportModel
    {
        public string CrimeIncidentReportId { get; set; }
        public CrimeIncidentCategoryModel CrimeIncidentCategory { get; set; }
        public SystemUserModel PostedBySystemUser { get; set; }
        public DateTime DateReported { get; set; }
        public DateTime PossibleDate { get; set; }
        public string PossibleTime { get; set; }
        public TimeSpan PossibleTimeSpan
        {
            get
            {
                //return DateTime.Parse(string.Format("{0} {1}", PossibleDate.ToString("MM/dd/yyyy"), PossibleTime.Remove(5).ToString())).TimeOfDay;
                //return DateTime.ParseExact(PossibleTime.ToString().Remove(5).ToString(), "hh:mm", null, System.Globalization.DateTimeStyles.None).TimeOfDay;
                var timeArray = PossibleTime.ToString().Split(new string[] { ":", " ", "." }, StringSplitOptions.RemoveEmptyEntries);
                return DateTime.ParseExact(string.Format("{0}:{1}", timeArray[0].PadLeft(2, '0'), timeArray[1].PadLeft(2, '0')), "hh:mmtt", null, System.Globalization.DateTimeStyles.None).TimeOfDay;
            }
            set
            {
                PossibleTime = value.ToString().Remove(5);
            }
        }
        public string Description { get; set; }
        public string GeoAddress { get; set; }
        public string GeoStreet { get; set; }
        public string GeoDistrict { get; set; }
        public string GeoCityMun { get; set; }
        public string GeoProvince { get; set; }
        public string GeoCountry { get; set; }
        public float GeoTrackerLatitude { get; set; }
        public float GeoTrackerLongitude { get; set; }
        public EntityApprovalStatusModel ApprovalStatus { get; set; }
        public bool IsReviewActionEnable { get; set; }
        public bool IsReviewCommentEnable { get; set; }
        public bool IsReportedAsanonymous { get; set; }
        public List<CrimeIncidentReportMediaModel> CrimeIncidentReportMedia { get; set; }
        public SystemRecordManagerModel SystemRecordManager { get; set; }
        public EntityStatusModel EntityStatus { get; set; }
    }
}
