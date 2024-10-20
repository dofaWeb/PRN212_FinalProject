using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PRN212_FinalProject.Entities;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PRN212_FinalProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DBContext>(options =>
                options.UseMySql(
                    Configuration.GetConnectionString("PRN212"),
                    ServerVersion.AutoDetect(Configuration.GetConnectionString("PRN212"))
                ));
        }
    }

}
