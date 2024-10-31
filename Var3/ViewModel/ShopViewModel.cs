using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Var3.Model;
using System.Globalization;
using System.Windows.Data;
using Var3.View;

namespace Var3.ViewModel
{
    internal class ShopViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Товар> _products;
        public ObservableCollection<Товар> Products
        {
            get { return _products; }
            set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged(nameof(Products));
                }
            }
        }

        private Товар _selectedProduct;
        public Товар SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged(nameof(SelectedProduct));
                }
            }
        }

        public ShopViewModel(UserControl parentUserControl)
        {
            _parentUserControl = parentUserControl ?? throw new ArgumentNullException(nameof(parentUserControl));
            Products = new ObservableCollection<Товар>();
            LoadProducts();

            AddToFavoritesCommand = new RelayCommand(ExecuteAddToFavorites);
            AddToCartCommand = new RelayCommand(ExecuteAddToCart);
            NavigateProfileCommand = new RelayCommand(ExecuteNavigateProfile);
            NavigateFavoritesCommand = new RelayCommand(ExecuteNavigateFavorites);
            GoHomeNavigateCommand = new RelayCommand(ExecuteGoHome);
            AddCommand = new RelayCommand(ExecuteAddCommand);
        }

        private void LoadProducts()
        {
            DataBase myDbContext = new DataBase();

            try
            {
                using (SqlConnection connection = new SqlConnection(myDbContext.connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            t.Id_Товар, 
                            t.Артикул, 
                            t.Название, 
                            t.Категория, 
                            t.Бренд, 
                            t.Животное, 
                            t.Описание, 
                            t.Состав, 
                            t.Колличество_за_ед, 
                            t.Единица, 
                            t.Стоимость, 
                            c.Название AS НазваниеКатегории 
                        FROM 
                            Товар t
                        INNER JOIN 
                            Категории c ON t.Категория = c.Id_Категории";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<Товар> products = new List<Товар>();

                            while (reader.Read())
                            {
                                Товар product = new Товар
                                {
                                    Id_Товар = reader.GetInt32(0),
                                    Артикул = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                    Название = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    Категория = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                    Бренд = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                    Животное = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                    Описание = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                    Состав = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                    Колличество_за_ед = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                    Единица = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                    Стоимость = reader.IsDBNull(10) ? (decimal?)null : reader.GetDecimal(10)
                                };

                                // Добавляем название категории в объект Категории
                                product.Категории = new Категории
                                {
                                    Id_Категории = product.Категория ?? 0,
                                    Название = reader.IsDBNull(11) ? string.Empty : reader.GetString(11)
                                };

                                products.Add(product);
                            }

                            Products = new ObservableCollection<Товар>(products);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке товаров: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AddToFavoritesCommand { get; private set; }
        public ICommand AddToCartCommand { get; private set; }
        public ICommand NavigateProfileCommand { get; private set; }
        public ICommand NavigateFavoritesCommand { get; private set; }
        public ICommand GoHomeNavigateCommand { get; private set; }
        public ICommand AddCommand { get; private set; }

        private void ExecuteAddToFavorites(object parameter)
        {
            // Логика добавления товара в избранное
            MessageBox.Show("Товар добавлен в избранное", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExecuteAddToCart(object parameter)
        {
            // Логика добавления товара в корзину
            MessageBox.Show("Товар добавлен в корзину", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExecuteNavigateProfile(object parameter)
        {
            // Логика перехода в профиль
            var profilePage = new ProfilePage();
            var profileViewModel = new ProfileViewModel(new Клиент());
            profilePage.DataContext = profileViewModel;
            SetPage(profilePage);
        }

        private void ExecuteNavigateFavorites(object parameter)
        {
            // Логика перехода в избранные товары
            MessageBox.Show("Переход в избранные товары", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExecuteGoHome(object parameter)
        {
            // Логика перехода на страницу авторизации
            var loginView = new LoginWindow();
            var loginViewModel = new LoginViewModel();
            loginView.DataContext = loginViewModel;
            SetPage(loginView);
        }

        private void ExecuteAddCommand(object parameter)
        {
            // Логика для добавления товара
            MessageBox.Show("Добавление товара", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private UserControl _parentUserControl;

        

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
                    MessageBox.Show("Не найден элемент управления для отображения страницы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Не найден элемент управления для отображения страницы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}