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
    [SilupostAuthorizationFilter]
    [RoutePrefix("api/v1/CrimeIncidentReport")]
    public class CrimeIncidentReportController : ApiController
    {
        private readonly ICrimeIncidentReportFacade _crimeIncidentReportFacade;
        private string RecordedBy { get; set; }
        #region CONSTRUCTORS
        public CrimeIncidentReportController(ICrimeIncidentReportFacade crimeIncidentReportFacade)
        {
            _crimeIncidentReportFacade = crimeIncidentReportFacade ?? throw new ArgumentNullException(nameof(crimeIncidentReportFacade));
        }
        #endregion


        [Route("getTablePage")]
        [HttpGet]
        [SwaggerOperation("getPage")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetTablePage(int Draw, 
                                         string Search,
                                         bool IsAdvanceSearchMode,
                                         long ApprovalStatusId,
                                         string CrimeIncidentReportId,
                                         string CrimeIncidentCategoryName,
                                         string PostedByFullName,
                                         DateTime DateReportedFrom,
                                         DateTime DateReportedTo,
                                         DateTime PossibleDateFrom,
                                         DateTime PossibleDateTo,
                                         string PossibleTimeFrom,
                                         string PossibleTimeTo,
                                         string Description,
                                         string GeoStreet,
                                         string GeoDistrict,
                                         string GeoCityMun,
                                         string GeoProvince,
                                         string GeoCountry,
                                         int PageNo, 
                                         int PageSize,
                                         string OrderColumn,
                                         string OrderDir)
        {
            DataTableResponseModel<IList<CrimeIncidentReportViewModel>> response = new DataTableResponseModel<IList<CrimeIncidentReportViewModel>>();

            if (IsAdvanceSearchMode && (string.IsNullOrEmpty(PossibleTimeFrom) || !GlobalFunctions.IsValidTimeFormat(PossibleTimeFrom)))
            {
                response.Message = string.Format(Messages.CustomError, "Invalid time format : PossibleTimeFrom");
                return new SilupostAPIHttpActionResult<DataTableResponseModel<IList<CrimeIncidentReportViewModel>>>(Request, HttpStatusCode.BadRequest, response);
            }
            if (IsAdvanceSearchMode && (string.IsNullOrEmpty(PossibleTimeTo) || !GlobalFunctions.IsValidTimeFormat(PossibleTimeTo)))
            {
                response.Message = string.Format(Messages.CustomError, "Invalid time format : PossibleTimeTo");
                return new SilupostAPIHttpActionResult<DataTableResponseModel<IList<CrimeIncidentReportViewModel>>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                long recordsFiltered = 0;
                long recordsTotal = 0;
                var pageResults = _crimeIncidentReportFacade.GetPage(
                    (Search = string.IsNullOrEmpty(Search) ? string.Empty : Search),
                     IsAdvanceSearchMode,
                     ApprovalStatusId,
                     CrimeIncidentReportId,
                     CrimeIncidentCategoryName,
                     PostedByFullName,
                     DateReportedFrom,
                     DateReportedTo,
                     PossibleDateFrom,
                     PossibleDateTo,
                     PossibleTimeFrom,
                     PossibleTimeTo,
                     Description,
                     GeoStreet,
                     GeoDistrict,
                     GeoCityMun,
                     GeoProvince,
                     GeoCountry,
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

                return new SilupostAPIHttpActionResult<DataTableResponseModel<IList<CrimeIncidentReportViewModel>>>(Request, HttpStatusCode.OK, response);
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

        [Route("GetPageByTracker")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult GetPageByTracker(string TrackerRadiusInKM,
                                                                string TrackerPointLatitude,
                                                                string TrackerPointLongitude,
                                                                long ApprovalStatusId,
                                                                string CrimeIncidentCategoryIds,
                                                                DateTime DateReportedFrom,
                                                                DateTime DateReportedTo,
                                                                DateTime PossibleDateFrom,
                                                                DateTime PossibleDateTo,
                                                                string PossibleTimeFrom,
                                                                string PossibleTimeTo)
        {
            AppResponseModel<PageResultsViewModel<CrimeIncidentReportViewModel>> response = new AppResponseModel<PageResultsViewModel<CrimeIncidentReportViewModel>>();

            if (string.IsNullOrEmpty(PossibleTimeFrom) || !GlobalFunctions.IsValidTimeFormat(PossibleTimeFrom))
            {
                response.Message = string.Format(Messages.CustomError, "Invalid time format : PossibleTimeFrom");
                return new SilupostAPIHttpActionResult<AppResponseModel<PageResultsViewModel<CrimeIncidentReportViewModel>>>(Request, HttpStatusCode.BadRequest, response);
            }
            if (string.IsNullOrEmpty(PossibleTimeTo) || !GlobalFunctions.IsValidTimeFormat(PossibleTimeTo))
            {
                response.Message = string.Format(Messages.CustomError, "Invalid time format : PossibleTimeTo");
                return new SilupostAPIHttpActionResult<AppResponseModel<PageResultsViewModel<CrimeIncidentReportViewModel>>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {

                var result = _crimeIncidentReportFacade.GetByTracker(TrackerRadiusInKM,
                                                                        TrackerPointLatitude,
                                                                        TrackerPointLongitude,
                                                                        ApprovalStatusId,
                                                                        CrimeIncidentCategoryIds,
                                                                        DateReportedFrom,
                                                                        DateReportedTo,
                                                                        PossibleDateFrom,
                                                                        PossibleDateTo,
                                                                        PossibleTimeFrom,
                                                                        PossibleTimeTo);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<PageResultsViewModel<CrimeIncidentReportViewModel>>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new SilupostAPIHttpActionResult<AppResponseModel<PageResultsViewModel<CrimeIncidentReportViewModel>>>(Request, HttpStatusCode.NotFound, response);
                }


            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<PageResultsViewModel<CrimeIncidentReportViewModel>>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("GetTablePageByPostedBySystemUserId")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult GetTablePageByPostedBySystemUserId(int Draw, string PostedBySystemUserId, int PageNo, int PageSize)
        {
            DataTableResponseModel<IList<CrimeIncidentReportViewModel>> response = new DataTableResponseModel<IList<CrimeIncidentReportViewModel>>();

            try
            {
                long recordsFiltered = 0;
                long recordsTotal = 0;
                var pageResults = _crimeIncidentReportFacade.GetPageByPostedBySystemUserId(PostedBySystemUserId, PageNo, PageSize);
                var records = pageResults.Items.ToList();
                recordsTotal = pageResults.TotalRows;
                recordsFiltered = pageResults.TotalRows;

                response.draw = Draw;
                response.recordsFiltered = recordsFiltered;
                response.recordsTotal = recordsTotal;
                response.data = pageResults.Items.ToList();

                return new SilupostAPIHttpActionResult<DataTableResponseModel<IList<CrimeIncidentReportViewModel>>>(Request, HttpStatusCode.OK, response);
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


        [Route("GetPageByPostedBySystemUserId")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult GetPageByPostedBySystemUserId(string PostedBySystemUserId, int PageNo, int PageSize)
        {
            AppResponseModel<IList<CrimeIncidentReportViewModel>> response = new AppResponseModel<IList<CrimeIncidentReportViewModel>>();

            if (string.IsNullOrEmpty(PostedBySystemUserId))
            {
                response.Message = string.Format(Messages.InvalidId, "PostedBySystemUserId Crime Incident Report");
                return new SilupostAPIHttpActionResult<AppResponseModel<IList<CrimeIncidentReportViewModel>>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {

                var result = _crimeIncidentReportFacade.GetPageByPostedBySystemUserId(PostedBySystemUserId, PageNo, PageSize);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result.Items.ToList();
                    return new SilupostAPIHttpActionResult<AppResponseModel<IList<CrimeIncidentReportViewModel>>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new SilupostAPIHttpActionResult<AppResponseModel<IList<CrimeIncidentReportViewModel>>>(Request, HttpStatusCode.NotFound, response);
                }


            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<IList<CrimeIncidentReportViewModel>>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("{id}/detail")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult Get(string id)
        {
            AppResponseModel<CrimeIncidentReportViewModel> response = new AppResponseModel<CrimeIncidentReportViewModel>();

            if (string.IsNullOrEmpty(id))
            {
                response.Message = string.Format(Messages.InvalidId, "Crime Incident Report");
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                CrimeIncidentReportViewModel result = _crimeIncidentReportFacade.Find(id, false);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.NotFound, response);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("")]
        [HttpPost]
        [ValidateModel]
        [SwaggerOperation("create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult Create([FromBody] CreateCrimeIncidentReportBindingModel model)
        {
            AppResponseModel<CrimeIncidentReportViewModel> response = new AppResponseModel<CrimeIncidentReportViewModel>();

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }

                string id = _crimeIncidentReportFacade.Add(model, RecordedBy);

                if (!string.IsNullOrEmpty(id))
                {
                    var result = _crimeIncidentReportFacade.Find(id, false);

                    response.IsSuccess = true;
                    response.Message = Messages.Created;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.Created, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadRequest, response);

                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("")]
        [HttpPut]
        [ValidateModel]
        [SwaggerOperation("update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult Update([FromBody] UpdateCrimeIncidentReportBindingModel model)
        {
            AppResponseModel<CrimeIncidentReportViewModel> response = new AppResponseModel<CrimeIncidentReportViewModel>();

            if (model != null && string.IsNullOrEmpty(model.CrimeIncidentReportId))
            {
                response.Message = string.Format(Messages.InvalidId, "Crime Incident Report");
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }
                var result = _crimeIncidentReportFacade.Find(model.CrimeIncidentReportId, false);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "Crime Incident Report");
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadRequest, response);
                }
                bool success = _crimeIncidentReportFacade.Update(model, RecordedBy);
                response.IsSuccess = success;

                if (success)
                {
                    result = _crimeIncidentReportFacade.Find(model.CrimeIncidentReportId, false);
                    response.Message = Messages.Updated;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadGateway, response);
                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }


        [Route("UpdateStatus")]
        [HttpPut]
        [ValidateModel]
        [SwaggerOperation("update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult UpdateStatus([FromBody] UpdateCrimeIncidentReportStatusBindingModel model)
        {
            AppResponseModel<CrimeIncidentReportViewModel> response = new AppResponseModel<CrimeIncidentReportViewModel>();

            if (model != null && string.IsNullOrEmpty(model.CrimeIncidentReportId))
            {
                response.Message = string.Format(Messages.InvalidId, "Crime Incident Report");
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }
                var result = _crimeIncidentReportFacade.Find(model.CrimeIncidentReportId, false);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "Crime Incident Report");
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadRequest, response);
                }
                bool success = _crimeIncidentReportFacade.UpdateStatus(model, RecordedBy);
                response.IsSuccess = success;

                if (success)
                {
                    result = _crimeIncidentReportFacade.Find(model.CrimeIncidentReportId, false);
                    response.Message = Messages.Updated;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadGateway, response);
                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadRequest, response);
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
                response.Message = string.Format(Messages.InvalidId, "Crime Incident Report");
                return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }

                var result = _crimeIncidentReportFacade.Find(id, false);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "Crime Incident Report");
                    return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
                }

                bool success = _crimeIncidentReportFacade.Remove(id, RecordedBy);
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


        [Route("createReport")]
        [HttpPost]
        [ValidateModel]
        [SwaggerOperation("create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public async Task<IHttpActionResult> CreateReport()
        {
            AppResponseModel<CrimeIncidentReportViewModel> response = new AppResponseModel<CrimeIncidentReportViewModel>();

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }

                if (!this.Request.Content.IsMimeMultipartContent())
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                var root = HttpContext.Current.Server.MapPath(GlobalVariables.goDefaultSystemUploadRootDirectory);
                var dataDirectory = Path.Combine(root,"Data", RecordedBy);
                Directory.CreateDirectory(dataDirectory);
                var provider = new MultipartFormDataStreamProvider(dataDirectory);
                await Request.Content.ReadAsMultipartAsync(provider);

                var formDataModel = provider.FormData["model"];
                if (formDataModel == null)
                {
                    response.Message = Messages.Failed;
                    response.DeveloperMessage = string.Format(Messages.Empty, "Crime Incident Report Model");
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadRequest, response);
                }
                var model = JsonConvert.DeserializeObject<CreateCrimeIncidentReportBindingModel>(formDataModel);

                if(model == null)
                {
                    response.Message = Messages.Failed;
                    response.DeveloperMessage = string.Format(Messages.Empty, "Crime Incident Report Model");
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadRequest, response);
                }

                if (model.CrimeIncidentReportMedia == null)
                    model.CrimeIncidentReportMedia = new List<NewCrimeIncidentReportMediaBindingModel>();

                var storageDirectory = Path.Combine(root, @"Storage\", string.Format(@"{0}\", RecordedBy));
                foreach (var file in provider.FileData)
                {
                    var newFileName = string.Format("{0}{1}-{2}-{3}{4}", storageDirectory, RecordedBy, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("HH-mm"), GlobalFunctions.GetFileExtensionByFileRawFormat(file.Headers.ContentType.MediaType));
                    var media = new NewCrimeIncidentReportMediaBindingModel()
                    {
                        File = new FileBindingModel()
                        {
                            FileName = newFileName,
                            MimeType = file.Headers.ContentType.MediaType,
                            IsDefault = false,
                            FileContent = System.IO.File.ReadAllBytes(file.LocalFileName),
                            FileSize = new FileInfo(file.LocalFileName).Length
                        },
                        DocReportMediaTypeId = GlobalFunctions.GetFileTypeByFileExtension(GlobalFunctions.GetFileExtensionByFileRawFormat(file.Headers.ContentType.MediaType))
                    };
                    model.CrimeIncidentReportMedia.Add(media);
                }



                Directory.CreateDirectory(storageDirectory);
                string id = _crimeIncidentReportFacade.Add(model, RecordedBy);

                if (!string.IsNullOrEmpty(id))
                {
                    var result = _crimeIncidentReportFacade.Find(id, false);

                    try
                    {
                        System.IO.DirectoryInfo di = new DirectoryInfo(dataDirectory);

                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                        foreach (DirectoryInfo dir in di.GetDirectories())
                        {
                            dir.Delete(true);
                        }
                    }
                    catch { }

                    response.IsSuccess = true;
                    response.Message = Messages.Created;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.Created, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadRequest, response);

                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<CrimeIncidentReportViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }
    }
}
