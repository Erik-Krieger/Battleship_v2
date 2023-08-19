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
            Assert.AreEqual(aPos, bPos);
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
    }
}
