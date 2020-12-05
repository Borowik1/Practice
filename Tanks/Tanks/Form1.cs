using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{

    public partial class Form1 : Form
    {
        List<GameObject> walls;
        List<GameObject> apples;
        List<DynamicGameObject> enemies;
        List<DynamicGameObject> bullets;
        DynamicGameObject player;
        public Graphics g;
        DateTime now;
        DateTime privStep;
        Color backgroundColor;

        public Form1()
        {
            InitializeComponent();
            g = this.pictureBox1.CreateGraphics();
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            walls = new List<GameObject>
            {
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(10, 10), 10),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(200, 20), 20)
            };

            enemies = new List<DynamicGameObject>
            {
                new DynamicGameObject(new Bitmap(imageList1.Images[0], new Size(30, 30)), new Point(250, 140), 30, 50, pictureBox1.Size),
                new DynamicGameObject(new Bitmap(imageList1.Images[0], new Size(30, 30)), new Point(140, 140), 40, 50, pictureBox1.Size)
            };
            enemies[0].ChangeDir();
            enemies[0].ChangeDir();

            backgroundColor = Color.Black;
            now = DateTime.Now;

            timer1.Enabled = true;
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            List<GameObject> dump = new List<GameObject>();

            foreach (GameObject item in walls)
            {
                dump.Add(item);
            }

            ////foreach (GameObject item in apples)
            ////{
            ////    dump.Add(item);
            ////}

            foreach (DynamicGameObject item in enemies)
            {
                dump.Add(item);
            }

            //foreach (DynamicGameObject item in bullets)
            //{
            //    dump.Add(item);
            //}
            
            privStep = now;
            now = DateTime.Now;
            g.Clear(backgroundColor);
            foreach (DynamicGameObject obj in enemies)
            {
                obj.Move(privStep, now, dump);                
                obj.Draw(this);
            }

            foreach (GameObject obj in walls)
            {
                obj.Draw(this);
            }
        }
    }
}
