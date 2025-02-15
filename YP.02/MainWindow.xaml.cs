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

namespace YP._02
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Entrance(object sender, RoutedEventArgs e)
        {
            // Создаем экземпляр HomePage
            HomePage homePage = new HomePage();
            // Создаем Frame для навигации
            Frame mainFrame = new Frame();
            mainFrame.Navigate(homePage);
            // Открываем новое окно с Frame
            Window homeWindow = new Window
            {
                Title = "HomePage",
                Content = mainFrame,
                Width = 800,
                Height = 450,
            };
            homeWindow.Show();
            this.Close(); // Закрываем текущее окно
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
