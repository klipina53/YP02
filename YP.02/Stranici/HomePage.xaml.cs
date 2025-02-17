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
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

    

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainWindow());

        }

        private void Discipline(object sender, RoutedEventArgs e)

        {

            this.NavigationService.Navigate(new Stranici.DisciplineWindow());

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click4(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click5(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click6(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click7(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click8(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click9(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click0(object sender, RoutedEventArgs e)
        {

        }
    }
}
