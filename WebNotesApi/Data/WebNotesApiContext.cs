using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebNotesApi.Models;

namespace WebNotesApi.Data
{
    public class WebNotesApiContext : DbContext
    {
        public WebNotesApiContext (DbContextOptions<WebNotesApiContext> options)
            : base(options)
        {
        }

        public DbSet<WebNotesApi.Models.Note> Note { get; set; }
    }
}
