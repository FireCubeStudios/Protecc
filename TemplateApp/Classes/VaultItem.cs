using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protecc.Classes
{
    public class VaultItem
    {
        public string Name { get; set; }
        public string Resource { get; set; }

        public VaultItem(string name, string resource)
        {
            Name = name;
            Resource = resource;
        }
    }
}
