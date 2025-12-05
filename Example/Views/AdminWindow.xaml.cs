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
using System.Windows.Shapes;

namespace Example.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private User _currentUser;

        public AdminWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            WelcomeText.Text = $"Администратор: {user.FullName}";

            // Загружаем страницу новостей по умолчанию
            NewsButton_Click(null, null);
        }

        private void NewsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new NewsPage());
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsersPage());
        }
    }
}
