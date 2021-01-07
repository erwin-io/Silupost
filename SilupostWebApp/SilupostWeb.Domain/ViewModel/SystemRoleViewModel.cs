using System;

namespace POSWeb.POSAdmin.Domain.ViewModel
{
    public class SystemRoleViewModel
    {
        public string SystemRoleId { get; set; }
        public string Name { get; set; }
        public LocationViewModel Location { get; set; }
        public SystemRecordManagerViewModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerViewModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
