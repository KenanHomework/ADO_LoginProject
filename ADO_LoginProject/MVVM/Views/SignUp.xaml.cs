using ADO_LoginProject.Enums;
using ADO_LoginProject.Interfaces;
using ADO_LoginProject.MVVM.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ADO_LoginProject.MVVM.Views
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Page,IResetable
    {
        public SignUp()
        {
            InitializeComponent();
            DataContext = App.Container.GetInstance<SignUpVM>();
        }

        public DialogResult DialogResult { get; set; } = DialogResult.Cancel;

        private void Username_TextChanged(object sender, TextChangedEventArgs e)
                    => RegxService.CheckControl(ref Username, 3, Color.FromRgb(237, 236, 239), "^([A-Za-z0-9]){4,20}$");


        private void Email_TextChanged(object sender, TextChangedEventArgs e)
                    => RegxService.CheckControl(ref Email, 3, Color.FromRgb(237, 236, 239), "\\b[\\w\\.-]+@[\\w\\.-]+\\.\\w{2,4}\\b");


        private void Phone_TextChanged(object sender, TextChangedEventArgs e)
                    => RegxService.CheckControl(ref Phone, 10, Color.FromRgb(237, 236, 239), "(\\+?( |-|\\.)?\\d{3}( |-|\\.)?)?(\\(?\\d{3}\\)?|\\d{2})( |-|\\.)?\\d{2}\\d{2}");

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            RegxService.CheckControl(ref Password, 8, Color.FromRgb(237, 236, 239), "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
            App.Container.GetInstance<SignUpVM>().Password = Password.Password;
        }

        public void Reset() => App.Container.GetInstance<SignUpVM>().Reset();
    }
}
