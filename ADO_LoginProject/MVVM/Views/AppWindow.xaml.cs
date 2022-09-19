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
using ADO_LoginProject.Interfaces;
using ADO_LoginProject.MVVM.ViewModels;

namespace ADO_LoginProject.MVVM.Views
{
    /// <summary>
    /// Interaction logic for AppWindow.xaml
    /// </summary>
    public partial class AppWindow : Window, IResetable
    {
        #region Members

        public int IndexSelected { get; set; } = -1;

        private AppWindowVM datacontextconverted;

        public AppWindowVM DatacontextConverted
        {
            get => ((AppWindowVM)DataContext);
        }



        #endregion

        public AppWindow()
        {
            InitializeComponent();
            DataContext = App.Container.GetInstance<AppWindowVM>();
            DatacontextConverted.Window = this;
            DatacontextConverted.InitializePages();
        }

        #region Methods

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set tooltip visibility
            Visibility visibility = Tg_Btn.IsChecked == true ? Visibility.Collapsed : Visibility.Visible;
            tt_profile.Visibility = visibility;
            tt_maps.Visibility = visibility;
            tt_history.Visibility = visibility;
            tt_about.Visibility = visibility;
            tt_logout.Visibility = visibility;
            tt_adjustment.Visibility = visibility;
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            SideBarGridColumn.Width = new GridLength(65);

            img_bg.Opacity = 1;
        }

        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            SideBarGridColumn.Width = new GridLength(200);
            img_bg.Opacity = 0.3;
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void LV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LV.SelectedIndex == IndexSelected)
                return;
            IndexSelected = LV.SelectedIndex;
            switch (LV.SelectedIndex)
            {
                case 5:
                    {
                        DatacontextConverted.LogoutClick();
                        break;
                    }
                default:
                    break;
            }
        }

        #endregion

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
                        App.DataFacade.Save();
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
        }
    }
}
