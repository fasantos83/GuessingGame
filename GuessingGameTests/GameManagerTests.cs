using Microsoft.VisualStudio.TestTools.UnitTesting;
using GuessingGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessingGame.Tests {

    [TestClass()]
    public class GameManagerTests {

        [TestMethod()]
        public void InitialSetupTest() {
            GameManager gm = new GameManager();
            List<Animal> animals = gm.Animals;

            //asserts animal list is not null
            Assert.IsNotNull(animals);

            //asserts animal list is empty
            Assert.AreEqual(animals.Count, 0);

            gm.InitialSetup();

            animals = gm.Animals;
            //asserts animal list is not null
            Assert.IsNotNull(animals);
        }
        
    }
}