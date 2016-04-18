using System;

namespace Jones.BuildWatcher.Model
{
    public interface IBuildResult
    {
        string ProjectName { get; set; }

        string BuildName { get; set; }
        
        string FriendlyName { get; set; }
    }
}