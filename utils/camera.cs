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
        private bool firstException = false;

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
            this.thread.IsBackground = false;
            this.thread.Start();
            (new Thread(this.FrameReadThread)).Start();
        }

        private void FrameReadThread()
        {
            while(this.threadRunning)
            {
                Mat newFrame = null;
                try 
                {
                    newFrame = this.capture.QueryFrame();
                }
                catch(Exception e)
                {
                    Console.WriteLine("{0}", e.ToString());
                }
                lock(this.__frameLock)
                {
                    try
                    {
                        if (!newFrame.IsEmpty)
                        {
                            //this.currentFrame = newFrame.Clone();
                            newFrame.CopyTo(this.currentFrame);
                            newFrame.Dispose();
                            this.firstException = true;
                        }
                        //Console.WriteLine("{0} new frame!", this.name);
                    }
                    catch(Exception e)
                    {
                        if (this.firstException)
                        {
                            Console.WriteLine("{0} >> {1}", this.name, e.ToString());
                            this.firstException = false;
                        }
                    }
                }

                Thread.Sleep(5);
            }
        }
        private void MainThread()
        {
            while((DateTime.Now - this.creationTime).TotalSeconds < 360)
            {
                Console.WriteLine("{0} is alive for {1}", this.name, (DateTime.Now - this.creationTime).TotalMilliseconds);
                lock(this.__frameLock)
                {
                    try
                    {
                        string filename = string.Format("{0}_{1}.jpg", this.name, DateTime.Now.ToString("yyyymmdd_HHmmssfff"));
                        string[] path = {@"/", "var", "collector", "data", filename};
                        if (!this.currentFrame.IsEmpty)
                        {
                            currentFrame.Save(Path.Combine(path));
                            Console.WriteLine("{0} frame saved to {1}", this.name, Path.Combine(path));
                        }
                        else
                        {
                            Console.WriteLine("{0} has empty frame, can't save to {1}", this.name, Path.Combine(path));
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                Console.WriteLine("entering sleep...");
                Thread.Sleep(this.phase);
                Console.WriteLine("done sleeping...");
            }
            Console.WriteLine("{0} terminated after {1}s", this.name, (DateTime.Now - this.creationTime).TotalSeconds);
            this.threadRunning = false;
        }
    }
}