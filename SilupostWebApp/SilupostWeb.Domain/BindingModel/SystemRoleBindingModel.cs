using System.Collections.Generic;

namespace SilupostWeb.Domain.BindingModel
{
    public class SystemRoleBindingModel
    {
        public string Name { get; set; }
    }
    public class UpdateSystemRoleBindingModel
    {
        public string SystemRoleId { get; set; }
        public string Name { get; set; }
    }
}
