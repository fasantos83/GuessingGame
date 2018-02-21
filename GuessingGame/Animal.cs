using System;
using System.Collections.Generic;

namespace GuessingGame {

    /// <summary>
    ///     Class that defines what an animal is
    /// </summary>
    public class Animal {

        // Name of the animal
        public string Name { get; set; }

        // List of traits specific to this animal
        List<string> traits;
        public List<string> Traits {
            get {
                if (traits == null) {
                    traits = new List<string>();
                }
                return traits;
            }
            set { traits = value; }
        }

    }

}
