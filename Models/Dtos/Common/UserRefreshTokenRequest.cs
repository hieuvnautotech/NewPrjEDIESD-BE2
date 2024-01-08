using System.ComponentModel.DataAnnotations;
using NewPrjESDEDIBE.CustomAttributes;

namespace NewPrjESDEDIBE.Models.Dtos.Common
{
    [JSGenerateAttribute]
    public class UserRefreshTokenRequest
    {
        [Required]
        public string? expiredToken { get; set; }
        [Required]
        public string? refreshToken { get; set; }
        public string? ipAddress { get; set; }
    }
}
