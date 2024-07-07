using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using System.Linq;
using System;
using System.Windows;

namespace JulyPractice
{
    public class CreateAccountVM : INotifyPropertyChanged
    {
        #region Init's
        public ICommand CreateAccountButton { get; }

        private string usernameTextBox;
        public string UsernameTextBox
        {
            get { return usernameTextBox; }
            set
            {
                usernameTextBox = value;
                OnPropertyChanged(nameof(UsernameTextBox));
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
        #endregion

        public Action CloseAction { get; set; }

        public CreateAccountVM() 
        {
            CreateAccountButton = new RelayCommand(CreateAccount);
        }

        private void CreateAccount(object parameter) 
        {
            if (string.IsNullOrEmpty(UsernameTextBox) || string.IsNullOrEmpty(PasswordTextBox)) 
            {
                MessageBlock = "Пожалуйста, введите логин и пароль";
            }

            if (PasswordTextBox.Length < 8)
            {
                MessageBlock = $"Пароль должен содержать не менее {8} символов.";
                return;
            }

            if (!IsPasswordValid(PasswordTextBox))
            {
                MessageBlock = "В пароле должны быть буквы обоих регистров";
                return;
            }

            using (var context = new CurrentDbContext()) 
            {
                if (context.Users.Any(u => u.Username == UsernameTextBox))
                {
                    MessageBlock = "Пользователь с таким логином уже существует.";
                    return;
                }

                var hashedPassword = PasswordHasher.HashPassword(PasswordTextBox);
                var user = new User
                {
                    Username = UsernameTextBox,
                    PasswordHash = hashedPassword
                };

                context.Users.Add(user);
                context.SaveChanges();

                MessageBox.Show("Аккаунт успешно создан.");
                CloseAction?.Invoke();
            }
        }
        private bool IsPasswordValid(string password)
        {
            return password.Any(char.IsDigit) && password.Any(char.IsUpper);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
