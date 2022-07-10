﻿using HowToSpeak_DesktopClient.MVVM.ViewModel.Base;
using System.Windows.Input;
using HowToSpeak_DesktopClient.Infrastructure.Command;

namespace HowToSpeak_DesktopClient.MVVM.ViewModel
{
    class RigisterLoginVM : BaseViewModel
    {
        // This is VM for general window, which can contain many other pages (Register and login in this case)
        private object _currentview;

        public object CurrentView
        {
            get => _currentview;
            set => Set(ref _currentview, value);
        }

        public RegisterVM RegisterVM_v { get; set; }

        public LoginVM LoginVM_v { get; set; }


        #region Commands

        #region Commands View Subwindows

        public ICommand RegisterViewCommand { get; }

        private bool CanRegisterViewCommandExecute(object p) => true;

        private void OnRegisterViewCommandExecuted(object p)
        {
            CurrentView = RegisterVM_v;
        }

        public ICommand LoginViewCommand { get; }

        private bool CanLoginViewCommandExecute(object p) => true;

        private void OnLoginViewCommandExecuted(object p)
        {
            CurrentView = LoginVM_v;
        }

        #endregion

        #endregion

        public RigisterLoginVM()
        {
            RegisterViewCommand = new ActionCommand(OnRegisterViewCommandExecuted, CanRegisterViewCommandExecute);
            LoginViewCommand = new ActionCommand(OnLoginViewCommandExecuted, CanLoginViewCommandExecute);

            // Set start screen
            RegisterVM_v = new RegisterVM();
            LoginVM_v = new LoginVM();

            CurrentView = LoginVM_v;
        }
    }
}
