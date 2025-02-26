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

namespace YP._02.Stranici
{
    /// <summary>
    /// Логика взаимодействия для PrepodavatelNagruzka.xaml
    /// </summary>
    public partial class PrepodavatelNagruzka : Page
    {
        private UserRole currentUserRole;

        public PrepodavatelNagruzka(UserRole userRole)
        {
            InitializeComponent();
            currentUserRole = userRole;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            switch (currentUserRole)
            {
                case UserRole.Admin:
                    this.NavigationService.Navigate(new HomePageAdministration(currentUserRole));
                    break;
                case UserRole.Teacher:
                    this.NavigationService.Navigate(new HomePage(currentUserRole));
                    break;
            }
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void searchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == "Поиск...")
            {
                searchTextBox.Text = "";
                searchTextBox.Foreground = Brushes.White;
            }
        }

        private void searchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                searchTextBox.Text = "Поиск...";
                searchTextBox.Foreground = Brushes.LightGray;
            }
        }

      

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ClosePanel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

