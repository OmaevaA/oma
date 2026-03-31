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
using TZOmPavMalPech.Entities;

namespace TZOmPavMalPech.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Page
    {
        public Autorization()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var currentuser = App.Context.User
             .FirstOrDefault(p => p.Login == TBoxLogin.Text && p.Password == PboxPassword.Password);

            if (currentuser != null)
            {
                // Сохраняем текущего пользователя
                App.CurrentUser = currentuser;

                // НАПРАВЛЯЕМ НА ProductPage
                NavigationService.Navigate(new Pages.Catalog());
            }
            else
            {
                // Если пользователь не найден, выводим ошибку
                MessageBox.Show("Неверный логин или пароль.");
            }
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = new User() { FIO = "Гость" };
            NavigationService.Navigate(new Pages.Catalog());
            MessageBox.Show("Вы вошли как гость!");
        }
    }
}
