using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
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


        public BuildResult GetSingleBuild(string project, string build, string friendlyName)
        {
            var buildServer = _tfs.GetService<IBuildServer>();

            var spec = buildServer.CreateBuildDetailSpec(project, build);
            spec.MaxBuildsPerDefinition = 2;
            spec.QueryOrder = BuildQueryOrder.FinishTimeDescending;

            var results = buildServer.QueryBuilds(spec);

            if (results.Builds.Any())
            {
                var builds = results.Builds.OrderByDescending(b => b.StartTime);

                switch (builds.First().Status)
                {
                    case BuildStatus.Succeeded:
                        return GetSuccessResult(builds.First(), project, build, friendlyName);
                    case BuildStatus.Failed:
                        return GetFailedResult(builds.First(), project, build, friendlyName);
                    case BuildStatus.InProgress:
                        return GetInProgressResult(builds.First(), builds.Last(), project, build, friendlyName);
                }
            }

            return new UnknownBuildResult(project, build, friendlyName);
        }

        private SuccessBuildResult GetSuccessResult(IBuildDetail buildDetail, string project, string build, string friendlyName)
        {
            var model = new SuccessBuildResult(project, build, friendlyName);

            model.PersonName = buildDetail.RequestedFor;
            model.Completed = buildDetail.FinishTime;
            return model;
        }

        private FailedBuildResult GetFailedResult(IBuildDetail buildDetail, string project, string build, string friendlyName)
        {
            var model = new FailedBuildResult(project, build, friendlyName);

            model.PersonName = buildDetail.RequestedFor;
            model.Failed = buildDetail.StartTime;

            return model;
        }

        private InProgressBuildResult GetInProgressResult(IBuildDetail buildDetail, IBuildDetail previous, string project, string build, string friendlyName)
        {
            var model = new InProgressBuildResult(project, build, friendlyName);

            model.PersonName = buildDetail.RequestedFor;
            model.BuildTime = DateTime.Now - buildDetail.StartTime;

            var timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += (s,e) => model.BuildTime = model.BuildTime.Add(TimeSpan.FromSeconds(1));
            timer.Start();
            
            model.PreviousBuildSucceeded = previous.Status == BuildStatus.Succeeded;

            return model;
        }
    }
}
    