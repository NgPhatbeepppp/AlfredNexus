using AlfredNexus.Enums;
using System;
using System.Diagnostics;
using System.Windows.Threading; // Cần cho DispatcherTimer

namespace AlfredNexus.Services
{
    public class ShutdownService : IShutdownService
    {
        private readonly DispatcherTimer _timer;
        private TimeSpan _remainingTime;
        private ShutdownMode _currentMode;

        // Cờ đánh dấu đã cảnh báo chưa (để không spam thông báo)
        private bool _isWarningTriggered;

        public event Action<TimeSpan>? Tick;
        public event Action? Warning;
        public event Action? Completed;

        public bool IsRunning => _timer.IsEnabled;

        public ShutdownService()
        {
            // Khởi tạo đồng hồ bấm giờ nội bộ
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); // Đếm mỗi 1 giây
            _timer.Tick += Timer_Tick;
        }

        public void Start(TimeSpan duration, ShutdownMode mode)
        {
            if (IsRunning) return;

            _remainingTime = duration;
            _currentMode = mode;
            _isWarningTriggered = false;

            _timer.Start();

            // Cập nhật ngay lập tức trạng thái đầu tiên
            Tick?.Invoke(_remainingTime);
        }

        public void Cancel()
        {
            _timer.Stop();
            _remainingTime = TimeSpan.Zero;
            // Reset lại UI về 0 hoặc trạng thái chờ
            Tick?.Invoke(TimeSpan.Zero);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // 1. Giảm thời gian
            _remainingTime = _remainingTime.Subtract(TimeSpan.FromSeconds(1));

            // 2. Bắn tin hiệu ra ngoài cho ViewModel cập nhật giao diện
            Tick?.Invoke(_remainingTime);

            // 3. Logic cảnh báo 5 phút cuối [cite: 10]
            if (_remainingTime.TotalMinutes <= 5 && _remainingTime.TotalSeconds > 0 && !_isWarningTriggered)
            {
                _isWarningTriggered = true;
                Warning?.Invoke(); // Rung chuông cảnh báo
            }

            // 4. Hết giờ -> Thực thi lệnh
            if (_remainingTime.TotalSeconds <= 0)
            {
                _timer.Stop();
                Completed?.Invoke();
                ExecuteSystemCommand();
            }
        }

        private void ExecuteSystemCommand()
        {
            string arguments = "/s /t 0"; // Mặc định là Shutdown

            switch (_currentMode)
            {
                case ShutdownMode.Restart:
                    arguments = "/r /t 0";
                    break;
                case ShutdownMode.Hibernate:
                    arguments = "/h";
                    break;
            }

            // Gọi CMD để thực thi lệnh hệ thống [cite: 165]
            Process.Start(new ProcessStartInfo("shutdown", arguments)
            {
                CreateNoWindow = true,
                UseShellExecute = false
            });
        }
    }
}