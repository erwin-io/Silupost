using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Domain.ViewModel
{
    public class CrimeIncidentReportViewModel
    {
        public string CrimeIncidentReportId { get; set; }
        public CrimeIncidentCategoryViewModel CrimeIncidentCategory { get; set; }
        public SystemUserViewModel PostedBySystemUser { get; set; }
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
        public List<CrimeIncidentReportMediaViewModel> CrimeIncidentReportMedia { get; set; }
        public EntityApprovalStatusViewModel ApprovalStatus { get; set; }
        public bool IsReviewActionEnable { get; set; }
        public bool IsReviewCommentEnable { get; set; }
        public SystemRecordManagerViewModel SystemRecordManager { get; set; }
        public EntityStatusViewModel EntityStatus { get; set; }
    }
}
