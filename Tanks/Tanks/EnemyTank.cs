using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    public class EnemyTank : GameObject
    {
        public Size gameField;

        int timeTillFire = 100;

        public int Speed { get; set; }

        public Direction Direction { get; set; }

        public EnemyTank(Bitmap image, Point point, int id, int speed, Size pictureBoxSize) : base(image, point, id)
        {
            this.Direction = Direction.RIGHT;
            this.Speed = speed;
            gameField = pictureBoxSize;
        }

        public virtual void Move(DateTime privStep, DateTime now, List<GameObject> listForCollisionCheck)
        {
            int delta = this.Speed * (now - privStep).Milliseconds / 1000;
            Point nextPoint = new Point(this.Point.X, this.Point.Y);

            if (delta == 0)
            {
                return;
            }

            switch (this.Direction)
            {
                case Direction.LEFT:
                    nextPoint.X -= delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, listForCollisionCheck))
                    {
                        ChangeDir();
                        nextPoint.X += delta;
                    }
                    break;

                case Direction.UP:
                    nextPoint.Y -= delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, listForCollisionCheck))
                    {
                        ChangeDir();
                        nextPoint.Y += delta;
                    }
                    break;

                case Direction.RIGHT:
                    nextPoint.X += delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, listForCollisionCheck))
                    {
                        ChangeDir();
                        nextPoint.X -= delta;
                    }
                    break;

                case Direction.DOWN:
                    nextPoint.Y += delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, listForCollisionCheck))
                    {
                        ChangeDir();
                        nextPoint.Y -= delta;
                    }
                    break;
            }

            Size size = new Size((gameField.Width - (2 * Size.Width)), (gameField.Height - (2 * Size.Height)));
            Rectangle modifiedGameField = new Rectangle(new Point(Size.Width, Size.Height), size);
            Rectangle objectRect = new Rectangle(nextPoint, Size);

            if (!objectRect.IntersectsWith(modifiedGameField))
            {
                ChangeDir();
            }
            else
            {
                this.Point = nextPoint;
            }
        }

        internal static List<EnemyTank> AddExplosion(List<EnemyTank> enemies, Tanks form)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsHit)
                {
                    form.AddExplosion(enemies[i].Point);
                    enemies.RemoveAt(i);
                    i--;
                }
            }

            return enemies;
        }

        public void Move(DateTime privStep, DateTime now, List<EnemyTank> listForCollisionCheck)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            foreach (EnemyTank item in listForCollisionCheck)
            {
                gameObjects.Add(item);
            }
            this.Move(privStep, now, gameObjects);
        }

        public static bool CheckCollision(Point nextPoint, Size size, int id, List<GameObject> listOfObjects)
        {
            foreach (GameObject item in listOfObjects)
            {
                Rectangle rec = new Rectangle(nextPoint, size);
                Rectangle rec2 = new Rectangle(item.Point, item.Size);

                if (id != item.Id)
                {
                    if (rec.IntersectsWith(rec2))
                        return true;
                }
            }
            return false;
        }

        internal void CheckKolobok(Kolobok kolobok, Tanks form1)
        {
            Rectangle rec = new Rectangle(this.Point, this.Size);
            Rectangle rec2 = new Rectangle(kolobok.Point, kolobok.Size);

            rec2.Inflate(60, 60);


            if (rec.IntersectsWith(rec2) && timeTillFire < 1)
            {
                this.Fire(form1);
                timeTillFire = 100;
            }
            else
            {
                timeTillFire--;
            }

        }

        public static bool CheckCollision(Point nextPoint, Size size, int id, List<EnemyTank> listOfObjects)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            foreach (EnemyTank item in listOfObjects)
            {
                gameObjects.Add(item);
            }
            return CheckCollision(nextPoint, size, id, gameObjects);
        }

        public static bool CheckCollision(Point nextPoint, Size size, int id, List<Apple> listOfObjects)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            foreach (Apple item in listOfObjects)
            {
                gameObjects.Add(item);
            }
            return CheckCollision(nextPoint, size, id, gameObjects);
        }

        public static List<EnemyTank> RemoveHited(List<EnemyTank> gameObjects)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].IsHit)
                {
                    gameObjects.RemoveAt(i);
                    i--;
                }
            }

            return gameObjects;
        }

        public void ChangeDir()
        {
            Random random = new Random();
            Direction dirPrev = this.Direction;
            this.Direction = (Direction)random.Next(0, 4);
            int rotationCount = -1;
            if (this.Direction > dirPrev)
            {
                rotationCount = this.Direction - dirPrev;
            }
            else if (this.Direction < dirPrev)
            {
                rotationCount = 4 - (dirPrev - this.Direction);
            }

            for (int i = 0; i < rotationCount; i++)
            {
                Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
        }

        public void Fire(Tanks form)
        {
            int x = 0;
            int y = 0;

            switch (this.Direction)
            {
                case Direction.LEFT:
                    x = this.Point.X - 16;
                    y = this.Point.Y + 8;
                    break;
                case Direction.UP:
                    x = this.Point.X + 8;
                    y = this.Point.Y - 16;
                    break;
                case Direction.RIGHT:
                    x = this.Point.X + this.Size.Width + 2;
                    y = this.Point.Y + 8;
                    break;
                case Direction.DOWN:
                    x = this.Point.X + 8;
                    y = this.Point.Y + this.Size.Height + 2;
                    break;
            }

            Point bulletPoint = new Point(x, y);

            form.AddBullet(bulletPoint, this.Direction);
        }
    }
}
