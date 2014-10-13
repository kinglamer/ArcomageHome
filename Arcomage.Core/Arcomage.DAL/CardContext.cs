using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Entity;

namespace Arcomage.DAL
{
    public partial class CardContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardParams> CardParamses { get; set; }

        public CardContext()
            : base("DefaultConnection") { }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Card>().HasKey(x => x.id);
            modelBuilder.Entity<CardParams>().HasKey(x => x.id);
            modelBuilder.Entity<Card>().HasMany<CardParams>(s => s.cardParams).WithRequired(x=>x.card);

          
            base.OnModelCreating(modelBuilder);

        }
    }
}
