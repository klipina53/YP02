using System;
using System.Collections.Generic;
using System.IO;
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
using OfficeOpenXml;
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
            this.NavigationService.Navigate(new Stranici.DisciplineManage(currentUserRole));
        }

        private void Students(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Stranici.GroupsPage(currentUserRole));
        }

        private void Nagruzka(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Stranici.PrepodavatelNagruzka(currentUserRole));
        }

        private void Propuski(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Stranici.PropuskiZanyatiy(currentUserRole));
        }

        private void Konsultacii(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Stranici.Konsultaishin(currentUserRole));
        }

        private void DiciplineReport(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Stranici.DisciplineWindow(currentUserRole));
        }


        private void DebtReport(object sender, RoutedEventArgs e)
        {
            ExportToExcel("Отчёт по должникам", new string[] { "Студент", "Не сданные работы", "Сданные за неделю" });
        }

        private void RetakeDirection(object sender, RoutedEventArgs e)
        {
            ExportToExcel("Направление на пересдачу", new string[] { "Студент", "Дисциплина", "Дата пересдачи" });
        }

        private void AttendanceSummary(object sender, RoutedEventArgs e)
        {
            ExportToExcel("Сводка посещаемости", new string[] { "Студент", "Дата", "Пропущенные занятия" });
        }

        private void ExportToExcel(string reportName, string[] headers)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(reportName);
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                }

                string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), reportName + ".xlsx");
                File.WriteAllBytes(filePath, package.GetAsByteArray());
                MessageBox.Show("Файл сохранен на рабочем столе: " + filePath, "Экспорт в Excel", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
