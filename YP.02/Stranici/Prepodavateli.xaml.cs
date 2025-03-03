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
using YP._02.Context;

namespace YP._02.Stranici
{
    /// <summary>
    /// Логика взаимодействия для Prepodavateli.xaml
    /// </summary>
    public partial class Prepodavateli : Page
    {
        private UserRole currentUserRole;
        private Instructors _selectedInstructor;
        private InstructorsContext _context = new InstructorsContext();

        public Prepodavateli(UserRole userRole)
        {
            InitializeComponent();
            currentUserRole = userRole;
            resultsListView.ItemsSource = _context.LoadInstructors();
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
                resultsListView.ItemsSource = _context.LoadInstructors();
            }
            else
            {
                var filteredInstructors = _context.LoadInstructors()
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
                    bool isDeleted = _context.Delete(_selectedInstructor.InstructorId);
                    if (isDeleted)
                    {
                        MessageBox.Show("Успешное удаление данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        resultsListView.ItemsSource = _context.LoadInstructors();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка удаления данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
                    var newInstructor = new Instructors
                    {
                        Lastname = LastnameTB.Text,
                        Firstname = FirstnameTB.Text,
                        Patronymic = PatronymicTB.Text,
                        Login = LoginTB.Text,
                        Password = PasswordTB.Text
                    };

                    bool isAdded = _context.Add(newInstructor);
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
                    _selectedInstructor.Lastname = LastnameTB.Text;
                    _selectedInstructor.Firstname = FirstnameTB.Text;
                    _selectedInstructor.Patronymic = PatronymicTB.Text;
                    _selectedInstructor.Login = LoginTB.Text;
                    _selectedInstructor.Password = PasswordTB.Text;

                    bool isUpdated = _context.Update(_selectedInstructor);
                    if (isUpdated)
                    {
                        MessageBox.Show("Успешное изменение данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка изменения данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                resultsListView.ItemsSource = _context.LoadInstructors();
                hiddenPanel.Visibility = Visibility.Hidden;
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(LastnameTB.Text) ||
                string.IsNullOrWhiteSpace(FirstnameTB.Text) ||
                string.IsNullOrWhiteSpace(PatronymicTB.Text) ||
                string.IsNullOrWhiteSpace(LoginTB.Text) ||
                string.IsNullOrWhiteSpace(PasswordTB.Text))
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
