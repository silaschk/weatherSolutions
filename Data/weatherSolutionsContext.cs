using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using weatherSolutions.Models;

namespace weatherSolutions.Data
{
    public class weatherSolutionsContext : DbContext
    {
        public weatherSolutionsContext (DbContextOptions<weatherSolutionsContext> options)
            : base(options)
        {
        }

        public DbSet<city> city { get; set; } 
    }
}
