using POSWeb.POSAdmin.Data.Core;
using POSWeb.POSAdmin.Data.Interface;
using POSWeb.POSAdmin.Data.Entity;
using System.Collections.Generic;
using System.Data;
using System;
using Dapper;
using System.Linq;

namespace POSWeb.POSAdmin.Data
{
    public class EntityInformationDAC : RepositoryBase<EntityInformationModel>, IEntityInformationRepository
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public EntityInformationDAC(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public override string Add(EntityInformationModel model)
        {
            try
            {
                var id = Convert.ToString(_dBConnection.ExecuteScalar("usp_entity_information_add", new
                {
                    model.FirstName,
                    model.LastName,
                    model.MiddleName,
                    model.Gender.GenderId,
                    model.BirthDate,
                    model.CivilStatusType.CivilStatusTypeId,
                    model.Location.LocationId,
                    CreatedBy = model?.CreatedBy?.SystemUserId,
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

        public override EntityInformationModel Find(string id) => throw new NotImplementedException();
        public override List<EntityInformationModel> GetAll() => throw new NotImplementedException();

        public override bool Remove(string id) => throw new NotImplementedException();

        public override bool Update(EntityInformationModel model) => throw new NotImplementedException();
    }
}
