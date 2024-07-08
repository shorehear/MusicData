using System.Windows;

namespace JulyPractice
{
    public partial class MainWindow : Window
    {
        private MainWindowVM mainWindowVM;
        public MainWindow()
        {
            InitializeComponent();

            var dataOrchestrator = new DataOrchestrator(new CurrentDbContext());
            mainWindowVM = new MainWindowVM(dataOrchestrator);
            DataContext = mainWindowVM;
        }
    }
}
