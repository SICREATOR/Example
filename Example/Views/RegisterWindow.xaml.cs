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
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка заполнения полей
            if (string.IsNullOrEmpty(SurnameTextBox.Text) ||
                string.IsNullOrEmpty(NameTextBox.Text) ||
                string.IsNullOrEmpty(LoginTextBox.Text) ||
                string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }

            // Проверка совпадения паролей
            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            // Создание нового пользователя
            var newUser = new User
            {
                Surname = SurnameTextBox.Text,
                Name = NameTextBox.Text,
                Patronymic = PatronymicTextBox.Text,
                Login = LoginTextBox.Text,
                Password = PasswordBox.Password // Без хеширования, как просили
            };

            // Регистрация в БД
            bool success = DbService.RegisterUser(newUser);

            if (success)
            {
                MessageBox.Show("Регистрация прошла успешно! Теперь вы можете войти в систему.");

                // Открываем окно пользователя (роль по умолчанию 'user')
                var userWindow = new UserWindow(newUser);
                userWindow.Show();

                // Закрываем текущее окно и окно авторизации
                var mainWindow = (MainWindow)Owner;
                mainWindow?.Close();
                this.Close();
            }
        }
    }
}
