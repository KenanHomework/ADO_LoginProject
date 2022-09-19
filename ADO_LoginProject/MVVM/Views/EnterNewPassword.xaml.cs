using ADO_LoginProject.Enums;
using ADO_LoginProject.Interfaces;
using ADO_LoginProject.Services;
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

namespace ADO_LoginProject.MVVM.Views
{
    /// <summary>
    /// Interaction logic for EnterNewPassword.xaml
    /// </summary>
    public partial class EnterNewPassword : Window, IResetable
    {
        public EnterNewPassword()
        {
            InitializeComponent();
        }

        public DialogResult DialogResult { get; set; } = DialogResult.Cancel;

        private void ResizeButton_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e) =>
        Submite.IsEnabled = RegxService.CheckControl(ref Password, 8, Color.FromRgb(237, 236, 239), "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");

        private void Submite_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Success;
            Close();
        }

        public void Reset()
        {
            Password.Text = String.Empty;
        }
    }
}
