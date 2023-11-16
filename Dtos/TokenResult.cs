namespace SampleJWT.Dtos
{
    public class TokenResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public List<string> Errors { get; set; }
    }
}
