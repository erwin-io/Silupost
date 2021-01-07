using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SilupostWeb.Domain.ViewModel
{
    public class SystemUserRolesViewModel
    {
        public string SystemUserRoleId { get; set; }
        public SystemRoleViewModel SystemRole { get; set; }
        public SystemUserViewModel SystemUser { get; set; }
        public LocationViewModel Location { get; set; }
        public SystemRecordManagerViewModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerViewModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
