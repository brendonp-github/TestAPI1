namespace TestAPI1.Data
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string? Token { get; set; }
    }
}
