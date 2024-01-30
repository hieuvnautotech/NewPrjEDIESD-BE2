namespace NewPrjESDEDIBE.Models.Dtos.Common
{
    public class ResponseModel<T>
    {
        public int HttpResponseCode { get; set; }
        public T? Data { get; set; } = default;
        public int? TotalRow { get; set; }
        public string? ResponseMessage { get; set; }

        public ResponseModel()
        {
            HttpResponseCode = 200;
            ResponseMessage = string.Empty; ;
            Data = default;
            TotalRow = 0;
        }
    }
}
//The provided code defines a generic class named ResponseModel<T>. This class is likely
//intended to represent a standardized response structure for API responses. Let's break
//down the key components of this class:
//Thuộc tính HttpResponseCode: Thuộc tính số nguyên biểu thị mã phản hồi HTTP. Nó được đặt
//thành giá trị mặc định là 200 (OK) trong hàm tạo mặc định.

//Thuộc tính dữ liệu: Thuộc tính chung (T) biểu thị dữ liệu thực tế của phản hồi. Nó là null
//(T?) và được khởi tạo thành giá trị mặc định của loại chung (mặc định) trong hàm tạo mặc định.

//Thuộc tính TotalRow: Thuộc tính số nguyên biểu thị tổng số hàng. Nó là null (int?), cho phép nó
//là null nếu không áp dụng được. Nó được khởi tạo với giá trị mặc định là 0 trong hàm tạo mặc định.

//Thuộc tính ResponseMessage: Thuộc tính chuỗi đại diện cho thông báo phản hồi. Nó là null (chuỗi?)
//và được khởi tạo thành một chuỗi trống trong hàm tạo mặc định.

//Trình xây dựng mặc định: Khởi tạo các thuộc tính của lớp với các giá trị mặc định. Hàm tạo mặc định
//này đang đặt HttpResponseCode thành 200 (OK), ResponseMessage thành một chuỗi trống, Data thành giá
//trị mặc định của loại chung (mặc định) và TotalRow thành 0.

//Lớp ResponseModel<T> này có thể được sử dụng để đóng gói các phản hồi API bằng cấu trúc được tiêu
//chuẩn hóa. Bản chất chung của lớp cho phép nó chứa các loại dữ liệu khác nhau trong thuộc tính Dữ liệu.
//Các thuộc tính như HttpResponseCode, TotalRow và ResponseMessage cung cấp siêu dữ liệu bổ sung về phản hồi.
