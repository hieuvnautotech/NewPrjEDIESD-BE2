using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewPrjESDEDIBE.Models.Dtos.Redis;

namespace NewPrjESDEDIBE.Models.Dtos.Common
{
    public class RoleMenuDto
    {
        public string roleCode { get; set; }
        public IEnumerable<RoleMenuRedis>? Menus { get; set; }
    }
}