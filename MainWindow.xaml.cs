using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BeautyIvanovZaycev.Windows;
using BeautyIvanovZaycev.DB;

namespace BeautyIvanovZaycev
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Entities Entities = new Entities();
        private int numPage = 0;
        List<Client> massClient = new List<Client>();
        CollectionView view = null;
        //private int chosenOutput = 0; //10-50-200-все
        List<string> alphaSort = new List<string>()
        {
            "Дата посещения(↓)",
            "Дата посещения(↑)",
            "Фамилия (А-Я)",
            "Фамилия (Я-А)",
            "Количество посещений(↓)",
            "Количество посещений(↑)",
            "Все",
        };
        public MainWindow()
        {
            InitializeComponent();
            listview.ItemsSource = Entities.Client.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(listview.ItemsSource);
            view.Filter = ClientFilter;
            RowColCounter.Content = listview.Items.Count + " строк; страница " + (numPage + 1);// + listview. + " столбцов;";
            cboxSort.ItemsSource = alphaSort;
            cboxSort.SelectedIndex = 6;
        }

        public void AlphaFilter()
        {
            massClient = Entities.Client.ToList();
            int selectSort = cboxSort.SelectedIndex; //Фамилия, дата посещения, количество посещений
            switch (selectSort)
            {
                case 0:
                    massClient = massClient.OrderBy(i => i.RegistrationDate).ToList();
                    break;
                case 1:
                    massClient = massClient.OrderByDescending(i => i.RegistrationDate).ToList();
                    break;
                case 2:
                    massClient = massClient.OrderBy(i => i.FirstName).ToList();
                    break;
                case 3:
                    massClient = massClient.OrderByDescending(i => i.FirstName).ToList();
                    break;
                default:
                    massClient = massClient.OrderBy(i => i.ID).ToList();
                    break;
            };
            listview.ItemsSource = massClient;
        }

        class ClientsTable
        {
            public ClientsTable(int ID, string FirstName, string LastName, string Patronymic, string Birthday,
                string RegistrationDate, string Email, string Phone, int GenderCode, string PhotoPath)
            {
                this.ID = ID;
                this.FirstName = FirstName;
                this.LastName = LastName;
                this.Patronymic = Patronymic;
                this.Birthday = Birthday;
                this.RegistrationDate = RegistrationDate;
                this.Email = Email;
                this.Phone = Phone;
                this.GenderCode = GenderCode;
                this.PhotoPath = PhotoPath;
            }
            public int ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Patronymic { get; set; }
            public string Birthday { get; set; }
            public string RegistrationDate { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public int GenderCode { get; set; }
            public string PhotoPath { get; set; }
        }

        private bool ClientFilter(object item)
        {
            if (String.IsNullOrEmpty(tboxSearch.Text))
                return true;
            else
            {
                string all = (item as Client).FirstName + (item as Client).LastName + (item as Client).Patronymic + (item as Client).Email + (item as Client).Phone;
                return all.IndexOf(tboxSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            }
                //return (item as Client).FirstName.IndexOf(tboxSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void tboxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listview.ItemsSource).Refresh(); // https://tinyurl.com/listview-poisk-normalniy
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Действительно добавить?", "Назад пути уже нет", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    AddClientWindow addClientWindow = new AddClientWindow();
                    addClientWindow.ShowDialog();
                    listview.ItemsSource = Entities.Client.ToList();
                    break;
                default:
                    break;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы хотите удалить данную запись?", "Удаление клиента", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (listview.SelectedItem is Client client)
                {
                    Entities.Client.Remove((Client)listview.SelectedItem);
                    Entities.SaveChanges();
                    MessageBox.Show("Запись была", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    listview.ItemsSource = Entities.Client.ToList();
                }
                else
                    MessageBox.Show("Вы не выбрали пользователя из списка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) { Close(); }

        private void btnApps_Click(object sender, RoutedEventArgs e) { MessageBox.Show("Посещения не найдены!", "Посещения клиента", MessageBoxButton.OK, MessageBoxImage.Error); }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (numPage > 0)
                numPage--;
            pageSwitch10();
        }

        private void btnUp50_Click(object sender, RoutedEventArgs e)
        {
            if (numPage > 0)
                numPage--;
            pageSwitch50();
        }

        private void btnUp200_Click(object sender, RoutedEventArgs e)
        {
            if (numPage > 0)
                numPage--;
            pageSwitch200();
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (massClient.Count > 0)
                numPage++;
            pageSwitch10();
        }

        private void btnDown50_Click(object sender, RoutedEventArgs e)
        {
            if (massClient.Count > 0)
                numPage++;
            pageSwitch50();
        }

        private void btnDown200_Click(object sender, RoutedEventArgs e)
        {
            if (massClient.Count > 0)
                numPage++;
            pageSwitch200();
        }

        private void pageSwitch10()
        {
            massClient = Entities.Client.ToList();
            massClient = massClient.Skip(10 * numPage).Take(10).ToList();
            listview.ItemsSource = massClient;
            RowColCounter.Content = listview.Items.Count + " строк; страница " + (numPage + 1);// + listview. + " столбцов;";
        }

        private void pageSwitch50()
        {
            massClient = Entities.Client.ToList();
            massClient = massClient.Skip(50 * numPage).Take(50).ToList();
            listview.ItemsSource = massClient;
            RowColCounter.Content = listview.Items.Count + " строк; страница " + (numPage + 1);// + listview. + " столбцов;";
        }

        private void pageSwitch200()
        {
            massClient = Entities.Client.ToList();
            massClient = massClient.Skip(200 * numPage).Take(200).ToList();
            listview.ItemsSource = massClient;
            RowColCounter.Content = listview.Items.Count + " строк; страница " + (numPage + 1);// + listview. + " столбцов;";
        }

        private void pageSwitchAll()
        {
            massClient = Entities.Client.ToList();
            listview.ItemsSource = massClient;
            numPage = 0;
            RowColCounter.Content = listview.Items.Count + " строк; страница " + (numPage + 1);// + listview. + " столбцов;";
            RowColCounter.UpdateLayout();
        }

        private void btnAll_Click(object sender, RoutedEventArgs e) { pageSwitchAll(); }

        private void cboxSort_SelectionChanged(object sender, SelectionChangedEventArgs e) { AlphaFilter(); }
    }
}
