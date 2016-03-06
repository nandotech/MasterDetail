using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using MasterDetail.Models;

namespace MasterDetail.DataLayer
{
    public class LogEntryConfiguration : EntityTypeConfiguration<LogEntry>
    {
        public LogEntryConfiguration()
        {
            Property(le => le.LogDate).IsRequired();
            Property(le => le.Logger).IsRequired().HasMaxLength(30);
            Property(le => le.LogLevel).IsRequired().HasMaxLength(5);
            Property(le => le.Thread).IsRequired().HasMaxLength(10);

            Property(le => le.EntityFormalNamePlural)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IDX_LogEntries_Entity", 2)));

            Property(le => le.EntityKeyValue)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IDX_LogEntries_Entity", 1)));

            Property(le => le.UserName).IsRequired().HasMaxLength(256);
            Property(le => le.Message).IsRequired().HasMaxLength(256);
            Property(le => le.Exception).IsOptional();
        }
    }
}