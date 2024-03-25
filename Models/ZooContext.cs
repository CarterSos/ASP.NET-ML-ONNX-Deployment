using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zoo.Models
{
    public class ZooContext : DbContext
    {
        public ZooContext(DbContextOptions<ZooContext> options) : base(options)
        {
        }
        public DbSet<Animal> zoo_animals { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite("Data Source= C:/Users/carte/Documents-Local/ZooC#/Zoo/zoo_animals.sqlite"); // NOT NEEDED THANKS TO CONNECTION STRING
    }
}
