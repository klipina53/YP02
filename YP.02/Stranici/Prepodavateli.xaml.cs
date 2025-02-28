using MySql.Data.MySqlClient;
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
    /// Логика взаимодействия для Prepodavateli.xaml
    /// </summary>
    public partial class Prepodavateli : Page
    {
        private UserRole currentUserRole;
        private Instructors _selectedInstructor;
        public Prepodavateli(UserRole userRole)
        {
            InitializeComponent();
            currentUserRole = userRole;
            resultsListView.ItemsSource = LoadInstructors();
        }
        private List<Instructors> LoadInstructors()
        {
            List<Instructors> instructors = new List<Instructors>();

            string query = "SELECT * FROM `Instructors`";
            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    instructors.Add(new Instructors
                    {
                        InstructorId = reader.GetInt32(0),
                        Lastname = reader.GetString(1),
                        Firstname = reader.GetString(2),
                        Patronymic = reader.GetString(3),
                        Login = reader.GetString(4),
                        Password = reader.GetString(5)
                    });
                }
            }
            return instructors;
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
                resultsListView.ItemsSource = LoadInstructors();
            }
            else
            {
                var filteredInstructors = LoadInstructors()
                    .Where(i => i.Lastname.ToLower().Contains(searchText) ||
                                  i.Firstname.ToLower().Contains(searchText) ||
                                  i.Patronymic.ToLower().Contains(searchText) ||
                                  i.Login.ToLower().Contains(searchText))
                    .ToList();

                resultsListView.ItemsSource = filteredInstructors;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _selectedInstructor = null;
            hiddenPanelTitle.Content = "Добавление";
            LastnameTB.Text = "";
            FirstnameTB.Text = "";
            PatronymicTB.Text = "";
            LoginTB.Text = "";
            PasswordTB.Text = "";
            hiddenPanel.Visibility = Visibility.Visible;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _selectedInstructor = resultsListView.SelectedItem as Instructors;
            if (_selectedInstructor != null)
            {
                hiddenPanelTitle.Content = "Редактирование";
                LastnameTB.Text = _selectedInstructor.Lastname;
                FirstnameTB.Text = _selectedInstructor.Firstname;
                PatronymicTB.Text = _selectedInstructor.Patronymic;
                LoginTB.Text = _selectedInstructor.Login;
                PasswordTB.Text = _selectedInstructor.Password;
                hiddenPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите преподавателя для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _selectedInstructor = resultsListView.SelectedItem as Instructors;
            if (_selectedInstructor != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить преподавателя? Это приведет к удалению всех смежных данных.", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Classes.Connection.Query($"DELETE FROM Instructors WHERE InstructorID = {_selectedInstructor.InstructorId}");
                    resultsListView.ItemsSource = LoadInstructors();
                }
            }
            else
            {
                MessageBox.Show("Выберите преподавателя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                if (_selectedInstructor == null)
                {
                    var query = Classes.Connection.Query($"INSERT INTO Instructors (Lastname, Firstname, Patronymic, Login, Password) VALUES ('{LastnameTB.Text}', '{FirstnameTB.Text}', '{PatronymicTB.Text}', '{LoginTB.Text}', '{PasswordTB.Text}');");
                    if (query != null)
                    {
                        MessageBox.Show("Успешное добавления данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else MessageBox.Show("Ошибка добавления данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    var query = Classes.Connection.Query($"UPDATE Instructors SET Lastname = '{LastnameTB.Text}', Firstname = '{FirstnameTB.Text}', Patronymic = '{PatronymicTB.Text}', Login = '{LoginTB.Text}', Password = '{PasswordTB.Text}' WHERE InstructorID = {_selectedInstructor.InstructorId};");
                    if (query != null)
                    {
                        MessageBox.Show("Успешное изменение данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else MessageBox.Show("Ошибка изменения данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                hiddenPanel.Visibility = Visibility.Hidden;
                resultsListView.ItemsSource = LoadInstructors();
            }
        }
        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(LastnameTB.Text) || string.IsNullOrWhiteSpace(FirstnameTB.Text) || string.IsNullOrWhiteSpace(PatronymicTB.Text) ||
                string.IsNullOrWhiteSpace(LoginTB.Text) || string.IsNullOrWhiteSpace(PasswordTB.Text))
            {
                MessageBox.Show("Все поля обязательны для заполнения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void ClosePanel_Click(object sender, RoutedEventArgs e)
        {
            hiddenPanel.Visibility = Visibility.Hidden;
        }
    }
}
