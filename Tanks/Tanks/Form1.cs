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
using System.Threading;

namespace Tanks
{

    public partial class Tanks : Form
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
        bool gameRun;

        int enemySpeed;
        int bulletSpeed;
        int kolobokSpeed;

        int id;

        Form2 form2;

        public Tanks()
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
        }


        private void Form1_Load(object sender, System.EventArgs e)
        {
            FillWalls();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            apples = Apple.RemoveHited(apples);
            bullets = Bullet.RemoveHited(bullets);
            enemies = EnemyTank.RemoveHited(enemies);
            walls = GameObject.RemoveHited(walls);

            if (kolobok.IsHit)
            {
                StopGame();
                return;
            }

            while (apples.Count < 5)
            {
                AddApple();
            }

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
            kolobok.RemoveApples(apples);
            kolobok.CheckTankCollision(enemies);
            kolobok.CheckBulletCollision(bullets);
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
                obj.CheckKolobok(kolobok, this);
                obj.Draw(this);
            }

            foreach (Bullet obj in bullets)
            {
                obj.Move(privStep, now, walls, enemies, kolobok);
                obj.Draw(this);
            }

            this.label2.Text = kolobok.Score.ToString();

            if (form2 != null) form2.UpdateData(apples, enemies, kolobok);
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

        public int GetId()
        {
            id++;
            return id;
        }

        public void StopGame()
        {
            gameRun = false;
            timer1.Enabled = false;
            apples.Clear();
            enemies.Clear();
            bullets.Clear();
            kolobok = null;
            g.Clear(backgroundColor);
            if (form2 != null)
                form2.Clear();
            this.label2.Text = "0";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!gameRun) return true;

            switch (keyData)
            {
                case Keys.Left:
                    kolobok.Speed = kolobokSpeed;
                    kolobok.ChangeDir(Direction.LEFT);
                    break;
                case Keys.Right:
                    kolobok.Speed = kolobokSpeed;
                    kolobok.ChangeDir(Direction.RIGHT);
                    break;
                case Keys.Up:
                    kolobok.Speed = kolobokSpeed;
                    kolobok.ChangeDir(Direction.UP);
                    break;
                case Keys.Down:
                    kolobok.Speed = kolobokSpeed;
                    kolobok.ChangeDir(Direction.DOWN);
                    break;
                case Keys.Space:
                    kolobok.Fire(this);
                    break;
                default: return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!gameRun) return;

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

        private void buttonStart_Click(object sender, EventArgs e)
        {

            kolobok = new Kolobok(new Bitmap(imageList1.Images[3], new Size(30, 30)), new Point(200, 290), 0, 0, pictureBox1.Size);

            foreach (Point point in respawnPoints)
            {
                AddEnemy(point);
            }

            for (int i = 0; i < 5; i++)
            {
                AddApple();
            }

            gameRun = true;
            timer1.Enabled = true;

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            if (form2 == null)
            {
                form2 = new Form2(apples, enemies, kolobok);
                form2.Visible = true;
            }
        }
    }
}
