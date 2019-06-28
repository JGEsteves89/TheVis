using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chg011_FluffyFalafel
{
    public class Level
    {
        public Frame frame;
        public int selectedRow;
        public int selectedCol;
        public List<Falafel> list;

        public Level(Frame _frame)
        {
            frame = _frame;
        }

        public void addFalafel(int i, int j, Color color)
        {
            throw new NotImplementedException();
        }


        public List<Falafel> col(int v)
        {
            throw new NotImplementedException();
        }

        public List<Falafel> row(int v)
        {
            throw new NotImplementedException();
        }

        public void dragRow(int i, int dx)
        {
            throw new NotImplementedException();
        }
        public void dragCol(int j, int dy)
        {
            throw new NotImplementedException();
        }

        public void select(int x, int y)
        {
            selectedRow = null;
            selectedCol = null;
            foreach (Falafel item in list)
            {
                if (item.isMouseOver(x, y))
                {
                    selectedRow = item.i;
                    selectedCol = item.j;
                    break;
                }
            }
        }
    }
}
