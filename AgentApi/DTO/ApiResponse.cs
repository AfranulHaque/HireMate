namespace AgentApi.DTO
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T data { get; set; }
    }
}
