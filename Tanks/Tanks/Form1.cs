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
using System.IO;
using static System.Windows.Forms.ImageList;

namespace Tanks
{

    public partial class Tanks : Form
    {
        Bitmap bp;
        Kolobok kolobok;
        List<GameObject> walls;
        List<GameObject> water;
        List<GameObject> explosions;
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

        public Bitmap Bitmap { get { return bp; } set { bp = value; } }

        public Tanks()
        {
            InitializeComponent();
            bp = new Bitmap(imageList1.Images[0], pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bp);

            enemySpeed = 50;
            bulletSpeed = 100;
            kolobokSpeed = 60;

            id = 0;

            apples = new List<Apple>();
            enemies = new List<EnemyTank>();
            bullets = new List<Bullet>();
            explosions = new List<GameObject>();
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


        public ImageCollection Images { get { return imageList1.Images; } }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            FillWalls();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            apples = Apple.RemoveHited(apples);
            bullets = Bullet.RemoveHited(bullets);
            enemies = EnemyTank.AddExplosion(enemies, this);
            walls = GameObject.AddExplosion(walls, this);
            explosions = GameObject.RemoveHited(explosions, this);

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

            foreach (GameObject item in water)
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

            pictureBox1.Image = bp;

            kolobok.Move(privStep, now, dump);
            kolobok.RemoveApples(apples);
            kolobok.CheckTankCollision(enemies);
            kolobok.CheckBulletCollision(bullets);
            kolobok.Draw(this);

            foreach (GameObject obj in walls)
            {
                obj.Draw(this);
            }

            foreach (GameObject obj in water)
            {
                obj.Draw(this);
            }

            foreach (Apple obj in apples)
            {
                obj.Draw(this);
            }

            foreach (Explosion obj in explosions)
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
            bool colWater = true;

            do
            {
                point.X = random.Next(0, pictureBox1.Size.Width - appleImg.Size.Width);
                point.Y = random.Next(0, pictureBox1.Size.Height - appleImg.Size.Height);

                colWalls = EnemyTank.CheckCollision(point, appleImg.Size, id, walls);
                colWater = EnemyTank.CheckCollision(point, appleImg.Size, id, water);
                colApples = EnemyTank.CheckCollision(point, appleImg.Size, id, apples);

            } while (colWalls || colApples || colWater);

            apples.Add(new Apple(appleImg, point, id));
        }

        internal void AddExplosion(Point point)
        {
            Bitmap explosionImg = new Bitmap(imageList1.Images[7], new Size(30, 30));
            explosions.Add(new Explosion(explosionImg, point, explosionImg.Size, GetId()));
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
            string line;
            water = new List<GameObject>();
            walls = new List<GameObject>();

            using (StreamReader reader = new StreamReader("items.txt"))
            {
                Point pt = new Point(0, 0);
                while ((line = reader.ReadLine()) != null)
                {
                    pt.X = 0;
                    foreach (char item in line)
                    {
                        switch (item)
                        {
                            case 'w':
                                water.Add(new GameObject(new Bitmap(imageList1.Images[6], new Size(32, 32)), pt, GetId()));
                                break;
                            case 'b':
                                walls.Add(new GameObject(new Bitmap(imageList1.Images[2], new Size(32, 32)), pt, GetId()));
                                break;
                            default:
                                break;
                        }
                        pt.X += 32;
                    }
                    pt.Y += 32;
                }


            }
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
            else
            {
                form2.Activate();
            }
        }
    }
}
