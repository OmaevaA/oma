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

namespace TZOmPavMalPech
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrameMain.Navigate(new Pages.Autorization());
            // инициализируем обработку события ContentRendered сразу после загрузки страницы:
            FrameMain.ContentRendered += new EventHandler(MainFrame_ContentRendered);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {

            if (FrameMain.CanGoBack)
                FrameMain.GoBack();
        }

        // Скрываем кнопку назад на первой странице (странице авторизации),
        // показываем её на последующих страницах.

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {

            if (!FrameMain.CanGoBack || this.FrameMain.Content.GetType() == typeof(Pages.Autorization))
            {
                BtnBack.Visibility = Visibility.Collapsed;
            }
            else
            {
                BtnBack.Visibility = Visibility.Visible;
            }
        }
    }
}
