using Microsoft.EntityFrameworkCore;
using NewPrjESDEDIBE.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace NewPrjESDEDIBE.Models.Dtos.Common
{
    [JSGenerateAttribute]
    public class LoginModelDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        [Unicode(false)]
        public string? userName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        [Unicode(false)]
        public string? userPassword { get; set; }

        public bool? isOnApp { get; set; } = false;

        public LoginModelDto()
        {
            isOnApp = false;
        }
    }
}
