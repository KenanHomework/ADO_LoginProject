using ADO_LoginProject.Commands;
using ADO_LoginProject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ADO_LoginProject.MVVM.ViewModels 
{
    public class EnterSecurityVM
    {

        #region Members

        private int code;

        public DialogResult DialogResult { get; set; }

        public int Code
        {
            get { return code; }
            set { code = value; OnPropertyChanged(); }
        }

        public int RefCode { get; set; } = 0;

        #endregion

        #region PropertyChangedEventHandler

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region Commands

        public RelayCommand Submit { get; set; }

        #endregion

        #region Methods

        #endregion

    }
}
