using System.Windows.Input;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace JulyPractice
{
    public class AuthorizationWindowVM : INotifyPropertyChanged
    {
        public ICommand EnterButton { get; }
        public ICommand CreateAccountButton { get; }

        private string loginTextBox;
        public string LoginTextBox
        {
            get { return loginTextBox; }
            set
            {
                loginTextBox = value;
                OnPropertyChanged(nameof(LoginTextBox));
            }
        }

        private string passwordTextBox;
        public string PasswordTextBox
        {
            get { return passwordTextBox; }
            set
            {
                passwordTextBox = value;
                OnPropertyChanged(nameof(PasswordTextBox));
            }
        }

        private string messageBlock;
        public string MessageBlock
        {
            get { return messageBlock; }
            set
            {
                messageBlock = value;
                OnPropertyChanged(nameof(MessageBlock));
            }
        }
        public AuthorizationWindowVM()
        {
            CreateAccountButton = new RelayCommand(CreateAccount);
            EnterButton = new RelayCommand(Enter);
        }

        private void Enter(object parameter)
        {
            if (string.IsNullOrEmpty(LoginTextBox) || string.IsNullOrEmpty(PasswordTextBox))
            {
                MessageBlock = "Пожалуйста, введите логин и пароль.";
                return;
            }

            using (var context = new CurrentDbContext())
            {
                context.Database.EnsureCreated();
                var user = context.Users.SingleOrDefault(u => u.Username == LoginTextBox);
                if (user != null && PasswordHasher.VerifyPassword(PasswordTextBox, user.PasswordHash))
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                else
                {
                    MessageBlock = "Неверный логин или пароль.";
                }
            }
        }
        private void CreateAccount(object parameter)
        {
            CreateAccountWindow createAccountWindow = new CreateAccountWindow();
            createAccountWindow.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
