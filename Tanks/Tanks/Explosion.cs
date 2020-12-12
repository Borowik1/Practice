using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    class Explosion : GameObject
    {
        private Bitmap img;
        private Point pt;
        private Size sz;
        private int id;
        private bool isHit;
        private DateTime time;

        public Explosion(Bitmap img, Point pt, Size sz, int id) : base(img, pt, id)
        {
            this.img = img;
            this.pt = pt;
            this.sz = img.Size;
            this.id = id;
            this.isHit = false;
            time = DateTime.Now;
        }

        public override void Draw(Tanks form)
        {

            img = (Bitmap)form.Images[7];

            if ((DateTime.Now - time).TotalMilliseconds > 300)
                IsHit = true;

            form.g.DrawImage(this.Img, this.Point);
        }
    }
}
