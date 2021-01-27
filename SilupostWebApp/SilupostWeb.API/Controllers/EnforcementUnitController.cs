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
    [RoutePrefix("api/v1/EnforcementUnit")]
    public class EnforcementUnitController : ApiController
    {
        private readonly IEnforcementUnitFacade _enforcementUnitFacade;
        private readonly ILegalEntityAddressFacade _legalEntityAddressFacade;
        private string RecordedBy { get; set; }
        private long LocationId { get; set; }
        #region CONSTRUCTORS
        public EnforcementUnitController(IEnforcementUnitFacade enforcementUnitFacade, ILegalEntityAddressFacade legalEntityAddressFacade)
        {
            _enforcementUnitFacade = enforcementUnitFacade ?? throw new ArgumentNullException(nameof(enforcementUnitFacade));
            _legalEntityAddressFacade = legalEntityAddressFacade ?? throw new ArgumentNullException(nameof(legalEntityAddressFacade));
        }
        #endregion


        [Route("getPage")]
        [HttpGet]
        [SwaggerOperation("getPage")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetPage(int Draw, string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir)
        {
            DataTableResponseModel<IList<EnforcementUnitViewModel>> response = new DataTableResponseModel<IList<EnforcementUnitViewModel>>();

            try
            {
                long recordsFiltered = 0;
                long recordsTotal = 0;
                var pageResults = _enforcementUnitFacade.GetPage(
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

                return new SilupostAPIHttpActionResult<DataTableResponseModel<IList<EnforcementUnitViewModel>>>(Request, HttpStatusCode.OK, response);
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
            AppResponseModel<EnforcementUnitViewModel> response = new AppResponseModel<EnforcementUnitViewModel>();

            if (string.IsNullOrEmpty(id))
            {
                response.Message = string.Format(Messages.InvalidId, "Enforcement Unit");
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementUnitViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                EnforcementUnitViewModel result = _enforcementUnitFacade.Find(id);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementUnitViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementUnitViewModel>>(Request, HttpStatusCode.NotFound, response);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementUnitViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("")]
        [HttpPost]
        [ValidateModel]
        [SwaggerOperation("create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult Create([FromBody] CreateEnforcementUnitBindingModel model)
        {
            AppResponseModel<EnforcementUnitViewModel> response = new AppResponseModel<EnforcementUnitViewModel>();

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }
                string id = _enforcementUnitFacade.Add(model, RecordedBy);

                if (!string.IsNullOrEmpty(id))
                {
                    var result = _enforcementUnitFacade.Find(id);

                    response.IsSuccess = true;
                    response.Message = Messages.Created;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementUnitViewModel>>(Request, HttpStatusCode.Created, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementUnitViewModel>>(Request, HttpStatusCode.BadRequest, response);

                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementUnitViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("")]
        [HttpPut]
        [ValidateModel]
        [SwaggerOperation("update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult Update([FromBody] UpdateEnforcementUnitBindingModel model)
        {
            AppResponseModel<EnforcementUnitViewModel> response = new AppResponseModel<EnforcementUnitViewModel>();

            if (model != null && string.IsNullOrEmpty(model.EnforcementUnitId))
            {
                response.Message = string.Format(Messages.InvalidId, "Enforcement Unit");
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementUnitViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }
                var result = _enforcementUnitFacade.Find(model.EnforcementUnitId);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "Enforcement Unit");
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementUnitViewModel>>(Request, HttpStatusCode.BadRequest, response);
                }
                bool success = _enforcementUnitFacade.Update(model, RecordedBy);
                response.IsSuccess = success;

                if (success)
                {
                    result = _enforcementUnitFacade.Find(model.EnforcementUnitId);
                    response.Message = Messages.Updated;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementUnitViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementUnitViewModel>>(Request, HttpStatusCode.BadGateway, response);
                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<EnforcementUnitViewModel>>(Request, HttpStatusCode.BadRequest, response);
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
                response.Message = string.Format(Messages.InvalidId, "Enforcement Unit");
                return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }

                var result = _enforcementUnitFacade.Find(id);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "Enforcement Unit");
                    return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
                }

                bool success = _enforcementUnitFacade.Remove(id, RecordedBy);
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

        [Route("GetAddressByLegalEntityId")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult GetAddressByLegalEntityId(string legalEntityId)
        {
            AppResponseModel<List<LegalEntityAddressViewModel>> response = new AppResponseModel<List<LegalEntityAddressViewModel>>();

            if (string.IsNullOrEmpty(legalEntityId))
            {
                response.Message = string.Format(Messages.InvalidId, "Enforcement Unit LegalEntity Address");
                return new SilupostAPIHttpActionResult<AppResponseModel<List<LegalEntityAddressViewModel>>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                List<LegalEntityAddressViewModel> result = _legalEntityAddressFacade.FindByLegalEntityId(legalEntityId);

                if (result != null)
                {
                    response.IsSuccess = true;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<List<LegalEntityAddressViewModel>>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.NoRecord;
                    return new SilupostAPIHttpActionResult<AppResponseModel<List<LegalEntityAddressViewModel>>>(Request, HttpStatusCode.NotFound, response);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<List<LegalEntityAddressViewModel>>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("createEnforcementUnitAddress")]
        [HttpPost]
        [ValidateModel]
        [SwaggerOperation("create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult CreateEnforcementUnitAddress([FromBody] CreateLegalEntityAddressBindingModel model)
        {
            AppResponseModel<LegalEntityAddressViewModel> response = new AppResponseModel<LegalEntityAddressViewModel>();

            try
            {
                string id = _legalEntityAddressFacade.Add(model);

                if (!string.IsNullOrEmpty(id))
                {
                    var result = _legalEntityAddressFacade.Find(id);

                    response.IsSuccess = true;
                    response.Message = Messages.Created;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<LegalEntityAddressViewModel>>(Request, HttpStatusCode.Created, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<LegalEntityAddressViewModel>>(Request, HttpStatusCode.BadRequest, response);

                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<LegalEntityAddressViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("UpdateEnforcementUnitAddress")]
        [HttpPut]
        [ValidateModel]
        [SwaggerOperation("update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult UpdateEnforcementUnitAddress([FromBody] UpdateLegalEntityAddressBindingModel model)
        {
            AppResponseModel<LegalEntityAddressViewModel> response = new AppResponseModel<LegalEntityAddressViewModel>();

            if (model != null && string.IsNullOrEmpty(model.LegalEntityAddressId))
            {
                response.Message = string.Format(Messages.InvalidId, "Enforcement Unit LegalEntity Address");
                return new SilupostAPIHttpActionResult<AppResponseModel<LegalEntityAddressViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var result = _legalEntityAddressFacade.Find(model.LegalEntityAddressId);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "Enforcement Unit LegalEntity Address");
                    return new SilupostAPIHttpActionResult<AppResponseModel<LegalEntityAddressViewModel>>(Request, HttpStatusCode.BadRequest, response);
                }
                bool success = _legalEntityAddressFacade.Update(model);
                response.IsSuccess = success;

                if (success)
                {
                    result = _legalEntityAddressFacade.Find(model.LegalEntityAddressId);
                    response.Message = Messages.Updated;
                    response.Data = result;
                    return new SilupostAPIHttpActionResult<AppResponseModel<LegalEntityAddressViewModel>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<LegalEntityAddressViewModel>>(Request, HttpStatusCode.BadGateway, response);
                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<LegalEntityAddressViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("RemoveEnforcementUnitAddress/{id}")]
        [HttpDelete]
        [SwaggerOperation("remove")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult RemoveEnforcementUnitAddress(string id)
        {
            AppResponseModel<object> response = new AppResponseModel<object>();

            if (string.IsNullOrEmpty(id))
            {
                response.Message = string.Format(Messages.InvalidId, "Enforcement Unit LegalEntity Address");
                return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {

                var result = _legalEntityAddressFacade.Find(id);
                if (result == null)
                {
                    response.Message = string.Format(Messages.InvalidId, "Enforcement Unit LegalEntity Address");
                    return new SilupostAPIHttpActionResult<AppResponseModel<object>>(Request, HttpStatusCode.BadRequest, response);
                }

                bool success = _legalEntityAddressFacade.Remove(id);
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
