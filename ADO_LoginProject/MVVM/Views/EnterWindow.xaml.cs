using ADO_LoginProject.Enums;
using ADO_LoginProject.Facade;
using ADO_LoginProject.Interfaces;
using ADO_LoginProject.MVVM.Models.DerivedClasses;
using ADO_LoginProject.MVVM.ViewModels;
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
using System.Windows.Shapes;
using ADO_LoginProject.Services;

namespace ADO_LoginProject.MVVM.Views
{
    /// <summary>
    /// Interaction logic for EnterWindow.xaml
    /// </summary>
    public partial class EnterWindow : Window, IResetable
    {

        SignUp sign = new();

        LoginPage login = new();

        public EnterWindow()
        {
            InitializeComponent();

            App.AppWindow = new();

            if (App.DataFacade.Remember && App.DataFacade.UserID     != null)
            {
                if (App.DataFacade.Login(App.DataFacade.UserID) == ProcessResult.Success)
                {
                    SoundService.Succes();
                    App.ToAppWindow();
                }
            }

            #region Implement Commands

            App.Container.GetInstance<LoginPageVM>().LoginClick = new(p =>
            {
                ProcessResult res = App.DataFacade.Login
                                                (
                                                    App.Container.GetInstance<LoginPageVM>().Username,
                                                    App.Container.GetInstance<LoginPageVM>().Password
                                                );


                if (res == ProcessResult.Success)
                {
                    App.DataFacade.Remember = App.Container.GetInstance<LoginPageVM>().Remember;
                    SoundService.Succes();
                    App.ToAppWindow();
                }
                else
                    CMessageBox.Show(res.ToString(), CMessageTitle.Warning, CMessageButton.Ok, CMessageButton.None);

            });

            App.Container.GetInstance<LoginPageVM>().SignUpClick = new(p => { Frame.Navigate(sign); });

            App.Container.GetInstance<SignUpVM>().SignIn = new(p => { Frame.Navigate(login); });

            App.Container.GetInstance<WelcomePageVM>().SignUp = new(p => { Frame.Navigate(sign); });

            App.Container.GetInstance<WelcomePageVM>().SignIn = new(p => { Frame.Navigate(login); });

            #endregion

            App.EnterWindow = this;

            Frame.Navigate(new WelcomePage());
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {


            if (sender is Button btn)
            {
                switch (btn.Content.ToString())
                {
                    case "_":
                        this.WindowState = WindowState.Minimized;
                        break;
                    case "X":
                        App.DataFacade.Save() ;
                        Application.Current.Shutdown();
                        break;
                    default:
                        break;
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        public void Reset()
        {
            sign.Reset();
            login.Reset();
        }

    }
}
