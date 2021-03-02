using System;
using System.Collections.Generic;

namespace SilupostWeb.Domain.BindingModel
{
    public class CrimeIncidentReportBindingModel
    {
        public string CrimeIncidentCategoryId { get; set; }
        public DateTime PossibleDate { get; set; }
        public string PossibleTime { get; set; }
        public string Description { get; set; }
        public string GeoStreet { get; set; }
        public string GeoDistrict { get; set; }
        public string GeoCityMun { get; set; }
        public string GeoProvince { get; set; }
        public string GeoCountry { get; set; }
        public string GeoTrackerLatitude { get; set; }
        public string GeoTrackerLongitude { get; set; }
        public bool IsReviewActionEnable { get; set; }
        public bool IsReviewCommentEnable { get; set; }
    }
    public class CreateCrimeIncidentReportBindingModel : CrimeIncidentReportBindingModel
    {
        public string PostedBySystemUserId { get; set; }
        public DateTime DateReported { get; set; }
        public List<NewCrimeIncidentReportMediaBindingModel> CrimeIncidentReportMedia { get; set; }
    }
    public class UpdateCrimeIncidentReportBindingModel : CrimeIncidentReportBindingModel
    {
        public string CrimeIncidentReportId { get; set; }
    }
    public class UpdateCrimeIncidentReportStatusBindingModel
    {
        public string CrimeIncidentReportId { get; set; }
        public long? ApprovalStatusId { get; set; }
    }
}
