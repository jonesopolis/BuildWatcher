using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jones.BuildWatcher
{
    public sealed class BuildConfig
    {
        public string Project { get; set; }
        public string Build { get; set; }
        public string FriendlyName { get; set; }
    }
}
