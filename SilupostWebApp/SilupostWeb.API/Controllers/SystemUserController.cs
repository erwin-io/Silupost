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
using System.Security.Claims;

namespace SilupostWeb.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/v1/SystemUser")]
    public class SystemUserController : ApiController
    {
        private readonly ISystemUserFacade _systemUserFacade;
        private string RecordedBy { get; set; }
        private long LocationId { get; set; }
        #region CONSTRUCTORS
        public SystemUserController(ISystemUserFacade systemUserFacade)
        {
            _systemUserFacade = systemUserFacade ?? throw new ArgumentNullException(nameof(systemUserFacade));
        }
        #endregion


        [Route("getPage")]
        [HttpGet]
        [SwaggerOperation("getPage")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetPage(string Search, long SystemUserType, long PageNo, long PageSize, string OrderColumn, string OrderDir)
        {
            AppResponseModel<PageResultsViewModel<SystemUserViewModel>> response = new AppResponseModel<PageResultsViewModel<SystemUserViewModel>>();

            try
            {
                var pageResults = _systemUserFacade.GetPage(Search, SystemUserType, PageNo, PageSize, OrderColumn, OrderDir);
                response.Data = pageResults;
                response.IsSuccess = true;
                return new POSAPIHttpActionResult<AppResponseModel<PageResultsViewModel<SystemUserViewModel>>>(Request, HttpStatusCode.OK, response);

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new POSAPIHttpActionResult<AppResponseModel<PageResultsViewModel<SystemUserViewModel>>>(Request, HttpStatusCode.OK, response);
            }
        }

        [Route("{id}/detail")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult Get(string id)
        {
            AppResponseModel<SystemUserViewModel> response = new AppResponseModel<SystemUserViewModel>();

            if (string.IsNullOrEmpty(id))
            {
                response.Message = string.Format(Messages.InvalidId, "System User");
                return new POSAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                SystemUserViewModel result = _systemUserFacade.Find(id);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.NotFound, response);
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

        [Route("")]
        [HttpGet]
        [SwaggerOperation("GetByCredentials")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult GetByCredentials(string username, string password)
        {
            AppResponseModel<SystemUserViewModel> response = new AppResponseModel<SystemUserViewModel>();

            if (string.IsNullOrEmpty(username))
            {
                response.Message = string.Format(Messages.CustomError, "Invalid Username!");
                return new POSAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            if (string.IsNullOrEmpty(password))
            {
                response.Message = string.Format(Messages.CustomError, "Invalid Username!");
                return new POSAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                SystemUserViewModel result = _systemUserFacade.Find(username, password);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = string.Format(Messages.CustomError, "Username or Password is incorrect!");
                    return new POSAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.NotFound, response);
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
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }
                string id = _systemUserFacade.Add(model, RecordedBy);

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
        [Route("")]
        [HttpPut]
        [ValidateModel]
        [SwaggerOperation("update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult Update([FromBody] UpdateSystemUserBindingModel model)
        {
            AppResponseModel<SystemUserViewModel> response = new AppResponseModel<SystemUserViewModel>();

            if (model != null && string.IsNullOrEmpty(model.SystemUserId))
            {
                response.Message = string.Format(Messages.InvalidId, "System User");
                return new POSAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }
                var result = _systemUserFacade.Find(model.SystemUserId);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "System User");
                    return new POSAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);
                }
                bool success = _systemUserFacade.Update(model, RecordedBy);
                response.IsSuccess = success;

                if (success)
                {
                    result = _systemUserFacade.Find(model.SystemUserId);
                    response.Message = Messages.Updated;
                    response.Data = result;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadGateway, response);
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

        [Route("{id}")]
        [HttpDelete]
        [SwaggerOperation("remove")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult Remove(string id)
        {
            AppResponseModel<object> response = new AppResponseModel<object>();

            if (string.IsNullOrEmpty(id))
            {
                response.Message = string.Format(Messages.InvalidId, "System User");
                return new POSAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }

                var result = _systemUserFacade.Find(id);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "System User");
                    return new POSAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
                }

                bool success = _systemUserFacade.Remove(id, RecordedBy);
                response.IsSuccess = success;

                if (success)
                {
                    response.Message = Messages.Removed;
                    return new POSAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new POSAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.NotFound, response);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new POSAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
            }
        }
    }
}
