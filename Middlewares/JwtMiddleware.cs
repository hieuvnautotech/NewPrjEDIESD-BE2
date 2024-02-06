using Microsoft.AspNetCore.Http;
using NewPrjESDEDIBE.Services.Common;

namespace NewPrjESDEDIBE.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //Đoạn mã trên là khai báo một middleware trong ASP.NET Core được sử dụng để xử lý
        //các yêu cầu liên quan đến JWT (JSON Web Tokens). Middleware này nhận một tham số
        //là RequestDelegate next, đại diện cho hàm middleware tiếp theo trong pipeline xử lý yêu cầu HTTP.

        //Middleware này có thể được sử dụng để thực hiện các công việc như xác thực và xác
        //định người dùng từ JWT trong yêu cầu HTTP, cũng như các nhiệm vụ khác liên quan đến
        //bảo mật và quản lý phiên.

        //Tuy nhiên, mã bạn cung cấp chỉ là constructor của middleware, không phải là một lớp
        //hoàn chỉnh.Để viết một middleware hoàn chỉnh, bạn cần thực hiện các bước xử lý trong
        //phương thức InvokeAsync của middleware.

        public async Task Invoke(HttpContext context, IJwtService _jwtService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = await _jwtService.ValidateTokenAsync(token);
            var userRole = _jwtService.GetRolenameFromToken(token);

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userRole))
            {
                // attach user to context on successful jwt validation
                context.Items["UserId"] = userId;
                context.Items["UserRole"] = userRole;
            }

            await _next(context);
        }
    }
}
//Mã được cung cấp dường như là một lớp phần mềm trung gian tùy chỉnh trong C#
//để xử lý xác thực JWT (Mã thông báo Web JSON) trong ứng dụng ASP.NET Core.
//Hãy để tôi chia nhỏ các thành phần chính của mã:
//Không gian tên: NewPrjESDEDIBE.Middlewares là không gian tên cho lớp JwtMiddleware.

//Lớp JwtMiddleware:

//Trình xây dựng: Nó nhận tham số requestDelegate trong hàm tạo. RequestDelegate đại
//diện cho phần mềm trung gian tiếp theo trong đường ống.

//Phương thức gọi: Phương thức này là điểm vào chính cho phần mềm trung gian.
//Nó được gọi cho mỗi yêu cầu HTTP. Phương thức này có hai tham số:
//HttpContext và một phiên bản của IJwtService (một dịch vụ để xử lý các hoạt động JWT).

//Bên trong phương thức Gọi:

//Nó trích xuất mã thông báo JWT từ tiêu đề "Ủy quyền" của yêu cầu.

//Gọi phương thức ValidateTokenAsync của _jwtService để xác thực mã thông báo, lấy ID người dùng.

//Gọi phương thức GetRolenameFromToken của _jwtService để truy xuất vai trò người dùng từ mã thông báo.

//Nếu ID người dùng và vai trò người dùng không trống, nó sẽ gắn chúng vào bộ sưu tập vật phẩm của ngữ cảnh.

//Cuối cùng, nó gọi phần mềm trung gian tiếp theo trong quy trình bằng cách sử dụng chờ đợi _next(context).

//Phần mềm trung gian này có thể được đặt trong đường dẫn ASP.NET Core để thực hiện xác thực
//JWT và đính kèm thông tin người dùng vào ngữ cảnh yêu cầu để phần mềm trung gian tiếp theo
//hoặc chính ứng dụng xử lý thêm.

//Lưu ý: Đảm bảo rằng các dịch vụ và cấu hình cần thiết (chẳng hạn như nội xạ phụ thuộc cho IJwtService)
//được thiết lập trong ứng dụng của bạn để phần mềm trung gian này hoạt động chính xác.