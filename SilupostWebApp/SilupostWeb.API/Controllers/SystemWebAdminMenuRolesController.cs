﻿using Newtonsoft.Json;
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
    [SilupostAuthorizationFilter]
    [RoutePrefix("api/v1/SystemWebAdminMenuRoles")]
    public class SystemWebAdminMenuRolesController : ApiController
    {
        private readonly ISystemWebAdminMenuRolesFacade _systemWebAdminMenuRolesFacade;
        private string RecordedBy { get; set; }
        #region CONSTRUCTORS
        public SystemWebAdminMenuRolesController(ISystemWebAdminMenuRolesFacade systemWebAdminMenuRolesFacade)
        {
            _systemWebAdminMenuRolesFacade = systemWebAdminMenuRolesFacade ?? throw new ArgumentNullException(nameof(systemWebAdminMenuRolesFacade));
        }
        #endregion


        [Route("getBySystemWebAdminRoleIdAndSystemWebAdminModuleId")]
        [HttpGet]
        [SwaggerOperation("")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetBySystemWebAdminRoleIdAndSystemWebAdminModuleId(string SystemWebAdminRoleId, long SystemWebAdminModuleId)
        {
            AppResponseModel<List<SystemWebAdminMenuRolesViewModel>> response = new AppResponseModel<List<SystemWebAdminMenuRolesViewModel>>();

            try
            {
                var data = _systemWebAdminMenuRolesFacade.FindBySystemWebAdminRoleIdandSystemWebAdminModuleId(SystemWebAdminRoleId, SystemWebAdminModuleId);
                response.Data = data;
                response.IsSuccess = true;
                return new SilupostAPIHttpActionResult<AppResponseModel<List<SystemWebAdminMenuRolesViewModel>>>(Request, HttpStatusCode.OK, response);

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<List<SystemWebAdminMenuRolesViewModel>>>(Request, HttpStatusCode.OK, response);
            }
        }

        [Route("SetSystemWebAdminMenuRoles")]
        [HttpPut]
        [ValidateModel]
        [SwaggerOperation("SetSystemWebAdminMenuRoles")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult SetSystemWebAdminMenuRoles([FromBody] SystemWebAdminMenuRolesBindingModel model)
        {
            AppResponseModel<List<SystemWebAdminMenuRolesViewModel>> response = new AppResponseModel<List<SystemWebAdminMenuRolesViewModel>>();

            if (model != null && string.IsNullOrEmpty(model.SystemWebAdminRoleId))
            {
                response.Message = string.Format(Messages.InvalidId, "System Web Admin Menu Role");
                return new SilupostAPIHttpActionResult<AppResponseModel<List<SystemWebAdminMenuRolesViewModel>>>(Request, HttpStatusCode.BadRequest, response);
            }

            if (!model.SystemWebAdminMenu.Any())
            {
                response.Message = string.Format(Messages.InvalidId, "System Web Admin Menu Role");
                return new SilupostAPIHttpActionResult<AppResponseModel<List<SystemWebAdminMenuRolesViewModel>>>(Request, HttpStatusCode.BadRequest, response);
            }

            if (model.SystemWebAdminMenu.Any(m=>m.SystemWebAdminMenuId == null || m.SystemWebAdminMenuId <= 0))
            {
                response.Message = string.Format(Messages.InvalidId, "System Web Admin Menu Role");
                return new SilupostAPIHttpActionResult<AppResponseModel<List<SystemWebAdminMenuRolesViewModel>>>(Request, HttpStatusCode.BadRequest, response);
            }

            try
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    RecordedBy = identity.FindFirst("SystemUserId").Value;
                }
                bool success = _systemWebAdminMenuRolesFacade.Set(model, RecordedBy);
                response.IsSuccess = success;

                if (success)
                {
                    var data = _systemWebAdminMenuRolesFacade.FindBySystemWebAdminRoleId(model.SystemWebAdminRoleId);
                    response.Message = Messages.Updated;
                    response.Data = data;
                    return new SilupostAPIHttpActionResult<AppResponseModel<List<SystemWebAdminMenuRolesViewModel>>>(Request, HttpStatusCode.OK, response);
                }
                else
                {
                    response.Message = Messages.Failed;
                    return new SilupostAPIHttpActionResult<AppResponseModel<List<SystemWebAdminMenuRolesViewModel>>>(Request, HttpStatusCode.BadGateway, response);
                }
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<List<SystemWebAdminMenuRolesViewModel>>>(Request, HttpStatusCode.BadRequest, response);
            }
        }
    }
}
