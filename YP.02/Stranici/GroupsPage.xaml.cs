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
    /// Логика взаимодействия для GroupsPage.xaml
    /// </summary>
    public partial class GroupsPage : Page
    {
        private UserRole currentUserRole;
        private Groups _selectedGroup;
        public GroupsPage(UserRole userRole)
        {
            InitializeComponent();
            this.currentUserRole = userRole;
            resultsListView.ItemsSource = LoadGroups();
        }
        private List<Groups> LoadGroups()
        {
            List<Groups> groups = new List<Groups>();
            string query = "SELECT * FROM `Groups`";
            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    groups.Add(new Groups
                    {
                        GroupId = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }
            return groups;
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
                resultsListView.ItemsSource = LoadGroups();
            }
            else
            {
                var filteredGroups = LoadGroups().Where(i => i.Name.ToLower().Contains(searchText));
                resultsListView.ItemsSource = filteredGroups;
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchTextBox.Foreground = Brushes.Black;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _selectedGroup = null;
            hiddenPanelTitle.Content = "Добавление";
            NameTB.Text = "";
            hiddenPanel.Visibility = Visibility.Visible;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _selectedGroup = resultsListView.SelectedItem as Groups;
            if (_selectedGroup != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить группу? Это приведет к удалению всех смежных данных.", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Classes.Connection.Query($"DELETE FROM `Groups` WHERE `GroupID`= {_selectedGroup.GroupId}");
                    resultsListView.ItemsSource = LoadGroups();
                }
            }
            else
            {
                MessageBox.Show("Выберите группу для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _selectedGroup = resultsListView.SelectedItem as Groups;
            if (_selectedGroup != null)
            {
                hiddenPanelTitle.Content = "Редактирование";
                NameTB.Text = _selectedGroup.Name;
                hiddenPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите дисциплину для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Students_Click(object sender, RoutedEventArgs e)
        {
            _selectedGroup = resultsListView.SelectedItem as Groups;
            if (_selectedGroup != null)
            {
                this.NavigationService.Navigate(new StudentsWindow(currentUserRole, _selectedGroup.GroupId));
            }
            else
            {
                MessageBox.Show("Выберите группу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            if (!string.IsNullOrWhiteSpace(NameTB.Text))
            { 
                if (_selectedGroup == null)
                {
                    var query = Connection.Query($"INSERT INTO `Groups`(`Name`) VALUES ('{NameTB.Text}')");
                    if (query != null)
                    {
                        MessageBox.Show("Успешное добавления данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else MessageBox.Show("Ошибка добавления данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    var query = Connection.Query($"UPDATE `Groups` SET `Name`= '{NameTB.Text}' WHERE GroupID = {_selectedGroup.GroupId}");
                    if (query != null)
                    {
                        MessageBox.Show("Успешное изменение данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else MessageBox.Show("Ошибка изменения данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                hiddenPanel.Visibility = Visibility.Hidden;
                resultsListView.ItemsSource = LoadGroups();
            }
        }
    }
}
