using ADO_LoginProject.Commands;
using ADO_LoginProject.Enums;
using ADO_LoginProject.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using ADO_LoginProject.MVVM.Views;
using ADO_LoginProject.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Security;
using ADO_LoginProject.MVVM.Models.DerivedClasses;

namespace ADO_LoginProject.MVVM.ViewModels
{
    public class LoginPageVM : IEnterPage, IResetable
    {

        #region Members

        private string usernam;

        public string Username
        {
            get { return usernam; }
            set { usernam = value; OnPropertyChanged(); }
        }

        private string password;


        private int SecurityCode { get; set; }

        public string Email { get; set; }


        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }

        private bool remeber = true;

        public bool Remember
        {
            get { return remeber; }
            set { remeber = value; OnPropertyChanged(); }
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

        public RelayCommand LoginClick { get; set; }

        public RelayCommand LoginLocal { get; set; }

        public RelayCommand ForgotPasswordClick { get; set; }

        public RelayCommand ResetPasswordClick { get; set; }

        public RelayCommand SignUpClick { get; set; }

        #endregion

        #region Methods

        bool LoginCanRun(object param) => AllInfoCorrect();

        public void Login(object param)
        {
            if (AllInfoCorrect())
            {
                LoginClick.Execute(param);
            }
        }

        public bool AllInfoCorrect() =>
            (!string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(Username) &&
            Regex.IsMatch(Username, "^([A-Za-z0-9]){4,20}$") &&
            Regex.IsMatch(Password, "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$"));


        //     !string.IsNullOrWhiteSpace(Password) &&
        //!string.IsNullOrWhiteSpace(Username) &&
        //Regex.IsMatch(Username, "^([A-Za-z0-9]){4,20}$") &&
        //Regex.IsMatch(Password, "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");

        public void Reset()
        {
            Username = String.Empty;
            Password = String.Empty;
        }

        public void SendMail()
        {
            SecurityCode = new Random().Next(1000, 9999);
            Email = UserService.Read(Username).Email;
            EmailService.Send(Email, "Reset Password Security Code", $"Your Reset Code: {SecurityCode}", "WolfTaxi");
        }

        public bool SendCode()
        {
            SendMail();
            CMessageBox.Show("Security code sended. If don't see mail please view SPAM.\nPlese write security code under 2 minute.", CMessageTitle.Info, CMessageButton.Ok, CMessageButton.None);
            EnterSecurityCode enter = new();
            enter.Reset();
            enter.Code = SecurityCode;
            enter.ShowDialog();
            if (enter.DialogResult == DialogResult.Success)
            {
                EnterNewPassword password = new();
                password.Reset();
                password.ShowDialog();
                if (password.DialogResult == DialogResult.Success)
                {
                    User user = UserService.Read(Username);
                    user.Password.UpdateValue(password.Password.Text);
                    UserService.Update(user);
                    SoundService.Succes();
                    return true;
                }
            }
            return false;
        }



        private void ResetPassword(object param)
        {

            if (SendCode() == false) return;

            CMessageBox.Show("Succes Reset Password !", CMessageTitle.Info, CMessageButton.Ok, CMessageButton.None);
        }

        public bool CanResetPassword(object param) => (!string.IsNullOrWhiteSpace(Username) && Regex.IsMatch(Username, "^([A-Za-z0-9]){4,20}$"));

        #endregion

        public LoginPageVM()
        {
            LoginLocal = new(Login, LoginCanRun);
            ResetPasswordClick = new(ResetPassword, CanResetPassword);
        }

    }
}
