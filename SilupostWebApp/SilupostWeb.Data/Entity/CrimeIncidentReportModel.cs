using System;
using System.Collections.Generic;

namespace SilupostWeb.Data.Entity
{
    public class CrimeIncidentReportModel
    {
        public string CrimeIncidentReportId { get; set; }
        public CrimeIncidentCategoryModel CrimeIncidentCategory { get; set; }
        public SystemUserModel PostedBySystemUser { get; set; }
        public DateTime DateReported { get; set; }
        public DateTime PossibleDate { get; set; }
        public string PossibleTime { get; set; }
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
        public List<CrimeIncidentReportMediaModel> CrimeIncidentReportMedia { get; set; }
        public SystemRecordManagerModel SystemRecordManager { get; set; }
        public EntityStatusModel EntityStatus { get; set; }
        public PageResultsModel PageResult { get; set; }
    }
}
