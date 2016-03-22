using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Newtonsoft.Json;

namespace Jones.BuildWatcher
{
    public sealed class BuildVM : NotifyBase
    {
        private IEnumerable<BuildConfiguration> _config;
        private readonly Uri _uri = new Uri("http://tfs.csiweb.com:8080/tfs/DefaultCollection");
        private readonly TfsTeamProjectCollection _tfs;

        public BuildVM()
        {
            try
            {
                _tfs = new TfsTeamProjectCollection(_uri);


                buildConfiguration();
                initializeItems();
                new Timer(poll, null, 1000, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                //TODO
            }
        }

        private void initializeItems()
        {
            foreach (var build in _config.SelectMany(c => c.Builds))
            {
                var model = new BuildModel();
                model.BuildName = build.Value;
                model.PersonName = "N/A";
                model.Success = false;
            }
        }

        private void poll(object state)
        {
            var buildServer = _tfs.GetService<IBuildServer>();

            foreach (var configProject in _config)
            {
                foreach (var configBuild in configProject.Builds)
                {
                    var spec = buildServer.CreateBuildDetailSpec(configProject.ProjectName, configBuild.Key);
                    spec.MaxBuildsPerDefinition = 1;
                    spec.QueryOrder = BuildQueryOrder.FinishTimeDescending;

                    var build = buildServer.QueryBuilds(spec);

                    if (build.Builds.Any())
                    {
                        var buildDetail = build.Builds[0];
                        Trace.WriteLine($"{configBuild.Value} - {buildDetail.Status} - {buildDetail.FinishTime} - {buildDetail.RequestedFor}");
                    }
                }
            }
        }

        public ObservableCollection<BuildModel> Items { get; } = new ObservableCollection<BuildModel>();

        private void buildConfiguration()
        {
            var config = Path.Combine(Environment.CurrentDirectory, "config.json");
            _config = JsonConvert.DeserializeObject<IEnumerable<BuildConfiguration>>(File.ReadAllText(config));
        }
    }
}