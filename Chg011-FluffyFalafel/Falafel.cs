using System;
using System.Drawing;
using static Engine.IEngine;


namespace Chg011_FluffyFalafel
{
    public class Falafel
    {
        public int x;
        public int y;
        public int i;
        public int j;
        private Frame frame;
        private Color color;
        private Color highlight;
        private bool isOver = false;
        public Falafel(int _i, int _j, Frame _frame, Color _color)
        {
            frame = _frame;
            color = _color;
            highlight = Color.FromArgb((int)(color.R * 1.1), (int)(color.G * 1.1), (int)(color.B * 1.1));
            setInd(_i, _j);
        }

        public void setPos(int _x, int _y)
        {
            x = _x;
            y = _y;

            int actual_x = getActualPos()[0];
            int actual_y = getActualPos()[1];

            i = (int) Math.Round((actual_x - frame.left) / (double) frame.size - 0.5);
            j = (int) Math.Round((actual_y - frame.top) / (double) frame.size - 0.5);
        }

        public void setInd(int _i, int _j)
        {
            i = _i;
            j = _j;
            x = frame.left + (int)((i + 0.5) * frame.size);
            y = frame.top + (int)((j + 0.5) * frame.size);
        }

        public bool isMouseOver(int _x, int _y)
        {
            isOver = false;
            if (_x <= x - frame.size * 0.5) return false;
            if (_x >= x + frame.size * 0.5) return false;
            if (_y <= y - frame.size * 0.5) return false;
            if (_y >= y + frame.size * 0.5) return false;
            isOver = true;
            return true;
        }

        public void snap()
        {
            setInd(i, j);
        }


        public void draw()
        {
            SetColor(color);

            if (isOver)
            {
                SetColor(highlight);
            }

            DrawCircle(x, y, frame.size);


            SetColor(color);
            if (x + frame.size / 2 > frame.right)
                DrawCircle(frame.left - (frame.right - x), y, frame.size);
            else if (x - frame.size / 2 < frame.left)
                DrawCircle(frame.right + (x - frame.left), y, frame.size);
            if (y + frame.size / 2 > frame.bottom)
                DrawCircle(x, frame.top - (frame.bottom - y), frame.size);
            else if (y - frame.size / 2 < frame.top)
                DrawCircle(x, frame.bottom + (y - frame.top), frame.size);
        }


        private int [] getActualPos()
        {
            int actual_x = x > frame.right ? frame.left + (x - frame.right) : x;
            actual_x = actual_x < frame.left ? frame.right - (frame.left - actual_x) : actual_x;

            int actual_y = y > frame.bottom ? frame.top + (y - frame.bottom) : y;
            actual_y = actual_y < frame.top ? frame.bottom - (frame.top - actual_y) : actual_y;
            return new int []{ actual_x, actual_y };
        }
    }
}
