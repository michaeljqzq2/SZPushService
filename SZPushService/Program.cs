using System;
using SZPushService.Model;
using System.Timers;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt;

namespace SZPushService
{
    class Program
    {
        private const int INTERVAL = 60000;
        private static List<Timer> timers;
        private static bool isWorking = false;
        static void Main(string[] args)
        {
            //Email.Send("this is a title", "web job started");
            timers = new List<Timer>();
            Initialize();
            while (true)
            {
                if(true)//!( DateTime.Now.Hour > 1 && DateTime.Now.Hour < 7 ))//not working time : DateTime.Now.Hour>1 && DateTime.Now.Hour < 7 
                {
                    if (!isWorking)
                    {
                        Console.WriteLine("---------------------Start Working-----------------------");
                        Console.WriteLine("Now time is " + DateTime.Now);
                        Start();
                    }
                }
                else
                {
                    if (isWorking)
                    {
                        Console.WriteLine("---------------------Stop  Working-----------------------");
                        Console.WriteLine("Now time is " + DateTime.Now);
                        Stop();
                    }
                }
                System.Threading.Thread.Sleep(600);
            }
        }
        private static void Initialize()
        {
            foreach (var url in UData.Urls)
            {
                Crawler c = new Crawler();
                //Thread thread = new Thread(new ParameterizedThreadStart(c.Start));
                //thread.Start("Domestic");
                Timer t = new Timer(INTERVAL);   //实例化Timer类，设置间隔时间为10000毫秒；   
                t.Elapsed += new System.Timers.ElapsedEventHandler(c.Start); //到达时间的时候执行事件；   
                t.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
                t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   
                timers.Add(t);
                
            }
        }
        
        private static void Start()
        {
            isWorking = true;
            foreach(Timer t in timers)
            {
                t.Start();
                new Crawler().Start(null,null);
            }

        }

        private static void Stop()
        {
            isWorking = false;
            foreach (Timer t in timers)
            {
                t.Stop();
            }
        }
    }
}
