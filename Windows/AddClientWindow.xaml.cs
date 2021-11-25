using System;
using System.Net.Mail;
using System.Windows;
using BeautyIvanovZaycev.DB;
namespace BeautyIvanovZaycev.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        public static Entities entities = new Entities();
        public static string path = "(пусто)";
        public AddClientWindow()
        {
            InitializeComponent();
        }

        private void btnFileOpen_Click(object sender, RoutedEventArgs e) {}

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbF.Text.Length > 0 || tbI.Text.Length > 0 && (bool)rbM.IsChecked)
                {
                    MailAddress def = null;
                    try { def = new MailAddress(tbMail.Text);}
                    catch(Exception ex) { MessageBox.Show("Битый эмейл"); def = null; throw ex; }

                    entities.Client.Add(new Client
                    {
                        FirstName = tbF.Text,
                        LastName = tbI.Text,
                        Patronymic = tbO.Text,
                        Birthday = calDR.SelectedDate,
                        RegistrationDate = DateTime.Now,
                        Email = tbMail.Text,
                        Phone = tbPhone.Text,
                        GenderCode = 1,
                        PhotoPath = path
                    });
                }
                else if (tbF.Text.Length > 0 || tbI.Text.Length > 0 && (bool)rbF.IsChecked)
                {
                    MailAddress def = null;
                    try { def = new MailAddress(tbMail.Text); }
                    catch (Exception ex) { MessageBox.Show("Битый эмейл"); def = null; throw ex; }

                    entities.Client.Add(new Client //Another way is to use the System.Net.Mail.MailAddress class. To determine whether an email address is valid, pass the email address to the MailAddress.MailAddress(String) class constructor.
                    {
                        FirstName = tbF.Text,
                        LastName = tbI.Text,
                        Patronymic = tbO.Text,
                        Birthday = calDR.SelectedDate,
                        RegistrationDate = DateTime.Now,
                        Email = tbMail.Text,
                        Phone = tbPhone.Text,
                        GenderCode = 2,
                        PhotoPath = path
                    });
                }
                entities.SaveChanges();
                MessageBox.Show("Клиент успешно добавлен", "Добавление клиента завершено!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch(Exception ex) {MessageBox.Show(ex.StackTrace);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
