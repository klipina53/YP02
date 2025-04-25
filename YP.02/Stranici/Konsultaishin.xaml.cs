using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using YP._02.Classes;
using YP._02.Context;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using Microsoft.Win32;
using System.Windows.Input;

namespace YP._02.Stranici
{
    public partial class Konsultaishin : Page
    {
        private UserRole currentUserRole;
        private Consultation _selectedConsultation;
        private ConsultationContext _context = new ConsultationContext();

        public Konsultaishin(UserRole userRole)
        {
            InitializeComponent();
            this.currentUserRole = userRole;
            resultsListView.ItemsSource = _context.LoadConsultations();
        }

        private void searchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == "Поиск...")
            {
                searchTextBox.Text = string.Empty;
                searchTextBox.Foreground = Brushes.Black; // Исправлено на Black для согласованности
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resultsListView.SelectedItem is Consultation selectedConsultation)
            {
                hiddenPanelTitle.Content = "Редактирование";
                DatePicker.SelectedDate = selectedConsultation.Date;
                StudentFullName.Text = selectedConsultation.StudentFullName.ToString();
                PracticeSubmittedTextBox.Text = selectedConsultation.PracticeSubmitted;
                hiddenPanel.Visibility = Visibility.Visible;
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
                resultsListView.ItemsSource = _context.LoadConsultations();
            }
            else
            {
                var filteredConsultations = _context.LoadConsultations()
                    .Where(i => i.Date.ToString().Contains(searchText) ||
                                (i.StudentFullName != null && i.StudentFullName.ToLower().Contains(searchText)) ||
                                (i.PracticeSubmitted != null && i.PracticeSubmitted.ToLower().Contains(searchText)));
                resultsListView.ItemsSource = filteredConsultations;
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchTextBox.Foreground = Brushes.Black;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _selectedConsultation = null;
            hiddenPanelTitle.Content = "Добавление";
            DatePicker.SelectedDate = null;
            StudentFullName.Text = string.Empty;
            PracticeSubmittedTextBox.Text = string.Empty;
            hiddenPanel.Visibility = Visibility.Visible;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            _selectedConsultation = resultsListView.SelectedItem as Consultation;
            if (_selectedConsultation != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить консультацию?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bool isDeleted = _context.Delete(_selectedConsultation.ConsultationID);
                    if (isDeleted)
                    {
                        MessageBox.Show("Успешное удаление данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        resultsListView.ItemsSource = _context.LoadConsultations();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите консультацию для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _selectedConsultation = resultsListView.SelectedItem as Consultation;
            if (_selectedConsultation != null)
            {
                hiddenPanelTitle.Content = "Редактирование";
                DatePicker.SelectedDate = _selectedConsultation.Date;
                StudentFullName.Text = _selectedConsultation.StudentFullName;
                PracticeSubmittedTextBox.Text = _selectedConsultation.PracticeSubmitted;
                hiddenPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Выберите консультацию для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            if (DatePicker.SelectedDate != null && !string.IsNullOrWhiteSpace(StudentFullName.Text))
            {
                if (_selectedConsultation == null) // Добавление
                {
                    var newConsultation = new Consultation
                    {
                        Date = DatePicker.SelectedDate.Value,
                        StudentFullName = StudentFullName.Text.Trim(),
                        PracticeSubmitted = PracticeSubmittedTextBox.Text
                    };

                    bool isAdded = _context.Add(newConsultation);
                    if (isAdded)
                    {
                        MessageBox.Show("Успешное добавление данных.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка добавления данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else // Обновление
                {
                    _selectedConsultation.Date = DatePicker.SelectedDate.Value;
                    _selectedConsultation.StudentFullName = StudentFullName.Text.Trim();
                    _selectedConsultation.PracticeSubmitted = PracticeSubmittedTextBox.Text;

                    bool isUpdated = _context.Update(_selectedConsultation);
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
                resultsListView.ItemsSource = _context.LoadConsultations();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx",
                Title = "Сохранить отчет",
                FileName = "ConsultationReport.docx"
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            try
            {
                var consultations = _context.LoadConsultations();
                if (!consultations.Any())
                {
                    MessageBox.Show("Нет данных для генерации отчета.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(saveFileDialog.FileName, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());

                    Paragraph titlePara = body.AppendChild(new Paragraph());
                    Run titleRun = titlePara.AppendChild(new Run());
                    titleRun.AppendChild(new Text("Отчет преподавателя по группе ИСП-21-2"));
                    titlePara.ParagraphProperties = new ParagraphProperties
                    {
                        Justification = new Justification { Val = JustificationValues.Center },
                        SpacingBetweenLines = new SpacingBetweenLines { After = "200" }
                    };
                    titleRun.RunProperties = new RunProperties(new Bold(), new FontSize { Val = "28" });

                    Paragraph subjectPara = body.AppendChild(new Paragraph());
                    Run subjectRun = subjectPara.AppendChild(new Run());
                    subjectRun.AppendChild(new Text("по предмету: МДК.01.01 Разработка программных модулей"));
                    subjectPara.ParagraphProperties = new ParagraphProperties
                    {
                        Justification = new Justification { Val = JustificationValues.Center },
                        SpacingBetweenLines = new SpacingBetweenLines { After = "200" }
                    };
                    subjectRun.RunProperties = new RunProperties(new FontSize { Val = "24" });

                    Paragraph datePara = body.AppendChild(new Paragraph());
                    Run dateRun = datePara.AppendChild(new Run());
                    dateRun.AppendChild(new Text($"на дату: {DateTime.Now:dd.MM.yyyy}"));
                    datePara.ParagraphProperties = new ParagraphProperties
                    {
                        Justification = new Justification { Val = JustificationValues.Center },
                        SpacingBetweenLines = new SpacingBetweenLines { After = "400" }
                    };
                    dateRun.RunProperties = new RunProperties(new FontSize { Val = "24" });

                    Table table = body.AppendChild(new Table());

                    TableProperties tableProps = new TableProperties(
                        new TableBorders(
                            new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                            new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                            new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                            new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                            new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                            new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 }
                        )
                    );
                    table.AppendChild(tableProps);

                    TableRow headerRow = table.AppendChild(new TableRow());
                    string[] headers = { "Студенты", "Работы, сданные за неделю" };
                    foreach (var header in headers)
                    {
                        TableCell headerCell = headerRow.AppendChild(new TableCell());
                        Paragraph headerPara = headerCell.AppendChild(new Paragraph());
                        Run headerRun = headerPara.AppendChild(new Run());
                        headerRun.AppendChild(new Text(header));
                        headerRun.RunProperties = new RunProperties(new Bold());
                        headerPara.ParagraphProperties = new ParagraphProperties
                        {
                            Justification = new Justification { Val = JustificationValues.Center }
                        };
                        headerCell.AppendChild(new TableCellProperties(
                            new TableCellWidth { Width = "3000", Type = TableWidthUnitValues.Dxa }
                        ));
                    }

                    foreach (var consultation in consultations)
                    {
                        TableRow row = table.AppendChild(new TableRow());

                        TableCell studentCell = row.AppendChild(new TableCell());
                        studentCell.AppendChild(new Paragraph(new Run(new Text(consultation.StudentFullName))));

                        TableCell submittedCell = row.AppendChild(new TableCell());
                        string submittedThisWeek = "";
                        DateTime weekStart = DateTime.Now.AddDays(-7); // Считаем последнюю неделю (с 19 по 25 апреля 2025)
                        if (consultation.Date >= weekStart && consultation.Date <= DateTime.Now)
                        {
                            submittedThisWeek = consultation.PracticeSubmitted ?? "Нет данных";
                        }
                        else
                        {
                            submittedThisWeek = "Нет данных";
                        }
                        submittedCell.AppendChild(new Paragraph(new Run(new Text(submittedThisWeek))));

                    }

                    // Сохраняем документ
                    wordDocument.Save();
                }

                MessageBox.Show("Отчет успешно сгенерирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}