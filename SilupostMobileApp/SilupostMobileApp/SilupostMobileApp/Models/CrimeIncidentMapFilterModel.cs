using System;
using System.Collections.Generic;

namespace SilupostMobileApp.Models
{
    public class CrimeIncidentMapFilterModel
    {
        public List<CrimeIncidentCategoryModel> SelectedCrimeIncidentCategory { get; set; }
        public string CrimeIncidentCategoryIds { get; set; }
        public double TrackerRadiusInKM { get; set; }
        public DateTime DateReportedFrom { get; set; }
        public DateTime DateReportedTo { get; set; }
        public DateTime PossibleDateFrom { get; set; }
        public DateTime PossibleDateTo { get; set; }
        public string PossibleTimeFrom { get; set; }
        public string PossibleTimeTo { get; set; }
    }
}
