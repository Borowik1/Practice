using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    public class Kolobok : EnemyTank
    {
        public int Score { get; set; }

        public Kolobok(Bitmap image, Point point, int id, int speed, Size pictureBoxSize) : base(image, point, id, speed, pictureBoxSize)
        {
            this.IsHit = false;
            this.Direction = Direction.RIGHT;
        }

        public void ChangeDir(Direction newDir)
        {
            Direction dirPrev = this.Direction;
            this.Direction = newDir;
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

        public override void Move(DateTime privStep, DateTime now, List<GameObject> gameObjects)
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
                    if (CheckCollision(nextPoint, this.Size, this.Id, gameObjects))
                    {
                        nextPoint.X += delta;
                    }
                    break;

                case Direction.UP:
                    nextPoint.Y -= delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, gameObjects))
                    {
                        nextPoint.Y += delta;
                    }
                    break;

                case Direction.RIGHT:
                    nextPoint.X += delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, gameObjects))
                    {
                        nextPoint.X -= delta;
                    }
                    break;

                case Direction.DOWN:
                    nextPoint.Y += delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, gameObjects))
                    {
                        nextPoint.Y -= delta;
                    }
                    break;
            }

            Size size = new Size((gameField.Width - (2 * Size.Width)), (gameField.Height - (2 * Size.Height)));
            Rectangle modifiedGameField = new Rectangle(new Point(Size.Width, Size.Height), size);
            Rectangle objectRect = new Rectangle(nextPoint, Size);

            if (objectRect.IntersectsWith(modifiedGameField))
            {
                this.Point = nextPoint;
            }            
        }

        internal void CheckBulletCollision(List<Bullet> bullets)
        {
            foreach (Bullet item in bullets)
            {
                Rectangle rec = new Rectangle(this.Point, this.Size);
                Rectangle rec2 = new Rectangle(item.Point, item.Size);

                if (rec.IntersectsWith(rec2))
                {
                    this.IsHit = true;
                }
            }
        }

        public void RemoveApples(List<Apple> apples)
        {
            foreach (Apple item in apples)
            {
                Rectangle rec = new Rectangle(this.Point, this.Size);
                Rectangle rec2 = new Rectangle(item.Point, item.Size);

                if (rec.IntersectsWith(rec2))
                {
                    item.IsHit = true;
                    this.Score++;
                }
            }
        }

        public void CheckTankCollision(List<EnemyTank> enemyTanks)
        {
            foreach (EnemyTank item in enemyTanks)
            {
                Rectangle rec = new Rectangle(this.Point, this.Size);
                Rectangle rec2 = new Rectangle(item.Point, item.Size);

                if (rec.IntersectsWith(rec2))
                {
                    this.IsHit = true;
                }               
            }
        }
    }
}
