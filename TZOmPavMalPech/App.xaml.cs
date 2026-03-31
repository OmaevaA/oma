using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TZOmPavMalPech
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Entities.BookCatalogEntities2 Context
        { get; } = new Entities.BookCatalogEntities2();

        //свойство для хранения авторизованного пользователя
        public static Entities.User CurrentUser { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Инициализация CurrentUser
            CurrentUser = new Entities.User(); // Или загрузка из базы данных
        }
    }
}
