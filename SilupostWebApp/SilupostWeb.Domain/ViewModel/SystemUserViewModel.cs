using System;
namespace POSWeb.POSAdmin.Domain.ViewModel
{
    public class SystemUserViewModel
    {
        public string SystemUserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public EntityInformationViewModel EntityInformation { get; set; }
        public LocationViewModel Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public TokenViewModel Token { get; set; }
    }
}
