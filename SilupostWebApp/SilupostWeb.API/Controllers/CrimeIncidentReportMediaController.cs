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
    [RoutePrefix("api/v1/CrimeIncidentReportMedia")]
    public class CrimeIncidentReportMediaController : ApiController
    {
        private readonly ICrimeIncidentReportMediaFacade _crimeIncidentReportMediaFacade;
        private string RecordedBy { get; set; }
        private long LocationId { get; set; }
        #region CONSTRUCTORS
        public CrimeIncidentReportMediaController(ICrimeIncidentReportMediaFacade crimeIncidentReportMediaFacade)
        {
            _crimeIncidentReportMediaFacade = crimeIncidentReportMediaFacade ?? throw new ArgumentNullException(nameof(crimeIncidentReportMediaFacade));
        }
        #endregion


        [Route("getByCrimeIncidentReportId")]
        [HttpGet]
        [SwaggerOperation("getByCrimeIncidentReportId")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetByCrimeIncidentReportId(string crimeIncidentReportId)
        {
            AppResponseModel<List<CrimeIncidentReportMediaViewModel>> response = new AppResponseModel<List<CrimeIncidentReportMediaViewModel>>();

            if (string.IsNullOrEmpty(crimeIncidentReportId))
            {
                response.Message = string.Format(Messages.InvalidId, "System User LegalEntity Address");
                return new SilupostAPIHttpActionResult<AppResponseModel<List<CrimeIncidentReportMediaViewModel>>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                List<CrimeIncidentReportMediaViewModel> result = _crimeIncidentReportMediaFacade.FindByCrimeIncidentReportId(crimeIncidentReportId);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<List<CrimeIncidentReportMediaViewModel>>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new SilupostAPIHttpActionResult<AppResponseModel<List<CrimeIncidentReportMediaViewModel>>>(Request, HttpStatusCode.NotFound, response);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<List<CrimeIncidentReportMediaViewModel>>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("{id}/detail")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult Get(string id)
        {
            AppResponseModel<CrimeIncidentReportMediaViewModel> response = new AppResponseModel<CrimeIncidentReportMediaViewModel>();

            if (string.IsNullOrEmpty(id))
            {
                response.Message = string.Format(Messages.InvalidId, "Crime Incident Report Media");
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportMediaViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                CrimeIncidentReportMediaViewModel result = _crimeIncidentReportMediaFacade.Find(id);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportMediaViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportMediaViewModel>>(Request, HttpStatusCode.NotFound, response);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportMediaViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("")]
        [HttpPost]
        [ValidateModel]
        [SwaggerOperation("create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult Create([FromBody] CreateCrimeIncidentReportMediaBindingModel model)
        {
            AppResponseModel<CrimeIncidentReportMediaViewModel> response = new AppResponseModel<CrimeIncidentReportMediaViewModel>();

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }

                string id = _crimeIncidentReportMediaFacade.Add(model, RecordedBy);

                if (!string.IsNullOrEmpty(id))
                {
                    var result = _crimeIncidentReportMediaFacade.Find(id);

                    response.IsSuccess = true;
                    response.Message = Messages.Created;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportMediaViewModel>>(Request, HttpStatusCode.Created, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportMediaViewModel>>(Request, HttpStatusCode.BadRequest, response);

                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportMediaViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }
        [Route("")]
        [HttpPut]
        [ValidateModel]
        [SwaggerOperation("update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult Update([FromBody] UpdateCrimeIncidentReportMediaBindingModel model)
        {
            AppResponseModel<CrimeIncidentReportMediaViewModel> response = new AppResponseModel<CrimeIncidentReportMediaViewModel>();

            if (model != null && string.IsNullOrEmpty(model.CrimeIncidentReportMediaId))
            {
                response.Message = string.Format(Messages.InvalidId, "Crime Incident Report Media");
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportMediaViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }
                var result = _crimeIncidentReportMediaFacade.Find(model.CrimeIncidentReportMediaId);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "Crime Incident Report Media");
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportMediaViewModel>>(Request, HttpStatusCode.BadRequest, response);
                }
                bool success = _crimeIncidentReportMediaFacade.Update(model, RecordedBy);
                response.IsSuccess = success;

                if (success)
                {
                    result = _crimeIncidentReportMediaFacade.Find(model.CrimeIncidentReportMediaId);
                    response.Message = Messages.Updated;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportMediaViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportMediaViewModel>>(Request, HttpStatusCode.BadGateway, response);
                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportMediaViewModel>>(Request, HttpStatusCode.BadRequest, response);
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
                response.Message = string.Format(Messages.InvalidId, "Crime Incident Report Media");
                return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }

                var result = _crimeIncidentReportMediaFacade.Find(id);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "Crime Incident Report Media");
                    return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
                }

                bool success = _crimeIncidentReportMediaFacade.Remove(id);
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
