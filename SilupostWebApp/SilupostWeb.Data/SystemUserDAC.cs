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
    public class SystemUserDAC : RepositoryBase<SystemUserModel>, ISystemUserRepository
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public SystemUserDAC(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public override string Add(SystemUserModel model)
        {
            try
            {
                var id = Convert.ToString(_dBConnection.ExecuteScalar("usp_systemuser_add", new
                {
                    model.EntityInformation.LegalEntityId,
                    model.UserName,
                    model.Password,
                    model.Location.LocationId,
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

        public override SystemUserModel Find(string id)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("usp_systemuser_getByID", new
                {
                    SystemUserId = id
                }, commandType: CommandType.StoredProcedure))
                {
                    var model = result.Read<SystemUserModel>().FirstOrDefault();
                    if (model != null)
                    {
                        model.Location = result.Read<LocationModel>().FirstOrDefault();
                        model.EntityInformation = result.Read<EntityInformationModel>().FirstOrDefault();
                        model.EntityInformation.Contact = result.Read<EntityContactInformationModel>().ToList();
                    }

                    return model;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SystemUserModel Find(string Username, string Password)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("usp_systemuser_getByCredentials", new
                {
                    Username = Username,
                    Password = Password,
                }, commandType: CommandType.StoredProcedure))
                {
                    var model = result.Read<SystemUserModel>().FirstOrDefault();
                    if(model != null)
                    {
                        model.Location = result.Read<LocationModel>().FirstOrDefault();
                        model.EntityInformation = result.Read<EntityInformationModel>().FirstOrDefault();
                        model.EntityInformation.Contact = result.Read<EntityContactInformationModel>().ToList();
                    }

                    return model;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override List<SystemUserModel> GetAll() => throw new NotImplementedException();

        public override bool Remove(string id) => throw new NotImplementedException();

        public override bool Update(SystemUserModel model) => throw new NotImplementedException();
    }
}
