namespace Application
{
    public class ApiResponseType<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ResponseCode { get; set; }

        public ApiResponseType(T data, bool success, string message = "", int responseCode = 200)
        {
            Data = data;
            Success = success;
            ResponseCode = responseCode;
            Message = message;
        }
    }
}
