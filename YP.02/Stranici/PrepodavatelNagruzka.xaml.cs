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
using YP._02.Context;

namespace YP._02.Stranici
{
    /// <summary>
    /// Логика взаимодействия для PrepodavatelNagruzka.xaml
    /// </summary>
    public partial class PrepodavatelNagruzka : Page
    {
        private UserRole currentUserRole;
        private InstructorLoads _selectedPrepodavatel;
        private InstructorLoadsContext _context = new InstructorLoadsContext();

        public PrepodavatelNagruzka(UserRole userRole)
        {
            InitializeComponent();
            this.currentUserRole = userRole;
            resultsListView.ItemsSource = _context.LoadInstructorLoads();
        }

        private void searchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == "Поиск...")
            {
                searchTextBox.Text = "";
                searchTextBox.Foreground = Brushes.White;
            }
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPrepodavatel = resultsListView.SelectedItem as InstructorLoads; 

            if (_selectedPrepodavatel != null)
            {
                hiddenPanelTitle.Content = "Редактирование";
                DisciplineComboBox.SelectedValue = _selectedPrepodavatel.DisciplineID;
                GroupComboBox.SelectedValue = _selectedPrepodavatel.GroupID;
                LectureHoursTextBox.Text = _selectedPrepodavatel.LectureHours?.ToString() ?? "";
                PracticalHoursTextBox.Text = _selectedPrepodavatel.PracticalHours?.ToString() ?? "";
                ConsultationHoursTextBox.Text = _selectedPrepodavatel.ConsultationHours?.ToString() ?? "";
                ProjectHoursTextBox.Text = _selectedPrepodavatel.ProjectHours?.ToString() ?? "";
                ExamHoursTextBox.Text = _selectedPrepodavatel.ExamHours?.ToString() ?? "";

                hiddenPanel.Visibility = Visibility.Visible;
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

            if (string.IsNullOrEmpty(searchText) || searchText == "поиск...")
            {
                resultsListView.ItemsSource = _context.LoadInstructorLoads();
            }
            else
            {
                var filteredPrepodavatels = _context.LoadInstructorLoads()
                    .Where(i => i.DisciplineID.ToString().Contains(searchText) ||
                                i.GroupID.ToString().Contains(searchText));
                resultsListView.ItemsSource = filteredPrepodavatels;
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchTextBox.Foreground = Brushes.Black;
        }
        
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _selectedPrepodavatel = null;
            hiddenPanelTitle.Content = "Добавление";
            DisciplineComboBox.SelectedItem = null;
            GroupComboBox.SelectedItem = null;
            LectureHoursTextBox.Text = "";
            PracticalHoursTextBox.Text = "";
            ConsultationHoursTextBox.Text = "";
            ProjectHoursTextBox.Text = "";
            ExamHoursTextBox.Text = "";
            hiddenPanel.Visibility = Visibility.Visible;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _selectedPrepodavatel = resultsListView.SelectedItem as InstructorLoads;
            if (_selectedPrepodavatel != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить преподавателя? Это приведет к удалению всех смежных данных.", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bool isDeleted = _context.Delete(_selectedPrepodavatel.LoadID);
                    if (isDeleted)
                    {
                        MessageBox.Show("Успешное удаление данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        resultsListView.ItemsSource = _context.LoadInstructorLoads();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _selectedPrepodavatel = resultsListView.SelectedItem as InstructorLoads;
            if (_selectedPrepodavatel != null)
            {
                hiddenPanelTitle.Content = "Редактирование";
                DisciplineComboBox.SelectedValue = _selectedPrepodavatel.DisciplineID;
                GroupComboBox.SelectedValue = _selectedPrepodavatel.GroupID;
                LectureHoursTextBox.Text = _selectedPrepodavatel.LectureHours?.ToString() ?? "";
                PracticalHoursTextBox.Text = _selectedPrepodavatel.PracticalHours?.ToString() ?? "";
                ConsultationHoursTextBox.Text = _selectedPrepodavatel.ConsultationHours?.ToString() ?? "";
                ProjectHoursTextBox.Text = _selectedPrepodavatel.ProjectHours?.ToString() ?? "";
                ExamHoursTextBox.Text = _selectedPrepodavatel.ExamHours?.ToString() ?? "";
                hiddenPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void ClosePanel_Click(object sender, RoutedEventArgs e)
        {
            hiddenPanel.Visibility = Visibility.Hidden;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(LectureHoursTextBox.Text, out int lectureHours) &&
                int.TryParse(PracticalHoursTextBox.Text, out int practicalHours) &&
                int.TryParse(ConsultationHoursTextBox.Text, out int consultationHours) &&
                int.TryParse(ProjectHoursTextBox.Text, out int projectHours) &&
                int.TryParse(ExamHoursTextBox.Text, out int examHours))
            {
                if (_selectedPrepodavatel == null) // Добавление новой записи
                {
                    var newPrepodavatel = new InstructorLoads
                    {
                        DisciplineID = (int?)DisciplineComboBox.SelectedValue,
                        GroupID = (int?)GroupComboBox.SelectedValue,
                        LectureHours = lectureHours,
                        PracticalHours = practicalHours,
                        ConsultationHours = consultationHours,
                        ProjectHours = projectHours,
                        ExamHours = examHours
                    };
                    bool isAdded = _context.Add(newPrepodavatel);
                    if (isAdded)
                    {
                        MessageBox.Show("Успешное добавление данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка добавления данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else // Обновление существующей записи
                {
                    _selectedPrepodavatel.DisciplineID = (int?)DisciplineComboBox.SelectedValue;
                    _selectedPrepodavatel.GroupID = (int?)GroupComboBox.SelectedValue;
                    _selectedPrepodavatel.LectureHours = lectureHours;
                    _selectedPrepodavatel.PracticalHours = practicalHours;
                    _selectedPrepodavatel.ConsultationHours = consultationHours;
                    _selectedPrepodavatel.ProjectHours = projectHours;
                    _selectedPrepodavatel.ExamHours = examHours;

                    bool isUpdated = _context.Update(_selectedPrepodavatel);
                    if (isUpdated)
                    {
                        MessageBox.Show("Успешное изменение данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка изменения данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                hiddenPanel.Visibility = Visibility.Hidden;
                resultsListView.ItemsSource = _context.LoadInstructorLoads();
            }
            else
            {
                MessageBox.Show("Пожалуйста, убедитесь, что вводимые значения корректны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}



