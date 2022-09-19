using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ADO_LoginProject.Commands;
using ADO_LoginProject.Interfaces;
using ADO_LoginProject.MVVM.Models.DerivedClasses;
using ADO_LoginProject.MVVM.Views;
using ADO_LoginProject.Services;

namespace ADO_LoginProject.MVVM.ViewModels
{
    public class AppWindowVM : IResetable
    {
        #region Members

        public User User { get; set; }

        public AppWindow Window { get; set; }


        #endregion

        #region Commands

        //public RelayCommand Logout { get; set; }

        //public RelayCommand About { get; set; }


        #endregion

        #region PropertyChangedEventHandler

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region Methods

        public void InfoGithubClick()
        {
            WebService.OpenWebsiteWithUrl("https://github.com/KenanHomework/ADO_LoginProject.git");
        }

        public void LogoutClick()
        {
            App.DataFacade.Exit();
            Reset();
            SoundService.Notification();
            App.ToEnterWindow();
        }

        public void Reset()
        {

        }

        public void InitializePages()
        {
          
        }

        #endregion

        public AppWindowVM()
        {

        }
    }
}
