using Dapper;
using NewPrjESDEDIBE.DbAccess;
using NewPrjESDEDIBE.Extensions;
using NewPrjESDEDIBE.Models.Dtos.Common;
using System.Data;
using static NewPrjESDEDIBE.Extensions.ServiceExtensions;

namespace NewPrjESDEDIBE.Services.Common
{
    public interface ILoginService
    {
        Task<string> CheckLogin(LoginModelDto model);
        Task<ResponseModel<IEnumerable<UserDto>?>> GetOnlineUsers();
    }

    [SingletonRegistration]
    public class LoginService : ILoginService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public LoginService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        //Mã bạn cung cấp định nghĩa một dịch vụ (service) khác, được gọi là LoginService.
        //Constructor của LoginService nhận một tham chiếu đến một đối tượng cung cấp truy
        //cập cơ sở dữ liệu (SQL data access) thông qua giao diện ISqlDataAccess.

        //Có vẻ như LoginService được thiết kế để thực hiện các chức năng liên quan đến
        //quản lý và xử lý việc đăng nhập trong ứng dụng.Bằng cách chuyển vào một đối tượng
        //ISqlDataAccess, dịch vụ này có thể tương tác với cơ sở dữ liệu để xác thực thông tin
        //đăng nhập và thực hiện các tác vụ liên quan đến quản lý tài khoản người dùng.
        public async Task<string> CheckLogin(LoginModelDto model) //từ FE nhập user pw truyền vào model, ở controller sẽ load từ model ra rồi truyền vào db để query theo store
        {

            model.userPassword = MD5Encryptor.MD5Hash(model.userPassword);

            string proc = "sysUsp_User_CheckLogin"; // gọi store ra để truyền tham số vào
            var param = new DynamicParameters();
            param.Add("@userName", model.userName);
            param.Add("@userPassword", model.userPassword);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            await _sqlDataAccess.LoadDataUsingStoredProcedure<string>(proc, param);
            string result = param.Get<string?>("@output") ?? string.Empty;
            return result;
        }

        public async Task<ResponseModel<IEnumerable<UserDto>?>> GetOnlineUsers() //lấy danh sách user đang online để kt trạng thái
        {
            var returnData = new ResponseModel<IEnumerable<UserDto>?>();
            string proc = "sysUsp_User_GetOnlineUsers";

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<UserDto>(proc);
            returnData.Data = data;
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }
    }
}
