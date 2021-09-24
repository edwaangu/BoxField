using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BoxField
{
    class Player
    {
        public int x, y, size, speed;
        public Player(int _x, int _y, int _size, int _speed)
        {
            x = _x;
            y = _y;
            size = _size;
            speed = _speed;
        }

        public void Move(int direction)
        {
            x += speed * direction;
        }

        public bool Collision(Box b)
        {
            Rectangle thisRect = new Rectangle(x, y, size, size);
            Rectangle boxRect = new Rectangle(b.x, b.y, b.size, b.size);

            return thisRect.IntersectsWith(boxRect);

        }
    }
}
