using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Var3.View;
using Var3.ViewModel;

namespace Var3
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создаем главное окно
            MainWindow mainWindow = new MainWindow();

            // Создаем начальную страницу
            var loginView = new LoginWindow();
            var loginViewModel = new LoginViewModel(loginView); // Передаем loginView как parentUserControl
            loginView.DataContext = loginViewModel;

            // Устанавливаем начальную страницу в главное окно
            mainWindow.MainFrame.Navigate(loginView);

            // Отображаем главное окно
            mainWindow.Show();
        }

    }
}
