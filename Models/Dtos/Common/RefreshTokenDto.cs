using System.ComponentModel.DataAnnotations;
using NewPrjESDEDIBE.CustomAttributes;

namespace NewPrjESDEDIBE.Models.Dtos.Common
{
    [JSGenerateAttribute]
    public class RefreshTokenDto
    {
        public long refreshTokenId { get; set; }
        public string? refreshToken { get; set; }
        public string? accessToken { get; set; }
        public DateTime? createdDate { get; set; }
        public DateTime? expiredDate { get; set; }
        public bool isActive
        {
            get
            {
                return expiredDate > DateTime.UtcNow;
            }
        }
        [StringLength(50)]
        public string? ipAddress { get; set; }
        public bool isValidated { get; set; }
        public long userId { get; set; }
        public bool? isOnApp { get; set; } = false;
    }
}
