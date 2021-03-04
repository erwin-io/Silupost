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
    [RoutePrefix("api/v1/EnforcementReportValidation")]
    public class EnforcementReportValidationController : ApiController
    {
        private readonly IEnforcementReportValidationFacade _enforcementReportValidationFacade;
        private string RecordedBy { get; set; }
        #region CONSTRUCTORS
        public EnforcementReportValidationController(IEnforcementReportValidationFacade enforcementReportValidationFacade)
        {
            _enforcementReportValidationFacade = enforcementReportValidationFacade ?? throw new ArgumentNullException(nameof(enforcementReportValidationFacade));
        }
        #endregion


        [Route("getTablePageByCrimeIncidentReportId")]
        [HttpGet]
        [SwaggerOperation("getPage")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetTablePageByCrimeIncidentReportId(int Draw, 
                                                string Search, 
                                                bool IsAdvanceSearchMode,
                                                string CrimeIncidentReportId,
                                                string EnforcementUnitName,
                                               DateTime DateSubmittedFrom,
                                               DateTime DateSubmittedTo,
                                                string ReportValidationStatusId,
                                                int PageNo,
                                                int PageSize,
                                                string OrderColumn,
                                                string OrderDir)
        {
            DataTableResponseModel<IList<EnforcementReportValidationViewModel>> response = new DataTableResponseModel<IList<EnforcementReportValidationViewModel>>();

            try
            {
                long recordsFiltered = 0;
                long recordsTotal = 0;
                var pageResults = _enforcementReportValidationFacade.GetTablePageByCrimeIncidentReportId(
                                                            (Search = string.IsNullOrEmpty(Search) ? string.Empty : Search),
                                                             IsAdvanceSearchMode,
                                                             CrimeIncidentReportId,
                                                             (EnforcementUnitName = string.IsNullOrEmpty(EnforcementUnitName) ? string.Empty : EnforcementUnitName),
                                                             DateSubmittedFrom,
                                                             DateSubmittedTo,
                                                             ReportValidationStatusId,
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

                return new SilupostAPIHttpActionResult<DataTableResponseModel<IList<EnforcementReportValidationViewModel>>>(Request, HttpStatusCode.OK, response);
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

        [Route("GetTablePageByEnforcementSystemUserId")]
        [HttpGet]
        [SwaggerOperation("getPage")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult GetTablePageByEnforcementSystemUserId(int Draw, 
                                                                        string Search,
                                                                        bool IsAdvanceSearchMode,
                                                                        string SystemUserId,
                                                                        string CrimeIncidentCategoryName,
                                                                        DateTime DateSubmittedFrom,
                                                                        DateTime DateSubmittedTo,
                                                                        string ReportValidationStatusId,
                                                                        int PageNo,
                                                                        int PageSize,
                                                                        string OrderColumn,
                                                                        string OrderDir)
        {
            DataTableResponseModel<IList<EnforcementReportValidationViewModel>> response = new DataTableResponseModel<IList<EnforcementReportValidationViewModel>>();

            try
            {
                long recordsFiltered = 0;
                long recordsTotal = 0;
                var pageResults = _enforcementReportValidationFacade.GetPageByEnforcementSystemUserId(
                                                            (Search = string.IsNullOrEmpty(Search) ? string.Empty : Search),
                                                             IsAdvanceSearchMode,
                                                             SystemUserId,
                                                             (CrimeIncidentCategoryName = string.IsNullOrEmpty(CrimeIncidentCategoryName) ? string.Empty : CrimeIncidentCategoryName),
                                                             DateSubmittedFrom,
                                                             DateSubmittedTo,
                                                             ReportValidationStatusId,
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

                return new SilupostAPIHttpActionResult<DataTableResponseModel<IList<EnforcementReportValidationViewModel>>>(Request, HttpStatusCode.OK, response);
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


        [Route("GetPageByEnforcementStationId")]
        [HttpGet]
        [SwaggerOperation("getPage")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult GetPageByEnforcementStationId(int Draw,
                                                                        string Search,
                                                                        bool IsAdvanceSearchMode,
                                                                        string EnforcementStationId,
                                                                        string CrimeIncidentCategoryName,
                                                                        DateTime DateSubmittedFrom,
                                                                        DateTime DateSubmittedTo,
                                                                        string ReportValidationStatusId,
                                                                        int PageNo,
                                                                        int PageSize,
                                                                        string OrderColumn,
                                                                        string OrderDir)
        {
            DataTableResponseModel<IList<EnforcementReportValidationViewModel>> response = new DataTableResponseModel<IList<EnforcementReportValidationViewModel>>();

            try
            {
                long recordsFiltered = 0;
                long recordsTotal = 0;
                var pageResults = _enforcementReportValidationFacade.GetPageByEnforcementStationId(
                                                            (Search = string.IsNullOrEmpty(Search) ? string.Empty : Search),
                                                             IsAdvanceSearchMode,
                                                             EnforcementStationId,
                                                             (CrimeIncidentCategoryName = string.IsNullOrEmpty(CrimeIncidentCategoryName) ? string.Empty : CrimeIncidentCategoryName),
                                                             DateSubmittedFrom,
                                                             DateSubmittedTo,
                                                             ReportValidationStatusId,
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

                return new SilupostAPIHttpActionResult<DataTableResponseModel<IList<EnforcementReportValidationViewModel>>>(Request, HttpStatusCode.OK, response);
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
            AppResponseModel<EnforcementReportValidationViewModel> response = new AppResponseModel<EnforcementReportValidationViewModel>();

            if (string.IsNullOrEmpty(id))
            {
                response.Message = string.Format(Messages.InvalidId, "Enforcement Report Validation");
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementReportValidationViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                EnforcementReportValidationViewModel result = _enforcementReportValidationFacade.Find(id);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementReportValidationViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementReportValidationViewModel>>(Request, HttpStatusCode.NotFound, response);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementReportValidationViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("")]
        [HttpPost]
        [ValidateModel]
        [SwaggerOperation("create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult Create([FromBody] CreateEnforcementReportValidationBindingModel model)
        {
            AppResponseModel<EnforcementReportValidationViewModel> response = new AppResponseModel<EnforcementReportValidationViewModel>();

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }

                string id = _enforcementReportValidationFacade.Add(model, RecordedBy);

                if (!string.IsNullOrEmpty(id))
                {
                    var result = _enforcementReportValidationFacade.Find(id);

                    response.IsSuccess = true;
                    response.Message = Messages.Created;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementReportValidationViewModel>>(Request, HttpStatusCode.Created, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementReportValidationViewModel>>(Request, HttpStatusCode.BadRequest, response);

                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementReportValidationViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("")]
        [HttpPut]
        [ValidateModel]
        [SwaggerOperation("update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult Update([FromBody] UpdateEnforcementReportValidationBindingModel model)
        {
            AppResponseModel<EnforcementReportValidationViewModel> response = new AppResponseModel<EnforcementReportValidationViewModel>();

            if (model != null && string.IsNullOrEmpty(model.EnforcementReportValidationId))
            {
                response.Message = string.Format(Messages.InvalidId, "Enforcement Report Validation");
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementReportValidationViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }
                var result = _enforcementReportValidationFacade.Find(model.EnforcementReportValidationId);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "Enforcement Report Validation");
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementReportValidationViewModel>>(Request, HttpStatusCode.BadRequest, response);
                }
                bool success = _enforcementReportValidationFacade.Update(model, RecordedBy);
                response.IsSuccess = success;

                if (success)
                {
                    result = _enforcementReportValidationFacade.Find(model.EnforcementReportValidationId);
                    response.Message = Messages.Updated;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementReportValidationViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementReportValidationViewModel>>(Request, HttpStatusCode.BadGateway, response);
                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementReportValidationViewModel>>(Request, HttpStatusCode.BadRequest, response);
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
                response.Message = string.Format(Messages.InvalidId, "Enforcement Report Validation");
                return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }

                var result = _enforcementReportValidationFacade.Find(id);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "Enforcement Report Validation");
                    return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
                }

                bool success = _enforcementReportValidationFacade.Remove(id, RecordedBy);
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
