using Fluent.Icons;
using Protecc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protecc.Helpers
{
    public class UIHelper
    {
        public static bool NullBoolToBool(bool? boolean) => (bool)boolean;

        // Privacy filter button text & icon 
        public static string PrivacyToText(bool? boolean) => (boolean ?? false) ? "Hidden" : "Visible";
        public static FluentSymbol PrivacyToIcon(bool? boolean) => (boolean ?? false) ? FluentSymbol.EyeHide20 : FluentSymbol.EyeShow20;

        // List can only have 20 items so disable Add new item button
        public static bool CanAddNew() => CredentialService.CredentialList.Count < 20;
    }
}
