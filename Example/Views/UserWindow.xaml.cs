using Example.Models;
using System.Windows;

namespace Example.Views
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private User _currentUser;

        public UserWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            Title = $"Новости - {user.FullName}";
            WelcomeText.Text = $"Добро пожаловать, {user.Name}!";

            LoadNews();
        }

        private void LoadNews()
        {
            try
            {
                // Получаем новости из базы данных
                var newsList = DbService.GetAllNews();

                if (newsList != null && newsList.Count > 0)
                {
                    NewsListControl.ItemsSource = newsList;
                }
                else
                {
                    // Если нет новостей, показываем сообщение
                    var noNewsList = new List<News>
                    {
                        new News
                        {
                            Title = "Новостей пока нет",
                            Description = "В данный момент нет доступных новостей. Зайдите позже.",
                            ImagePath = "Нет изображения"
                        }
                    };
                    NewsListControl.ItemsSource = noNewsList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки новостей: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }
    }
}
