using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Var3.Model;
using Var3.View;

namespace Var3.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        readonly DataBase dataBase = new DataBase();

        private Клиент _employes;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Login
        {
            get { return _employes.Логин; }
            set
            {
                if (_employes.Логин != value)
                {
                    _employes.Логин = value;
                    OnPropertyChanged(nameof(Login));
                }
            }
        }

        public string Password
        {
            get { return _employes.Пароль; }
            set
            {
                if (_employes.Пароль != value)
                {
                    _employes.Пароль = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public ICommand AuthorizeCommand { get; set; }
        public string loginText { get; set; }
        public string passwordText { get; set; }

        public LoginViewModel()
        {
            AuthorizeCommand = new RelayCommand(Authorize);
        }

        private UserControl _parentUserControl;

        public LoginViewModel(UserControl parentUserControl)
        {
            _parentUserControl = parentUserControl;
            AuthorizeCommand = new RelayCommand(Authorize);
        }

        private void SetPage(UserControl page)
        {
            Window parentWindow = Window.GetWindow(_parentUserControl);

            if (parentWindow != null)
            {
                var mainFrame = parentWindow.FindName("MainFrame") as Frame;
                if (mainFrame != null)
                {
                    mainFrame.Navigate(page);
                }
                else
                {
                    MessageBox.Show("Не найден элемент управления MainFrame", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Не найден родительское окно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Authorize(object obj)
        {
            if (obj is string cmdParam && cmdParam == "NoAuth")
            {
                NavigateToShop();
                return;
            }
            //else if (obj is string cmdParam && cmdParam == "reg")
            //{
             //   NavigateToReg();
             //   return;
            //}

            if (string.IsNullOrWhiteSpace(loginText) || string.IsNullOrWhiteSpace(passwordText))
            {
                MessageBox.Show("Вы не ввели логин или пароль", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (dataBase.SqlSelect("select * from [dbo].[Клиент] where [Логин] = '" + loginText + "' and [Пароль] = '" + passwordText + "'").Rows.Count > 0)
            {
                MessageBox.Show("Пользователь", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigateToShop();
            }
            else
            {
                MessageBox.Show("Данные введены неккоректно", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void NavigateToShop()
        {
            var shopView = new Shop();
            var shopViewModel = new ShopViewModel(shopView);
            shopView.DataContext = shopViewModel;
            SetPage(shopView);
        }
        private void NavigateToReg()
        {
            var shopView = new RegWindow();
            //var RegViewModel = new RegViewModel(shopView);
            //shopView.DataContext = RegViewModel;
            SetPage(shopView);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}