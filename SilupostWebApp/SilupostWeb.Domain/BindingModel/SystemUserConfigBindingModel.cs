using System;
using System.Collections.Generic;

namespace SilupostWeb.Domain.BindingModel
{
    public class UpdateSystemUserConfigBindingModel
    {
        public string SystemUserConfigId { get; set; }
        public bool IsUserEnable { get; set; }
        public bool IsUserAllowToPostNextReport { get; set; }
        public bool IsNextReportPublic { get; set; }
        public bool IsAnonymousNextReport { get; set; }
        public bool AllowReviewActionNextPost { get; set; }
        public bool AllowReviewCommentNextPost { get; set; }
        public bool IsAllReportPublic { get; set; }
        public bool IsAnonymousAllReport { get; set; }
        public bool AllowReviewActionAllReport { get; set; }
        public bool AllowReviewCommentAllReport { get; set; }
    }
}
