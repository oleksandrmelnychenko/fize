using Harvested.AI.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harvested.AI.Databases.TableMaps.Identity {
    public class AccountTypeMap : EntityBaseMap<AccountType> {
        public override void Map(EntityTypeBuilder<AccountType> entity) {
            base.Map(entity);

            entity.ToTable("AccountTypes");

            entity.HasOne(e => e.UserIdentity)
                .WithMany(e => e.AccountTypes)
                .HasForeignKey(e => e.UserIdentityId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.CompanyInfo)
                .WithMany(e => e.AccountTypes)
                .HasForeignKey(e => e.CompanyInfoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
