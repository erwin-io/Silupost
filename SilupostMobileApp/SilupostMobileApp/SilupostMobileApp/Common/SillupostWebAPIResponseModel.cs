

namespace SilupostMobileApp.Common
{
    public class SillupostWebAPIResponseModel<T>
    {
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; }
        public string DeveloperMessage { get; set; }
        public T Data { get; set; }
        public bool IsWarning { get; set; } = false;

        public SillupostWebAPIResponseModel()
        {
            this.Message = string.Empty;
            this.DeveloperMessage = string.Empty;
        }
    }
}
