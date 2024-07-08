using System.Windows;
using System;

namespace JulyPractice
{
    public partial class CreateAccountWindow : Window
    {
        private CreateAccountVM createAccVM;
        public CreateAccountWindow()
        {
            InitializeComponent();

            createAccVM = new CreateAccountVM();
            DataContext = createAccVM;
        }
    }
}
