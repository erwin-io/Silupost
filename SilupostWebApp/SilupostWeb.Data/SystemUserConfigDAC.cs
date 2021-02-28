using SilupostWeb.Data.Core;
using SilupostWeb.Data.Interface;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;
using System.Data;
using System;
using Dapper;
using System.Linq;

namespace SilupostWeb.Data
{
    public class SystemUserConfigDAC : RepositoryBase<SystemUserConfigModel>, ISystemUserConfigRepositoryDAC
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public SystemUserConfigDAC(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public override string Add(SystemUserConfigModel model)
        {
            try
            {
                var id = Convert.ToString(_dBConnection.ExecuteScalar("usp_systemuserconfig_add", new
                {
                    model.SystemUser.SystemUserId,
                    model.IsUserEnable,
                    model.IsUserAllowToPostNextReport,
                    model.IsNextReportPublic,
                    model.IsAnonymousNextReport,
                    model.AllowReviewActionNextPost,
                    model.AllowReviewCommentNextPost,
                    model.IsAllReportPublic,
                    model.IsAnonymousAllReport,
                    model.AllowReviewActionAllReport,
                    model.AllowReviewCommentAllReport,
                }, commandType: CommandType.StoredProcedure));

                if (id.Contains("Error"))
                    throw new Exception(id);

                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override SystemUserConfigModel Find(string id) => throw new NotImplementedException();
        public override List<SystemUserConfigModel> GetAll() => throw new NotImplementedException();
        public override bool Remove(string id) => throw new NotImplementedException();

        public override bool Update(SystemUserConfigModel model)
        {
            bool success = false;
            try
            {
                int affectedRows = 0;
                var result = Convert.ToString(_dBConnection.ExecuteScalar("usp_systemuserconfig_update", new
                {
                    model.SystemUserConfigId,
                    model.IsUserEnable,
                    model.IsUserAllowToPostNextReport,
                    model.IsNextReportPublic,
                    model.IsAnonymousNextReport,
                    model.AllowReviewActionNextPost,
                    model.AllowReviewCommentNextPost,
                    model.IsAllReportPublic,
                    model.IsAnonymousAllReport,
                    model.AllowReviewActionAllReport,
                    model.AllowReviewCommentAllReport,
                }, commandType: CommandType.StoredProcedure));

                if (result.Contains("Error"))
                    throw new Exception(result);

                affectedRows = Convert.ToInt32(result);
                success = affectedRows > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return success;
        }
    }
}
