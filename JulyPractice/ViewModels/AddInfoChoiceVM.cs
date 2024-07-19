using System.Windows.Input;
using System.IO;
using System.Windows;
using System.ComponentModel;
using Microsoft.Win32;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace JulyPractice
{
    public class AddInfoChoiceVM : INotifyPropertyChanged
    {
        public static Guid NotDefinedCountryId { get; } = new Guid("00000000-0000-0000-0000-000000000000");

        private CurrentDbContext context;

        private string selectedInfoType;

        public string SelectedInfoType
        {
            get { return selectedInfoType; }
            set
            {
                if (selectedInfoType != value)
                {
                    selectedInfoType = value;
                    OnPropertyChanged(nameof(SelectedInfoType));
                }
            }
        }

        public ICommand AcceptInfoCommand { get; }

        public AddInfoChoiceVM(CurrentDbContext context)
        {
            this.context = context;
            AcceptInfoCommand = new RelayCommand(Accept);
        }

        private void Accept(object parameter)
        {
            if (parameter is string infoType)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    string selectedFileName = openFileDialog.FileName;

                    switch (infoType)
                    {
                        case "Добавить артиста":
                            UnpackMusicianData(selectedFileName);
                            break;
                        case "Добавить альбом":
                            UnpackAlbumData(selectedFileName);
                            break;
                        case "Добавить песню":
                            UnpackSongData(selectedFileName);
                            break;
                        default:
                            break;
                    }

                    MessageBox.Show($"Выбран файл '{selectedFileName}' для типа '{infoType}'.");
                }
            }
        }

        private void UnpackMusicianData(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                int addedMusicianCount = 0;

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length < 2)
                    {
                        MessageBox.Show($"Ошибка в строке: '{line}'. Не удалось распознать имя музыканта и страну.");
                        continue;
                    }

                    string musicianName = parts[0].Trim();
                    string countryName = parts[1].Trim();

                    Country country = context.Countries.FirstOrDefault(c => c.CountryName == countryName);
                    if (country == null)
                    {
                        country = new Country { CountryName = countryName };
                        context.Countries.Add(country);
                    }

                    Musician existingMusician = context.Musicians
                        .Include(m => m.Country)
                        .FirstOrDefault(m => m.Name == musicianName);

                    if (existingMusician == null)
                    {
                        Musician newMusician = new Musician
                        {
                            Name = musicianName,
                            Country = country
                        };
                        context.Musicians.Add(newMusician);
                        addedMusicianCount++;
                    }
                    else
                    {
                        MessageBox.Show($"Музыкант '{musicianName}' уже существует в базе данных.");
                    }
                }

                context.SaveChanges();
                Logger.LogInformation($"Была использована опция добавления исполнителей: {addedMusicianCount}.");
                MessageBox.Show("Данные успешно добавлены в базу данных.");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Ошибка при добавлении музыкантов: {ex.Message}");
                MessageBox.Show(ex.Message);
            }
        }

        private void UnpackAlbumData(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                int addedAlbumsCount = 0;

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length < 2)
                    {
                        MessageBox.Show($"Ошибка в строке: '{line}'. Не удалось распознать название альбома и исполнителя.");
                        continue;
                    }

                    string albumTitle = parts[0].Trim();
                    string musicianName = parts[1].Trim();
                    int releaseYear = 0;

                    if (parts.Length > 2 && int.TryParse(parts[2].Trim(), out int year))
                    {
                        releaseYear = year;
                    }

                    Musician musician = context.Musicians.FirstOrDefault(m => m.Name == musicianName);
                    if (musician == null)
                    {
                        musician = new Musician
                        {
                            MusicianID = Guid.NewGuid(),
                            Name = musicianName,
                            CountryID = NotDefinedCountryId
                        };
                        context.Musicians.Add(musician);
                        context.SaveChanges();
                        Logger.LogInformation($"Добавлен новый музыкант '{musicianName}' в базу данных.");
                    }

                    Album existingAlbum = context.Albums.FirstOrDefault(a => a.Title == albumTitle && a.MusicianID == musician.MusicianID);
                    if (existingAlbum != null)
                    {
                        MessageBox.Show($"Альбом '{albumTitle}' для исполнителя '{musicianName}' уже существует в базе данных. Пропускаем.");
                        continue;
                    }

                    Album newAlbum = new Album
                    {
                        AlbumID = Guid.NewGuid(),
                        Title = albumTitle,
                        MusicianID = musician.MusicianID,
                        ReleaseYear = releaseYear
                    };
                    context.Albums.Add(newAlbum);
                    addedAlbumsCount++;
                }

                context.SaveChanges();
                Logger.LogInformation($"Была использована опция добавления альбомов: {addedAlbumsCount}.");
                MessageBox.Show("Данные успешно добавлены в базу данных.");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Ошибка при добавлении альбомов: {ex.Message}");
                MessageBox.Show(ex.Message);
            }
        }

        private void UnpackSongData(string selectedFileName) { }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
