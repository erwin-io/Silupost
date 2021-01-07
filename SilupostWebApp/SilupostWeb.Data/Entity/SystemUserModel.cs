using System;
using System.Collections.Generic;

namespace POSWeb.POSAdmin.Data.Entity
{
    public class SystemUserModel
    {
        public string SystemUserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public EntityInformationModel EntityInformation { get; set; }
        public LocationModel Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
