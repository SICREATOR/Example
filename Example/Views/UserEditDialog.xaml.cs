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
    /// Логика взаимодействия для UserEditDialog.xaml
    /// </summary>
    public partial class UserEditDialog : Window
    {
        private User _user;

        public UserEditDialog(User user)
        {
            InitializeComponent();
            _user = user;

            // Заполняем поля
            SurnameTextBox.Text = user.Surname;
            NameTextBox.Text = user.Name;
            PatronymicTextBox.Text = user.Patronymic;
            LoginTextBox.Text = user.Login;

            // Устанавливаем роль
            foreach (ComboBoxItem item in RoleComboBox.Items)
            {
                if (item.Content.ToString() == user.Role)
                {
                    RoleComboBox.SelectedItem = item;
                    break;
                }
            }

            // Если не выбрана роль, выбираем первую
            if (RoleComboBox.SelectedItem == null)
            {
                RoleComboBox.SelectedIndex = 0;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SurnameTextBox.Text) ||
                string.IsNullOrEmpty(NameTextBox.Text) ||
                string.IsNullOrEmpty(LoginTextBox.Text))
            {
                MessageBox.Show("Заполните обязательные поля");
                return;
            }

            _user.Surname = SurnameTextBox.Text;
            _user.Name = NameTextBox.Text;
            _user.Patronymic = PatronymicTextBox.Text;
            _user.Login = LoginTextBox.Text;
            _user.Role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();

            // Если пароль не пустой, обновляем его
            if (!string.IsNullOrEmpty(PasswordBox.Password))
            {
                _user.Password = PasswordBox.Password;
            }

            if (DbService.UpdateUser(_user))
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
