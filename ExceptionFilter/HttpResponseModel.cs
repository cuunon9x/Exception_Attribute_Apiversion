namespace _2_Exception_TrinhCV.ExceptionFilter
{
    public class HttpResponseModel
    {
        public HttpErrorModel Error { get; set; }
        public HttpErrorModel Success { get; set; }
        public object Data { get; set; }
    }
}
