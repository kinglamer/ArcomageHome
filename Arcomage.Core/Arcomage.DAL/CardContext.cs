using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcomage.Core;

namespace Arcomage.DAL
{
    public class CardContext : DbContext
    {
        public CardContext()
            : base("DefaultConnection") { }

        public DbSet<CardParametrs> ArcomageCards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

    }
}
