namespace Nemezida.Rationalizator.Web.ViewModels
{
    using System;

    public class ApiErrorResponce
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public ApiErrorResponce(Exception ex)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
            StackTrace = ex.ToString();
        }
    }
}
