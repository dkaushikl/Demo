namespace Demo.Utility
{
    public static class ApiResponse
    {
        public static object SetResponse(ApiResponseStatus responseStatus, string message, object data)
        {
            return new { responseStatus, message, data };
        }
    }
}