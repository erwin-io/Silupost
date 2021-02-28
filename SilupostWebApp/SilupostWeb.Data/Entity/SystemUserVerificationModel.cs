using System;
using System.Collections.Generic;

namespace SilupostWeb.Data.Entity
{
    public class SystemUserVerificationModel
    {
        public long Id { get; set; }
        public string VerificationSender { get; set; }
        public string VerificationTypeId { get; set; }
        public string VerificationCode { get; set; }
        public EntityStatusModel EntityStatus { get; set; }
    }
}
