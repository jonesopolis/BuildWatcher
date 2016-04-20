using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Jones.BuildWatcher.Model;
using Microsoft.TeamFoundation.Build.Client;

namespace Jones.BuildWatcher.Repository
{
    public interface IBuildRepository
    {
        BuildResult GetSingleBuild(string project, string build, string friendlyName);
    }
}