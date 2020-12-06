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

        public override void Move(DateTime privStep, DateTime now, List<GameObject> listForCollisionCheck)
        {
            int delta = this.Speed * (now - privStep).Milliseconds / 1000;
            Point nextPoint = new Point(this.Point.X, this.Point.Y);

            if(delta == 0)
            {
                return;
            }

            switch (this.Direction)
            {
                case Direction.LEFT:
                    nextPoint.X -= delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, listForCollisionCheck))
                    {
                        nextPoint.X += delta;
                    }
                    break;

                case Direction.UP:
                    nextPoint.Y -= delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, listForCollisionCheck))
                    {
                        nextPoint.Y += delta;
                    }
                    break;

                case Direction.RIGHT:
                    nextPoint.X += delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, listForCollisionCheck))
                    {
                        nextPoint.X -= delta;
                    }
                    break;

                case Direction.DOWN:
                    nextPoint.Y += delta;
                    if (CheckCollision(nextPoint, this.Size, this.Id, listForCollisionCheck))
                    {
                        nextPoint.Y -= delta;
                    }
                    break;
            }
            this.Point = nextPoint;
        }
    }
}
