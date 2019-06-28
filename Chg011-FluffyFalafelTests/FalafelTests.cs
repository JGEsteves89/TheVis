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
    public class FalafelTests
    {
        private Frame frame = new Frame(2, 2, 50, 100, 100);
        private Color color = Color.White;

        [TestMethod()]
        public void FalafelSetPosTest()
        {
            Falafel falafel = new Falafel(0, 0, this.frame, this.color);

            Assert.AreEqual(25, falafel.x);
            Assert.AreEqual(25, falafel.y);
            Assert.AreEqual(0, falafel.i);
            Assert.AreEqual(0, falafel.j);

            falafel.setPos(49, 49);
            Assert.AreEqual(49, falafel.x);
            Assert.AreEqual(49, falafel.y);
            Assert.AreEqual(0, falafel.i);
            Assert.AreEqual(0, falafel.j);

            falafel.setPos(51, 51);
            Assert.AreEqual(51, falafel.x);
            Assert.AreEqual(51, falafel.y);
            Assert.AreEqual(1, falafel.i);
            Assert.AreEqual(1, falafel.j);

            falafel.setPos(49, 51);
            Assert.AreEqual(49, falafel.x);
            Assert.AreEqual(51, falafel.y);
            Assert.AreEqual(0, falafel.i);
            Assert.AreEqual(1, falafel.j);
        }
        [TestMethod()]
        public void FalafelSetPosOutBoudTest()
        {
            Falafel falafel = new Falafel(0, 0, this.frame, this.color);

            falafel.setPos(101, 101);
            Assert.AreEqual(101, falafel.x);
            Assert.AreEqual(101, falafel.y);
            Assert.AreEqual(0, falafel.i);
            Assert.AreEqual(0, falafel.j);

            falafel.setPos(-1, -1);
            Assert.AreEqual(-1, falafel.x);
            Assert.AreEqual(-1, falafel.y);
            Assert.AreEqual(1, falafel.i);
            Assert.AreEqual(1, falafel.j);

            falafel.setPos(-1, 101);
            Assert.AreEqual(-1, falafel.x);
            Assert.AreEqual(101, falafel.y);
            Assert.AreEqual(1, falafel.i);
            Assert.AreEqual(0, falafel.j);
        }
        [TestMethod()]
        public void FalafelSanpTest()
        {
            Falafel falafel = new Falafel(0, 0, this.frame, this.color);

            falafel.setPos(49, 49);
            falafel.snap();
            Assert.AreEqual(25, falafel.x);
            Assert.AreEqual(25, falafel.y);
            Assert.AreEqual(0, falafel.i);
            Assert.AreEqual(0, falafel.j);

            falafel.setPos(51, 51);
            falafel.snap();
            Assert.AreEqual(75, falafel.x);
            Assert.AreEqual(75, falafel.y);
            Assert.AreEqual(1, falafel.i);
            Assert.AreEqual(1, falafel.j);

            falafel.setPos(49, 51);
            falafel.snap();
            Assert.AreEqual(25, falafel.x);
            Assert.AreEqual(75, falafel.y);
            Assert.AreEqual(0, falafel.i);
            Assert.AreEqual(1, falafel.j);

            falafel.setPos(-1, -1);
            falafel.snap();
            Assert.AreEqual(75, falafel.x);
            Assert.AreEqual(75, falafel.y);
            Assert.AreEqual(1, falafel.i);
            Assert.AreEqual(1, falafel.j);

            falafel.setPos(101, 101);
            falafel.snap();
            Assert.AreEqual(25, falafel.x);
            Assert.AreEqual(25, falafel.y);
            Assert.AreEqual(0, falafel.i);
            Assert.AreEqual(0, falafel.j);

        }

        [TestMethod()]
        public void FalafelSetIndTest()
        {
            Falafel falafel = new Falafel(0, 0, this.frame, this.color);


            falafel.setInd(0, 0);
            Assert.AreEqual(25, falafel.x);
            Assert.AreEqual(25, falafel.y);
            Assert.AreEqual(0, falafel.i);
            Assert.AreEqual(0, falafel.j);

            falafel.setInd(1, 1);
            Assert.AreEqual(75, falafel.x);
            Assert.AreEqual(75, falafel.y);
            Assert.AreEqual(1, falafel.i);
            Assert.AreEqual(1, falafel.j);

            falafel.setInd(0, 1);
            Assert.AreEqual(25, falafel.x);
            Assert.AreEqual(75, falafel.y);
            Assert.AreEqual(0, falafel.i);
            Assert.AreEqual(1, falafel.j);
        }
        [TestMethod()]
        public void FalafelMouseOverTest()
        {
            Falafel falafel = new Falafel(0, 0, this.frame, this.color);

            Assert.IsFalse(falafel.isMouseOver(0, 0));
            Assert.IsFalse(falafel.isMouseOver(50, 50));
            Assert.IsTrue(falafel.isMouseOver(49, 49));
            Assert.IsTrue(falafel.isMouseOver(1, 1));
        }
    }
}