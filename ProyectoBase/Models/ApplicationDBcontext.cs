using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ProyectoBase.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
         public DbSet<rh_cat_personas> rh_cat_personas { get; set; }
        public DbSet<rh_cat_domicilios> rh_cat_domicilios { get; set; }
        public DbSet<rh_cat_telefonos> rh_cat_telefonos { get; set; }
        public DbSet<rh_cat_dir_web> rh_cat_dir_web { get; set; }
        public DbSet<cat_paises> cat_paises { get; set; }
        public DbSet<cat_estados> cat_estados { get; set; }
        public DbSet<cat_municipios> cat_municipios { get; set; }
        public DbSet<cat_localidades> cat_localidades { get; set; }
        public DbSet<cat_institutos> cat_institutos { get; set; }

        public DbSet<cat_colonias> cat_colonias { get; set; }
        public DbSet<cat_usuarios> cat_usuarios { get; set; }
        public DbSet<seg_expira_claves> seg_expira_claves { get; set; }
        public DbSet<seg_usuarios_estatus> seg_usuarios_estatus { get; set; }
        public DbSet<seg_usuarios_grupos> seg_usuarios_grupos { get; set; }
        // public DbSet<rh_cat_tipo_grupo> rh_cat_tipo_grupos { get; set; }
        // public DbSet<rh_cat_grupo> rh_cat_grupos { get; set; }
        public DbSet<cat_tipos_generales> cat_tipo_generales { get; set; }
        public DbSet<cat_generales> cat_generales { get; set; }
        public DbSet<cat_tipos_estatus> cat_tipo_estatus { get; set; }
        public DbSet<cat_estatus> cat_estatus { get; set; }
        }
    }

    

