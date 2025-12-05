using Example.Models;
using System.Windows;


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
