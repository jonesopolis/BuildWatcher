using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Jones.BuildWatcher.Model;
using Jones.BuildWatcher.Repository;
using Jones.Logger;
using Microsoft.Practices.Unity;
using Moq;

namespace Jones.BuildWatcher
{
    public partial class App : Application
    {
        public App()
        {
            IUnityContainer unity = new UnityContainer();

            var repository = true
                ? new BuildRepository("http://tfs.csiweb.com:8080/tfs/DefaultCollection")
                : getMock();
            
            Logger.Logger logger = new FileLogger.FileLogger("log.txt", LogLevel.Info);

            unity.RegisterInstance(repository);
            unity.RegisterInstance(logger, new ContainerControlledLifetimeManager());

            var window = new BuildView(unity.Resolve<BuildVM>());
            window.Show();
        }

        private IBuildRepository getMock()
        {
            Mock<IBuildRepository> mock = new Mock<IBuildRepository>();

            List<string> names = new List<string>
                                        {
                                            "David",
                                            "Katherine",
                                            "Elijah",
                                            "Atlas",
                                            "Chris",
                                            "Jones"
                                        };


            mock.Setup(r => r.GetSingleBuild(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((one, two) =>
                {
                    var rand = new Random();

                    var b = new SuccessBuildResult(one, two, "test test");
                    b.BuildName = two;
                    b.PersonName = names[rand.Next(0, names.Count() - 1)];
                    b.Completed = DateTime.Now.AddDays(-1 * rand.Next(0, 3)).AddHours(rand.Next(0, 10)).AddMinutes(rand.Next(0,60));
                   
                    return b;
                });

            return mock.Object;
        }


    }
}