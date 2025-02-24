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
using YP._02.Stranici;

namespace YP._02
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Page
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Entrance(object sender, RoutedEventArgs e)
        {
            string adminUsername = "admin";
            string adminPassword = "admin123";

            string teacherUsername = "prepod";
            string teacherPassword = "prepod123";

            string enteredUsername = usernameTextBox.Text;
            string enteredPassword = password.Password;

            if (enteredUsername == adminUsername && enteredPassword == adminPassword)
            {
                this.NavigationService.Navigate(new HomePageAdministration());
            }
            else if (enteredUsername == teacherUsername && enteredPassword == teacherPassword)
            {
                this.NavigationService.Navigate(new HomePage());
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Forgot_my_password(object sender, RoutedEventArgs e)
        {

        }
        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            placeholderText.Visibility = Visibility.Collapsed;
        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(password.Password))
            {
                placeholderText.Visibility = Visibility.Visible;
            }
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(password.Password))
            {
                placeholderText.Visibility = Visibility.Visible;
            }
            else
            {
                placeholderText.Visibility = Visibility.Collapsed;
            }
        }
    

        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text == "Username")
            {
                usernameTextBox.Text = ""; 
                usernameTextBox.Foreground = Brushes.White; 
            }
        }

        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(usernameTextBox.Text))
            {
                usernameTextBox.Text = "Username"; 
                usernameTextBox.Foreground = Brushes.LightGray; 
            }
        }
    }
}
