using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chg011_FluffyFalafel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chg011_FluffyFalafel.Tests
{
    [TestClass()]
    public class FrameTests
    {
        [TestMethod()]
        public void SmallFrameTest()
        {
            Frame frame = new Frame(1, 1, 50, 200, 200);
            Assert.AreEqual(1, frame.rows);
            Assert.AreEqual(1, frame.cols);
            Assert.AreEqual(50, frame.size);
            Assert.AreEqual(200, frame.width);
            Assert.AreEqual(200, frame.height);
            Assert.AreEqual(75, frame.left);
            Assert.AreEqual(125, frame.right);
            Assert.AreEqual(75, frame.top);
            Assert.AreEqual(125, frame.bottom);

        }
        [TestMethod()]
        public void NormalFrameTest()
        {
            Frame frame = new Frame(7, 5, 50, 1200, 800);
            Assert.AreEqual(7, frame.rows);
            Assert.AreEqual(5, frame.cols);
            Assert.AreEqual(50, frame.size);
            Assert.AreEqual(1200, frame.width);
            Assert.AreEqual(800, frame.height);
            Assert.AreEqual(600 - 50 * 2.5, frame.left);
            Assert.AreEqual(600 + 50 * 2.5, frame.right);
            Assert.AreEqual(400 - 50 * 3.5, frame.top);
            Assert.AreEqual(400 + 50 * 3.5, frame.bottom);
        }
    }
}