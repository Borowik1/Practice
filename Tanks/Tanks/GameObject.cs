﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    public class GameObject
    {
        private Bitmap img;
        private Point pt;
        private Size sz;
        private int id;
        private bool isHit;

        public int Id { get { return id; }}

        public Bitmap Img { get { return img; } }

        public Point Point { get { return pt; } set { pt = value; } }

        public Size Size { get { return sz; } }

        public bool IsHit { get { return isHit; } set { isHit = value; } }

        public GameObject(Bitmap image, Point point, int id)
        {
            img = image;
            pt = point;
            sz = image.Size;
            this.id = id;
            isHit = false;
        }

        public void Draw(Form1 form)
        {
            form.g.DrawImage(img, pt);
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
