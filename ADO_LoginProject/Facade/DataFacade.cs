using ADO_LoginProject.Enums;
using ADO_LoginProject.MVVM.Models.DerivedClasses;
using ADO_LoginProject.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ADO_LoginProject.Facade
{
    public class DataFacade
    {

        #region Members

        public int? UserID { get; set; }

        public bool Remember { get; set; } = false;

        #endregion

        #region PropertyChangedEventHandler

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region Methods

        public void ReadData(DataFacade dataFacade)
        {
            this.UserID = dataFacade.UserID ;
            this.Remember = dataFacade.Remember;
        }

        public void Save()
        {
            JSONService.Write("dataset/dataFacade.json", this);
        }

        public void Load()
        {

            DataFacade loaded = this;

            try
            {
                loaded = JSONService.Read<DataFacade>("dataset/dataFacade.json");
                ReadData(loaded);
            }
            catch (Exception) { }


        }

        public void Exit()
        {
            UserID = null;
            Save();
        }

        public ProcessResult Login(string username, string password)
        {

            ProcessResult result = UserService.Search(username, password);
            if (result == ProcessResult.Success)
                UserID = UserService.Read(username).ID;

            return result;
        }

        public ProcessResult Login(User user)
        {

            ProcessResult result = UserService.Search(user);
            if (result == ProcessResult.Success)
                UserID = UserService.Read(user.Username).ID;
            return result;
        }

        public ProcessResult Login(int? id)
        {

            ProcessResult result = UserService.Search(id);
            if (result == ProcessResult.Success)
                UserID = UserService.Read(id).ID;
            return result;
        }

        public ProcessResult Signin(User user, bool autoSaveUser = true)
        {
            ProcessResult result = UserService.Search(user);
            if (result == ProcessResult.NotFound)
            {
                UserID = user.ID;
                if (autoSaveUser)
                    UserService.Write(user);
            }

            return ProcessResult.Success;
        }

        #endregion

        public DataFacade() { }

        public override string ToString() => $"UserID: {UserID}  \n Remember: {Remember}";
    }
}
