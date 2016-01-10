using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Radar
{
    public partial class MainForm : Form
    {
        SerialPort serialPort;
        string serialData;
        Graphics graph;
        Radar radar;

        public MainForm()
        {
            InitializeComponent();
            radar = new Radar();
            radarPictureBox.Paint += RadarPictureBox_Paint;
            this.Resize += MainForm_Resize;
            this.FormClosed += MainForm_FormClosed;
            serialPort = new SerialPort("COM14");
            serialPort.BaudRate = 56000;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.DataBits = 8;
            serialPort.Handshake = Handshake.None;
            serialPort.DtrEnable = true;
            graph = radarPictureBox.CreateGraphics();

            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

        }
     
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting();
            SetControlText(richTextBox1, data.ToString());
        }
        public void SetControlText(Control control, string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Control, string>(SetControlText), new object[] { control, text });
            }
            else
            {
                int arcSize = radarPictureBox.Size.Width / 12;

                Point center = new Point(radarPictureBox.Size.Width / 2 - arcSize / 2, radarPictureBox.Size.Height - arcSize / 2);
                Random rnd = new Random();
                Point hitPoint = radar.MeasurePoint(text, arcSize/20);
                graph.FillEllipse(Brushes.Red, new Rectangle(center.X - hitPoint.X +arcSize/16, center.Y - hitPoint.Y+arcSize/16 , arcSize/8, arcSize/8));

                control.Text += text;
                //graph.DrawRectangle(Pens.AliceBlue, new Rectangle(100, 100, 100, 100));
                //radarPictureBox.Invalidate();
            }
        }
       

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(serialPort.IsOpen) serialPort.Close();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            radarPictureBox.Width = this.Width - 40;
            radarPictureBox.Height = this.Height - 110;
            radarPictureBox.Invalidate();
        }

        private void RadarPictureBox_Paint(object sender, PaintEventArgs e)
        {
            int arcSize = e.ClipRectangle.Size.Width/12;
            Point center = new Point(e.ClipRectangle.Width/2 - arcSize/2, e.ClipRectangle.Height-arcSize/2);

            e.Graphics.FillEllipse(Brushes.Yellow, center.X, center.Y, arcSize / 1f, arcSize / 1f);
            for (int i = 0; i < 10; i++)
            {
                var arc = new Rectangle(new Point((int)(center.X - arcSize * (i/2f)), (int)(center.Y - arcSize * (i/2f))), new Size(arcSize * (i+1), arcSize * (i+1)));
                e.Graphics.DrawArc(Pens.DarkBlue, arc, 0, -180);
            }


            //if (serialPort.IsOpen)
            //{
            //    Point hitPoint = radar.MeasurePoint(serialPort.ReadLine(), 1);
            //    e.Graphics.FillEllipse(Brushes.Red, new Rectangle(center.X - hitPoint.X - 5, center.Y - hitPoint.Y - 5, 10, 10));
            //}
        }

        

        Rectangle drawArcs(Rectangle rect)
        {
            //  Rectangle x = new Rectangle(rect.X - arc)

            return Rectangle.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort.Open();
        }

       
    }
}
