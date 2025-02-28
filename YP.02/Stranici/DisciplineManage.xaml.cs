using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
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
using YP._02.Classes;

namespace YP._02.Stranici
{
    /// <summary>
    /// Логика взаимодействия для DisciplineManage.xaml
    /// </summary>
    public partial class DisciplineManage : Page
    {
        private Disciplines _selectedDiscipline;
        private UserRole currentUserRole;
        public DisciplineManage(UserRole userRole)
        {
            InitializeComponent();
            currentUserRole = userRole;
            //resultsListView.ItemsSource = LoadDiscipline();
            resultsListView.ItemsSource = DisciplineContext.All();
        }
        private List<Disciplines> LoadDiscipline()
        {
            List<Disciplines> disciplines = new List<Disciplines>();
            string query = "SELECT * FROM `Disciplines`";
            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    int disciplineId = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    int totalHours = CalculateTotalHours(disciplineId);

                    disciplines.Add(new Disciplines
                    {
                        DisciplineId = disciplineId,
                        Name = name,
                        TotalHours = totalHours
                    });
                }
            }
            return disciplines;
        }

        private int CalculateTotalHours(int disciplineId)
        {
            int totalHours = 0;
            string query = $"SELECT SUM(Hours) AS TotalHours FROM DisciplinePrograms WHERE DisciplineId = {disciplineId}";
            using (var reader = Connection.Query(query))
            {
                if (reader.Read())
                {
                    // Проверяем, является ли значение NULL
                    if (!reader.IsDBNull(0))
                    {
                        totalHours = reader.GetInt32(0);
                    }
                }
            }
            return totalHours; // Возвращаем 0, если записей не найдено или значение NULL
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

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            string searchText = searchTextBox.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                resultsListView.ItemsSource = LoadDiscipline();
            }
            else
            {
                var filteredDiscipline = LoadDiscipline().Where(i => i.Name.ToLower().Contains(searchText)).ToList();
                resultsListView.ItemsSource = filteredDiscipline;
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchTextBox.Foreground = Brushes.Black;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _selectedDiscipline = null;
            hiddenPanelTitle.Content = "Добавление";
            NameTB.Text = "";
            hiddenPanel.Visibility = Visibility.Visible;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _selectedDiscipline = resultsListView.SelectedItem as Disciplines;
            if (_selectedDiscipline != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить дисциплину? Это приведет к удалению всех смежных данных.", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Classes.Connection.Query($"DELETE FROM `Disciplines` WHERE `DisciplineId`= {_selectedDiscipline.DisciplineId}");
                    resultsListView.ItemsSource = LoadDiscipline();
                }
            }
            else
            {
                MessageBox.Show("Выберите дисциплину для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _selectedDiscipline = resultsListView.SelectedItem as Disciplines;
            if(_selectedDiscipline != null)
            {
                hiddenPanelTitle.Content = "Редактирование";
                NameTB.Text = _selectedDiscipline.Name;
                hiddenPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите группу для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameTB.Text))
            {
                if (_selectedDiscipline == null)
                {
                    var query = Classes.Connection.Query($"INSERT INTO `Disciplines`(`Name`,`TotalHours`) VALUES ('{NameTB.Text}')");
                    if (query != null)
                    {
                        MessageBox.Show("Успешное добавления данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else MessageBox.Show("Ошибка добавления данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    var query = Connection.Query($"UPDATE `Disciplines` SET `Name`= '{NameTB.Text}' WHERE `DisciplineId`= '{_selectedDiscipline.DisciplineId}'");
                    if (query != null)
                    {
                        MessageBox.Show("Успешное изменение данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else MessageBox.Show("Ошибка изменения данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                hiddenPanel.Visibility = Visibility.Hidden;
                resultsListView.ItemsSource = LoadDiscipline();
            }
        }

        private void ClosePanel_Click(object sender, RoutedEventArgs e)
        {
            hiddenPanel.Visibility = Visibility.Hidden;
        }

        private void DisciplineProgramm_Click(object sender, RoutedEventArgs e)
        {
            _selectedDiscipline = resultsListView.SelectedItem as Disciplines;
            if (_selectedDiscipline != null)
            {
                this.NavigationService.Navigate(new DisciplineProgramManage(currentUserRole, _selectedDiscipline.DisciplineId));
            }
            else
            {
                MessageBox.Show("Выберите дисциплину.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
