using System;
using System.Collections.Generic;
using System.Text;
using Harvested.AI.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harvested.AI.Databases.TableMaps.Identity
{
    public class UserIdentityMap : EntityBaseMap<UserIdentity>
    {
        public override void Map(EntityTypeBuilder<UserIdentity> entity)
        {
            base.Map(entity);
            entity.ToTable("UserIdentities");
            entity
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
