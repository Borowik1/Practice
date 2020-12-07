using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    public class Apple : GameObject
    {
        public Apple(Bitmap image, Point point, int id) : base(image, point, id)
        {

        }

        public static List<Apple> RemoveHited(List<Apple> gameObjects)
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
