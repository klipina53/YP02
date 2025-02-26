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
    public enum UserRole
    {
        None,
        Admin,
        Teacher
    }

    public partial class MainWindow : Page
    {
        public static MainWindow init;
        private UserRole currentUserRole = UserRole.None;
        public MainWindow()
        {
            InitializeComponent();
            init = this;
        }

        private void Entrance(object sender, RoutedEventArgs e)
        {
            try
            {
                string adminUsername = "admin";
                string adminPassword = "admin123";

                string teacherUsername = "prepod";
                string teacherPassword = "prepod123";

             
                string enteredUsername = usernameTextBox.Text.Trim();
                string enteredPassword = password.Password.Trim();

                if (string.IsNullOrEmpty(enteredUsername) || string.IsNullOrEmpty(enteredPassword))
                {
                    MessageBox.Show("Пожалуйста, введите логин и пароль.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

               
                if (enteredUsername == adminUsername && enteredPassword == adminPassword)
                {
                    currentUserRole = UserRole.Admin;
                    MessageBox.Show("Вы успешно вошли как Администратор", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.NavigationService.Navigate(new HomePageAdministration(currentUserRole));
                    return;
                }

                
                if (enteredUsername == teacherUsername && enteredPassword == teacherPassword)
                {
                    currentUserRole = UserRole.Teacher; 
                    MessageBox.Show("Вы успешно вошли как Преподаватель", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.NavigationService.Navigate(new HomePage(currentUserRole));
                    return;
                }

               
                MessageBox.Show("Неправильный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
