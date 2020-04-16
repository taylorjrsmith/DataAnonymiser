using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAnonymiser
{
    public class DataAccessor : DbContext
    {
        public string ConnectionString { get; set; }
        public DbQuery<MoverDisqualificationModel> MoverDisqualificationModels { get; set; }

        public DataAccessor(string connectionString, DbContextOptions<DataAccessor> options) : base(options)
        {
            ConnectionString = connectionString;
            Database.SetCommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }
    }
}
