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
    [RoutePrefix("api/v1/CrimeIncidentType")]
    public class CrimeIncidentTypeController : ApiController
    {
        private readonly ICrimeIncidentTypeFacade _crimeIncidentTypeFacade;
        private string RecordedBy { get; set; }
        private long LocationId { get; set; }
        #region CONSTRUCTORS
        public CrimeIncidentTypeController(ICrimeIncidentTypeFacade crimeIncidentTypeFacade)
        {
            _crimeIncidentTypeFacade = crimeIncidentTypeFacade ?? throw new ArgumentNullException(nameof(crimeIncidentTypeFacade));
        }
        #endregion


        [Route("getPage")]
        [HttpGet]
        [SwaggerOperation("getPage")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetPage(int Draw, string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir)
        {
            DataTableResponseModel<IList<CrimeIncidentTypeViewModel>> response = new DataTableResponseModel<IList<CrimeIncidentTypeViewModel>>();

            try
            {
                long recordsFiltered = 0;
                long recordsTotal = 0;
                var pageResults = _crimeIncidentTypeFacade.GetPage(
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

                return new SilupostAPIHttpActionResult<DataTableResponseModel<IList<CrimeIncidentTypeViewModel>>>(Request, HttpStatusCode.OK, response);
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
            AppResponseModel<CrimeIncidentTypeViewModel> response = new AppResponseModel<CrimeIncidentTypeViewModel>();

            if (string.IsNullOrEmpty(id))
            {
                response.Message = string.Format(Messages.InvalidId, "Crime Incident Type");
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                CrimeIncidentTypeViewModel result = _crimeIncidentTypeFacade.Find(id);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.NotFound, response);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("")]
        [HttpPost]
        [ValidateModel]
        [SwaggerOperation("create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult Create([FromBody] CreateCrimeIncidentTypeBindingModel model)
        {
            AppResponseModel<CrimeIncidentTypeViewModel> response = new AppResponseModel<CrimeIncidentTypeViewModel>();

            if (model != null && model.IconFile == null)
            {
                response.Message = string.Format(Messages.CustomError, "Icon file");
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
            if (string.IsNullOrEmpty(model.IconFile.FileFromBase64String) || string.IsNullOrEmpty(model.IconFile.FileName) || model.IconFile.FileSize <= 0)
            {
                response.Message = string.Format(Messages.CustomError, "Icon file");
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }

                string id = _crimeIncidentTypeFacade.Add(model, RecordedBy);

                if (!string.IsNullOrEmpty(id))
                {
                    var result = _crimeIncidentTypeFacade.Find(id);

                    response.IsSuccess = true;
                    response.Message = Messages.Created;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.Created, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.BadRequest, response);

                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }
        [Route("")]
        [HttpPut]
        [ValidateModel]
        [SwaggerOperation("update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult Update([FromBody] UpdateCrimeIncidentTypeBindingModel model)
        {
            AppResponseModel<CrimeIncidentTypeViewModel> response = new AppResponseModel<CrimeIncidentTypeViewModel>();

            if (model != null && string.IsNullOrEmpty(model.CrimeIncidentTypeId))
            {
                response.Message = string.Format(Messages.InvalidId, "Crime Incident Type");
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
            if (model != null && model.IconFile == null)
            {
                response.Message = string.Format(Messages.CustomError, "Icon file");
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
            if (string.IsNullOrEmpty(model.IconFile.FileId) || string.IsNullOrEmpty(model.IconFile.FileFromBase64String) || string.IsNullOrEmpty(model.IconFile.FileName) || model.IconFile.FileSize <= 0)
            {
                response.Message = string.Format(Messages.CustomError, "Icon file");
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }
                var result = _crimeIncidentTypeFacade.Find(model.CrimeIncidentTypeId);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "Crime Incident Type");
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.BadRequest, response);
                }
                bool success = _crimeIncidentTypeFacade.Update(model, RecordedBy);
                response.IsSuccess = success;

                if (success)
                {
                    result = _crimeIncidentTypeFacade.Find(model.CrimeIncidentTypeId);
                    response.Message = Messages.Updated;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.BadGateway, response);
                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentTypeViewModel>>(Request, HttpStatusCode.BadRequest, response);
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
                response.Message = string.Format(Messages.InvalidId, "Crime Incident Type");
                return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }

                var result = _crimeIncidentTypeFacade.Find(id);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "Crime Incident Type");
                    return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
                }

                bool success = _crimeIncidentTypeFacade.Remove(id, RecordedBy);
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
