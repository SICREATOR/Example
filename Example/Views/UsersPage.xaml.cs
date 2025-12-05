using Example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Example.Views
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            var usersList = DbService.GetAllUsers();
            UsersGrid.ItemsSource = usersList;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            // безопасное приведение типа, если не удастся, вернет null
            var button = sender as Button;
            var user = button?.DataContext as User;

            if (user != null)
            {
                var dialog = new UserEditDialog(user);
                if (dialog.ShowDialog() == true)
                {
                    LoadUsers();
                }
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var user = button?.DataContext as User;

            if (user != null)
            {
                // Нельзя удалять самого себя
                if (user.Login == "admin" && user.Id == 1)
                {
                    MessageBox.Show("Нельзя удалить главного администратора");
                    return;
                }

                var result = MessageBox.Show($"Удалить пользователя '{user.FullName}'?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (DbService.DeleteUser(user.Id))
                    {
                        MessageBox.Show("Пользователь удален");
                        LoadUsers();
                    }
                }
            }
        }
    }
}
