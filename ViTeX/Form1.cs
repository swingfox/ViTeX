using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViTeX
{
    public partial class ViTeX : Form
    {
        string file;

        VideoSourcePlayer videoSourcePlayer;
        VideoFileSource videoSource;
        VideoFileReader reader = new VideoFileReader();
        public ViTeX()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
      
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                file = openFileDialog1.FileName;
                videoSource = new VideoFileSource(file);
                reader.Open(file);

                videoSourcePlayer.VideoSource = videoSource;
                videoSource.NewFrame += new AForge.Video.NewFrameEventHandler(Video_NewFrame);
             //      MessageBox.Show();

                type.Text = file.Split('.')[1];
                fps.Text = reader.FrameRate+"";
                
            }

            else if (result == DialogResult.Abort || result == DialogResult.Cancel)
            {
             //   MessageBox.Show("ABORTED");
            }
;        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
        }

        private void load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "MP4 files (*.mp4)|*.mp4|AVI files (*.avi)|*.avi|All files (*.*)|*.*";
            openFileDialog1.Title = "Please select an video file to play.";
            openFileDialog1.FileName = "";

            videoSourcePlayer = new VideoSourcePlayer();


        }

        private void Video_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            //Create Bitmap from frame
            Bitmap FrameData = new Bitmap(eventArgs.Frame);
            //Add to PictureBox
            pictureBox1.Image = FrameData;
            Bitmap processed = new Bitmap(eventArgs.Frame);
            pictureBox2.Image = processed;
            //compare current frame to specific fram
            /*    if (pictureBox1.Image == pictureBox2.Image)
                {
                    MessageBox.Show("Frame Match");
                }*/
         //   pictureBox1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(videoSourcePlayer != null)
                videoSourcePlayer.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        // Close video source if it is running
        private void CloseCurrentVideoSource()
        {
            if (videoSourcePlayer.VideoSource != null)
            {
                videoSourcePlayer.SignalToStop();

                // wait ~ 3 seconds
                for (int i = 0; i < 30; i++)
                {
                    if (!videoSourcePlayer.IsRunning)
                        break;
                    System.Threading.Thread.Sleep(100);
                }

                if (videoSourcePlayer.IsRunning)
                {
                    videoSourcePlayer.Stop();
                }

                videoSourcePlayer.VideoSource = null;
            }
        }

        private void closing(object sender, FormClosingEventArgs e)
        {
            CloseCurrentVideoSource();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(videoSourcePlayer != null)
                videoSourcePlayer.Stop();
        }
    }
}
