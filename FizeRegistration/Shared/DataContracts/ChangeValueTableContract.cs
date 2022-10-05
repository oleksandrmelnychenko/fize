using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FizeRegistration.Shared.DataContracts
{
    public class ChangeValueTableContract
    {
        public string Id { get; set; }
        public string ColumnName { get; set; }

        public string ValueInTable { get; set; }
    }
}
