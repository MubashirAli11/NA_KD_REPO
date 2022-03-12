using Newtonsoft.Json;

namespace Shop.API.ApiModel.Response
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object? Data { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Total { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PageIndex { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PageSize { get; set; }

        public ApiResponse(bool isSuccess, int statusCode, string message, object? data, int? total, int? pageIndex, int? pageSize)
        {
            this.IsSuccess = isSuccess;
            this.StatusCode = statusCode;
            this.Message = message;
            this.Data = data;
            this.Total = total;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        public static ApiResponse CreateSuccessResponse(string message, object data)
        {
            return new ApiResponse(true, 200, message, data, null, null, null);
        }

        public static ApiResponse ReturnCreatedResponse(string message, object data)
        {
            return new ApiResponse(true, 201, message, data, null, null, null);
        }

        public static ApiResponse CreateFailedResponse(string message)
        {
            return new ApiResponse(false, 400, message, null, null, null, null);
        }

        public static ApiResponse CreateUnauthorizeResponse()
        {
            return new ApiResponse(false, 401, "", null, null, null, null);
        }

        public static ApiResponse CreateNotFoundResponse()
        {
            return new ApiResponse(false, 404, "", null, null, null, null);
        }

        public static ApiResponse CreateForbiddenResponse()
        {
            return new ApiResponse(false, 403, "", null, null, null, null);
        }

        public static ApiResponse CreateNoContentResponse()
        {
            return new ApiResponse(false, 204, "", null, null, null, null);
        }

        public static ApiResponse CreatePaginationResponse(string message, object data, int? total, int? pageIndex, int? pageSize)

        {
            return new ApiResponse(true, 200, message, data, total, pageIndex, pageSize);
        }
    }
}
