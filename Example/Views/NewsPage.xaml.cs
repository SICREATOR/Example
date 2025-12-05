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
            var button = sender as Button;
            var news = button?.DataContext as News;

            if (news != null)
            {
                var dialog = new NewsEditDialog(news);
                if (dialog.ShowDialog() == true)
                {
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
