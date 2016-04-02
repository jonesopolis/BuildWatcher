using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Jones.BuildWatcher.Model;
using Jones.BuildWatcher.Repository;
using Microsoft.Practices.Unity;
using Moq;

namespace Jones.BuildWatcher
{
    public partial class App : Application
    {
        public App()
        {
            IUnityContainer unity = new UnityContainer();

            var instance = false
                ? new BuildRepository("http://tfs.csiweb.com:8080/tfs/DefaultCollection")
                : getMock();

            unity.RegisterInstance(instance);
            
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


            mock.Setup(r => r.GetBuildResults(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((one, two) =>
                {
                    var rand = new Random();

                    var b = new Build();
                    b.BuildName = two;
                    b.PersonName = names[rand.Next(0, names.Count() - 1)];
                    b.LastCompleted = DateTime.Now.AddDays(-1 * rand.Next(0, 3)).AddHours(rand.Next(0, 10));
                    b.IsGreen = rand.Next(0, 5) % 2 == 0;

                    return b;
                });

            return mock.Object;
        }


    }
}