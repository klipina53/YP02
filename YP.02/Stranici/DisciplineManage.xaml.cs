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
            resultsListView.ItemsSource = LoadDiscipline();
        }
        private List<Disciplines> LoadDiscipline()
        {
            List<Disciplines> disciplines = new List<Disciplines>();
            string query = "SELECT * FROM `Disciplines`";
            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    disciplines.Add(new Disciplines
                    {
                        DisciplineId = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }
            return disciplines;
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
                    Classes.Connection.Query($"DELETE FROM `Disciplines` WHERE `DisciplineID`= {_selectedDiscipline.DisciplineId}");
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
                MessageBox.Show("Выберите дисциплину для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HomePageAdministration(currentUserRole));
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameTB.Text))
            {
                if (_selectedDiscipline == null)
                {
                    var query = Classes.Connection.Query($"INSERT INTO `Disciplines`(`Name`) VALUES ('{NameTB.Text}')");
                    if (query != null)
                    {
                        MessageBox.Show("Успешное добавления данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else MessageBox.Show("Ошибка добавления данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    var query = Connection.Query($"UPDATE `Disciplines` SET `Name`= '{NameTB.Text}' WHERE `DisciplineID`= '{_selectedDiscipline.DisciplineId}'");
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

        }
    }
}
