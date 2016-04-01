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
            unity.RegisterInstance<IBuildRepository>(getMock());
            
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


            mock.Setup(r => r.GetBuildResults(It.IsAny<Tuple<string, string>>()))
                .Returns<Tuple<string, string>>(t =>
                {
                    var rand = new Random();

                    var b = new Build();
                    b.BuildName = t.Item2;
                    b.PersonName = names[rand.Next(0, names.Count() - 1)];
                    b.LastCompleted = DateTime.Now.AddDays(-1*rand.Next(0, 3)).AddHours(rand.Next(0, 10));
                    b.IsGreen = rand.Next(0, 1) == 0;

                    return b;
                });

            return mock.Object;
        }

        
    }
}