using Dapper;
using Elasticsearch.Net;
using Microsoft.Extensions.Caching.Memory;
using Nest;
using NewPrjESDEDIBE.DbAccess;
using NewPrjESDEDIBE.Models.Dtos.Common;
using System.Data;
using static NewPrjESDEDIBE.Extensions.ServiceExtensions;

namespace NewPrjESDEDIBE.Services.Common
{
    public interface IRefreshTokenService
    {
        Task<string> Create(RefreshTokenDto model);
        Task<string> Modify(RefreshTokenDto model);
        Task<IEnumerable<string>> GetUserTokensByUserId(long userId);
        Task<RefreshTokenDto?> Get(UserRefreshTokenRequest request);
        Task<HashSet<string>> GetAvailables();

        // void UpdateAvailableTokensCache();

    }

    [SingletonRegistration]
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        // private readonly IMemoryCache _memoryCache;

        public RefreshTokenService(
            ISqlDataAccess sqlDataAccess
            // , IMemoryCache memoryCache
            )
        {
            _sqlDataAccess = sqlDataAccess;
            // _memoryCache = memoryCache;
        }

        public async Task<string> Create(RefreshTokenDto model)
        {
            string proc = "sysUsp_RefreshToken_Create";
            var param = new DynamicParameters();
            param.Add("@accessToken", model.accessToken);
            param.Add("@refreshToken", model.refreshToken);
            param.Add("@createdDate", model.createdDate);
            param.Add("@expirationDate", model.expiredDate);
            param.Add("@ipAddress", model.ipAddress);
            param.Add("@userId", model.userId);
            param.Add("@isOnApp", model.isOnApp);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure
            return await _sqlDataAccess.SaveDataUsingStoredProcedure<string>(proc, param); ;
        }

        //Mã trên định nghĩa một phương thức Create trong một dịch vụ (service) nào đó, có thể là
        //một dịch vụ liên quan đến quản lý Refresh Token.

        //Phương thức này có một tham số đầu vào là RefreshTokenDto model, chứa thông tin về Refresh
        //Token cần tạo.

        //Phương thức này sử dụng một stored procedure có tên là "sysUsp_RefreshToken_Create" để tạo
        //Refresh Token trong cơ sở dữ liệu.Để gọi stored procedure, phương thức này sử dụng một đối
        //tượng DynamicParameters để chứa các tham số cần thiết cho stored procedure, như là accessToken,
        //refreshToken, createdDate, expirationDate, ipAddress, userId, và isOnApp.

        //Sau đó, phương thức gọi phương thức SaveDataUsingStoredProcedure từ đối tượng _sqlDataAccess
        //để thực hiện lưu trữ dữ liệu bằng cách sử dụng stored procedure đã được chỉ định.Đối số đầu
        //tiên của phương thức này là tên của stored procedure ("sysUsp_RefreshToken_Create"), đối số
        //thứ hai là các tham số cần thiết(param). Phương thức này trả về một chuỗi kết quả từ việc thực
        //thi stored procedure.

        //Phương thức Create có kiểu trả về là Task<string>, cho thấy nó là một phương thức bất đồng bộ
        //(asynchronous) và sẽ trả về một chuỗi sau khi hoàn thành.

        public async Task<RefreshTokenDto?> Get(UserRefreshTokenRequest request)
        {
            string proc = "sysUsp_RefreshToken_Get";
            var param = new DynamicParameters();
            param.Add("@accessToken", request.expiredToken);
            param.Add("@refreshToken", request.refreshToken);
            param.Add("@ipAddress", request.ipAddress);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<RefreshTokenDto>(proc, param);
            return data.FirstOrDefault() ?? default;
        }

        public async Task<HashSet<string>> GetAvailables()
        {
            string proc = "sysUsp_RefreshToken_GetAvailables";
            var param = new DynamicParameters();
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<string>(proc, param);
            return data.ToHashSet();
        }

        public async Task<IEnumerable<string>> GetUserTokensByUserId(long userId)
        {
            string proc = "sysUsp_RefreshToken_GetUserTokensByUserId";
            var param = new DynamicParameters();
            param.Add("@userId", userId);
            var returnData = await _sqlDataAccess.LoadDataUsingStoredProcedure<string>(proc, param);
            return returnData;
        }

        public async Task<string> Modify(RefreshTokenDto model)
        {
            string proc = "sysUsp_RefreshToken_Modify";
            var param = new DynamicParameters();
            param.Add("@refreshTokenId", model.refreshTokenId);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output);//luôn để DataOutput trong stored procedure
            return await _sqlDataAccess.SaveDataUsingStoredProcedure<string>(proc, param);
        }

        // public async void UpdateAvailableTokensCache()
        // {
        //     var availableTokens = await GetAvailables();
        //     _memoryCache.Remove("availableTokens");
        //     _memoryCache.Set("availableTokens", availableTokens);
        // }
    }
}
