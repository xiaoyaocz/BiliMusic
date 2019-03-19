using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Required,Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime datetime { get; set; }
        public string content { get; set; }
    }

  


}
