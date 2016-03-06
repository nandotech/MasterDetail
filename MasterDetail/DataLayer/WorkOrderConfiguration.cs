using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using MasterDetail.Models;

namespace MasterDetail.DataLayer
{
    public class WorkOrderConfiguration : EntityTypeConfiguration<WorkOrder>
    {
        public WorkOrderConfiguration()
        {
            Property(wo => wo.OrderDateTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(wo => wo.Description).HasMaxLength(256).IsOptional();
            Property(wo => wo.Total).HasPrecision(18, 2).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(wo => wo.CertificationRequirements).HasMaxLength(120).IsOptional();
            HasOptional(wo => wo.CurrentWorker).WithMany(au => au.WorkOrders).WillCascadeOnDelete(false);
            HasRequired(wo => wo.Customer).WithMany(c => c.WorkOrders).WillCascadeOnDelete(false);
            Property(wo => wo.RowVersion).IsRowVersion();
        }
    }
}