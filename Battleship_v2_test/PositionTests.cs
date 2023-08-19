using Battleship_v2.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Battleship_v2_test
{
    [TestClass]
    public class PositionTests
    {
        [TestMethod]
        public void Test1()
        {
            var aPos = new Position();
            var bPos = new Position();
            bool e = aPos == bPos;
            Assert.AreEqual(e, true);
        }
        
        [TestMethod]
        public void Test2()
        {
            var aPos = new Position(1,1);
            var bPos = new Position(1,1);
            Assert.AreEqual(aPos, bPos);
        }

        [TestMethod]
        public void Test3()
        {
            var aPos = new Position(1,1);
            var bPos = new Position(1,2);
            Assert.AreNotEqual(aPos, bPos);
        }

        [TestMethod]
        public void Test4()
        {
            var aPos = new Position(1,1);
            var bPos = new Position(1,1);
            bPos.WasHit = true;
            Assert.AreEqual(aPos, bPos);
        }

        [TestMethod]
        public void Test5()
        {
            var aPos = new Position(1, 2);
            var bPos = new Position(2, 1);
            aPos.Swap();
            Assert.AreEqual(aPos, bPos);
        }

        [TestMethod]
        public void Test6()
        {
            var aPos = new Position(1, 1);
            var bPos = aPos.Clone();
            bPos.WasHit = true;
            Assert.AreEqual(aPos, bPos);
        }

        [TestMethod]
        public void Test7()
        {
            var aPos = new Position(-1, -1);
            var bPos = new Position(-1, -1);
            Assert.AreEqual(aPos, bPos);
        }

        [TestMethod]
        public void Test8()
        {
            var aPos = new Position(1, 1);
            var bPos = new Position(0, 1);
            bPos.MoveRight();
            Assert.AreEqual(aPos, bPos);
        }

        [TestMethod]
        public void Test9()
        {
            var aPos = new Position(1, 1);
            var bPos = new Position(2, 1);
            bPos.MoveLeft();
            Assert.AreEqual(aPos, bPos);
        }

        [TestMethod]
        public void Test10()
        {
            var aPos = new Position(1, 1);
            var bPos = new Position(1, 2);
            bPos.MoveUp();
            Assert.AreEqual(aPos, bPos);
        }

        [TestMethod]
        public void Test11()
        {
            var aPos = new Position(1, 1);
            var bPos = new Position(1, 0);
            bPos.MoveDown();
            Assert.AreEqual(aPos, bPos);
        }

        [TestMethod]
        public void Test12()
        {
            var aPos = new Position(1, 1);
            Assert.AreEqual(aPos.IsValid(), true);
        }

        [TestMethod]
        public void Test13()
        {
            var aPos = new Position(0, -1);
            Assert.AreEqual(aPos.IsValid(), false);
        }

        [TestMethod]
        public void Test14()
        {
            var aPos = new Position(0, 10);
            Assert.AreEqual(aPos.IsValid(), false);
        }

        [TestMethod]
        public void Test15()
        {
            var aPos = new Position(9, 9);
            Assert.AreEqual(aPos.IsValid(), true);
        }
    }
}
