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
        public IHttpActionResult GetPage(int Draw, long SystemUserType, string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir)
        {
            DataTableResponseModel<IList<SystemUserViewModel>> response = new DataTableResponseModel<IList<SystemUserViewModel>>();

            try
            {
                long recordsFiltered = 0;
                long recordsTotal = 0;
                var pageResults = _systemUserFacade.GetPage(
                    (Search = string.IsNullOrEmpty(Search) ? string.Empty : Search),
                    SystemUserType,
                    PageNo,
                    PageSize,
                    OrderColumn,
                    OrderDir);
                var records = pageResults.Items.ToList();
                recordsTotal = pageResults.TotalRows;
                recordsFiltered = pageResults.TotalRows;

                response.draw = Draw;
                response.recordsFiltered = recordsFiltered;
                response.recordsTotal = recordsTotal;
                response.data = pageResults.Items.ToList();

                return new SilupostAPIHttpActionResult<DataTableResponseModel<IList<SystemUserViewModel>>>(Request, HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var exception = new AppResponseModel<object>();
                exception.DeveloperMessage = ex.Message;
                exception.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.OK, exception);
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
                return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                SystemUserViewModel result = _systemUserFacade.Find(id);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.NotFound, response);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);
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
                return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            if (string.IsNullOrEmpty(password))
            {
                response.Message = string.Format(Messages.CustomError, "Invalid Username!");
                return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                SystemUserViewModel result = _systemUserFacade.Find(username, password);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = string.Format(Messages.CustomError, "Username or Password is incorrect!");
                    return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.NotFound, response);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);
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
                    return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.Created, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);

                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);
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
                return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);
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
                    return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);
                }
                bool success = _systemUserFacade.Update(model, RecordedBy);
                response.IsSuccess = success;

                if (success)
                {
                    result = _systemUserFacade.Find(model.SystemUserId);
                    response.Message = Messages.Updated;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadGateway, response);
                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<SystemUserViewModel>>(Request, HttpStatusCode.BadRequest, response);
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
                return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
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
                    return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
                }

                bool success = _systemUserFacade.Remove(id, RecordedBy);
                response.IsSuccess = success;

                if (success)
                {
                    response.Message = Messages.Removed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.NotFound, response);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
            }
        }
    }
}
