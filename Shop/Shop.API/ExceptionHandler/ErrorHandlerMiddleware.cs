using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shop.API.ApiModel.Response;

namespace Shop.API.ExceptionHandler
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                ApiResponse apiRespponse;

                apiRespponse = ApiResponse.CreateFailedResponse(error.Message);

                var result = JsonConvert.SerializeObject(apiRespponse, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                await response.WriteAsync(result);
            }
        }
    }
}
