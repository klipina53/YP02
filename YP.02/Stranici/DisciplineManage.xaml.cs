using MySql.Data.MySqlClient;
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
using YP._02.Context;

namespace YP._02.Stranici
{
    /// <summary>
    /// Логика взаимодействия для DisciplineManage.xaml
    /// </summary>
    public partial class DisciplineManage : Page
    {
        private Disciplines _selectedDiscipline;
        private UserRole currentUserRole;
        private DisciplineContext _context = new DisciplineContext();

        public DisciplineManage(UserRole userRole)
        {
            InitializeComponent();
            currentUserRole = userRole;
            resultsListView.ItemsSource = _context.LoadDisciplines();
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
                resultsListView.ItemsSource = _context.LoadDisciplines();
            }
            else
            {
                var filteredDiscipline = _context.LoadDisciplines().Where(i => i.Name.ToLower().Contains(searchText)).ToList();
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
                    bool isDeleted = _context.Delete(_selectedDiscipline.DisciplineId);
                    if (isDeleted)
                    {
                        MessageBox.Show("Успешное удаление данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        resultsListView.ItemsSource = _context.LoadDisciplines();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
            if (_selectedDiscipline != null)
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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameTB.Text))
            {
                if (_selectedDiscipline == null)
                {
                    var newDiscipline = new Disciplines
                    {
                        Name = NameTB.Text
                    };
                    bool isAdded = _context.Add(newDiscipline);
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
                    _selectedDiscipline.Name = NameTB.Text;
                    bool isUpdated = _context.Update(_selectedDiscipline);
                    if (isUpdated)
                    {
                        MessageBox.Show("Успешное изменение данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка изменения данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                resultsListView.ItemsSource = _context.LoadDisciplines();
                hiddenPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Поле Название не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClosePanel_Click(object sender, RoutedEventArgs e)
        {
            hiddenPanel.Visibility = Visibility.Hidden;
        }

        private void DisciplineProgramm_Click(object sender, RoutedEventArgs e)
        {
            _selectedDiscipline = resultsListView.SelectedItem as Disciplines;
            if (_selectedDiscipline != null)
            {
                this.NavigationService.Navigate(new DisciplineProgramManage(currentUserRole, _selectedDiscipline.DisciplineId));
            }
            else
            {
                MessageBox.Show("Выберите дисциплину.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
