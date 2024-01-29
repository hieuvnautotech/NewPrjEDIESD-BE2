using Microsoft.EntityFrameworkCore;
using NewPrjESDEDIBE.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace NewPrjESDEDIBE.Models.Dtos.Common
{
    [JSGenerateAttribute] // cho phép mở rộng thuộc tính
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

//Tóm lại, lớp này là một đối tượng truyền dữ liệu (DTO) đại diện cho mô hình đăng nhập.
//Nó bao gồm các thuộc tính cho tên người dùng, mật khẩu và cờ boolean cho biết người
//dùng có sử dụng ứng dụng hay không. Lớp này có các thuộc tính xác thực để thực thi
//các ràng buộc về độ dài của tên người dùng và mật khẩu, đồng thời nó có một hàm tạo
//mặc định khởi tạo thuộc tính isOnApp thành false. Thuộc tính tùy chỉnh JSGenerateAttribution
//hiện có nhưng chức năng của nó không rõ ràng nếu không có ngữ cảnh bổ sung hoặc cách triển khai nó.
