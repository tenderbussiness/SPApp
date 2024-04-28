using Domain.Data;
using Infrastraction.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Security.Cryptography.Xml;
using System.Windows;

namespace WpfAppThread
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create an instance of MainWindow
            UsersWindow mainWindow = new();

            using (MyDataContext context = new())
            {
                context.Database.Migrate();
                if(context.Users.Count()==0)
                {
                    UserService usersService = new UserService();
                    usersService.InsertRandomUser(10);
                }
            }

            // Show MainWindow
            mainWindow.Show();
        }
    }

}
