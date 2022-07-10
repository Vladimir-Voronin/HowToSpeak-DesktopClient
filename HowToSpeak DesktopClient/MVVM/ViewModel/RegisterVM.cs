using HowToSpeak_DesktopClient.Infrastructure.Command;
using HowToSpeak_DesktopClient.Infrastructure.Server;
using HowToSpeak_DesktopClient.MVVM.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HowToSpeak_DesktopClient.MVVM.ViewModel
{
    class RegisterVM : BaseViewModel
    {
        private string _username = "";

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _password = "";

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _error;

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged();
            }
        }


        public ICommand RegisterCommand { get; }

        private bool CanRegisterCommandExecute(object p) => true;

        private void OnRegisterCommandExecuted(object p)
        {
            Regex regex = new Regex(@"[A-Za-z][A-Za-z\d]{5,}");
            if (!regex.IsMatch(Username))
            {
                Error = "Username should start with english letter, contain more than 4 characters without spaces";
                return;
            }
            regex = new Regex(@"[\S]{8,}");
            if (!regex.IsMatch(Password))
            {
                Error = "password should contain more than 7 characters without spaces";
                return;
            }
            var server = HTSServer.getInstance();
            var response = server.register(Username, Password);
            if (!response.IsOk())
                Error = response.getJsonByKey("error");
        }

        public RegisterVM()
        {
            RegisterCommand = new ActionCommand(OnRegisterCommandExecuted, CanRegisterCommandExecute);
        }
    }
}
