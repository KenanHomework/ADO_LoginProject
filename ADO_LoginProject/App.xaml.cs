using ADO_Lesson_1.Querys;
using ADO_LoginProject.Facade;
using ADO_LoginProject.Interfaces;
using ADO_LoginProject.MVVM.ViewModels;
using ADO_LoginProject.MVVM.Views;
using ADO_LoginProject.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ADO_LoginProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        #region Members

        public static string SuccesSoundEffect = "https://res.cloudinary.com/kysbv/video/upload/v1661935108/WolfTaxi/success-sound-effect.mp3";
        public static string ErrorSoundEffect = "https://res.cloudinary.com/kysbv/video/upload/v1661936264/WolfTaxi/error-sound.mp3";
        public static string NotificationSoundEffect = "https://res.cloudinary.com/kysbv/video/upload/v1661940169/WolfTaxi/notification-sound.mp3";
        public static string ConnectionString;

        public static DataFacade DataFacade = new();

        public static EnterWindow EnterWindow;
        public static AppWindow AppWindow;

        public static SimpleInjector.Container Container = new();

        #endregion

        #region Methods

        void Register()
        {
            Container.RegisterSingleton<EnterSecurityVM>();
            Container.RegisterSingleton<SignUpVM>();
            Container.RegisterSingleton<DataFacade>();
            Container.RegisterSingleton<WelcomePageVM>();
            Container.RegisterSingleton<AppWindowVM>();
            Container.RegisterSingleton<LoginPageVM>();

            Container.Verify();
        }

        public void ResetAll()
        {
            
        }

        void ReadConnectionString()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appconfig.json");
            var config = builder.Build();
            ConnectionString = config.GetConnectionString("SqlServerConnection");
        }

        public static void ToEnterWindow()
        {
            if (AppWindow != null)
            {
                AppWindow.Reset();
                AppWindow.Close();
            }

            EnterWindow = new();
            EnterWindow.Reset();
            EnterWindow.ShowDialog();
        }

        public static void ToAppWindow()
        {
            if (EnterWindow != null)
            {
                EnterWindow.Reset();
                EnterWindow.Close();
            }
            AppWindow = new();
            AppWindow.ShowDialog();
        }

        public static void CreateDirectorys()
        {
            Directory.CreateDirectory("Dataset");
        }

        public static void CheckSQl()
        {
            bool result;

            SqlService.ExecuteNonQuery(QuerysStorage.CheckDB);

            result = SqlService.ExecuteNonQuery(QuerysStorage.BackUpDifferent);
            if (!result) SqlService.ExecuteNonQuery(QuerysStorage.BackUpFull);

            result = SqlService.ExecuteNonQuery(QuerysStorage.CheckTable);
            if (!result) SqlService.ExecuteNonQuery(QuerysStorage.CreateTable);
        }

        #endregion

        public App()
        {
            Register();
            CreateDirectorys();
            ReadConnectionString();
            CheckSQl();
            DataFacade.Load();
        }

    }
}
