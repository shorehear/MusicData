using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

namespace JulyPractice
{
    public class DatabaseBackupManager
    {
        private readonly string backupFilePath = "backup.json";

        public void BackupDatabase(DbContext context)
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                };

                var backupData = new BackupData
                {
                    Countries = context.Set<Country>().AsNoTracking().Include(c => c.Musicians).ToList(),
                    Musicians = context.Set<Musician>().AsNoTracking().Include(m => m.Country).ToList(),
                    Albums = context.Set<Album>().AsNoTracking().Include(a => a.Musician).ToList(),
                    Songs = context.Set<Song>().AsNoTracking().Include(s => s.Musician).Include(s => s.Album).ToList(),
                    Users = context.Set<User>().AsNoTracking().ToList()
                };

                File.WriteAllText(backupFilePath, JsonConvert.SerializeObject(backupData, serializerSettings));
                Logger.LogInformation("Резервная копия данных успешно создана.");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Ошибка при создании резервной копии данных: {ex.Message}");
            }
        }

        public void RestoreDatabase(DbContext context)
        {
            try
            {
                if (!File.Exists(backupFilePath))
                {
                    Logger.LogError("Файл резервной копии не найден.");
                    return;
                }

                var serializerSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                };

                var jsonData = File.ReadAllText(backupFilePath);
                var backupData = JsonConvert.DeserializeObject<BackupData>(jsonData, serializerSettings);

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.ChangeTracker.Clear();

                context.Set<Country>().AddRange(backupData.Countries);
                context.SaveChanges();

                foreach (var musician in backupData.Musicians)
                {
                    var existingMusician = context.Set<Musician>().Find(musician.MusicianID);
                    if (existingMusician == null)
                    {
                        context.Set<Musician>().Add(musician);
                    }
                    else
                    {
                        context.Entry(existingMusician).CurrentValues.SetValues(musician);
                    }
                }
                context.SaveChanges();

                foreach (var album in backupData.Albums)
                {
                    var existingAlbum = context.Set<Album>().Find(album.AlbumID);
                    if (existingAlbum == null)
                    {
                        var existingMusician = context.Set<Musician>().Find(album.MusicianID);
                        if (existingMusician != null)
                        {
                            album.Musician = existingMusician;
                        }
                        context.Set<Album>().Add(album);
                    }
                    else
                    {
                        context.Entry(existingAlbum).CurrentValues.SetValues(album);
                    }
                }
                context.SaveChanges();

                foreach (var song in backupData.Songs)
                {
                    var existingSong = context.Set<Song>().Find(song.SongID);
                    if (existingSong == null)
                    {
                        var existingMusician = context.Set<Musician>().Find(song.MusicianID);
                        var existingAlbum = context.Set<Album>().Find(song.AlbumID);
                        if (existingMusician != null && existingAlbum != null)
                        {
                            song.Musician = existingMusician;
                            song.Album = existingAlbum;
                        }
                        context.Set<Song>().Add(song);
                    }
                    else
                    {
                        context.Entry(existingSong).CurrentValues.SetValues(song);
                    }
                }
                context.SaveChanges();

                context.Set<User>().AddRange(backupData.Users);
                context.SaveChanges();

                Logger.LogInformation($"Данные успешно восстановлены из резервной копии. {backupData.Users.Count}, {backupData.Countries.Count}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Ошибка при восстановлении данных из резервной копии: {ex.Message}");
            }
        }


        private class BackupData
        {
            public List<User> Users { get; set; }
            public List<Song> Songs { get; set; }
            public List<Musician> Musicians { get; set; }
            public List<Album> Albums { get; set; }
            public List<Country> Countries { get; set; }
        }
    }
}
