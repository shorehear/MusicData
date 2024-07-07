using System.Windows;
using System.Windows.Controls;

namespace JulyPractice
{
    public static class PasswordBoxBinding
    {
        public static readonly DependencyProperty BindPasswordProperty =
            DependencyProperty.RegisterAttached("BindPassword",
            typeof(string), typeof(PasswordBoxBinding),
            new PropertyMetadata(string.Empty, OnBindPasswordChanged));

        public static string GetBindPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(BindPasswordProperty);
        }

        public static void SetBindPassword(DependencyObject dp, string value)
        {
            dp.SetValue(BindPasswordProperty, value);
        }

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                if (e.NewValue != null && passwordBox.Password != e.NewValue.ToString())
                {
                    passwordBox.Password = e.NewValue.ToString();
                }
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetBindPassword(passwordBox, passwordBox.Password);
            }
        }
    }
}