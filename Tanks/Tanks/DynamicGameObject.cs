using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    class DynamicGameObject : GameObject
    {

        private Direction dir;
        private int speed;
        Size gameField;
        public DynamicGameObject(Bitmap image, Point point, int id, int speed, Size pictureBoxSize) : base(image, point, id)
        {
            dir = Direction.RIGHT;
            this.speed = speed;
            gameField = pictureBoxSize;
        }

        public void Move(DateTime privStep, DateTime now, List<GameObject> listForCollisionCheck)
        {
            int delta = speed * (now - privStep).Milliseconds / 1000;
            Point nextPoint = new Point(this.Point.X, this.Point.Y);

            switch (dir)
            {
                case Direction.LEFT:
                    nextPoint.X -= delta;
                    if (CheckCollision(nextPoint, listForCollisionCheck))
                    {
                        ChangeDir();
                        nextPoint.X += delta;
                    }
                    break;

                case Direction.UP:
                    nextPoint.Y -= delta;
                    if (CheckCollision(nextPoint, listForCollisionCheck))
                    {
                        ChangeDir();
                        nextPoint.Y += delta;
                    }
                    break;

                case Direction.RIGHT:
                    nextPoint.X += delta;
                    if (CheckCollision(nextPoint, listForCollisionCheck))
                    {
                        ChangeDir();
                        nextPoint.X -= delta;
                    }
                    break;

                case Direction.DOWN:
                    nextPoint.Y += delta;
                    if (CheckCollision(nextPoint, listForCollisionCheck))
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

        public void Move(DateTime privStep, DateTime now, List<DynamicGameObject> listForCollisionCheck)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            foreach (DynamicGameObject item in listForCollisionCheck)
            {
                gameObjects.Add(item);
            }
            this.Move(privStep, now, gameObjects);
        }

        public bool CheckCollision(Point nextPoint, List<GameObject> listOfObjects)
        {
            foreach (GameObject item in listOfObjects)
            {
                Rectangle rec = new Rectangle(nextPoint, this.Size);
                Rectangle rec2 = new Rectangle(item.Point, item.Size);

                if (this.Id != item.Id)
                {
                    if (rec.IntersectsWith(rec2))
                        return true;
                }
            }
            return false;
        }

        public bool CheckCollision(Point nextPoint, List<DynamicGameObject> listOfObjects)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            foreach (DynamicGameObject item in listOfObjects)
            {
                gameObjects.Add(item);
            }
            return CheckCollision(nextPoint, gameObjects);
        }

        public void ChangeDir()
        {
            Random random = new Random();
            Direction dirPrev = dir;
            dir = (Direction)random.Next(0, 3);
            int rotationCount = -1;
            if (dir > dirPrev)
            {
                rotationCount = dir - dirPrev;
            }
            else if (dir < dirPrev)
            {
                rotationCount = 4 - (dirPrev - dir);
            }           

            for (int i = 0; i <= rotationCount; i++)
            {
                Img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
        }
    }
}
