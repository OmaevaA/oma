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
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Page
    {
        public Catalog()
        {
            InitializeComponent();
            // Получаем объект текущего пользователя из приложения
            var currentUser = App.CurrentUser;
            // Устанавливаем текст метки именем пользователя
            UsernameLabel.Text = $"Добро пожаловать! {currentUser.FIO}";
            DataContext = this; // устанавливаем DataContext для биндинга
            //вывод списка товаров
            LviewProduct.ItemsSource = App.Context.Books.ToList();
            //для сортировки
            ComboSortBy.SelectedIndex = 0; // Устанавливает начальное положение первого выпадающего списка сортировки
            ComboSortSuplier.SelectedIndex = 0; // для поставщиков
            DataContext = this;
            UpdateProduct();// Сразу вызывает обновление списка продуктов
        }


        //создание метода
        private void UpdateProduct()
        {
            var product = App.Context.Books.ToList();
            // Получаем все книги
    var books = App.Context.Books.ToList();
    
    // Получаем ID текущего пользователя (если это не гость)
    int currentUserId = -1;
    if (App.CurrentUser != null && App.CurrentUser.ID_User > 0)
    {
        currentUserId = App.CurrentUser.ID_User;
    }
    
    // Для каждой книги проверяем, прочитал ли её текущий пользователь
    foreach (var book in books)
    {
        if (currentUserId > 0)
        {
            // Ищем запись в таблице Readed
            var readRecord = App.Context.Readed
                .FirstOrDefault(r => r.User_Id == currentUserId && r.Books_Id== book.Id_Books);
            
            // Если запись есть и is_read = 1, то книга прочитана
            book.IsReadByUser = readRecord != null && readRecord.is_read == 1;
        }
        else
        {
            // Если гость — не показываем отметки
            book.IsReadByUser = false;
        }
    }
            LviewProduct.ItemsSource = product;
            if (ComboSortBy.SelectedIndex == 0) ;

            else if (ComboSortBy.SelectedIndex == 1)
                product = product.Where(p => p.Genre != null && p.Genre.Name == "Роман-эпопея").ToList();
            else if (ComboSortBy.SelectedIndex == 2)
                product = product.Where(p => p.Genre != null && p.Genre.Name == "Роман").ToList();
            else if (ComboSortBy.SelectedIndex == 3)
                product = product.Where(p => p.Genre != null && p.Genre.Name == "Сатира").ToList();
            else if (ComboSortBy.SelectedIndex == 4)
                product = product.Where(p => p.Genre != null && p.Genre.Name == "Проза / Притча").ToList();
            else if (ComboSortBy.SelectedIndex == 5)
                product = product.Where(p => p.Genre != null && p.Genre.Name == "Трагедия").ToList();
            //фильтрация по Поставщику
            if (ComboSortSuplier.SelectedIndex == 0);

            // Ничего не делаем, оставляем все товары

            else if (ComboSortSuplier.SelectedIndex == 1)
                product = product.Where(p => p.Publisher != null && p.Publisher.Name == "Староста").ToList();
            else if (ComboSortSuplier.SelectedIndex == 2)
                product = product.Where(p => p.Publisher != null && p.Publisher.Name == "Око").ToList();
            else if (ComboSortSuplier.SelectedIndex == 3)
                product = product.Where(p => p.Publisher != null && p.Publisher.Name == "Стветлая знать").ToList();

            //Поиск товара по названию
            product = product.Where(p => p.Name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            LviewProduct.ItemsSource = product;// Присваиваем отсортированный список продуктов источнику данных ListView
        }

        //обработчики событий для функций: поиск, фильтрация, сортировка
        private void ComboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void ComboSortCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void ComboSortSuplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void TBoxSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateProduct();
        }
        //кнопка "добавить" (навигация для перехода на следующую страницу)
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddEditBook());
        }
        

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateProduct();
        }
        //метод - При каждой смене состояния видимости страницы на видимую,
        //выполняется автоматическое обновление данных.
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue && this.Visibility == Visibility.Visible)
            {
                // Переинициализация контекста или простой запрос к базе данных
                using (var context = new BookCatalogEntities2())
                {
                    // Просто получаем свежие данные из базы
                    var freshProducts = context.Books.ToList();

                    // Теперь можем обновить список с этими свежими данными
                    LviewProduct.ItemsSource = freshProducts;
                }

            }
        }

        //кнопка редактировать
        private void Edid_Click(object sender, RoutedEventArgs e)
        {
            var currentProduct = (sender as Button).DataContext as Entities.Books;
            NavigationService.Navigate(new Pages.AddEditBook(currentProduct));
        }
        //кнопка удалить
        private void Del_Click(object sender, RoutedEventArgs e)
        {
            var currentProduct = (sender as Button)?.DataContext as Entities.Books;

            // Подтверждаем удаление только если количество на складе больше нуля
            if (MessageBox.Show($"Вы уверены, что хотите удалить продукт '{currentProduct.Name}'?",
                                 "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                App.Context.Books.Remove(currentProduct);
                App.Context.SaveChanges();
                UpdateProduct();
            }


        }



        private void Page_Loaded_2(object sender, RoutedEventArgs e)
 {
     // Проверяем, есть ли пользователь и роль
     if (App.CurrentUser != null && App.CurrentUser.Role != null)
     {
         string roleName = App.CurrentUser.Role.Name;

         if (roleName == "Администратор")
         {
             // Админ видит всё
             ComboSortBy.Visibility = Visibility.Visible;
             ComboSortSuplier.Visibility = Visibility.Visible;
             TBoxSearch.Visibility = Visibility.Visible;
             BtnAdd.Visibility = Visibility.Visible;
         }
         else if (roleName == "Авторизированный клиент")
         {
             // Пользователь видит поиск и фильтры, но НЕ видит кнопку добавить
             ComboSortBy.Visibility = Visibility.Visible;
             ComboSortSuplier.Visibility = Visibility.Visible;
             TBoxSearch.Visibility = Visibility.Visible;
             BtnAdd.Visibility = Visibility.Collapsed; // Скрываем кнопку Добавить
         }
     }
     else
     {
         // Если пользователь не авторизован — скрываем всё
         ComboSortBy.Visibility = Visibility.Visible;
         ComboSortSuplier.Visibility = Visibility.Visible;
         TBoxSearch.Visibility = Visibility.Visible;
         BtnAdd.Visibility = Visibility.Collapsed;
     }
 }

        private void ChkRead_Click(object sender, RoutedEventArgs e)
        {
            {
    var checkBox = sender as CheckBox;
    var book = checkBox?.DataContext as Entities.Books;
    
    if (book == null) return;
    
    // Проверяем, авторизован ли пользователь (не гость)
    if (App.CurrentUser == null || App.CurrentUser.ID_User <= 0)
    {
        MessageBox.Show("Только авторизованные пользователи могут отмечать прочитанные книги.");
        checkBox.IsChecked = false;
        return;
    }
    
    int currentUserId = App.CurrentUser.ID_User;
    bool isRead = checkBox.IsChecked == true;
    
    // Ищем существующую запись
    var readRecord = App.Context.Readed
        .FirstOrDefault(r => r.User_Id == currentUserId && r.Books_Id == book.Id_Books);
    
    if (readRecord != null)
    {
        // Обновляем существующую запись
        readRecord.is_read = isRead ? 1 : 0;
    }
    else
    {
        // Создаем новую запись
        var newRecord = new Entities.Readed
        {
            User_Id = currentUserId,
            Books_Id = book.Id_Books,
            is_read = isRead ? 1 : 0
        };
        App.Context.Readed.Add(newRecord);
    }
    
    // Сохраняем изменения
    App.Context.SaveChanges();
    
    // Обновляем локальное свойство книги
    book.IsReadByUser = isRead;
    
    // Обновляем список (чтобы пересчитать фильтры, если нужно)
    UpdateProduct();
}
        }
    }
    }

