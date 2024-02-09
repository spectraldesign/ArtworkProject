namespace Application
{
    public class ApiResponseType<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;

        public ApiResponseType(T data, bool success, string message = "")
        {
            Data = data;
            Success = success;
            Message = message;
        }

        public ApiResponseType() { }
    }
}
