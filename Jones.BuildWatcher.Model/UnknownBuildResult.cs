using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jones.BuildWatcher.Model
{
    public sealed class UnknownBuildResult : BuildResult
    {
        public UnknownBuildResult(string projectName, string buildName, string friendlyName) : base(projectName, buildName, friendlyName) {}
    }
}
