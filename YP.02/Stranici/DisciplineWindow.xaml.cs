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
using YP._02.Context;

namespace YP._02.Stranici
{
    /// <summary>
    /// Логика взаимодействия для DisciplineWindow.xaml
    /// </summary>
    public partial class DisciplineWindow : Page
    {
        private Lessons _selectedLesson; // Изменено на класс Lesson
        private UserRole currentUserRole;
        private LessonsContext _context = new LessonsContext();
        private int _currentDisciplineId; // Для хранения ID выбранной дисциплины

        public DisciplineWindow(UserRole userRole, int disciplineId)
        {
            InitializeComponent();
            currentUserRole = userRole;
            _currentDisciplineId = disciplineId;
            resultsListView.ItemsSource = _context.LoadLessons(disciplineId); // Загрузка уроков
        }

        public DisciplineWindow(UserRole currentUserRole)
        {
            this.currentUserRole = currentUserRole;
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

            if (string.IsNullOrEmpty(searchText) || searchText == "поиск...")
            {
                resultsListView.ItemsSource = _context.LoadLessons(_currentDisciplineId);
            }
            else
            {
                var filteredLessons = _context.LoadLessons(_currentDisciplineId)
                    .Where(i => i.Name.ToLower().Contains(searchText) ||
                                i.GroupId.ToLower().Contains(searchText)).ToList();
                resultsListView.ItemsSource = filteredLessons;
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchTextBox.Foreground = Brushes.Black;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _selectedLesson = null;
            hiddenPanelTitle.Content = "Добавление";
            Name.Text = "";
            GroupId.Text = "";
            TotalClasses.Text = "";
            ConductedHours.Text = "";
            hiddenPanel.Visibility = Visibility.Visible;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _selectedLesson = resultsListView.SelectedItem as Lessons;
            if (_selectedLesson != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить занятие? Это приведет к удалению всех смежных данных.", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bool isDeleted = _context.Delete(_selectedLesson.ID); // Убедитесь, что метод Delete принимает ID урока
                    if (isDeleted)
                    {
                        MessageBox.Show("Успешное удаление данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        resultsListView.ItemsSource = _context.LoadLessons(_currentDisciplineId);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите занятие для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _selectedLesson = resultsListView.SelectedItem as Lessons;
            if (_selectedLesson != null)
            {
                hiddenPanelTitle.Content = "Редактирование";
                Name.Text = _selectedLesson.Name;
                GroupId.Text = "";
                TotalClasses.Text = _selectedLesson.TotalClasses.ToString();
                ConductedHours.Text = _selectedLesson.ConductedHours.ToString();
                hiddenPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите занятие для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            // Код для навигации обратно
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

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Name.Text) &&
                !string.IsNullOrWhiteSpace(GroupId.Text) &&
                int.TryParse(TotalClasses.Text, out int totalClasses) &&
                int.TryParse(ConductedHours.Text, out int conductedHours))
            {
                if (_selectedLesson == null) // Добавление нового занятия
                {
                    var newLesson = new Lessons
                    {
                        Name = Name.Text,
                        GroupId = GroupId.Text,
                        TotalClasses = totalClasses,
                        ConductedHours = conductedHours
                    };
                    bool isAdded = _context.Add(newLesson); // Убедитесь, что метод Add работает с классом Lessons
                    if (isAdded)
                    {
                        MessageBox.Show("Успешное добавление данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка добавления данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else // Обновление существующего занятия
                {
                    _selectedLesson.Name = Name.Text;
                    _selectedLesson.GroupId = GroupId.Text;
                    _selectedLesson.TotalClasses = totalClasses;
                    _selectedLesson.ConductedHours = conductedHours;

                    bool isUpdated = _context.Update(_selectedLesson); // Убедитесь, что метод Update работает с классом Lessons
                    if (isUpdated)
                    {
                        MessageBox.Show("Успешное изменение данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка изменения данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                resultsListView.ItemsSource = _context.LoadLessons(_currentDisciplineId);
                hiddenPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Пожалуйста, убедитесь, что вводимые значения корректны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}


