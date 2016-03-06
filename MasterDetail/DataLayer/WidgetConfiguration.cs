using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using MasterDetail.Models;

namespace MasterDetail.DataLayer
{
    public class WidgetConfiguration : EntityTypeConfiguration<Widget>
    {
        public WidgetConfiguration()
        {
            Property(w => w.Description).HasMaxLength(256).IsRequired();
            Property(w => w.MainBusCode).HasMaxLength(12).IsOptional();
            HasOptional(w => w.CurrentWorker).WithMany().WillCascadeOnDelete(false);  
        }
    }
}