using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IMMRequest.Importer
{
    public class ImportInfoDTO      
    {
        public string importationMethod { get; set; }
        public List<ImportationField> requiredFields { get; set; }

        public ImportInfoDTO() { }
    }

}
