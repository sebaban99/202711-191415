using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IMMRequest.WebApi
{
    public class ImportInfoDTO      
    {
        public string importationMethod { get; set; }
        public string filePath { get; set; }

        public ImportInfoDTO() { }
    }

}
