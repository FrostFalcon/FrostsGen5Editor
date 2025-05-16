using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEditor.Data
{
    public static class PatchingSystem
    {
        public static string MakePatch(NDSFileSystem newRom, NDSFileSystem oldRom)
        {
            Dictionary<string, IEnumerable<byte>> data = new Dictionary<string, IEnumerable<byte>>();



            return "";
        }
    }

    public class PatchComponent
    {
        public string sectionName;
    }
}
