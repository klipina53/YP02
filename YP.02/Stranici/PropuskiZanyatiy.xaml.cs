using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
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

        // Порог минут для направления на пересдачу
        private const int RetakeThresholdMinutes = 90;

        public PropuskiZanyatiy(UserRole userRole)
        {
            InitializeComponent();
            // ваш существующий код…

            // Добавляем кнопку «Отчёт на пересдачу»
            var btnRetakeReport = new Button
            {
                Content = "Отчёт: на пересдачу",
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            btnRetakeReport.Click += GenerateRetakeReport_Click;
            // Предположим, что у вас есть панель с названием toolbarPanel
            toolbarPanel.Children.Add(btnRetakeReport);
        }

        private void GenerateRetakeReport_Click(object sender, RoutedEventArgs e)
        {
            // 1) собираем все пропуски, подлежащие пересдаче:
            //    либо без объяснительной, либо превышают порог
            var items = _context.LoadZaniyatia()
                .Where(z => z.ExplanationText == "Нет объяснительной"
                         || z.MinutesMissed >= RetakeThresholdMinutes)
                .ToList();

            // 2) строим FlowDocument
            var doc = new FlowDocument
            {
                PagePadding = new Thickness(40),
                ColumnWidth = double.PositiveInfinity
            };
            // Заголовок
            doc.Blocks.Add(new Paragraph(new Run("Направления на пересдачу"))
            {
                FontSize = 22,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            });

            // Таблица: Название, Минут пропуска, Объяснительная
            var table = new Table();
            for (int i = 0; i < 3; i++)
                table.Columns.Add(new TableColumn());
            // Шапка
            var header = new TableRow();
            header.Cells.Add(new TableCell(new Paragraph(new Run("Занятие"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Мин. пропусков"))) { FontWeight = FontWeights.Bold });
            header.Cells.Add(new TableCell(new Paragraph(new Run("Объяснительная"))) { FontWeight = FontWeights.Bold });
            var headGroup = new TableRowGroup();
            headGroup.Rows.Add(header);
            table.RowGroups.Add(headGroup);
            // Данные
            var bodyGroup = new TableRowGroup();
            foreach (var z in items)
            {
                var row = new TableRow();
                row.Cells.Add(new TableCell(new Paragraph(new Run(z.Name))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(z.MinutesMissed.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(z.ExplanationText))));
                bodyGroup.Rows.Add(row);
            }
            table.RowGroups.Add(bodyGroup);
            doc.Blocks.Add(table);

            // 3) печать или сохранение
            var printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == true)
            {
                printDlg.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator,
                                       "Направления на пересдачу");
            }
            else
            {
                var saveDlg = new SaveFileDialog
                {
                    Filter = "XPS Document (*.xps)|*.xps",
                    FileName = "RetakeReport.xps"
                };
                if (saveDlg.ShowDialog() == true)
                {
                    using (var xpsDoc = new XpsDocument(saveDlg.FileName, FileAccess.ReadWrite))
                    {
                        var writer = XpsDocument.CreateXpsDocumentWriter(xpsDoc);
                        writer.Write(((IDocumentPaginatorSource)doc).DocumentPaginator);
                    }
                    MessageBox.Show($"Отчёт сохранён: {saveDlg.FileName}",
                                    "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

    }
}



