using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BoxField
{
    class Box
    {
        public int x, y, size, speed;
        public List<int> color = new List<int>();

        public Box(int _x, int _y, int _size, int _speed, List<int> _color)
        {
            x = _x;
            y = _y;
            size = _size;
            speed = _speed;
            color = _color;
        }

        public void Move()
        {
            y += speed;
        }
    }
}
