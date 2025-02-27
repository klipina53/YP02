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

        public StudentsWindow(UserRole userRole, int selectedGroupId)
        {
            InitializeComponent();
            currentUserRole = userRole;
            this.selectedGroupId = selectedGroupId;
            resultsListView.ItemsSource = LoadStudents();
        }
        private List<Student> LoadStudents()
        {
            List<Student> students = new List<Student>();
            string query = $"SELECT * FROM `Students` WHERE GroupID = {selectedGroupId}";
            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        StudentId = reader.GetInt32(0),
                        GroupId = reader.GetInt32(1),
                        Lastname = reader.GetString(2),
                        Firstname = reader.GetString(3),
                        Patronymic = reader.GetString(4),
                        DismissalDate = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5)
                    });
                }
            }
            return students;
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
                resultsListView.ItemsSource = LoadStudents();
            }
            else
            {
                var filteredStudents = LoadStudents()
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
                    Classes.Connection.Query($"DELETE FROM `Students` WHERE `StudentID` = {_selectedStudent.StudentId}");
                    resultsListView.ItemsSource = LoadStudents();
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
                string dismissalDate = DismissalDateTB.SelectedDate.HasValue
                    ? $"'{DismissalDateTB.SelectedDate.Value.ToString("yyyy-MM-dd")}'"
                    : "NULL";

                if (_selectedStudent == null)
                {
                    // Добавление нового студента
                    var query = Connection.Query($"INSERT INTO `Students`(`GroupID`, `LastName`, `FirstName`, `Patronymic`, `DismissalDate`) " +
                                                $"VALUES ({selectedGroupId}, '{LastnameTB.Text}', '{FirstnameTB.Text}', '{PatronymicTB.Text}', {dismissalDate})");
                    if (query != null)
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
                    var query = Connection.Query($"UPDATE `Students` SET " +
                                                $"`LastName` = '{LastnameTB.Text}', " +
                                                $"`FirstName` = '{FirstnameTB.Text}', " +
                                                $"`Patronymic` = '{PatronymicTB.Text}', " +
                                                $"`DismissalDate` = {dismissalDate} " +
                                                $"WHERE `StudentID` = {_selectedStudent.StudentId}");
                    if (query != null)
                    {
                        MessageBox.Show("Успешное изменение данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка изменения данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                // Обновляем список студентов
                resultsListView.ItemsSource = LoadStudents();
                hiddenPanel.Visibility = Visibility.Hidden;
            }
        }
        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(LastnameTB.Text) ||
        string.IsNullOrWhiteSpace(FirstnameTB.Text) ||
        string.IsNullOrWhiteSpace(PatronymicTB.Text))
            {
                MessageBox.Show("Все поля обязательны для заполнения.",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}

