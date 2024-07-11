using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;

namespace JulyPractice
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public ICommand SaveAndExitCommand { get; set; }
        public ICommand AddInformationCommand { get; set; }

        private readonly DataOrchestrator dataOrchestrator;

        private ObservableCollection<Musician> musicians;
        public ObservableCollection<Musician> Musicians
        {
            get { return musicians; }
            set { musicians = value; OnPropertyChanged(); }
        }

        private Musician selectedMusician;
        public Musician SelectedMusician
        {
            get { return selectedMusician; }
            set
            {
                selectedMusician = value;
                OnPropertyChanged();

                LoadMusicianData();
            }
        }

        private ObservableCollection<Album> albums;
        public ObservableCollection<Album> Albums
        {
            get { return albums; }
            set { albums = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Song> songs;
        public ObservableCollection<Song> Songs
        {
            get { return songs; }
            set { songs = value; OnPropertyChanged(); }
        }

        private User user;
        public User User
        {
            get { return user; }
            set { user = value; OnPropertyChanged(); }
        }

        public MainWindowVM(DataOrchestrator dataOrchestrator)
        {
            this.dataOrchestrator = dataOrchestrator;

            SaveAndExitCommand = new RelayCommand(SaveAndExit);
            AddInformationCommand = new RelayCommand(AddInformation);

            LoadData();
        }

        private void LoadData()
        {
            Musicians = dataOrchestrator.LoadMusicians();
            User = dataOrchestrator.LoadUser();
        }

        private void LoadMusicianData()
        {
            if (SelectedMusician != null)
            {
                Albums = dataOrchestrator.LoadAlbums(SelectedMusician.MusicianID);
                Songs = dataOrchestrator.LoadSongs(SelectedMusician.MusicianID);
            }
            else
            {
                Albums = new ObservableCollection<Album>();
                Songs = new ObservableCollection<Song>();
            }
        }

        private void SaveAndExit(object parameter) { }

        private void AddInformation(object parameter) 
        {
            dataOrchestrator.LoadChoiceWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
