using Newtonsoft.Json;
using SilupostWeb.API.Filters;
using SilupostWeb.API.Helpers;
using SilupostWeb.API.Models;
using SilupostWeb.Domain.ViewModel;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Facade.Interface;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Linq;

namespace SilupostWeb.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/UserAuth")]
    public class UserAuthController : ApiController
    {
        private readonly ISystemUserFacade _systemUserFacade;
        #region CONSTRUCTORS
        public UserAuthController(ISystemUserFacade systemUserFacade)
        {
            _systemUserFacade = systemUserFacade ?? throw new ArgumentNullException(nameof(systemUserFacade));
        }
        #endregion


        [Route("")]
        [HttpPost]
        [ValidateModel]
        [SwaggerOperation("create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult Create([FromBody] CreateSystemUserBindingModel model)
        {
            AppResponseModel<SystemUserViewModel> response = new AppResponseModel<SystemUserViewModel>();

            try
            {
                string id = _systemUserFacade.Add(model);

                if (!string.IsNullOrEmpty(id))
                {
                    var result = _systemUserFacade.Find(id);

                    response.IsSuccess = true;
                    response.Message = Messages.Created;
                    response.Data = result;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.Created, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);

                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new POSAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }
    }
}
