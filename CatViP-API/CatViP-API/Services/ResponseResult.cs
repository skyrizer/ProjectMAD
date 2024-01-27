namespace CatViP_API.Services
{
    public class ResponseResult<T>
    {
        public bool IsSuccessful { get; set; } = true;
        public string ErrorMessage { get; set; } = string.Empty;
        public T? Result { get; set; }
    }

    public class ResponseResult
    {
        public bool IsSuccessful { get; set; } = true;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
