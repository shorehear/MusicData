using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace JulyPractice
{
    public class DatabaseBackupManager
    {
        private readonly string _backupFilePath = "backup.json";

        public void BackupDatabase(CurrentDbContext context)
        {
            try
            {
                var backupData = new BackupData
                {
                    Users = context.Users.AsNoTracking().ToList(),
                    Songs = context.Songs.AsNoTracking().Include(s => s.Musician).Include(s => s.Album).ToList(),
                    Musicians = context.Musicians.AsNoTracking().Include(m => m.Country).ToList(),
                    Albums = context.Albums.AsNoTracking().Include(a => a.Musician).Include(a => a.Songs).ToList(),
                    Countries = context.Countries.AsNoTracking().Include(c => c.Musicians).ToList()
                };
                File.WriteAllText(_backupFilePath, JsonConvert.SerializeObject(backupData, Formatting.Indented));
                Logger.LogInformation("Резервная копия данных успешно создана.");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Ошибка при создании резервной копии данных: {ex.Message}");
            }
        }

        public void RestoreDatabase(CurrentDbContext context)
        {
            try
            {
                if (!File.Exists(_backupFilePath))
                {
                    Logger.LogError("Файл резервной копии не найден.");
                    return;
                }

                var backupData = JsonConvert.DeserializeObject<BackupData>(File.ReadAllText(_backupFilePath));

                context.Users.RemoveRange(context.Users);
                context.Songs.RemoveRange(context.Songs);
                context.Musicians.RemoveRange(context.Musicians);
                context.Albums.RemoveRange(context.Albums);
                context.Countries.RemoveRange(context.Countries);
                context.SaveChanges();

                context.Users.AddRange(backupData.Users);
                context.Songs.AddRange(backupData.Songs);
                context.Musicians.AddRange(backupData.Musicians);
                context.Albums.AddRange(backupData.Albums);
                context.Countries.AddRange(backupData.Countries);

                context.SaveChanges();
                Logger.LogInformation("Данные успешно восстановлены из резервной копии.");
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
