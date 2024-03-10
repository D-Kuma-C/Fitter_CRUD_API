namespace Fitter_API.Controllers.Exceptions
{
    public class NotFoundException : System.Exception
    {
        public int StatusCode { get; set; } = 404;
        public string Message { get; }

        public NotFoundException(string message)
        {
            Message = message;
        }
    }
}
