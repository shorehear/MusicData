using System.Windows;

namespace JulyPractice
{
    public partial class AuthorizationWindow : Window
    {
        private AuthorizationWindowVM authVM;
        public AuthorizationWindow()
        {
            InitializeComponent();
            authVM = new AuthorizationWindowVM();
            DataContext = authVM;
        }
    }
}
