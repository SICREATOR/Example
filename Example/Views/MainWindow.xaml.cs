using Example.Models;
using Example.Views;
using System.Windows;
using System.Windows.Controls;

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

        // Обработчик события нажатия на кнопку "Войти"
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем логин из текстового поля и удаляем лишние пробелы
            string login = LoginTextBox.Text.Trim();
            // Получаем пароль из поля для пароля (PasswordBox скрывает ввод)
            string password = PasswordBox.Password;

            // Валидация ввода: проверяем, что оба поля заполнены
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }

            // Вызываем метод аутентификации из сервиса базы данных
            // DbService.AuthenticateUser проверяет логин/пароль в базе данных
            var user = DbService.AuthenticateUser(login, password);

            // Проверяем результат аутентификации
            if (user != null)
            {
                // Если пользователь найден (аутентификация успешна):

                // Открываем окно, соответствующее роли пользователя
                OpenRoleWindow(user);
                // скрываем, но не уничтожаем
                // Окно невидимо, но продолжает существовать в памяти
                // Можно вернуться к нему позже (например, при разлогине)
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }

        // Обработчик события нажатия на кнопку "Зарегистрироваться"
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем экземпляр окна регистрации
            var registerWindow = new RegisterWindow();

            // Устанавливаем текущее окно как владельца окна регистрации
            // Это обеспечивает модальность окна (блокирует родительское окно)
            registerWindow.Owner = this;

            // Открываем окно регистрации в модальном режиме
            // ShowDialog() блокирует выполнение кода до закрытия окна
            registerWindow.ShowDialog();
        }

        // Метод для открытия окна в зависимости от роли пользователя
        private void OpenRoleWindow(User user)
        {
            Window roleWindow = null; // Переменная для хранения ссылки на окно

            // Определяем какое окно открыть на основе роли пользователя
            switch (user.Role.ToLower()) // Приводим роль к нижнему регистру для унификации
            {
                case "admin":
                    // Для администратора открываем AdminWindow
                    roleWindow = new AdminWindow(user);
                    break;

                case "editor":
                    // Для редактора открываем EditorWindow
                    roleWindow = new EditorWindow(user);
                    break;

                default: // user или любая другая роль
                    // Для обычного пользователя открываем UserWindow
                    roleWindow = new UserWindow(user);
                    break;
            }

            // Если окно успешно создано
            if (roleWindow != null)
            {
                // Отображаем окно (не модально, позволяет работать с другими окнами)
                roleWindow.Show();

                // Подписываемся на событие закрытия окна роли
                roleWindow.Closed += (s, args) => this.Close();
                // Когда окно роли закрывается, закрывается и главное окно (и приложение)
                // Лямбда-выражение создает обработчик события, который вызывает Close() для MainWindow
            }
        }
    }
}