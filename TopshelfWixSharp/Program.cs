using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Topshelf;

namespace TopshelfWixSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>                                
            {
                x.Service<TownCrier>(s =>                       
                {
                    s.ConstructUsing(name => new TownCrier());  
                    s.WhenStarted(tc => tc.Start());            
                    s.WhenStopped(tc => tc.Stop());             
                });
                x.RunAsLocalSystem();                           

                x.SetDescription("----------------");       
                x.SetDisplayName("TopshelfWixSharp");                      
                x.SetServiceName("TopshelfWixSharp");                      
            });
            Console.ReadLine();                                                       
        }

        public class TownCrier
        {
            readonly Timer _timer;
            public TownCrier()
            {
                _timer = new Timer(1000) { AutoReset = true };
                _timer.Elapsed += (sender, eventArgs) => Console.WriteLine("It is {0} and all is well", DateTime.Now);
            }
            public void Start() { _timer.Start(); }
            public void Stop() { _timer.Stop(); }
        }
    }
}
