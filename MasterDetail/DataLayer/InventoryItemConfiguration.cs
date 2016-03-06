using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;
using MasterDetail.Models;

namespace MasterDetail.DataLayer
{
    public class InventoryItemConfiguration : EntityTypeConfiguration<InventoryItem>
    {
        public InventoryItemConfiguration()
        {
            Property(ii => ii.InventoryItemCode)
                .HasMaxLength(15)
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("AK_InventoryItem_InventoryItemCode") {IsUnique = true}));

            Property(ii => ii.InventoryItemName)
                .HasMaxLength(80)
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("AK_InventoryItem_InventoryItemName") {IsUnique = true}));

            Property(ii => ii.UnitPrice).HasPrecision(18, 2);

            HasRequired(ii => ii.Category).WithMany().WillCascadeOnDelete(false);
        }
    }
}