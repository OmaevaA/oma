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

namespace TZOmPavMalPech.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditBook.xaml
    /// </summary>
    public partial class AddEditBook : Page
    {
        private Entities.Books _currentProduct = null;
        private string _selectedImagePath; // Переменная для хранения пути к изображению

        public AddEditBook()
        {
            InitializeComponent();
        }

        public AddEditBook(Entities.Books product)
        {
            InitializeComponent();
            _currentProduct = product;
            Title = "Редактировать книгу";
            ComboGenre.Text = _currentProduct.Genre.Name;
            TextBoxName.Text = _currentProduct.Name;
            TextBoxDisc.Text = _currentProduct.Discription;
            TextBoxAvtor.Text = _currentProduct.Avtor;
            TextBoxPublisher.Text = _currentProduct.Publisher.Name;
            TextBoxDate.SelectedDate = _currentProduct.DateBook;

            // Загружаем текущий путь к изображению
            _selectedImagePath = _currentProduct.Image;
        }

        //метод с проверкой введеных значений
        private string CheckErrors()
        {
            var errorbuilder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(TextBoxName.Text))
                errorbuilder.AppendLine("Наименование должно быть заполнено!");
            if (string.IsNullOrWhiteSpace(TextBoxDisc.Text))
                errorbuilder.AppendLine("Описание должно быть заполнено!");
            if (string.IsNullOrWhiteSpace(TextBoxPublisher.Text))
                errorbuilder.AppendLine("Издатель должен быть заполнен!");
            return errorbuilder.ToString();
        }

        //кнопка Сохранить
        private void BtnSave_Click(object sender, RoutedEventArgs e)
 {
     var errorMessage = CheckErrors();
     if (errorMessage.Length > 0)
     {
         MessageBox.Show(errorMessage, "Ошибка!",
         MessageBoxButton.OK, MessageBoxImage.Error);
     }
     else
     {
         // Ищем или создаем издателя
         var publisher = App.Context.Publisher
             .FirstOrDefault(p => p.Name == TextBoxPublisher.Text);
         if (publisher == null)
         {
             publisher = new Entities.Publisher { Name = TextBoxPublisher.Text };
         }

         // Ищем или создаем жанр
         var genre = App.Context.Genre
             .FirstOrDefault(g => g.Name == ComboGenre.Text);
         if (genre == null)
         {
             genre = new Entities.Genre { Name = ComboGenre.Text };
         }

         // ⭐ ПРОВЕРКА НА ПУСТУЮ КАРТИНКУ ⭐
         string imagePath = _selectedImagePath;
         if (string.IsNullOrWhiteSpace(imagePath))
         {
             // Если картинка не выбрана — ставим заглушку
             imagePath = "/res/picture.jpg";
         }

         if (_currentProduct == null)
         {
             var book = new Entities.Books
             {
                 Name = TextBoxName.Text,
                 Discription = TextBoxDisc.Text,
                 Avtor = TextBoxAvtor.Text,
                 Publisher = publisher,
                 Genre = genre,
                 DateBook = TextBoxDate.SelectedDate.Value,
                 Image = imagePath  // Используем проверенный путь
             };
             App.Context.Books.Add(book);
             App.Context.SaveChanges();
             MessageBox.Show("Информация добавлена.", "Успех",
                             MessageBoxButton.OK, MessageBoxImage.Information);
         }
         else
         {
             _currentProduct.Genre = genre;
             _currentProduct.Name = TextBoxName.Text;
             _currentProduct.Discription = TextBoxDisc.Text;
             _currentProduct.Publisher = publisher;
             _currentProduct.Avtor = TextBoxAvtor.Text;
             _currentProduct.DateBook = TextBoxDate.SelectedDate.Value;

             // Обновляем изображение только если выбрано новое
             if (!string.IsNullOrEmpty(_selectedImagePath))
             {
                 _currentProduct.Image = _selectedImagePath;
             }
             // Если изображение не выбрано, но в базе уже было пустое — оставляем пустым
             // или тоже ставим заглушку
             if (string.IsNullOrEmpty(_currentProduct.Image))
             {
                 _currentProduct.Image = "/res/no-image.png";
             }

             App.Context.SaveChanges();
             MessageBox.Show("Информация редактирована.", "Успех",
                             MessageBoxButton.OK, MessageBoxImage.Information);
         }
         NavigationService.Navigate(new Pages.Catalog());
     }
 }

        //кнопка выбора фото
        private void BtnFoto_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;";
            dialog.Title = "Выберите изображение";

            if (dialog.ShowDialog() == true)
            {
                _selectedImagePath = dialog.FileName;
            }
        }
    }
}
