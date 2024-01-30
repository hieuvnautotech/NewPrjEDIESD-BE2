using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using NewPrjESDEDIBE.Extensions;
using NewPrjESDEDIBE.Helpers;
using NewPrjESDEDIBE.Hubs;
using NewPrjESDEDIBE.Models.Dtos.Common;
using NewPrjESDEDIBE.Services.Cache;
using NewPrjESDEDIBE.Services.Common;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Nest;
using static System.Net.WebRequestMethods;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace NewPrjESDEDIBE.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [SwaggerTag("Login")]
    public class LoginController : ControllerBase
    {

        private readonly ILoginService _loginService;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        // private readonly IMemoryCache _memoryCache;
        // private readonly IHttpContextAccessor _httpContextAccessor;
        // private readonly IUserAuthorizationService _userAuthorizationService;
        private readonly ISysCacheService _sysCacheService;
        private readonly SignalRHub _signalRHub;
        public LoginController(
            ILoginService loginService
            , IUserService userService
            , IJwtService jwtService
            , IRefreshTokenService refreshTokenService
            // , IMemoryCache memoryCache
            // , IHttpContextAccessor httpContextAccessor
            // , IUserAuthorizationService userAuthorizationService
            , ISysCacheService sysCacheService
            , SignalRHub signalRHub
        //, IMenuService menuService
        )
        {
            _loginService = loginService; //kt trong db thông tin user, pw
            _userService = userService;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            // _memoryCache = memoryCache;
            // _httpContextAccessor = httpContextAccessor;
            // _userAuthorizationService = userAuthorizationService;
            _sysCacheService = sysCacheService;
            _signalRHub = signalRHub;
        }

        [HttpPost("check-login")]
        //[SwaggerOperation(Tags = new[] { "Login" })]
        [SwaggerOperation
        (
            Summary = "Check userid/password",
            Description = "Returns access token."
        )]

        public async Task<IActionResult> Login([FromBody] LoginModelDto loginModel)
        {
            // Check Login Condition
            var result = await _loginService.CheckLogin(loginModel);
            var returnData = new ResponseModel<AuthorizationResponse>
            {
                ResponseMessage = result
            };

            switch (result)
            {
                //Login successfully
                case StaticReturnValue.SUCCESS:

                    //Get User Information
                    var data = await _userService.GetByUserName(loginModel.userName);

                    // Generate Access Token and Refresh Token
                    var authResponse = await _jwtService.GetTokenAsync(data.Data);
                    data.Data.Token = authResponse.accessToken;

                    var userRefreshTokenModel = new RefreshTokenDto
                    {
                        accessToken = authResponse.accessToken,
                        refreshToken = authResponse.refreshToken,
                        createdDate = DateTime.UtcNow,
                        expiredDate = DateTime.UtcNow.AddDays(60),
                        ipAddress = UserIPHelper.UserIp,
                        isValidated = false,
                        userId = data.Data.userId,
                        isOnApp = loginModel.isOnApp
                    };

                    // Insert Access Token and Refresh Token into Database
                    if (await _refreshTokenService.Create(userRefreshTokenModel) == StaticReturnValue.SUCCESS)
                    {
                        // Update Available Access Token to Redis
                        await _sysCacheService.SetAvailableTokensToRedis();

                        // Update User Authorization to Redis
                        // await _sysCacheService.UpdateUserAuthorizationCache();

                        data.Data.RefreshToken = authResponse.refreshToken;

                        // Update User last login time to Database
                        await SetLastLogin(data.Data, loginModel.isOnApp);
                        returnData.Data = authResponse;
                    }
                    else
                    {
                        SetInternalServerError(returnData);
                    }

                    break;

                case StaticReturnValue.ACCOUNT_PASSWORD_INVALID:
                case StaticReturnValue.ACCOUNT_BLOCKED:
                    SetBadRequest(returnData);
                    break;

                default:
                    SetInternalServerError(returnData);
                    break;
            }

            return Ok(returnData);
        }
        //Nó kiểm tra điều kiện đăng nhập bằng cách sử dụng _loginService.CheckLogin(loginModel)
        //và lưu kết quả vào biến kết quả.

        //Nó tạo ra một ResponseModel<AuthorizationResponse> có tên returnData để giữ dữ liệu phản hồi.

        //Nó sử dụng câu lệnh switch để xử lý các trường hợp khác nhau dựa trên kết quả đăng nhập (result).


        //Trong trường hợp đăng nhập thành công (StaticReturnValue.SUCCESS), nó sẽ truy xuất thông tin
        //người dùng, tạo mã thông báo truy cập và mã thông báo làm mới, đồng thời lưu trữ chúng trong
        //cơ sở dữ liệu bằng cách sử dụng _refreshTokenService.Create(userRefreshTokenModel).


        //Nếu quá trình tạo và lưu trữ mã thông báo thành công, nó sẽ cập nhật Redis bằng các mã thông
        //báo có sẵn, đặt mã thông báo làm mới trong dữ liệu người dùng, cập nhật thời gian đăng nhập
        //cuối cùng của người dùng và đặt dữ liệu phản hồi trong returnData.
        //Nếu đăng nhập không thành công do thông tin đăng nhập không hợp lệ hoặc tài khoản bị chặn,
        //nó sẽ đặt mã trạng thái HTTP thích hợp trong returnData.

        //Nếu đăng nhập không thành công vì một lý do không xác định, nó sẽ gây ra lỗi máy chủ nội
        //bộ trong returnData.

        //Cuối cùng, nó trả về một phản hồi Ok với returnData đã được điền sẵn.

        //Lưu ý: Hành vi thực tế và tiêu chí thành công có thể phụ thuộc vào chi tiết triển khai của
        //các dịch vụ và phương thức được gọi trong phương thức này.







        private async Task SetLastLogin(UserDto userDto, bool? isOnApp)
        {
            if (isOnApp == null || isOnApp == false)
            {
                userDto.lastLoginOnWeb = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                userDto.lastLoginOnApp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            userDto.isOnline = true;
            // await _sysCacheService.SetOnlineUserToRedis(userDto);

            if (isOnApp == null || isOnApp == false)
            {
                await _userService.SetLastLoginOnWeb(userDto);
            }
            else
            {
                await _userService.SetLastLoginOnApp(userDto);
            }

            await _signalRHub.SetUserLoggedIn(userDto);
        }

        //private void UpdateAvailableTokensCache()
        //{
        //    var availableTokens = _refreshTokenService.GetAvailables();
        //    _memoryCache.Remove("availableTokens");
        //    _memoryCache.Set("availableTokens", availableTokens.Result);
        //}

        //private void UpdateUserAuthorizationCache()
        //{
        //    var userAuthorization = _userAuthorizationService.GetUserAuthorization();
        //    _memoryCache.Remove("userAuthorization");
        //    _memoryCache.Set("userAuthorization", userAuthorization.Result);
        //}

        private static void SetBadRequest(ResponseModel<AuthorizationResponse> returnData)
        {
            returnData.HttpResponseCode = 400;
        }

        private static void SetInternalServerError(ResponseModel<AuthorizationResponse> returnData)
        {
            returnData.HttpResponseCode = 500;
            returnData.ResponseMessage = StaticReturnValue.SYSTEM_ERROR;
        }


        [HttpGet("user-info")]
        [SwaggerOperation
        (
            Summary = "Get user info by ID",
            Description = "Returns user info and user menus."
        )]
        public async Task<IActionResult> GetUserInfo()
        {
            var userAgent = Request.Headers[HeaderNames.UserAgent].ToString();
            bool? isAndroid = null;
            if (userAgent.Contains("android") || userAgent.Contains("iphone") || userAgent.Contains("ipad") || userAgent.Contains("ipod") || userAgent.Contains("blackberry") || userAgent.Contains("windows phone"))
            {
                isAndroid = true;
            }
            else
            {
                isAndroid = false;
            }
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            //long userId = _jwtService.GetUserIdFromToken(token);
            var userId = _jwtService.GetUserIdFromToken(token);
            var user = await _userService.GetUserInfo(userId);
            var returnData = new ResponseModel<UserDto>();

            if (user != null)
            {
                returnData.Data = user;
                returnData.ResponseMessage = StaticReturnValue.SUCCESS;
            }
            else
            {
                returnData.HttpResponseCode = 400;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return Ok(returnData);
        }

    }
}
