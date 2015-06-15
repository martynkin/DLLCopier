using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DllCopier
{

        public List<string> DestPathes;
        public Dictionary<int, List<string>> DllNames;
        public string DllFolder;

        public DataMover()
        {
            fileCopier = null;           
        }
    }
}
