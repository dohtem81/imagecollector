using System;
using System.Threading;
using System.IO;
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
        private bool threadRunning;
        private readonly object __frameLock = new object();

        public Camera(string cameraName, string connectionLink, int phase)
        {
            this.name = cameraName;
            this.link = connectionLink;
            this.phase = phase;
            this.creationTime = DateTime.Now;
            this.currentFrame = new Mat();
            this.capture = new VideoCapture(this.link);
            this.threadRunning = true;

            this.thread = new Thread(this.MainThread);
            this.thread.IsBackground = true;
            this.thread.Start();
            (new Thread(this.FrameReadThread)).Start();
        }

        private void FrameReadThread()
        {
            while(this.threadRunning)
            {
                Mat newFrame = this.capture.QueryFrame();
                lock(this.__frameLock)
                {
                    try
                    {
                        this.currentFrame = newFrame.Clone();
                        //Console.WriteLine("{0} new frame!", this.name);
                    }
                    catch
                    {

                    }
                }
            }
        }
        private void MainThread()
        {
            while((DateTime.Now - this.creationTime).TotalSeconds < 15)
            {
                Console.WriteLine("{0} is alive for {1}", this.name, (DateTime.Now - this.creationTime).TotalMilliseconds);
                lock(this.__frameLock)
                {
                    try
                    {
                        string filename = string.Format("{0}_{1}.jpg", this.name, DateTime.Now.ToString("HHmmssfff"));
                        string[] path = {@"/", "var", "collector", "data", filename};
                        if (!this.currentFrame.IsEmpty)
                        {
                            this.currentFrame.Save(Path.Combine(path));
                            Console.WriteLine("{0} frame saved to {1}", this.name, Path.Combine(path));
                        }
                        else
                        {
                            Console.WriteLine("{0} has empty frame, can't save to {1}", this.name, Path.Combine(path));
                        }
                    }
                    catch
                    {

                    }
                }
                Thread.Sleep(this.phase);
            }
            this.threadRunning = false;
        }
    }
}