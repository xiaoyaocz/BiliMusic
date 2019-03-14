using System;
using Microsoft.EntityFrameworkCore;

namespace BiliMusic.SqliteModel
{

    public class BiliMusicContext : DbContext
    {
        public DbSet<SearchHistory> SearchHistorys { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=bilimusic.db");
        }
    }

    public class SearchHistory
    {
        public DateTime datetime { get; set; }
        public string content { get; set; }
    }

  


}
