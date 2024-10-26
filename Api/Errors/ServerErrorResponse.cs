namespace Api.Errors
{
    public class ServerErrorResponse:ApiResponse
    {
        public string? Details { get; set; }
        public ServerErrorResponse(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }
    }
}
