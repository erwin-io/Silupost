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
using System.Net.Http.Headers;
using System.Threading;

namespace SilupostWeb.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/File")]
    public class FileController : ApiController
    {
        private readonly IFileFacade _fileFacade;
        private string RecordedBy { get; set; }
        #region CONSTRUCTORS
        public FileController(IFileFacade fileFacade)
        {
            _fileFacade = fileFacade ?? throw new ArgumentNullException(nameof(fileFacade));
        }

        #endregion
        //[AllowAnonymous]
        [Route("getFile")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetFile(string FileId)
        {
            IHttpActionResult response;

            try
            {
                var result = _fileFacade.Find(FileId);
                string mimeType = MimeMapping.GetMimeMapping(result.FileName);
                var contentType = new MediaTypeHeaderValue(mimeType);
                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                Stream fileStream;
                if (result.IsFromStorage)
                {
                    fileStream = new MemoryStream(System.IO.File.ReadAllBytes(result.FileName));
                }
                else
                {
                    fileStream = new MemoryStream(result.FileContent);
                }
                responseMessage.Content = new StreamContent(fileStream);
                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = result.FileName,
                    Inline = false,
                };
                responseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue(cd.DispositionType.ToString());
                responseMessage.Content.Headers.ContentDisposition.FileName = result.FileName;
                //responseMessage.Content.Headers.ContentType = contentType;
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = ResponseMessage(responseMessage);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
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


        [Route("GetDefaultCrimeReportMarkerIcon")]
        [HttpGet]
        [SwaggerOperation("get")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetDefaultCrimeReportMarkerIcon()
        {
            IHttpActionResult response;

            try
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                string filePath = HttpContext.Current.Server.MapPath(GlobalVariables.goDefaultCrimeReportMarkerIconFilePath);
                string fileName = Path.GetFileName(filePath);
                Stream fileStream = new MemoryStream(System.IO.File.ReadAllBytes(filePath));
                responseMessage.Content = new StreamContent(fileStream);
                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = fileName,
                    Inline = false,
                };
                responseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue(cd.DispositionType.ToString());
                responseMessage.Content.Headers.ContentDisposition.FileName = fileName;
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = ResponseMessage(responseMessage);
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
