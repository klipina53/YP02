using System;
using System.Collections;
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
    /// Логика взаимодействия для StudentsWindow.xaml
    /// </summary>
    public partial class StudentsWindow : Page
    {
        private UserRole currentUserRole;
        private int selectedGroupId;
        private Student _selectedStudent;
        private StudentContext _context = new StudentContext();

        public StudentsWindow(UserRole userRole, int selectedGroupId)
        {
            InitializeComponent();
            currentUserRole = userRole;
            this.selectedGroupId = selectedGroupId;
            resultsListView.ItemsSource = _context.LoadStudents(selectedGroupId);
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

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchTextBox.Foreground = Brushes.Black;
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            string searchText = searchTextBox.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                resultsListView.ItemsSource = _context.LoadStudents(selectedGroupId);
            }
            else
            {
                var filteredStudents = _context.LoadStudents(selectedGroupId)
                    .Where(s => s.Lastname.ToLower().Contains(searchText) ||
                                 s.Firstname.ToLower().Contains(searchText) ||
                                 s.Patronymic.ToLower().Contains(searchText))
                    .ToList();
                resultsListView.ItemsSource = filteredStudents;
            }
        }

        private void ClosePanel_Click(object sender, RoutedEventArgs e)
        {
            hiddenPanel.Visibility = Visibility.Hidden;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _selectedStudent = null;
            LastnameTB.Text = "";
            FirstnameTB.Text = "";
            PatronymicTB.Text = "";
            DismissalDateTB.SelectedDate = null;
            hiddenPanelTitle.Content = "Добавление";
            hiddenPanel.Visibility = Visibility.Visible;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _selectedStudent = resultsListView.SelectedItem as Student;

            if (_selectedStudent != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить студента?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bool isDeleted = _context.Delete(_selectedStudent.StudentId);
                    if (isDeleted)
                    {
                        MessageBox.Show("Успешное удаление данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        resultsListView.ItemsSource = _context.LoadStudents(selectedGroupId);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите студента для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _selectedStudent = resultsListView.SelectedItem as Student;

            if (_selectedStudent != null)
            {
                hiddenPanelTitle.Content = "Редактирование";
                LastnameTB.Text = _selectedStudent.Lastname;
                FirstnameTB.Text = _selectedStudent.Firstname;
                PatronymicTB.Text = _selectedStudent.Patronymic;
                DismissalDateTB.SelectedDate = _selectedStudent.DismissalDate;
                hiddenPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите студента для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                if (_selectedStudent == null)
                {
                    var newStudent = new Student
                    {
                        GroupId = selectedGroupId,
                        Lastname = LastnameTB.Text,
                        Firstname = FirstnameTB.Text,
                        Patronymic = PatronymicTB.Text,
                        DismissalDate = DismissalDateTB.SelectedDate
                    };

                    bool isAdded = _context.Add(newStudent);
                    if (isAdded)
                    {
                        MessageBox.Show("Успешное добавление данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка добавления данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    _selectedStudent.Lastname = LastnameTB.Text;
                    _selectedStudent.Firstname = FirstnameTB.Text;
                    _selectedStudent.Patronymic = PatronymicTB.Text;
                    _selectedStudent.DismissalDate = DismissalDateTB.SelectedDate;

                    bool isUpdated = _context.Update(_selectedStudent);
                    if (isUpdated)
                    {
                        MessageBox.Show("Успешное изменение данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка изменения данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                resultsListView.ItemsSource = _context.LoadStudents(selectedGroupId);
                hiddenPanel.Visibility = Visibility.Hidden;
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(LastnameTB.Text) ||
                string.IsNullOrWhiteSpace(FirstnameTB.Text) ||
                string.IsNullOrWhiteSpace(PatronymicTB.Text))
            {
                MessageBox.Show("Все поля обязательны для заполнения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}

