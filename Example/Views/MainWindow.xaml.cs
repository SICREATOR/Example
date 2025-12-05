using Example.Models;
using Example.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }

            var user = DbService.AuthenticateUser(login, password);

            if (user != null)
            {
                OpenRoleWindow(user);
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Открываем окно регистрации
            var registerWindow = new RegisterWindow();
            registerWindow.Owner = this;
            registerWindow.ShowDialog();
        }

        private void OpenRoleWindow(User user)
        {
            Window roleWindow = null;

            switch (user.Role.ToLower())
            {
                case "admin":
                    roleWindow = new AdminWindow(user);
                    break;
                case "editor":
                    roleWindow = new EditorWindow(user);
                    break;
                default: // user
                    roleWindow = new UserWindow(user);
                    break;
            }

            if (roleWindow != null)
            {
                roleWindow.Show();
                roleWindow.Closed += (s, args) => this.Close(); // Закрыть приложение при закрытии окна роли
            }
        }
    }
}