using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    public class Bullet : EnemyTank
    {
        public Bullet(Bitmap image, Point point, int id, int speed, Size pictureBoxSize) : base(image, point, id, speed, pictureBoxSize)
        {
            this.IsHit = false;
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


        //Переопределить метод Move, чтобы он помечал на уничтожение пулю при соприкосновением с любым объектом
        public void Move(DateTime privStep, DateTime now, List<GameObject> walls, List<EnemyTank> enemies, Kolobok kolobok)
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
                    if (CheckCollision(nextPoint, this.Size, this.Id, walls))
                    {
                        this.IsHit = true;
                        nextPoint.X += delta;
                    }
                    break;

                case Direction.UP:
                    nextPoint.Y -= delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, walls))
                    {
                        this.IsHit = true;
                        nextPoint.Y += delta;
                    }
                    break;

                case Direction.RIGHT:
                    nextPoint.X += delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, walls))
                    {
                        this.IsHit = true;
                        nextPoint.X -= delta;
                    }
                    break;

                case Direction.DOWN:
                    nextPoint.Y += delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, walls))
                    {
                        this.IsHit = true;
                        nextPoint.Y -= delta;
                    }
                    break;
            }

            switch (this.Direction)
            {
                case Direction.LEFT:
                    nextPoint.X -= delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, enemies))
                    {
                        this.IsHit = true;
                        nextPoint.X += delta;
                    }
                    break;

                case Direction.UP:
                    nextPoint.Y -= delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, enemies))
                    {
                        this.IsHit = true;
                        nextPoint.Y += delta;
                    }
                    break;

                case Direction.RIGHT:
                    nextPoint.X += delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, enemies))
                    {
                        this.IsHit = true;
                        nextPoint.X -= delta;
                    }
                    break;

                case Direction.DOWN:
                    nextPoint.Y += delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, enemies))
                    {
                        this.IsHit = true;
                        nextPoint.Y -= delta;
                    }
                    break;
            }

            Size size = new Size((gameField.Width - (2 * Size.Width)), (gameField.Height - (2 * Size.Height)));
            Rectangle modifiedGameField = new Rectangle(new Point(Size.Width, Size.Height), size);
            Rectangle objectRect = new Rectangle(nextPoint, Size);

            if (!objectRect.IntersectsWith(modifiedGameField))
            {
                this.IsHit = true;
            }
            else
            {
                this.Point = nextPoint;
            }
        }

        public new bool CheckCollision(Point nextPoint, Size size, int id, List<GameObject> listOfObjects)
        {
            foreach (GameObject item in listOfObjects)
            {
                Rectangle rec = new Rectangle(nextPoint, size);
                Rectangle rec2 = new Rectangle(item.Point, item.Size);

                if (id != item.Id)
                {
                    if (rec.IntersectsWith(rec2))
                    {
                        item.IsHit = true;
                        return true;
                    }
                        
                }
            }
            return false;
        }

        public new bool CheckCollision(Point nextPoint, Size size, int id, List<EnemyTank> listOfObjects)
        {
            foreach (EnemyTank item in listOfObjects)
            {
                Rectangle rec = new Rectangle(nextPoint, size);
                Rectangle rec2 = new Rectangle(item.Point, item.Size);

                if (id != item.Id)
                {
                    if (rec.IntersectsWith(rec2))
                    {
                        item.IsHit = true;
                        return true;
                    }

                }
            }
            return false;
        }

        public static List<Bullet> RemoveHited(List<Bullet> gameObjects)
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
    }
}
