using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        private ZaniyatieContext _context = new ZaniyatieContext();
        private ObservableCollection<Zaniyatie> _zaniyatiaList;

        public PropuskiZanyatiy(UserRole userRole)
        {
            InitializeComponent();
            currentUserRole = userRole;
            _zaniyatiaList = new ObservableCollection<Zaniyatie>(_context.LoadZaniyatia());
            resultsListView.ItemsSource = _zaniyatiaList;
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

            if (string.IsNullOrEmpty(searchText) || searchText == "поиск...")
            {
                resultsListView.ItemsSource = _zaniyatiaList;
            }
            else
            {
                var filteredZaniyatia = _zaniyatiaList
                    .Where(i => i.Name.ToLower().Contains(searchText) ||
                                i.MinutesMissed.ToString().Contains(searchText))
                    .ToList();

                resultsListView.ItemsSource = new ObservableCollection<Zaniyatie>(filteredZaniyatia);
            }
        }

        private void ClosePanel_Click(object sender, RoutedEventArgs e)
        {
            hiddenPanel.Visibility = Visibility.Hidden;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _selectedZaniyatie = null;
            hiddenPanelTitle.Content = "Добавление";
            PropuskiZaniyatie.Text = "";
            Propuskimin.Text = "";
            hasExplanationCheckBox.IsChecked = false; // Обнуляем чек-бокс для объяснительной
            hiddenPanelButton.Content = "Добавить";
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
                        _zaniyatiaList.Remove(_selectedZaniyatie);
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
                PropuskiZaniyatie.Text = _selectedZaniyatie.Name;
                Propuskimin.Text = _selectedZaniyatie.MinutesMissed.ToString();

                hasExplanationCheckBox.IsChecked =
                    _selectedZaniyatie.ExplanationText == "Есть объяснительная";
                hiddenPanelButton.Content = "Сохранить";
                hiddenPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите занятие для редактирования.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PropuskiZaniyatie.Text) &&
                int.TryParse(Propuskimin.Text, out int minutesMissed))
            {
                if (_selectedZaniyatie == null) // Добавление новой записи
                {
                    var newZaniyatie = new Zaniyatie
                    {
                        Name = PropuskiZaniyatie.Text,
                        MinutesMissed = minutesMissed,
                        ExplanationText = hasExplanationCheckBox.IsChecked == true ? "Есть объяснительная" : "Нет объяснительной" // Установка текста объяснительной
                    };
                    bool isAdded = _context.Add(newZaniyatie);
                    if (isAdded)
                    {
                        MessageBox.Show("Успешное добавление данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        _zaniyatiaList.Add(newZaniyatie);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка добавления данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else // Обновление существующей записи
                {
                    _selectedZaniyatie.Name = PropuskiZaniyatie.Text;
                    _selectedZaniyatie.MinutesMissed = minutesMissed;
                    _selectedZaniyatie.ExplanationText = hasExplanationCheckBox.IsChecked == true ? "Есть объяснительная" : "Нет объяснительной"; // Обновление текста объяснительной

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
                _zaniyatiaList.Clear();
                foreach (var item in _context.LoadZaniyatia())
                    _zaniyatiaList.Add(item);
            }
            else
            {
                MessageBox.Show("Пожалуйста, убедитесь, что вводимые значения корректны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedZaniyatie = resultsListView.SelectedItem as Zaniyatie;
            if (_selectedZaniyatie != null)
            {
                hiddenPanelTitle.Content = "Редактирование";
                PropuskiZaniyatie.Text = _selectedZaniyatie.Name;
                Propuskimin.Text = _selectedZaniyatie.MinutesMissed.ToString();

                hasExplanationCheckBox.IsChecked =
                    _selectedZaniyatie.ExplanationText == "Есть объяснительная";
                hiddenPanelButton.Content = "Сохранить";
                hiddenPanel.Visibility = Visibility.Visible;
            }
        }

    }
}



