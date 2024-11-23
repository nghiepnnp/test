namespace WebAPI.Utilities
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public ApiResult(object? data)
        {
            IsSuccess = true;
            Data = data;
        }

        public ApiResult(string errorCode, string message)
        {
            IsSuccess = false;
            ErrorCode = errorCode;
            Message = message;
        }

        public static ApiResult Success()
        {
            return new ApiResult(default);
        }

        public static ApiResult Success(object data)
        {
            return new ApiResult(data);
        }

        public static ApiResult Failure(string errorCode, string message)
        {
            return new ApiResult(errorCode, message);
        }
    }
}
