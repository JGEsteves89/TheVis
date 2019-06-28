using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chg011_FluffyFalafel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chg011_FluffyFalafel.Tests
{
    [TestClass()]
    public class LevelTests
    {
        private Frame frame = new Frame(2, 2, 50, 100, 100);
        private Color color = Color.White;
        [TestMethod()]
        public void LevelValuesTest()
        {
            Level level = new Level(frame);

            level.addFalafel(0, 0, color);
            level.addFalafel(0, 1, color);
            level.addFalafel(1, 0, color);
            level.addFalafel(1, 1, color);

            Assert.AreEqual(2, level.rows.Count());
            Assert.AreEqual(2, level.cols.Count());
            Assert.AreEqual(4, level.list.Count());
        }
        public void LevelSelectTest()
        {
            Level level = new Level(frame);

            level.addFalafel(0, 0, color);
            level.addFalafel(0, 1, color);
            level.addFalafel(1, 0, color);
            level.addFalafel(1, 1, color);

            level.select(0, 0);
            Assert.IsNull(level.selectedRow);
            Assert.IsNull(level.selectedCol);

            level.select(25,25);
            Assert.AreEqual(0, level.selectedRow);
            Assert.AreEqual(0, level.selectedCol);

            level.select(100, 100);
            Assert.IsNull(level.selectedRow);
            Assert.IsNull(level.selectedCol);
        }
        public void LevelDragRowTest()
        {
            Level level = new Level(frame);

            level.addFalafel(0, 0, color);
            level.addFalafel(0, 1, color);
            level.addFalafel(1, 0, color);
            level.addFalafel(1, 1, color);

            level.dragRow(0, 10);
            Assert.AreEqual(35, level.row(0)[0].x);
            Assert.AreEqual(85, level.row(0)[1].x);

            level.dragRow(0, -10);
            Assert.AreEqual(25, level.row(0)[0].x);
            Assert.AreEqual(75, level.row(0)[1].x);
        }
        public void LevelDragColTest()
        {
            Level level = new Level(frame);

            level.addFalafel(0, 0, color);
            level.addFalafel(0, 1, color);
            level.addFalafel(1, 0, color);
            level.addFalafel(1, 1, color);

            level.dragCol(0, 10);
            Assert.AreEqual(35, level.col(0)[0].y);
            Assert.AreEqual(85, level.col(0)[1].y);

            level.dragCol(0, -10);
            Assert.AreEqual(25, level.row(0)[0].y);
            Assert.AreEqual(75, level.row(0)[1].y);
        }
        public void LevelDragRow1IndTest()
        {
            Level level = new Level(frame);

            level.addFalafel(0, 0, color);
            level.addFalafel(0, 1, color);
            level.addFalafel(1, 0, color);
            level.addFalafel(1, 1, color);

            Assert.AreEqual(0, level.row(0)[0].j);
            Assert.AreEqual(1, level.row(0)[1].j);

            level.dragRow(0, 50);
            Assert.AreEqual(75, level.row(0)[0].x);
            Assert.AreEqual(125, level.row(0)[1].x);

            Assert.AreEqual(1, level.row(0)[0].j);
            Assert.AreEqual(0, level.row(0)[1].j);

        }
        public void LevelDragCol1IndTest()
        {
            Level level = new Level(frame);

            level.addFalafel(0, 0, color);
            level.addFalafel(0, 1, color);
            level.addFalafel(1, 0, color);
            level.addFalafel(1, 1, color);

            Assert.AreEqual(0, level.col(0)[0].i);
            Assert.AreEqual(1, level.col(0)[1].i);

            level.dragCol(0, 50);
            Assert.AreEqual(75, level.col(0)[0].x);
            Assert.AreEqual(125, level.col(0)[1].x);

            Assert.AreEqual(1, level.col(0)[0].i);
            Assert.AreEqual(0, level.col(0)[1].i);

        }
    }
}