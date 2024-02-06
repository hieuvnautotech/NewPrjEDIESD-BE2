using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;
using NewPrjESDEDIBE.Cache;
using NewPrjESDEDIBE.Services.Cache;
using Newtonsoft.Json.Linq;
using System;

namespace NewPrjESDEDIBE.Services.Common
{
    public class InitializeCacheService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISysCacheService _sysCacheService;
        public InitializeCacheService(
            IServiceProvider serviceProvider
            , ISysCacheService sysCacheService
        )
        {
            _serviceProvider = serviceProvider;
            _sysCacheService = sysCacheService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                _sysCacheService.SetAvailableTokensToRedis();
                _sysCacheService.SetRoleMenusToRedis();
                _sysCacheService.SetRoleMissingMenuPermissionsToRedis();
                _sysCacheService.SetRoleMenuPermissionsToRedis();
                _sysCacheService.SetPoliciesToRedis();
            }

            return Task.CompletedTask;
        }

        //Đoạn mã trên định nghĩa một lớp InitializeCacheService thực thi giao diện IHostedService.
        //Cụ thể, lớp này có các thành viên sau:

        //Constructor: Constructor này nhận hai tham số:

        //serviceProvider: Đối tượng cung cấp dịch vụ, thường được sử dụng để tạo ra các phạm vi(scope)
        //mới trong ứng dụng.
        //sysCacheService: Đối tượng implement từ giao diện ISysCacheService, được sử dụng để thực hiện
        //các hoạt động liên quan đến cache.
        //StartAsync: Phương thức này được gọi khi dịch vụ bắt đầu chạy.Trong phương thức này, một phạm
        //vi (scope) mới được tạo ra từ serviceProvider và các phương thức của đối tượng sysCacheService
        //được gọi để thiết lập các thông tin cần thiết vào cache.Các phương thức gọi bao gồm:

        //SetAvailableTokensToRedis
        //SetRoleMenusToRedis
        //SetRoleMissingMenuPermissionsToRedis
        //SetRoleMenuPermissionsToRedis
        //SetPoliciesToRedis
        //StopAsync: Phương thức này được gọi khi dịch vụ được yêu cầu dừng lại.Trong trường hợp này,
        //không có hành động cụ thể nào được thực hiện, do đó, phương thức chỉ trả về Task.CompletedTask.

        //Tóm lại, lớp InitializeCacheService được thiết kế để khởi tạo các dữ liệu cần thiết trong
        //cache khi dịch vụ được khởi động, thông qua việc sử dụng đối tượng sysCacheService.

        // Thêm:
        //Redis là một hệ thống cơ sở dữ liệu key-value in-memory(lưu trữ trên bộ nhớ) mã nguồn mở.Tên
        //"Redis" viết tắt từ "Remote Dictionary Server". Nó được thiết kế để cung cấp một bộ công cụ linh
        //hoạt và hiệu quả cho việc lưu trữ và truy xuất dữ liệu, đặc biệt là trong các ứng dụng web nơi hiệu
        //suất và thời gian phản hồi nhanh là yếu tố quan trọng.
        //Một số điểm nổi bật về Redis bao gồm:

        //Lưu trữ dữ liệu trong bộ nhớ: Redis lưu trữ dữ liệu trong bộ nhớ chính, giúp nó đạt được hiệu suất
        //cao và thời gian phản hồi nhanh.

        //Cấu trúc dữ liệu linh hoạt: Redis hỗ trợ nhiều cấu trúc dữ liệu như chuỗi, danh sách, bộ đếm, tập
        //hợp, bản đồ băm và các cấu trúc dữ liệu phức tạp khác.

        //Hỗ trợ cho các phép toán đặc biệt trên dữ liệu: Redis cung cấp nhiều phép toán đặc biệt như đặt hạn
        //chế thời gian sống cho các khóa (key), các phép toán atomic (không thể chia nhỏ) và các phép toán
        //khác giúp quản lý dữ liệu một cách hiệu quả.

        //Hỗ trợ cho việc nhân bản và có thể mở rộng: Redis hỗ trợ nhân bản và có thể mở rộng một cách linh
        //hoạt, cho phép nó được sử dụng trong các môi trường có nhu cầu mở rộng lớn.

        //Đa nhiệm và đa ngôn ngữ: Redis có thể tích hợp với các ứng dụng viết bằng nhiều ngôn ngữ lập trình
        //và hỗ trợ cho nhiều luồng làm việc cùng một lúc.

        //Nhờ vào những tính năng mạnh mẽ này, Redis thường được sử dụng trong việc lưu trữ cache, phiên người
        //dùng, xếp hàng công việc (job queue), real-time analytics và nhiều ứng dụng khác nữa.

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
