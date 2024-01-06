using NewPrjESDEDIBE.CustomAttributes;

namespace ESD_EDI_BE.Models.Dtos.Common
{
    [JSGenerateAttribute]
    public class PageModel
    {
        public int? page { get; set; }
        public int? pageSize { get; set; }
        public int? totalRow { get; set; }


        public PageModel()
        {
            page = 1;
            pageSize = 10;
        }
    }
}
