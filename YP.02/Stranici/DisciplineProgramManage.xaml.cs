using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для DisciplineProgramManage.xaml
    /// </summary>
    public partial class DisciplineProgramManage : Page
    {
        private UserRole currentUserRole;
        private int selectedDisciplineId;
        private DisciplinePrograms _selectedPrograms;
        public DisciplineProgramManage(UserRole currentUserRole, int selectedDisciplineId)
        {
            InitializeComponent();
            this.currentUserRole = currentUserRole;
            this.selectedDisciplineId = selectedDisciplineId;
            resultsListView.ItemsSource = LoadDisciplinePrograms();
        }
        private List<DisciplinePrograms> LoadDisciplinePrograms()
        {
            List<DisciplinePrograms> programs = new List<DisciplinePrograms>();
            string query = $"SELECT * FROM `DisciplinePrograms` WHERE DisciplineId = {selectedDisciplineId};";
            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    programs.Add(new DisciplinePrograms
                    {
                        DisciplineProgramId = reader.GetInt32(0),
                        DisciplineId = reader.GetInt32(1),
                        Topic = reader.GetString(2),
                        Type = reader.GetString(3),
                        Hours = reader.GetInt32(4)
                    });
                }
            }
            return programs;
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
                resultsListView.ItemsSource = LoadDisciplinePrograms();
            }
            else
            {
                var filteredPrograms = LoadDisciplinePrograms().Where(i => i.Topic.ToLower().Contains(searchText) || i.Type.ToLower().Contains(searchText)).ToList();
                resultsListView.ItemsSource = filteredPrograms;
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchTextBox.Foreground = Brushes.Black;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _selectedPrograms = null;
            hiddenPanelTitle.Content = "Добавление";
            NameTB.Text = "";
            TypeTB.Text = "";
            HoursTB.Text = "";
            hiddenPanel.Visibility = Visibility.Visible;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _selectedPrograms = resultsListView.SelectedItem as DisciplinePrograms;
            if (_selectedPrograms != null)
            {
                hiddenPanelTitle.Content = "Редактирование";
                NameTB.Text = _selectedPrograms.Topic;
                TypeTB.Text = _selectedPrograms.Type;
                HoursTB.Text = _selectedPrograms.Hours.ToString();
                hiddenPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите тему для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _selectedPrograms = resultsListView.SelectedItem as DisciplinePrograms;
            if (_selectedPrograms != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить тему? ", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Classes.Connection.Query($"DELETE FROM `DisciplinePrograms` WHERE `DisciplineProgramId`= {_selectedPrograms.DisciplineProgramId}");
                    resultsListView.ItemsSource = LoadDisciplinePrograms();
                }
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new DisciplineManage(currentUserRole));
        }

        private void ClosePanel_Click(object sender, RoutedEventArgs e)
        {
            hiddenPanel.Visibility = Visibility.Hidden;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                if (_selectedPrograms == null)
                {
                    var query = Connection.Query($"INSERT INTO `DisciplinePrograms`(`DisciplineId`, `Topic`, `Type`, `Hours`) VALUES ({selectedDisciplineId},'{NameTB.Text}','{TypeTB.Text}', {Convert.ToInt32(HoursTB.Text)})");
                    if (query != null)
                    {
                        MessageBox.Show("Успешное добавления данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else MessageBox.Show("Ошибка добавления данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    var query = Connection.Query($"UPDATE `DisciplinePrograms` SET `DisciplineId`= {selectedDisciplineId},`Topic`= '{NameTB.Text}',`Type`= '{TypeTB.Text}',`Hours`= {Convert.ToInt32(HoursTB.Text)} WHERE `DisciplineProgramId`= {_selectedPrograms.DisciplineProgramId}");
                    if (query != null)
                    {
                        MessageBox.Show("Успешное изменение данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else MessageBox.Show("Ошибка изменения данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                hiddenPanel.Visibility = Visibility.Hidden;
                resultsListView.ItemsSource = LoadDisciplinePrograms();
            }
        }
        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(NameTB.Text) ||
                string.IsNullOrWhiteSpace(TypeTB.Text) ||
                string.IsNullOrWhiteSpace(HoursTB.Text))
            {
                MessageBox.Show("Все поля обязательны для заполнения.",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            // Исправлено условие регулярного выражения
            if (!Regex.IsMatch(HoursTB.Text, @"^\d+$"))
            {
                MessageBox.Show("Поле Часы должно содержать только целые числа!",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}
