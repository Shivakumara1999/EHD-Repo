using EHD.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHD.DAL.DataContext
{
    public class EHDContext : DbContext
    {
        public EHDContext(DbContextOptions<EHDContext> options) : base(options) { }

        public DbSet<Department> departments { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<Designations> designations { get; set; }
        public DbSet<Feedback> feedbacks { get; set; }
        public DbSet<Issue> issues { get; set; }
        public DbSet<Priority> priorities { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Status> status { get; set; }
        public DbSet<Ticket> tickets { get; set; }
        public DbSet<Label> labels { get; set; }

    }
}
