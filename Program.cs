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
            cameraThreads.Add("Camera1", new utils.Camera("Camera1", "rtsp://admin:ARadecka@192.168.1.250:554/Streaming/channels/101", 5000));
            cameraThreads.Add("Camera2", new utils.Camera("Camera2", "rtsp://admin:ARadecka@192.168.1.250:554/Streaming/channels/201", 5000));
            cameraThreads.Add("Camera3", new utils.Camera("Camera3", "rtsp://admin:ARadecka@192.168.1.250:554/Streaming/channels/301", 5000));
            cameraThreads.Add("Camera4", new utils.Camera("Camera4", "rtsp://admin:ARadecka@192.168.1.250:554/Streaming/channels/401", 5000));
            cameraThreads.Add("Camera5", new utils.Camera("Camera5", "rtsp://admin:ARadecka@192.168.1.250:554/Streaming/channels/501", 5000));
            cameraThreads.Add("Camera6", new utils.Camera("Camera6", "rtsp://admin:ARadecka@192.168.1.250:554/Streaming/channels/601", 5000));
            cameraThreads.Add("Camera7", new utils.Camera("Camera7", "rtsp://admin:ARadecka@192.168.1.250:554/Streaming/channels/701", 5000));
            cameraThreads.Add("Camera8", new utils.Camera("Camera8", "rtsp://admin:ARadecka@192.168.1.250:554/Streaming/channels/801", 5000));            

            while(true)
            {
                Thread.Sleep(1000);
            }

            foreach(KeyValuePair<string, utils.Camera> kvp in cameraThreads)
            {
                kvp.Value.MyThread.Join();
            }
            Console.WriteLine("Gentle shutdown...");
        }
    }
}
