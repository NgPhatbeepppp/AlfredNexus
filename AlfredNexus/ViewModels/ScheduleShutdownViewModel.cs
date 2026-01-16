using AlfredNexus.Enums;
using AlfredNexus.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Net.Sockets;

namespace AlfredNexus.ViewModels
{
    public partial class ScheduleShutdownViewModel : ObservableObject
    {
        private readonly IShutdownService _shutdownService;

        // Các thuộc tính để Binding lên giao diện
        [ObservableProperty]
        private string _timerDisplay = "00:00:00"; // Hiển thị đồng hồ

        [ObservableProperty]
        private double _selectedDuration = 60; // Mặc định 60 phút (dùng cho Slider)

        [ObservableProperty]
        private ShutdownMode _selectedMode = ShutdownMode.Shutdown; // Mặc định là Tắt máy

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsIdle))] // Khi IsRunning đổi, IsIdle cũng đổi theo
        private bool _isRunning;

        public bool IsIdle => !IsRunning; // Để vô hiệu hóa Slider khi đang chạy

        // Lấy danh sách enum để đổ vào ComboBox
        public ShutdownMode[] Modes => (ShutdownMode[])Enum.GetValues(typeof(ShutdownMode));

        public ScheduleShutdownViewModel(IShutdownService shutdownService)
        {
            _shutdownService = shutdownService;

            // Đăng ký lắng nghe sự kiện từ Service
            _shutdownService.Tick += OnServiceTick;
            _shutdownService.Completed += OnServiceCompleted;

            if (_shutdownService.IsRunning)
            {
                IsRunning = true;
                // Nếu Service có property CurrentMode hay RemainingTime public thì gán vào đây luôn
                // Ví dụ: TimerDisplay = _shutdownService.RemainingTime.ToString(...)
            }
        }

        [RelayCommand]
        private void ToggleTimer()
        {
            if (IsRunning)
            {
                // Đang chạy -> Bấm thì Hủy
                _shutdownService.Cancel();
                IsRunning = false;
                TimerDisplay = "00:00:00";
            }
            else
            {
                // Đang dừng -> Bấm thì Chạy
                var duration = TimeSpan.FromMinutes(SelectedDuration);
                _shutdownService.Start(duration, SelectedMode);
                IsRunning = true;
            }
        }

        private void OnServiceTick(TimeSpan remaining)
        {
            // Cập nhật UI từ luồng Service (cần đảm bảo chạy trên UI Thread nếu cần, nhưng ObservableObject thường lo được)
            TimerDisplay = remaining.ToString(@"hh\:mm\:ss");
        }

        private void OnServiceCompleted()
        {
            IsRunning = false;
            TimerDisplay = "Goodbye!";
        }
    }
}