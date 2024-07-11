using System.Windows.Input;
using System.IO;
using System.Windows;
using System.ComponentModel;
using Microsoft.Win32;
using Microsoft.EntityFrameworkCore;

namespace JulyPractice
{
    public class AddInfoChoiceVM : INotifyPropertyChanged
    {
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
                            //UnpackAlbumData(selectedFileName);
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
                    }
                    else
                    {
                        MessageBox.Show($"Музыкант '{musicianName}' уже существует в базе данных.");
                    }
                }

                context.SaveChanges();
                MessageBox.Show("Данные успешно добавлены в базу данных.");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        //private void UnpackAlbumData(string filePath)
        //{
        //    try
        //    {
        //        string[] lines = File.ReadAllLines(filePath);

        //        using (var context = new CurrentDbContext())
        //        {
        //            foreach (string line in lines)
        //            {
        //                string[] parts = line.Split(',');
        //                if (parts.Length < 2)
        //                {
        //                    MessageBox.Show($"Ошибка в строке: '{line}'. Не удалось распознать название альбома и исполнителя.");
        //                    continue;
        //                }

        //                string albumTitle = parts[0].Trim();
        //                string musicianName = parts[1].Trim();
        //                int releaseYear = 0;

        //                if (parts.Length > 2 && int.TryParse(parts[2].Trim(), out int year))
        //                {
        //                    releaseYear = year;
        //                }

        //                Musician musician = context.Musicians.FirstOrDefault(m => m.Name == musicianName);
        //                if (musician == null)
        //                {
        //                    MessageBox.Show($"Исполнитель '{musicianName}' не найден в базе данных. Пропускаем альбом '{albumTitle}'.");
        //                    continue;
        //                }

        //                Album existingAlbum = context.Albums.FirstOrDefault(a => a.Title == albumTitle && a.MusicianID == musician.MusicianID);
        //                if (existingAlbum != null)
        //                {
        //                    MessageBox.Show($"Альбом '{albumTitle}' для исполнителя '{musicianName}' уже существует в базе данных. Пропускаем.");
        //                    continue;
        //                }

        //                MessageBox.Show(musician.Name, musician.MusicianID.ToString());


        //                Album newAlbum = new Album
        //                {
        //                    AlbumID = Guid.NewGuid(),
        //                    Title = albumTitle,
        //                    MusicianID = musician.MusicianID,
        //                    ReleaseYear = releaseYear
        //                };
        //                context.Albums.Add(newAlbum);
        //            }

        //            context.SaveChanges();
        //            MessageBox.Show("Данные успешно добавлены в базу данных.");
        //        }
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}");
        //        if (ex.InnerException != null)
        //        {
        //            MessageBox.Show($"Inner Exception: {ex.InnerException.Message}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Общая ошибка: {ex.Message}");
        //    }
        //}

        private void UnpackSongData(string selectedFileName) { }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
