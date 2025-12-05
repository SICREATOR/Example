using Example.Models;
using System.Windows;
using System.Windows.Controls;


namespace Example.Views
{
    /// <summary>
    /// Логика взаимодействия для NewsPage.xaml
    /// </summary>
    public partial class NewsPage : Page
    {
        public NewsPage()
        {
            InitializeComponent();
            LoadNews();
        }

        private void LoadNews()
        {
            var newsList = DbService.GetAllNews();
            NewsGrid.ItemsSource = newsList;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNews();
        }

        private void AddNewsButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NewsEditDialog();
            if (dialog.ShowDialog() == true)
            {
                LoadNews();
            }
        }

        private void EditNews_Click(object sender, RoutedEventArgs e)
        {
            // Получаем кнопку, которая вызвала событие
            // При нажатии на кнопку
            // WPF вызывает EditNews_Click(sender, e), где sender - эта самая кнопка
            // sender as Button преобразует объект sender в тип Button
            // as оператор безопасного приведения типов
            Button? button = sender as Button;

            if (sender.GetType() == typeof(Button))
            {
                Button a = (Button)sender;
            } 

            // Получаем объект News, привязанный к этой строке DataGrid
            // Если sender не Button, вернет null
            News? news = button?.DataContext as News;

            // Если news не null - объект успешно получен
            if (news != null)
            {
                // Открываем диалог редактирования с передачей news
                NewsEditDialog dialog = new NewsEditDialog(news);

                // Если диалог завершился успешно (OK)
                if (dialog.ShowDialog() == true)
                {
                    // Обновляем список новостей
                    LoadNews();
                }
            }
        }

        private void DeleteNews_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var news = button?.DataContext as News;

            if (news != null)
            {
                var result = MessageBox.Show($"Удалить новость '{news.Title}'?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (DbService.DeleteNews(news.Id))
                    {
                        MessageBox.Show("Новость удалена");
                        LoadNews();
                    }
                }
            }
        }
    }
}
