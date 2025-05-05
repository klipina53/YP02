using System.IO;
using System.Linq;
using System.Printing;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using YP._02;

// ...

public partial class Markss : Page
{
    // В конструкторе после InitializeComponent добавляем:
    public Markss(UserRole currentUserRole)
    {
        InitializeComponent();
        // ваш существующий код...
        var btnReport = new Button
        {
            Content = "Сгенерировать отчёт",
            Margin = new Thickness(5),
            HorizontalAlignment = HorizontalAlignment.Left
        };
        btnReport.Click += GenerateReport_Click;
        // Добавьте кнопку куда вам нужно, например в панель над ListView:
        toolbarPanel.Children.Add(btnReport);
    }

    private void GenerateReport_Click(object sender, RoutedEventArgs e)
    {
        // 1) Загружаем данные
        var marks = _context.LoadMarks()
                            .ToList();

        // 2) Создаём FlowDocument
        var doc = new FlowDocument
        {
            PagePadding = new Thickness(50),
            ColumnWidth = double.PositiveInfinity
        };

        // Заголовок
        var title = new Paragraph(new Run("Отчёт по оценкам"))
        {
            FontSize = 24,
            FontWeight = FontWeights.Bold,
            TextAlignment = TextAlignment.Center,
            Margin = new Thickness(0, 0, 0, 20)
        };
        doc.Blocks.Add(title);

        // Таблица
        var table = new Table();
        doc.Blocks.Add(table);

        // задаём 4 столбца: Студент, Дисциплина, Оценка, Комментарий
        for (int i = 0; i < 4; i++)
            table.Columns.Add(new TableColumn());

        // Шапка таблицы
        var headerRow = new TableRow();
        headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Студент"))) { FontWeight = FontWeights.Bold });
        headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Дисциплина"))) { FontWeight = FontWeights.Bold });
        headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Оценка"))) { FontWeight = FontWeights.Bold });
        headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Комментарий"))) { FontWeight = FontWeights.Bold });

        var headerGroup = new TableRowGroup();
        headerGroup.Rows.Add(headerRow);
        table.RowGroups.Add(headerGroup);

        // Данные
        var bodyGroup = new TableRowGroup();
        foreach (var m in marks)
        {
            var student = _context.LoadStudents().FirstOrDefault(s => s.StudentId == m.StudentId)?.FullName ?? "";
            var discipline = _context.LoadDisciplinePrograms().FirstOrDefault(d => d.DisciplineId == m.DisciplineProgramId)?.Topic ?? "";

            var row = new TableRow();
            row.Cells.Add(new TableCell(new Paragraph(new Run(student))));
            row.Cells.Add(new TableCell(new Paragraph(new Run(discipline))));
            row.Cells.Add(new TableCell(new Paragraph(new Run(m.Mark))));
            row.Cells.Add(new TableCell(new Paragraph(new Run(m.Description ?? ""))));
            bodyGroup.Rows.Add(row);
        }
        table.RowGroups.Add(bodyGroup);

        // 3) Показываем диалог печати / сохранения
        var dlg = new PrintDialog();
        if (dlg.ShowDialog() == true)
        {
            // печать
            IDocumentPaginatorSource idp = doc;
            dlg.PrintDocument(idp.DocumentPaginator, "Отчёт по оценкам");
        }
        else
        {
            // или сохраняем как XPS
            var saveDlg = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "XPS Document (*.xps)|*.xps",
                FileName = "ОтчётПоОценкам.xps"
            };
            if (saveDlg.ShowDialog() == true)
            {
                using (var xpsStream = new XpsDocument(saveDlg.FileName, FileAccess.ReadWrite))
                {
                    var writer = XpsDocument.CreateXpsDocumentWriter(xpsStream);
                    IDocumentPaginatorSource idp = doc;
                    writer.Write(idp.DocumentPaginator);
                }
                MessageBox.Show("Отчёт сохранён в " + saveDlg.FileName,
                                "Готово",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }
    }
}
