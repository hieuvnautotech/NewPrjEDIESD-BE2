using Elasticsearch.Net;
using NewPrjESDEDIBE.CustomAttributes;
using System;

namespace NewPrjESDEDIBE.Models.Dtos.Common
{
    [JSGenerateAttribute]
    public class AuthorizationResponse
    {
        public string? accessToken { get; set; } = default;
        public string? refreshToken { get; set; } = default;
        public bool isSuccess { get; set; } = true;
        public string? reason { get; set; } = default; 
    }

    //Mã trên định nghĩa một lớp AuthorizationResponse, có các thuộc tính sau:

    //accessToken: Một chuỗi(string) chứa Access Token, có thể là null. Thuộc tính này có một giá trị
    //mặc định là null.

    //refreshToken: Một chuỗi(string) chứa Refresh Token, cũng có thể là null. Thuộc tính này có một
    //giá trị mặc định là null.

    //isSuccess: Một giá trị boolean đại diện cho kết quả của quá trình xác thực hoặc quyền truy cập.
    //Nếu giá trị là true, quá trình được coi là thành công.Giá trị này có một giá trị mặc định là true.

    //reason: Một chuỗi (string) chứa lý do nếu quá trình xác thực hoặc quyền truy cập không thành công.
    //Thuộc tính này cũng có thể là null. Nếu quá trình xác thực hoặc quyền truy cập là thành công,
    //thuộc tính này không cần thiết. Giá trị này có một giá trị mặc định là null.

    //Lớp này có mục tiêu là chứa thông tin phản hồi từ quá trình xác thực hoặc quyền truy cập,
    //bao gồm Access Token, Refresh Token, trạng thái thành công và lý do (nếu có) nếu quá trình không thành công.
}
