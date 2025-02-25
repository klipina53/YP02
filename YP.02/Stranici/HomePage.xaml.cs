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
        private UserRole currentUserRole;
        public HomePage(UserRole userRole)
        {
            InitializeComponent();
            currentUserRole = userRole;
        }

    

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainWindow());

        }

        private void Discipline(object sender, RoutedEventArgs e)

        {

            this.NavigationService.Navigate(new Stranici.DisciplineWindow(currentUserRole));

        }

        private void Students(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Stranici.StudentsWindow(currentUserRole));
        }

     

        private void Nagruzka(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Stranici.PrepodavatelNagruzka(currentUserRole));
        }

        private void Propuski(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Stranici.PropuskiZanyatiy(currentUserRole));
        }

        private void Programma_Disciplini(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Stranici.ProgramsDsciplin(currentUserRole));
        }

        private void Konsultacii(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Stranici.Konsultaishin(currentUserRole));
        }
    }
}
