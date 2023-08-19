using Battleship_v2.Ships;
using Battleship_v2.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Battleship_v2_test
{
    [TestClass]
    public class ShipTests
    {
        [TestMethod]
        public void Test1()
        {
            var aShip = new Carrier();
            var aPos = new Position(0, 0);
            aShip.SetShipValues(aPos, Orientation.Horizontal, false);
            var bPos = new Position(4, 0);
            Assert.AreEqual(aShip.IsHit(bPos), true);
        }

        [TestMethod]
        public void Test2()
        {
            var aShip = new Carrier();
            var aPos = new Position(0, 0);
            aShip.SetShipValues(aPos, Orientation.Horizontal, false);
            var bPos = new Position(5, 0);
            Assert.AreEqual(aShip.IsHit(bPos), false);
        }

        [TestMethod]
        public void Test3()
        {
            var aShip = new Carrier();
            var aPos = new Position(0, 4);
            aShip.SetShipValues(aPos, Orientation.Horizontal, false);

            var bShip = new Carrier();
            var bPos = new Position(2, 0);
            bShip.SetShipValues(bPos, Orientation.Vertical, false);

            Assert.AreEqual(aShip.IntersectsWith(bShip), true);
        }

        [TestMethod]
        public void Test4()
        {
            var a = new List<int>()
            {
                1, 2, 3,
            };
            var b = new List<int>() { 
                3,4, 5,
            };

            var c = a.Intersect(b);
            var d = c.ToList().Count;

            Assert.AreEqual(d, 1);
        }

        [TestMethod]
        public void Test5()
        {
            var a = new List<Position>()
            {
                new Position(),
                new Position(0,0),
                new Position(1,0),
            };
            var b = new List<Position>() {
                new Position(2,0),
                new Position(3,0),
                new Position(-1, 9),
                new Position(1,0)
            };

            var c = a.Intersect(b);
            var d = c.ToList().Count;

            Assert.AreEqual(d, 1);
        }
    }
}
