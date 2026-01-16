using AlfredNexus.Enums;
using System;

namespace AlfredNexus.Services
{
    public interface IShutdownService
    {
        // Sự kiện: Mỗi giây trôi qua, báo cho giao diện biết (cập nhật đồng hồ)
        event Action<TimeSpan> Tick;

        // Sự kiện: Còn 5 phút cuối, báo động!
        event Action Warning;

        // Sự kiện: Đã hết giờ, máy sắp tắt
        event Action Completed;

        // Hành động
        void Start(TimeSpan duration, ShutdownMode mode);
        void Cancel();

        // Kiểm tra xem Kiến có đang làm việc không
        bool IsRunning { get; }
    }
}