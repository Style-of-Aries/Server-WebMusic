namespace MyApi.Middlewares // hoặc MyApi.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public string? Details { get; set; }

        // Ghi đè ToString để dùng System.Text.Json hoặc Newtonsoft.Json
        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}