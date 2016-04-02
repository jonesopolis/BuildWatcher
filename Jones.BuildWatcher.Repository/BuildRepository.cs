using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Jones.BuildWatcher.Model;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;

namespace Jones.BuildWatcher.Repository
{
    public class BuildRepository : IBuildRepository
    {
        private readonly TfsTeamProjectCollection _tfs;

        public BuildRepository(string uri)
        {
            _tfs = new TfsTeamProjectCollection(new Uri(uri));
            try
            {
                _tfs.GetService<IBuildServer>();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not reach the tfs server", ex);
            }
        }


        public Build GetBuildResults(string project, string build)
        {
            var buildServer = _tfs.GetService<IBuildServer>();

            var spec = buildServer.CreateBuildDetailSpec(project, build);
            spec.MaxBuildsPerDefinition = 1;
            spec.QueryOrder = BuildQueryOrder.FinishTimeDescending;

            var results = buildServer.QueryBuilds(spec);

            if (results.Builds.Any())
            {
                var buildDetail = results.Builds[0];
                
                var b = new Build();
                b.BuildName = build;
                b.IsGreen = buildDetail.Status == BuildStatus.Succeeded;
                b.LastCompleted = buildDetail.FinishTime;
                b.PersonName = buildDetail.RequestedFor;

                return b;
            }

            return null;
        }
    }
}
    