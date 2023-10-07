namespace _2_Exception_TrinhCV.ExceptionFilter
{
    public class HttpErrorModel
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public object Errors { get; set; }
    }
}
