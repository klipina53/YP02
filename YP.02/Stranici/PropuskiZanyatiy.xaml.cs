using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Логика взаимодействия для PropuskiZanyatiy.xaml
    /// </summary>
    public partial class PropuskiZanyatiy : Page
    {
        private UserRole currentUserRole;
        private Zaniyatie _selectedZaniyatie;
        private ZaniyatieContext _context ;

        public PropuskiZanyatiy(UserRole userRole)
        {
            InitializeComponent();
            this.currentUserRole = userRole;
            resultsListView.ItemsSource = _context.LoadZaniyatia();
            string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionStringName"].ConnectionString;
            _context = new ZaniyatieContext(connectionString); // Исправлено

            resultsListView.ItemsSource = _context.LoadZaniyatia();
        
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resultsListView.SelectedItem is Zaniyatie selectedZaniyatie)
            {
                hiddenPanelTitle.Content = "Редактирование";
                PropuskiZaniyatie.Text = selectedZaniyatie.ZaniyatieName;
                Propuskimin.Text = selectedZaniyatie.MinutesMissed.ToString();

                hiddenPanel.Visibility = Visibility.Visible;
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

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            string searchText = searchTextBox.Text.ToLower();

            if (string.IsNullOrEmpty(searchText) || searchText == "поиск...")
            {
                resultsListView.ItemsSource = _context.LoadZaniyatia();
            }
            else
            {
                var filteredZaniyatia = _context.LoadZaniyatia()
                    .Where(i => i.ZaniyatieName.ToLower().Contains(searchText) ||
                                i.MinutesMissed.ToString().Contains(searchText));
                resultsListView.ItemsSource = filteredZaniyatia;
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchTextBox.Foreground = Brushes.Black;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _selectedZaniyatie = null;
            hiddenPanelTitle.Content = "Добавление";
            PropuskiZaniyatie.Text = "";
            Propuskimin.Text = "";
            hiddenPanel.Visibility = Visibility.Visible;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _selectedZaniyatie = resultsListView.SelectedItem as Zaniyatie;
            if (_selectedZaniyatie != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить занятие?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bool isDeleted = _context.Delete(_selectedZaniyatie.Id);
                    if (isDeleted)
                    {
                        MessageBox.Show("Успешное удаление данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        resultsListView.ItemsSource = _context.LoadZaniyatia();
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
            _selectedZaniyatie = resultsListView.SelectedItem as Zaniyatie;
            if (_selectedZaniyatie != null)
            {
                hiddenPanelTitle.Content = "Редактирование";
                PropuskiZaniyatie.Text = _selectedZaniyatie.ZaniyatieName;
                Propuskimin.Text = _selectedZaniyatie.MinutesMissed.ToString();
                hiddenPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите занятие для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PropuskiZaniyatie.Text) &&
                int.TryParse(Propuskimin.Text, out int minutesMissed))
            {
                if (_selectedZaniyatie == null) // Добавление новой записи
                {
                    var newZaniyatie = new Zaniyatie
                    {
                        ZaniyatieName = PropuskiZaniyatie.Text,
                        MinutesMissed = minutesMissed,
                        Obyasnitelnaya = null // Здесь можно добавить логику для загрузки PDF
                    };
                    bool isAdded = _context.Add(newZaniyatie);
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
                    _selectedZaniyatie.ZaniyatieName = PropuskiZaniyatie.Text;
                    _selectedZaniyatie.MinutesMissed = minutesMissed;

                    bool isUpdated = _context.Update(_selectedZaniyatie);
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
                resultsListView.ItemsSource = _context.LoadZaniyatia();
            }
            else
            {
                MessageBox.Show("Пожалуйста, убедитесь, что вводимые значения корректны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void PropuskiObyasnitelnaya_Click(object sender, RoutedEventArgs e)
        {
            // Здесь необходимо добавить логику для загрузки PDF-файла
        }
    }
}

