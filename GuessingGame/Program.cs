using System;
using System.Collections.Generic;

namespace GuessingGame {

    class Program {
        
        //[STAThread]
        static void Main() {
            GameManager gm = new GameManager();
            gm.InitialSetup();
            gm.StartGame();
        }
        
    }
}
