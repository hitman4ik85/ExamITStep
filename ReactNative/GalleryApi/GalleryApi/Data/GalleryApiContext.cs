using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GalleryApi.Models;

namespace GalleryApi.Data
{
    public class GalleryApiContext : DbContext
    {
        public GalleryApiContext (DbContextOptions<GalleryApiContext> options)
            : base(options)
        {
        }

        public DbSet<GalleryApi.Models.Picture> Picture { get; set; } = default!;
        public DbSet<GalleryApi.Models.Tag> Tag { get; set; } = default!;
    }
}
