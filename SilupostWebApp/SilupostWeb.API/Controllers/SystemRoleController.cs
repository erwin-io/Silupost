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
    [RoutePrefix("api/v1/SystemRole")]
    public class SystemRoleController : ApiController
    {
        private readonly ISystemRoleFacade _systemRoleFacade;
        private string RecordedBy { get; set; }
        private long LocationId { get; set; }
        #region CONSTRUCTORS
        public SystemRoleController(ISystemRoleFacade systemRoleFacade)
        {
            _systemRoleFacade = systemRoleFacade ?? throw new ArgumentNullException(nameof(systemRoleFacade));
        }
        #endregion


        [Route("getPage")]
        [HttpGet]
        [SwaggerOperation("getPage")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetPage(string SystemRoleId, string Name, string CreatedAt, string UpdatedAt, int PageNo, int PageSize, long LocationId)
        {
            AppResponseModel<PageResultsViewModel<SystemRoleViewModel>> response = new AppResponseModel<PageResultsViewModel<SystemRoleViewModel>>();

            try
            {
                var pageResults = _systemRoleFacade.GetPage(SystemRoleId, Name, CreatedAt, UpdatedAt, PageNo, PageSize, LocationId);
                response.Data = pageResults;
                response.IsSuccess = true;
                return new POSAPIHttpActionResult<AppResponseModel<PageResultsViewModel<SystemRoleViewModel>>>(Request, HttpStatusCode.OK, response);

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new POSAPIHttpActionResult<AppResponseModel<PageResultsViewModel<SystemRoleViewModel>>>(Request, HttpStatusCode.OK, response);
            }
        }

        [Route("{id}/detail")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult Get(string id)
        {
            AppResponseModel<SystemRoleViewModel> response = new AppResponseModel<SystemRoleViewModel>();

            if (string.IsNullOrEmpty(id))
            {
                response.Message = string.Format(Messages.InvalidId, "Pet Group");
                return new POSAPIHttpActionResult<AppResponseModel<SystemRoleViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                SystemRoleViewModel result = _systemRoleFacade.Find(id);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemRoleViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemRoleViewModel>>(Request, HttpStatusCode.NotFound, response);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new POSAPIHttpActionResult<AppResponseModel<SystemRoleViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("")]
        [HttpPost]
        [ValidateModel]
        [SwaggerOperation("create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult Create([FromBody] SystemRoleBindingModel model)
        {
            AppResponseModel<SystemRoleViewModel> response = new AppResponseModel<SystemRoleViewModel>();

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                    LocationId = Convert.ToInt64(identity.FindFirst("LocationId").Value);
                }
                string id = _systemRoleFacade.Add(model, LocationId, RecordedBy);

                if (!string.IsNullOrEmpty(id))
                {
                    var result = _systemRoleFacade.Find(id);

                    response.IsSuccess = true;
                    response.Message = Messages.Created;
                    response.Data = result;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemRoleViewModel>>(Request, HttpStatusCode.Created, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemRoleViewModel>>(Request, HttpStatusCode.BadRequest, response);

                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new POSAPIHttpActionResult<AppResponseModel<SystemRoleViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }
        [Route("")]
        [HttpPut]
        [ValidateModel]
        [SwaggerOperation("update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult Update([FromBody] UpdateSystemRoleBindingModel model)
        {
            AppResponseModel<SystemRoleViewModel> response = new AppResponseModel<SystemRoleViewModel>();

            if (model != null && string.IsNullOrEmpty(model.SystemRoleId))
            {
                response.Message = string.Format(Messages.InvalidId, "agent");
                return new POSAPIHttpActionResult<AppResponseModel<SystemRoleViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }
                var result = _systemRoleFacade.Find(model.SystemRoleId);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "System Role");
                    return new POSAPIHttpActionResult<AppResponseModel<SystemRoleViewModel>>(Request, HttpStatusCode.BadRequest, response);
                }
                bool success = _systemRoleFacade.Update(model, RecordedBy);
                response.IsSuccess = success;

                if (success)
                {
                    result = _systemRoleFacade.Find(model.SystemRoleId);
                    response.Message = Messages.Updated;
                    response.Data = result;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemRoleViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new POSAPIHttpActionResult<AppResponseModel<SystemRoleViewModel>>(Request, HttpStatusCode.BadGateway, response);
                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new POSAPIHttpActionResult<AppResponseModel<SystemRoleViewModel>>(Request, HttpStatusCode.BadRequest, response);
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
                response.Message = string.Format(Messages.InvalidId, "System Role");
                return new POSAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }

                var result = _systemRoleFacade.Find(id);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "System Role");
                    return new POSAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
                }

                bool success = _systemRoleFacade.Remove(id, RecordedBy);
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
