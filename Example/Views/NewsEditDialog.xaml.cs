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
    /// Логика взаимодействия для NewsEditDialog.xaml
    /// </summary>
    public partial class NewsEditDialog : Window
    {
        private News _news;
        private bool _isEditMode;

        public NewsEditDialog()
        {
            InitializeComponent();
            _isEditMode = false;
            Title = "Добавление новости";
        }

        public NewsEditDialog(News news) : this()
        {
            _news = news;
            _isEditMode = true;
            Title = "Редактирование новости";

            TitleTextBox.Text = news.Title;
            DescriptionTextBox.Text = news.Description;
            ImagePathTextBox.Text = news.ImagePath;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TitleTextBox.Text))
            {
                MessageBox.Show("Введите заголовок");
                return;
            }

            var newsToSave = new News
            {
                Title = TitleTextBox.Text,
                Description = DescriptionTextBox.Text,
                ImagePath = ImagePathTextBox.Text
            };

            bool success;

            if (_isEditMode)
            {
                newsToSave.Id = _news.Id;
                success = DbService.UpdateNews(newsToSave);
            }
            else
            {
                success = DbService.AddNews(newsToSave);
            }

            if (success)
            {
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
