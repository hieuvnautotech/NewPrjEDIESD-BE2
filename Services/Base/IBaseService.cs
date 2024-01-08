﻿using NewPrjESDEDIBE.Models;
using NewPrjESDEDIBE.Models.Dtos.Common;

namespace NewPrjESDEDIBE.Services.Base
{
    public interface IBaseService<T>
    {
        Task<ResponseModel<IEnumerable<T>?>> Get(PageModel pageInfo, string roleName, string? keyWord, bool showDelete);
        //Task<ResponseModel<T?>> GetById(long id);
        Task<ResponseModel<T?>> GetByCode(string code);
        Task<string> Create(T model);
        Task<string> Modify(T model);
        Task<string> Delete(string commonMasterCode, bool isActived, byte[] row_version);

    }
}
