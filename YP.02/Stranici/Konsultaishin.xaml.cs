using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для Konsultaishin.xaml
    /// </summary>
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
                searchTextBox.Foreground = Brushes.White;
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
                    .Where(i => i.Date.ToString().Contains(searchText));
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
            StudentFullName.Text = string.Empty; // Обновлено на FullNameTextBox
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
                StudentFullName.Text = _selectedConsultation.StudentFullName; // Обновлено на FullNameTextBox
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
                        StudentFullName = StudentFullName.Text.Trim(), // Используйте текст из FullNameTextBox
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
                    _selectedConsultation.StudentFullName = StudentFullName.Text.Trim(); // Используйте текст из FullNameTextBox
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


       

    
    }
}
