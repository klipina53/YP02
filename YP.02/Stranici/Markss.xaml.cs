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
using YP._02.Context;
using YP._02.Classes;
using YP._02.Stranici;


namespace YP._02.Stranici
{
    public partial class Markss : Page
    {
        private UserRole currentUserRole;
        private MarksContext _context = new MarksContext();
        private YP._02.Classes.Marks _selectedMark;


        public Markss(UserRole currentUserRole)
        {
            InitializeComponent();
            resultsListView.ItemsSource = _context.LoadMarks();
            this.currentUserRole = currentUserRole;
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            // Загрузите студентов
            var students = _context.LoadStudents();
            studentIdComboBox.ItemsSource = students; // Прямое связывание с источником
            studentIdComboBox.DisplayMemberPath = "FullName"; // Имя, если есть

            // Загрузите дисциплины
            var disciplines = _context.LoadDisciplinePrograms();
            disciplineProgramIdComboBox.ItemsSource = disciplines; // Прямое связывание с источником
            disciplineProgramIdComboBox.DisplayMemberPath = "Topic"; // Название темы
        }


        private void searchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == "Поиск...")
            {
                searchTextBox.Text = string.Empty;
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
            if (string.IsNullOrEmpty(searchText))
            {
                resultsListView.ItemsSource = _context.LoadMarks();
            }
            else
            {
                var filteredMarks = _context.LoadMarks().Where(m =>
                    m.Mark.ToLower().Contains(searchText) ||
                    m.Description?.ToLower().Contains(searchText) == true ||
                    m.StudentId.ToString().Contains(searchText) ||
                    m.DisciplineProgramId.ToString().Contains(searchText));
                resultsListView.ItemsSource = filteredMarks;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _selectedMark = null;
            hiddenPanelTitle.Content = "Добавление оценки";
            markTextBox.Text = string.Empty;
            studentIdComboBox.SelectedIndex = -1;
            disciplineProgramIdComboBox.SelectedIndex = -1;
            descriptionTextBox.Text = string.Empty;
            hiddenPanel.Visibility = Visibility.Visible;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedMark = resultsListView.SelectedItem as YP._02.Classes.Marks;
            if (_selectedMark != null)
            {
                hiddenPanelTitle.Content = "Редактирование оценки";
                markTextBox.Text = _selectedMark.Mark; // Используйте заглавные буквы
                studentIdComboBox.SelectedItem = studentIdComboBox.Items
                    .OfType<Student>()
                    .FirstOrDefault(s => s.StudentId == _selectedMark.StudentId);
                disciplineProgramIdComboBox.SelectedItem = disciplineProgramIdComboBox.Items
                    .OfType<DisciplinePrograms>()
                    .FirstOrDefault(dp => dp.DisciplineId == _selectedMark.DisciplineProgramId);
                descriptionTextBox.Text = _selectedMark.Description; // Также с заглавной буквы
                hiddenPanel.Visibility = Visibility.Visible;
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            ListView_SelectionChanged(sender, null); // Вызов внутри метода выбора
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _selectedMark = resultsListView.SelectedItem as YP._02.Classes.Marks;
            if (_selectedMark != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить эту оценку?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bool isDeleted = _context.Delete(_selectedMark.Id);
                    if (isDeleted)
                    {
                        MessageBox.Show("Оценка успешно удалена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        resultsListView.ItemsSource = _context.LoadMarks();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении оценки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите оценку для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClosePanel_Click(object sender, RoutedEventArgs e)
        {
            hiddenPanel.Visibility = Visibility.Hidden;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(markTextBox.Text) &&
                studentIdComboBox.SelectedItem is Student selectedStudent &&
                disciplineProgramIdComboBox.SelectedItem is DisciplinePrograms selectedDiscipline)
            {
                if (_selectedMark == null) // Добавление
                {
                    var newMark = new YP._02.Classes.Marks
                    {
                        Mark = markTextBox.Text.Trim(),
                        StudentId = selectedStudent.StudentId,
                        DisciplineProgramId = selectedDiscipline.DisciplineId,
                        Description = descriptionTextBox.Text.Trim()
                    };

                    bool isAdded = _context.Add(newMark);
                    if (isAdded)
                    {
                        MessageBox.Show("Оценка успешно добавлена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при добавлении оценки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else // Обновление
                {
                    _selectedMark.Mark = markTextBox.Text.Trim();
                    _selectedMark.StudentId = selectedStudent.StudentId;
                    _selectedMark.DisciplineProgramId = selectedDiscipline.DisciplineId;
                    _selectedMark.Description = descriptionTextBox.Text.Trim();

                    bool isUpdated = _context.Update(_selectedMark);
                    if (isUpdated)
                    {
                        MessageBox.Show("Оценка успешно обновлена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при обновлении оценки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                hiddenPanel.Visibility = Visibility.Hidden;
                resultsListView.ItemsSource = _context.LoadMarks();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchTextBox.Foreground = Brushes.Black;
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
    }
}




