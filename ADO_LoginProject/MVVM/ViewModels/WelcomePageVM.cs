using ADO_LoginProject.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_LoginProject.MVVM.ViewModels
{
    public class WelcomePageVM
    {
        #region Commands

        public RelayCommand SignUp { get; set; }

        public RelayCommand SignIn { get; set; }

        #endregion
    }
}
