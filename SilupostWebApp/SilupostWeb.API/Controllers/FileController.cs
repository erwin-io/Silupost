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
    [RoutePrefix("api/v1/File")]
    public class FileController : ApiController
    {
        private string RecordedBy { get; set; }
        private long LocationId { get; set; }
        #region CONSTRUCTORS
        public FileController()
        {
        }
        #endregion

        [Route("getFile")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetFile(string FileId)
        {
            AppResponseModel<FileViewModel> response = new AppResponseModel<FileViewModel>();

            try
            {
                string filePath = HttpContext.Current.Server.MapPath(string.Format("~/App_Data/UploadedFiles/{0}", FileId));
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                var fileSize = new FileInfo(filePath).Length;
                using (Image image = Image.FromFile(filePath))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        var file = new FileViewModel()
                        {
                            FileName = fileName,
                            FileSize = int.Parse(fileSize.ToString()),
                            MimeType = image.RawFormat.ToString()
                        };
                        response.Data = file;
                        response.IsSuccess = true;
                        return new SilupostAPIHttpActionResult<AppResponseModel<FileViewModel>>(Request, HttpStatusCode.OK, response);
                    }
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<FileViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("getDefaultSystemUserProfilePic")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetDefaultSystemUserProfilePic()
        {
            AppResponseModel<FileViewModel> response = new AppResponseModel<FileViewModel>();

            try
            {
                string filePath = HttpContext.Current.Server.MapPath(GlobalVariables.goDefaultSystemUserProfilePicPath);
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                var fileSize = new FileInfo(filePath).Length;
                using (Image image = Image.FromFile(filePath))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        var file = new FileViewModel()
                        {
                            FileName = fileName,
                            FileSize = int.Parse(fileSize.ToString()),
                            MimeType = image.RawFormat.ToString(),
                            FileContent = imageBytes
                        };
                        response.Data = file;
                        response.IsSuccess = true;
                        return new SilupostAPIHttpActionResult<AppResponseModel<FileViewModel>>(Request, HttpStatusCode.OK, response);
                    }
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<FileViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }

        [Route("getDefaultCrimeIncidentTypeProfilePic")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetDefaultCrimeIncidentTypeProfilePic()
        {
            AppResponseModel<FileViewModel> response = new AppResponseModel<FileViewModel>();

            try
            {
                string filePath = HttpContext.Current.Server.MapPath(GlobalVariables.goDefaultCrimeIncidentTypeIconFilePath);
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                var fileSize = new FileInfo(filePath).Length;
                using (Image image = Image.FromFile(filePath))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        var file = new FileViewModel()
                        {
                            FileName = fileName,
                            FileSize = int.Parse(fileSize.ToString()),
                            MimeType = image.RawFormat.ToString(),
                            FileContent = imageBytes
                        };
                        response.Data = file;
                        response.IsSuccess = true;
                        return new SilupostAPIHttpActionResult<AppResponseModel<FileViewModel>>(Request, HttpStatusCode.OK, response);
                    }
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<FileViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }


        [Route("getDefaultEnforcementTypeProfilePic")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetDefaultEnforcementTypeProfilePic()
        {
            AppResponseModel<FileViewModel> response = new AppResponseModel<FileViewModel>();

            try
            {
                string filePath = HttpContext.Current.Server.MapPath(GlobalVariables.goDefaultEnforcementTypeIconFilePath);
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                var fileSize = new FileInfo(filePath).Length;
                using (Image image = Image.FromFile(filePath))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        var file = new FileViewModel()
                        {
                            FileName = fileName,
                            FileSize = int.Parse(fileSize.ToString()),
                            MimeType = image.RawFormat.ToString(),
                            FileContent = imageBytes
                        };
                        response.Data = file;
                        response.IsSuccess = true;
                        return new SilupostAPIHttpActionResult<AppResponseModel<FileViewModel>>(Request, HttpStatusCode.OK, response);
                    }
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<FileViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }


        [Route("getDefaultEnforcementUnitProfilePic")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetDefaultEnforcementUnitProfilePic()
        {
            AppResponseModel<FileViewModel> response = new AppResponseModel<FileViewModel>();

            try
            {
                string filePath = HttpContext.Current.Server.MapPath(GlobalVariables.goDefaultEnforcementUnitIconFilePicPath);
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                var fileSize = new FileInfo(filePath).Length;
                using (Image image = Image.FromFile(filePath))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        var file = new FileViewModel()
                        {
                            FileName = fileName,
                            FileSize = int.Parse(fileSize.ToString()),
                            MimeType = image.RawFormat.ToString(),
                            FileContent = imageBytes
                        };
                        response.Data = file;
                        response.IsSuccess = true;
                        return new SilupostAPIHttpActionResult<AppResponseModel<FileViewModel>>(Request, HttpStatusCode.OK, response);
                    }
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<FileViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }


        [Route("getDefaultEnforcementStationProfilePic")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetDefaultEnforcementStationProfilePic()
        {
            AppResponseModel<FileViewModel> response = new AppResponseModel<FileViewModel>();

            try
            {
                string filePath = HttpContext.Current.Server.MapPath(GlobalVariables.goDefaultEnforcementStationIconFilePath);
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                var fileSize = new FileInfo(filePath).Length;
                using (Image image = Image.FromFile(filePath))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        var file = new FileViewModel()
                        {
                            FileName = fileName,
                            FileSize = int.Parse(fileSize.ToString()),
                            MimeType = image.RawFormat.ToString(),
                            FileContent = imageBytes
                        };
                        response.Data = file;
                        response.IsSuccess = true;
                        return new SilupostAPIHttpActionResult<AppResponseModel<FileViewModel>>(Request, HttpStatusCode.OK, response);
                    }
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                //TODO Logging of exceptions
                return new SilupostAPIHttpActionResult<AppResponseModel<FileViewModel>>(Request, HttpStatusCode.BadRequest, response);
            }
        }
    }
}
