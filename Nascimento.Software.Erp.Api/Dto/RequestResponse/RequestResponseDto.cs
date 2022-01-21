namespace Nascimento.Software.Erp.Api.Dto.RequestResponse
{
    public class RequestResponseDto
    {
        public RequestResponseDto()
        {
            Messages = new List<string>();
        }
        public bool Success { get; set; } = false;
        public List<string> Messages { get; private set; }

        public void AddSucessMessage()
        {
            Success = true;
            Messages.Add("Operation was completed sucessfuly");
        }
        public void AddErrorMessage(string failReason)
        {
            Success = false;
            Messages.Add($"The operation wasn't completed sucessfuly. Take notes: {failReason}");
        }
    }
}