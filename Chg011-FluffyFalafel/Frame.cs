using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chg011_FluffyFalafel
{
    public class Frame
    {
        public int rows;
        public int cols;
        public int size;
        public int width;
        public int height;
        public int right;
        public int left;
        public int top;
        public int bottom;

        public Frame(int rows, int cols, int size, int width, int height)
        {
            this.rows   = rows;
            this.cols   = cols;
            this.size   = size;
            this.width  = width;
            this.height = height;
            this.right  = this.width / 2 + this.cols * this.size / 2;
            this.left   = this.width / 2 - this.cols * this.size / 2;
            this.top    = this.height / 2 - this.rows * this.size / 2;
            this.bottom = this.height / 2 + this.rows * this.size / 2;
        }
    }
}
