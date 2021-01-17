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
using System.Security.Claims;

namespace SilupostWeb.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/v1/SystemWebAdminRole")]
    public class SystemWebAdminRoleController : ApiController
    {
        private readonly ISystemWebAdminRoleFacade _systemWebAdminRoleFacade;
        private string RecordedBy { get; set; }
        private long LocationId { get; set; }
        #region CONSTRUCTORS
        public SystemWebAdminRoleController(ISystemWebAdminRoleFacade systemWebAdminRoleFacade)
        {
            _systemWebAdminRoleFacade = systemWebAdminRoleFacade ?? throw new ArgumentNullException(nameof(systemWebAdminRoleFacade));
        }
        #endregion


        [Route("getPage")]
        [HttpGet]
        [SwaggerOperation("getPage")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetPage(string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir)
        {
            AppResponseModel<PageResultsViewModel<SystemWebAdminRoleViewModel>> response = new AppResponseModel<PageResultsViewModel<SystemWebAdminRoleViewModel>>();

            try
            {
                var pageResults = _systemWebAdminRoleFacade.GetPage(Search, PageNo, PageSize, OrderColumn, OrderDir);
                response.Data = pageResults;
                response.IsSuccess = true;
                return new POSAPIHttpActionResult<AppResponseModel<PageResultsViewModel<SystemWebAdminRoleViewModel>>>(Request, HttpStatusCode.OK, response);

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new POSAPIHttpActionResult<AppResponseModel<PageResultsViewModel<SystemWebAdminRoleViewModel>>>(Request, HttpStatusCode.OK, response);
            }
        }

        [Route("{id}/detail")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult Get(string id)
        {
            AppResponseModel<SystemWebAdminRoleViewModel> response = new AppResponseModel<SystemWebAdminRoleViewModel>();

            if (string.IsNullOrEmpty(id))
            {
                response.Message = string.Format(Messages.InvalidId, "System Web Admin Role");
                return new POSAPIHttpActionResult<AppResponseModel<SystemWebAdminRoleViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                SystemWebAdminRoleViewModel result = _systemWebAdminRoleFacade.Find(id);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemWebAdminRoleViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemWebAdminRoleViewModel>>(Request, HttpStatusCode.NotFound, response);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new POSAPIHttpActionResult<AppResponseModel<SystemWebAdminRoleViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("")]
        [HttpPost]
        [ValidateModel]
        [SwaggerOperation("create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult Create([FromBody] SystemWebAdminRoleBindingModel model)
        {
            AppResponseModel<SystemWebAdminRoleViewModel> response = new AppResponseModel<SystemWebAdminRoleViewModel>();

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }
                string id = _systemWebAdminRoleFacade.Add(model, RecordedBy);

                if (!string.IsNullOrEmpty(id))
                {
                    var result = _systemWebAdminRoleFacade.Find(id);

                    response.IsSuccess = true;
                    response.Message = Messages.Created;
                    response.Data = result;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemWebAdminRoleViewModel>>(Request, HttpStatusCode.Created, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemWebAdminRoleViewModel>>(Request, HttpStatusCode.BadRequest, response);

                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new POSAPIHttpActionResult<AppResponseModel<SystemWebAdminRoleViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }
        [Route("")]
        [HttpPut]
        [ValidateModel]
        [SwaggerOperation("update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult Update([FromBody] UpdateSystemWebAdminRoleBindingModel model)
        {
            AppResponseModel<SystemWebAdminRoleViewModel> response = new AppResponseModel<SystemWebAdminRoleViewModel>();

            if (model != null && string.IsNullOrEmpty(model.SystemWebAdminRoleId))
            {
                response.Message = string.Format(Messages.InvalidId, "System Web Admin Role");
                return new POSAPIHttpActionResult<AppResponseModel<SystemWebAdminRoleViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }
                var result = _systemWebAdminRoleFacade.Find(model.SystemWebAdminRoleId);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "System Web Admin Role");
                    return new POSAPIHttpActionResult<AppResponseModel<SystemWebAdminRoleViewModel>>(Request, HttpStatusCode.BadRequest, response);
                }
                bool success = _systemWebAdminRoleFacade.Update(model, RecordedBy);
                response.IsSuccess = success;

                if (success)
                {
                    result = _systemWebAdminRoleFacade.Find(model.SystemWebAdminRoleId);
                    response.Message = Messages.Updated;
                    response.Data = result;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemWebAdminRoleViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemWebAdminRoleViewModel>>(Request, HttpStatusCode.BadGateway, response);
                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new POSAPIHttpActionResult<AppResponseModel<SystemWebAdminRoleViewModel>>(Request, HttpStatusCode.BadRequest, response);
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
                response.Message = string.Format(Messages.InvalidId, "System Web Admin Role");
                return new POSAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }

                var result = _systemWebAdminRoleFacade.Find(id);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "System Web Admin Role");
                    return new POSAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
                }

                bool success = _systemWebAdminRoleFacade.Remove(id, RecordedBy);
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
