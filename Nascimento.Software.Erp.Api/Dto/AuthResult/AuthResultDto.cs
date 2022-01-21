namespace Nascimento.Software.Erp.Api.Dto.AuthResult
{
    public class AuthResultDto
    {
        public AuthResultDto()
        {
            Errors = new List<string>();
        }
        public List<string> Errors { get; set; }
        public bool Succesfull { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

    }
}
