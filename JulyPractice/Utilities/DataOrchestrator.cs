﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;


namespace JulyPractice
{
    public class DataOrchestrator
    {
        private CurrentDbContext context;

        public DataOrchestrator(CurrentDbContext context)
        {
            this.context = context;
        }

        public User LoadUser()
        {
            return context.Users.FirstOrDefault();
        }

        public ObservableCollection<Musician> LoadMusicians()
        {
            Logger.LogInformation($"LoadMusicians:{context.Musicians.Count()}");
            return new ObservableCollection<Musician>(context.Musicians.ToList());
        }

        public ObservableCollection<Album> LoadAlbums(Guid musicianID)
        {
            var allAlbums = context.Albums.ToList();

            var albums = allAlbums.Where(a => a.MusicianID == musicianID).ToList();
            Logger.LogInformation($"LoadAlbums:{albums.Count()}, Artist: {musicianID}");
            return new ObservableCollection<Album>(albums);
        }

        public ObservableCollection<Song> LoadSongs(Guid musicianID) 
        {
            var allSongs = context.Songs.ToList();

            var songs = allSongs.Where(s => s.MusicianID == musicianID).ToList();
            Logger.LogInformation($"LoadSongs:{songs.Count()}");
            return new ObservableCollection<Song>(songs);
        }

        public void LoadChoiceWindow()
        {
            AddInfoChoiceWindow addInfoChoiceWindow = new AddInfoChoiceWindow(context);
            addInfoChoiceWindow.Show();
        }
    }
}
