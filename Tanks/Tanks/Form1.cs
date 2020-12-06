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
        Kolobok kolobok;
        List<GameObject> walls;
        List<Apple> apples;
        List<EnemyTank> enemies;
        List<Bullet> bullets;
        public Graphics g;
        DateTime now;
        DateTime privStep;
        Color backgroundColor;
        List<Point> respawnPoints;

        int enemySpeed;
        int bulletSpeed;
        int kolobokSpeed;

        int id;

        public Form1()
        {
            InitializeComponent();
            g = this.pictureBox1.CreateGraphics();
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            enemySpeed = 50;
            bulletSpeed = 100;
            kolobokSpeed = 60;

            id = 0;

            apples = new List<Apple>();
            enemies = new List<EnemyTank>();
            bullets = new List<Bullet>();
            respawnPoints = new List<Point>
            {
                new Point(1, 1),
                new Point(32, 1),
                new Point(96, 1),
                new Point(160, 1),
                new Point(224, 1)
            };
            backgroundColor = Color.Black;
            now = DateTime.Now;

            timer1.Enabled = true;
        }


        private void Form1_Load(object sender, System.EventArgs e)
        {
            FillWalls();

            kolobok = new Kolobok(new Bitmap(imageList1.Images[3], new Size(30, 30)), new Point(200, 290), 0, 0, pictureBox1.Size);

            foreach (Point point in respawnPoints)
            {
                AddEnemy(point);
            }

            for (int i = 0; i < 5; i++)
            {
                AddApple();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            List<GameObject> dump = new List<GameObject>();

            foreach (GameObject item in walls)
            {
                dump.Add(item);
            }

            foreach (EnemyTank item in enemies)
            {
                dump.Add(item);
            }

            privStep = now;
            now = DateTime.Now;
            g.Clear(backgroundColor);

            kolobok.Move(privStep, now, dump);
            kolobok.Draw(this);

            foreach (GameObject obj in walls)
            {
                obj.Draw(this);
            }

            foreach (Apple obj in apples)
            {
                obj.Draw(this);
            }

            foreach (EnemyTank obj in enemies)
            {
                obj.Move(privStep, now, dump);
                obj.Draw(this);
            }

            foreach (Bullet obj in bullets)
            {
                obj.Move(privStep, now, dump);
                obj.Draw(this);
            }
        }

        public void AddBullet(Point point, Direction direction)
        {
            Bitmap bulletImg = new Bitmap(imageList1.Images[4], new Size(14, 13));

            Bullet bullet = new Bullet(bulletImg, point, GetId(), bulletSpeed, this.pictureBox1.Size);
            bullet.ChangeDir(direction);
            bullets.Add(bullet);
        }

        public void AddEnemy(Point point)
        {
            Bitmap enemyImg = new Bitmap(imageList1.Images[0], new Size(30, 30));
            this.enemies.Add(new EnemyTank(enemyImg, point, GetId(), enemySpeed, pictureBox1.Size));

        }

        public void AddApple()
        {            
            int id = GetId();
            Bitmap appleImg = new Bitmap(imageList1.Images[5], new Size(30, 30));
            Random random = new Random();
            Point point = new Point(0, 0);
            bool colWalls = true;
            bool colApples = true;
            do
            {
                point.X = random.Next(0, pictureBox1.Size.Width - appleImg.Size.Width);
                point.Y = random.Next(0, pictureBox1.Size.Height - appleImg.Size.Height);

                colWalls = EnemyTank.CheckCollision(point, appleImg.Size, id, walls);
                colApples = EnemyTank.CheckCollision(point, appleImg.Size, id, apples);

            } while (colWalls || colApples);

            apples.Add(new Apple(appleImg, point, id));           
        }

        public void RemoveHited(List<GameObject> gameObjects)
        {

        }

        public void RemoveHited(List<EnemyTank> gameObjects)
        {

        }

        public void RemoveById(int id)
        {

        }

        //public List<GameObject> LeadToBasicClass()
        //{

        //}

        public int GetId()
        {
            id++;
            return id;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Left:
                    kolobok.Speed = kolobokSpeed;
                    kolobok.ChangeDir(Direction.LEFT);
                    break;
                case Keys.Up:
                    kolobok.Speed = kolobokSpeed;
                    kolobok.ChangeDir(Direction.UP);
                    break;
                case Keys.Right:
                    kolobok.Speed = kolobokSpeed;
                    kolobok.ChangeDir(Direction.RIGHT);
                    break;
                case Keys.Down:
                    kolobok.Speed = kolobokSpeed;
                    kolobok.ChangeDir(Direction.DOWN);
                    break;
            }

            if (e.KeyData == Keys.Space)
            {
                kolobok.Fire(this);
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Left:
                    kolobok.Speed = 0;
                    break;
                case Keys.Up:
                    kolobok.Speed = 0;
                    break;
                case Keys.Right:
                    kolobok.Speed = 0;
                    break;
                case Keys.Down:
                    kolobok.Speed = 0;
                    break;
            }
        }

        private void FillWalls()
        {
            walls = new List<GameObject>
            {
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(32, 32), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(32, 64), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(32, 96), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(0, 128), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(32, 160), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(32, 192), GetId()),

                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(96, 32), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(96, 64), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(96, 96), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(96, 128), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(96, 160), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(96, 192), GetId()),

                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(160, 32), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(160, 64), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(160, 96), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(160, 128),GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(160, 160),GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(160, 192),GetId()),

                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(224, 32), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(224, 64), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(224, 96), GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(224, 128),GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(224, 160),GetId()),
                new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), new Point(224, 192),GetId())
            };

        }
    }
}
