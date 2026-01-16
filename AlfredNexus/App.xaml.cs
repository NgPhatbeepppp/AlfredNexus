using AlfredNexus.Services; 
using AlfredNexus.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace AlfredNexus
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;

        // Container chứa các dịch vụ
        public IServiceProvider Services { get; }

        public App()
        {
            Services = ConfigureServices();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // 1. Đăng ký Services (Logic)
            services.AddSingleton<IShutdownService, ShutdownService>();

            // 2. Đăng ký ViewModels
            services.AddTransient<MainViewModel>();

            // [THAY ĐỔI QUAN TRỌNG] 
            // Đổi từ AddTransient sang AddSingleton.
            // Việc này đảm bảo ViewModel giữ nguyên trạng thái khi bạn điều hướng qua lại.
            services.AddSingleton<ScheduleShutdownViewModel>();

            return services.BuildServiceProvider();
        }
    }
}