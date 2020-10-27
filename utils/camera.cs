using System;
using System.Threading;
using Emgu.CV;

namespace cameraimagecollection.utils
{
    public class Camera
    {
        private string name;
        private string link;
        private int phase;
        private Thread thread;
        public Thread MyThread { get { return this.thread; } }
        private DateTime creationTime;
        private Mat currentFrame;
        private VideoCapture capture;

        public Camera(string cameraName, string connectionLink, int phase)
        {
            this.name = cameraName;
            this.link = connectionLink;
            this.phase = phase;
            this.creationTime = DateTime.Now;
            this.currentFrame = new Mat();
            this.capture = new VideoCapture(this.link);

            this.thread = new Thread(this.MainThread);
            this.thread.IsBackground = true;
            this.thread.Start();
        }

        private void MainThread()
        {
            while((DateTime.Now - this.creationTime).TotalSeconds < 5)
            {
                Console.WriteLine("{0} is alive for {1}", this.name, (DateTime.Now - this.creationTime).TotalMilliseconds);
                Thread.Sleep(this.phase);
            }
        }
    }
}