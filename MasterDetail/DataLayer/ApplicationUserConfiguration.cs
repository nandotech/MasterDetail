using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using MasterDetail.Models;

namespace MasterDetail.DataLayer
{
    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            Property(au => au.FirstName).HasMaxLength(15).IsOptional();
            Property(au => au.LastName).HasMaxLength(15).IsOptional();
            Property(au => au.Address).HasMaxLength(30).IsOptional();
            Property(au => au.City).HasMaxLength(20).IsOptional();
            Property(au => au.State).HasMaxLength(2).IsOptional();
            Property(au => au.ZipCode).HasMaxLength(10).IsOptional();
            Ignore(au => au.RolesList);
        }
    }
}