using NewPrjESDEDIBE.CustomAttributes;

namespace NewPrjESDEDIBE.Models.Dtos.Common
{
    [JSGenerateAttribute]
    public class AuthorizationResponse
    {
        public string? accessToken { get; set; } = default;
        public string? refreshToken { get; set; } = default;
        public bool isSuccess { get; set; } = true;
        public string? reason { get; set; } = default; 
    }
}
