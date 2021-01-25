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
    [RoutePrefix("api/v1/EnforcementStation")]
    public class EnforcementStationController : ApiController
    {
        private readonly IEnforcementStationFacade _enforcementStation;
        private string RecordedBy { get; set; }
        private long LocationId { get; set; }
        #region CONSTRUCTORS
        public EnforcementStationController(IEnforcementStationFacade enforcementStation)
        {
            _enforcementStation = enforcementStation ?? throw new ArgumentNullException(nameof(enforcementStation));
        }
        #endregion


        [Route("getPage")]
        [HttpGet]
        [SwaggerOperation("getPage")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetPage(int Draw, string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir)
        {
            DataTableResponseModel<IList<EnforcementStationViewModel>> response = new DataTableResponseModel<IList<EnforcementStationViewModel>>();

            try
            {
                long recordsFiltered = 0;
                long recordsTotal = 0;
                var pageResults = _enforcementStation.GetPage(
                    (Search = string.IsNullOrEmpty(Search) ? string.Empty : Search),
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

                return new SilupostAPIHttpActionResult<DataTableResponseModel<IList<EnforcementStationViewModel>>>(Request, HttpStatusCode.OK, response);
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
            AppResponseModel<EnforcementStationViewModel> response = new AppResponseModel<EnforcementStationViewModel>();

            if (string.IsNullOrEmpty(id))
            {
                response.Message = string.Format(Messages.InvalidId, "Enforcement Type");
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                EnforcementStationViewModel result = _enforcementStation.Find(id);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.NotFound, response);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("")]
        [HttpPost]
        [ValidateModel]
        [SwaggerOperation("create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult Create([FromBody] CreateEnforcementStationBindingModel model)
        {
            AppResponseModel<EnforcementStationViewModel> response = new AppResponseModel<EnforcementStationViewModel>();

            if (model != null && model.IconFile == null)
            {
                response.Message = string.Format(Messages.CustomError, "Icon file");
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
            if (string.IsNullOrEmpty(model.IconFile.FileFromBase64String) || string.IsNullOrEmpty(model.IconFile.FileName) || model.IconFile.FileSize <= 0)
            {
                response.Message = string.Format(Messages.CustomError, "Icon file");
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }

                string id = _enforcementStation.Add(model, RecordedBy);

                if (!string.IsNullOrEmpty(id))
                {
                    var result = _enforcementStation.Find(id);

                    response.IsSuccess = true;
                    response.Message = Messages.Created;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.Created, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.BadRequest, response);

                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }
        [Route("")]
        [HttpPut]
        [ValidateModel]
        [SwaggerOperation("update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult Update([FromBody] UpdateEnforcementStationBindingModel model)
        {
            AppResponseModel<EnforcementStationViewModel> response = new AppResponseModel<EnforcementStationViewModel>();

            if (model != null && string.IsNullOrEmpty(model.EnforcementStationId))
            {
                response.Message = string.Format(Messages.InvalidId, "Enforcement Type");
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
            if (model != null && model.IconFile == null)
            {
                response.Message = string.Format(Messages.CustomError, "Icon file");
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
            if (string.IsNullOrEmpty(model.IconFile.FileId) || string.IsNullOrEmpty(model.IconFile.FileFromBase64String) || string.IsNullOrEmpty(model.IconFile.FileName) || model.IconFile.FileSize <= 0)
            {
                response.Message = string.Format(Messages.CustomError, "Icon file");
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }
                var result = _enforcementStation.Find(model.EnforcementStationId);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "Enforcement Type");
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.BadRequest, response);
                }
                bool success = _enforcementStation.Update(model, RecordedBy);
                response.IsSuccess = success;

                if (success)
                {
                    result = _enforcementStation.Find(model.EnforcementStationId);
                    response.Message = Messages.Updated;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.BadGateway, response);
                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementStationViewModel>>(Request, HttpStatusCode.BadRequest, response);
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
                response.Message = string.Format(Messages.InvalidId, "Enforcement Type");
                return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }

                var result = _enforcementStation.Find(id);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "Enforcement Type");
                    return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
                }

                bool success = _enforcementStation.Remove(id, RecordedBy);
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
