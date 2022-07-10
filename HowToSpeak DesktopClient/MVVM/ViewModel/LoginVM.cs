using HowToSpeak_DesktopClient.Infrastructure.Command;
using HowToSpeak_DesktopClient.Infrastructure.Server;
using HowToSpeak_DesktopClient.MVVM.ViewModel.Base;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace HowToSpeak_DesktopClient.MVVM.ViewModel
{
    class LoginVM : BaseViewModel
    {
        private string _username = Properties.Settings.Default["Username"] == null ? "" : Properties.Settings.Default["Username"] as string;

        public string Username
        {
            get { return _username; }
            set { _username = value;
                OnPropertyChanged();
            }
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


        public ICommand LogInCommand { get; }

        private bool CanLogInCommandExecute(object p) => true;

        private void OnLogInCommandExecuted(object p)
        {
            Regex regex = new Regex(@"[A-Za-z][A-Za-z\d]{5,}");
            if (!regex.IsMatch(Username))
            {
                Error = "Username should start with english letter, contain more than 4 characters without spaces";
                return;
            }
            Properties.Settings.Default["Username"] = Username;
            regex = new Regex(@"[\S]{8,}");
            if (!regex.IsMatch(Password))
            {
                Error = "password should contain more than 7 characters without spaces";
                return;
            }
            var server = HTSServer.getInstance();
            var response = server.login(Username, Password);
            if (!response.IsOk())
                Error = response.getJsonByKey("error");
        }

        public LoginVM()
        {
            LogInCommand = new ActionCommand(OnLogInCommandExecuted, CanLogInCommandExecute);
        }
    }
}
