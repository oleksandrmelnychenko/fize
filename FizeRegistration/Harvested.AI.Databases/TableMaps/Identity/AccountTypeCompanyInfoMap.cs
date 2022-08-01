using Harvested.AI.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Harvested.AI.Databases.TableMaps.Identity {
    public class AccountTypeCompanyInfoMap : EntityBaseMap<AccountTypeCompanyInfo> {
        public override void Map(EntityTypeBuilder<AccountTypeCompanyInfo> entity) {
            base.Map(entity);

            entity.ToTable("AccountTypeCompanyInfo");
        }
    }
}
