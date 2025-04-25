using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using YP._02.Classes;
using YP._02.Context;

namespace YP._02.Stranici
{
    public partial class PrepodavatelNagruzka : Page
    {
        private UserRole currentUserRole;
        private InstructorLoads _selectedPrepodavatel;
        private InstructorLoadsContext _context = new InstructorLoadsContext();

        public PrepodavatelNagruzka(UserRole userRole)
        {
            InitializeComponent();
            this.currentUserRole = userRole;
            LoadComboBoxes();
            resultsListView.ItemsSource = _context.LoadInstructorLoads();
        }

        private void LoadComboBoxes()
        {
            var disciplines = _context.LoadDisciplines();
            var groups = _context.LoadGroups();

            DisciplineComboBox.ItemsSource = disciplines;
            DisciplineComboBox.DisplayMemberPath = "Name";
            DisciplineComboBox.SelectedValuePath = "DisciplineID";
            if (disciplines.Any())
                DisciplineComboBox.SelectedIndex = 0;

            GroupComboBox.ItemsSource = groups;
            GroupComboBox.DisplayMemberPath = "Name";
            GroupComboBox.SelectedValuePath = "GroupID";
            if (groups.Any())
                GroupComboBox.SelectedIndex = 0;
        }

        private void searchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == "Поиск...")
            {
                searchTextBox.Text = "";
                searchTextBox.Foreground = Brushes.Black;
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
                    .Where(i => (i.DisciplineName != null && i.DisciplineName.ToLower().Contains(searchText)) ||
                                (i.GroupName != null && i.GroupName.ToLower().Contains(searchText)));
                resultsListView.ItemsSource = filteredPrepodavatels;
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchTextBox.Foreground = Brushes.Black;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPrepodavatel = resultsListView.SelectedItem as InstructorLoads;
            if (_selectedPrepodavatel != null)
            {
                hiddenPanelTitle.Content = "Редактирование";

                // Ищем соответствующий элемент в ComboBox по DisciplineID
                DisciplineComboBox.SelectedItem = DisciplineComboBox.Items
                    .OfType<Disciplines>()
                    .FirstOrDefault(d => d.DisciplineId == _selectedPrepodavatel.DisciplineID);

                // Ищем соответствующий элемент в ComboBox по GroupID
                GroupComboBox.SelectedItem = GroupComboBox.Items
                    .OfType<Groups>()
                    .FirstOrDefault(g => g.GroupId == _selectedPrepodavatel.GroupID);

                LectureHoursTextBox.Text = _selectedPrepodavatel.LectureHours?.ToString() ?? "";
                PracticalHoursTextBox.Text = _selectedPrepodavatel.PracticalHours?.ToString() ?? "";
                ConsultationHoursTextBox.Text = _selectedPrepodavatel.ConsultationHours?.ToString() ?? "";
                ProjectHoursTextBox.Text = _selectedPrepodavatel.ProjectHours?.ToString() ?? "";
                ExamHoursTextBox.Text = _selectedPrepodavatel.ExamHours?.ToString() ?? "";
                hiddenPanel.Visibility = Visibility.Visible;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _selectedPrepodavatel = null;
            hiddenPanelTitle.Content = "Добавление";
            DisciplineComboBox.SelectedIndex = 0;
            GroupComboBox.SelectedIndex = 0;
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
                if (MessageBox.Show("Вы уверены, что хотите удалить запись? Это приведет к удалению всех смежных данных.", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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

                // Ищем соответствующий элемент в ComboBox по DisciplineID
                DisciplineComboBox.SelectedItem = DisciplineComboBox.Items
                    .OfType<Disciplines>()
                    .FirstOrDefault(d => d.DisciplineId == _selectedPrepodavatel.DisciplineID);

                // Ищем соответствующий элемент в ComboBox по GroupID
                GroupComboBox.SelectedItem = GroupComboBox.Items
                    .OfType<Groups>()
                    .FirstOrDefault(g => g.GroupId == _selectedPrepodavatel.GroupID);

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
            // Получаем выбранные элементы из ComboBox
            var selectedDiscipline = DisciplineComboBox.SelectedItem as Disciplines;
            var selectedGroup = GroupComboBox.SelectedItem as Groups;

            if (selectedDiscipline == null || selectedGroup == null)
            {
                MessageBox.Show("Пожалуйста, выберите дисциплину и группу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int? lectureHours = string.IsNullOrWhiteSpace(LectureHoursTextBox.Text) ? null : int.TryParse(LectureHoursTextBox.Text, out int lh) ? lh : (int?)null;
            int? practicalHours = string.IsNullOrWhiteSpace(PracticalHoursTextBox.Text) ? null : int.TryParse(PracticalHoursTextBox.Text, out int ph) ? ph : (int?)null;
            int? consultationHours = string.IsNullOrWhiteSpace(ConsultationHoursTextBox.Text) ? null : int.TryParse(ConsultationHoursTextBox.Text, out int ch) ? ch : (int?)null;
            int? projectHours = string.IsNullOrWhiteSpace(ProjectHoursTextBox.Text) ? null : int.TryParse(ProjectHoursTextBox.Text, out int prh) ? prh : (int?)null;
            int? examHours = string.IsNullOrWhiteSpace(ExamHoursTextBox.Text) ? null : int.TryParse(ExamHoursTextBox.Text, out int eh) ? eh : (int?)null;

            // Проверяем, что хотя бы одно поле с часами заполнено корректно
            if (lectureHours == null && practicalHours == null && consultationHours == null && projectHours == null && examHours == null)
            {
                MessageBox.Show("Хотя бы одно поле с часами должно быть заполнено корректным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_selectedPrepodavatel == null)
            {
                var newPrepodavatel = new InstructorLoads
                {
                    DisciplineID = selectedDiscipline.DisciplineId,
                    GroupID = selectedGroup.GroupId,
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
            else 
            {
                _selectedPrepodavatel.DisciplineID = selectedDiscipline.DisciplineId;
                _selectedPrepodavatel.GroupID = selectedGroup.GroupId;
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
    }
}