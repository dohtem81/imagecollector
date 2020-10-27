using System;
using System.Threading;
using System.Collections.Generic;

namespace cameraimagecollection
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, utils.Camera> cameraThreads = new Dictionary<string, utils.Camera>();

            foreach(KeyValuePair<string, utils.Camera> kvp in cameraThreads)
            {
                kvp.Value.MyThread.Join();
            }
        }
    }
}
