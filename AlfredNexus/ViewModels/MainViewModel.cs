using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection; // Cần cái này để gọi ServiceProvider

namespace AlfredNexus.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        // Biến này chứa ViewModel hiện tại đang hiển thị
        [ObservableProperty]
        private object? _currentView;

        // Constructor
        public MainViewModel()
        {
            // Mặc định khi mở app sẽ hiện màn hình Shutdown luôn
            ShowScheduleShutdownView();
        }

        [RelayCommand]
        public void ShowScheduleShutdownView()
        {
            // Nhờ App lấy hộ cái ViewModel đã đăng ký (Dependency Injection)
            // Lưu ý: App.Current là class App của mình
            CurrentView = App.Current.Services.GetService<ScheduleShutdownViewModel>();
        }

        [RelayCommand]
        public void ShowFocusModeView()
        {
            // Tạm thời chưa làm, để trống hoặc hiện thông báo
            // CurrentView = App.Current.Services.GetService<FocusModeViewModel>();
        }
    }
}