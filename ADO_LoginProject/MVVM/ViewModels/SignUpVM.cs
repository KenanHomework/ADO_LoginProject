using ADO_LoginProject.Commands;
using ADO_LoginProject.Enums;
using ADO_LoginProject.Interfaces;
using ADO_LoginProject.MVVM.Models.DerivedClasses;
using ADO_LoginProject.MVVM.Views;
using ADO_LoginProject.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ADO_LoginProject.MVVM.ViewModels
{
    public class SignUpVM : IEnterPage
    {

        #region Members

        private bool remember = true;

        public bool Remember
        {
            get { return remember; }
            set { remember = value; OnPropertyChanged(); }
        }


        private int SecurityCode { get; set; }

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(); }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }

        private string phone;

        public string Phone
        {
            get { return phone; }
            set { phone = value; OnPropertyChanged(); }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        #endregion

        #region PropertyChangedEventHandler

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region Commands

        public RelayCommand SignIn { get; set; }

        public RelayCommand SignUp { get; set; }

        #endregion

        #region Methods

        public void Reset()
        {
            Username = String.Empty;
            Email = String.Empty;
            Phone = String.Empty;
            Password = String.Empty;
        }

        public void Signup(object paam)
        {
            if (UserService.UserExist(Username))
            {
                CMessageBox.Show("User already Sign Up !", CMessageTitle.Warning, CMessageButton.Ok, CMessageButton.None);
                return;
            }
            SecurityCode = new Random().Next(1000, 9999);
            CMessageBox.Show("Security code sended. If don't see mail plese view SPAM.\nPlese write security code under 2 minute.", CMessageTitle.Info,CMessageButton.Ok, CMessageButton.None);
            EmailService.Send(Email, "Email Verification", $"Your Security Code: {SecurityCode}", "WolfTaxi");
            EnterSecurityCode enter = new();
            enter.Reset();
            enter.Code = SecurityCode;
            enter.ShowDialog();
            if (enter.DialogResult == DialogResult.Success)
            {
                User user = new(Username, Password, Email, Phone);
                UserService.Write(user);
                App.DataFacade.UserID = user.ID;
                App.DataFacade.Remember = Remember;
                SoundService.Succes();
                App.ToAppWindow();
            }
        }

        public bool CanSignUp(object param) => AllInfoCorrect();

        public bool AllInfoCorrect()
        => !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Phone) &&
            Phone.Length == 10 &&
            !string.IsNullOrWhiteSpace(Password) &&
            Regex.IsMatch(Username, "^([A-Za-z0-9]){4,20}$") &&
            Regex.IsMatch(Email, "\\b[\\w\\.-]+@[\\w\\.-]+\\.\\w{2,4}\\b") &&
            Regex.IsMatch(Password, "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$") &&
            Regex.IsMatch(Phone, "(\\+?( |-|\\.)?\\d{3}( |-|\\.)?)?(\\(?\\d{3}\\)?|\\d{2})( |-|\\.)?\\d{2}\\d{2}");

        #endregion

        public SignUpVM()
        {
            SignUp = new(Signup, CanSignUp);
        }

    }
}
