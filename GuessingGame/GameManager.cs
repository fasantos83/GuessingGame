using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GuessingGame {

    /// <summary>
    ///     Class the manages the game's core loop
    /// </summary>
    public class GameManager {
        // List of all known Animals
        List<Animal> animals;
        public List<Animal> Animals {
            get {
                if (animals == null) {
                    animals = new List<Animal>();
                }
                return animals;
            }
            set { animals = value; }
        }

        // List of possible Animals through the game
        List<Animal> possibleAnimals;

        // Last animal questioned
        Animal lastAnimal;

        // Last trait questioned
        string lastTrait;

        // Checks if the game is over (won or not)
        bool gameOver;

        // List of trais already asked
        List<string> traits;

        // List of trais already asked
        List<string> usedTraits;

        /// <summary>
        ///     Initializes the lists with predetermined Animals
        /// </summary>
        public void InitialSetup() {
            Animals = new List<Animal>();

            Animal shark = new Animal {
                Name = "shark"
            };
            shark.Traits.Add("lives in water");
            Animals.Add(shark);

            Animal monkey = new Animal {
                Name = "monkey"
            };
            Animals.Add(monkey);

            possibleAnimals = new List<Animal>();
            traits = new List<string>();
            usedTraits = new List<string>();
            gameOver = false;
        }

        /// <summary>
        ///     Main loop of the game:
        ///     Asks if animal has or not a trait and searchs for Animals with names and traits in a list.
        ///     Narrows down what possible animal it is with each question and if no animal is found asks for a new one with 
        ///     another specific trait to be added to the list.
        /// </summary>
        public void StartGame() {
            DialogResult result = DialogResult.Cancel;
            do {
                result = MessageBox.Show("Think about an animal...", "Guessing Game", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK) {
                    gameOver = false;

                    //updates list of possible Animals with all Animals
                    UpdatePossibleAnimalList();

                    //updates possible traits from all possible Animals
                    UpdateTraits();

                    // loops through all possible traits while game is not over
                    if (traits != null) {
                        int i = 0;
                        while (!gameOver && i < traits.Count) {
                            string trait = traits[i];
                            if (trait != null) {
                                lastTrait = trait;

                                result = MessageBox.Show("Does the animal that you thought about " + trait + "?", "Guessing Game", MessageBoxButtons.YesNo);

                                Boolean hasTrait = result == DialogResult.Yes;
                                if (hasTrait) {
                                    // mark used trait
                                    usedTraits.Add(trait);
                                }
                                //updates list of possible Animals based on trait
                                UpdatePossibleAnimalList(trait, hasTrait);

                                // if multiple Animals are still possible
                                if (possibleAnimals != null && possibleAnimals.Count > 0) {
                                    // if only one animal remaining
                                    if (possibleAnimals.Count == 1) {
                                        Animal animal = possibleAnimals[0];
                                        if (animal != null) {
                                            lastAnimal = animal;
                                            // asks if animal found
                                            result = MessageBox.Show("Is the animal that you thought about a " + animal.Name + "?", "Guessing Game", MessageBoxButtons.YesNo);

                                            EndGame(result == DialogResult.Yes);
                                        }
                                    } else {
                                        // updates possible traits from all possible Animals minus the current trait
                                        List<string> newTraits = UpdateTraits(trait);

                                        // restart loop counter
                                        i = -1;
                                    }
                                    // if not, continue guessing from next possible trait
                                } else {
                                    // end game if no more animal is on possible list
                                    EndGame();
                                }
                            }
                            i++;
                        }
                    }
                }
            } while (result != DialogResult.Cancel);
        }

        /// <summary>
        ///     Updates list of all traits from possible Animals
        /// </summary>
        List<string> UpdateTraits(string currentTrait = null) {
            traits = new List<string>();

            //loops through all possible Animals
            if (possibleAnimals != null) {
                foreach (Animal animal in possibleAnimals) {
                    if (animal.Traits != null) {

                        // loops through all animal traits
                        foreach (string trait in animal.Traits) {
                            // Add trait if trait not on the list yet and has not been used yet and not equals to current trait
                            if (!traits.Contains(trait) && !usedTraits.Contains(trait) && !trait.Equals(currentTrait)) {
                                traits.Add(trait);
                            }
                        }
                    }
                }
            }
            return traits;
        }

        /// <summary>
        ///     Updates list of possible Animals
        /// </summary>
        List<Animal> UpdatePossibleAnimalList(string trait = null, bool hasTrait = true) {
            // if no trait was passed, fill wth all Animals
            if (trait == null) {
                possibleAnimals = new List<Animal>();
                foreach (Animal animal in Animals) {
                    possibleAnimals.Add(animal);
                }
            } else {
                //if trait was passed, fill only with Animals that has it or not based on 'hasTrait'
                foreach (Animal animal in possibleAnimals.ToList()) {
                    if ((hasTrait && !animal.Traits.Contains(trait)) || // possible animal that have the trait that guessed animal does not
                        (!hasTrait && animal.Traits.Contains(trait))) { // possible animal that doesn't have the trait that guessed animal has
                        possibleAnimals.Remove(animal);
                    }
                }
            }
            return possibleAnimals;
        }

        /// <summary>
        ///     Asks player to input another animal on the list.
        ///     Uses the list of asked traits to pre build the animal
        ///     and asks for a specific trait that differs from the last
        ///     possible animal on the list
        /// </summary>
        void AskForNewAnimal() {
            // asks for new animal name
            string newName = InputBox.ShowDialog("What was the animal that you thought about?", "Guessing Game");
            // asks for new animal trait
            string newTrait = InputBox.ShowDialog("A " + newName + " __________ but a " + lastAnimal.Name + " does not (Fill it with an animal trait, like \"" + lastTrait + "\").", "Guessing Game");

            // create new animal with new name
            Animal newAnimal = new Animal {
                Name = newName
            };

            // populate new animal trait with known used traits
            foreach (string trait in usedTraits) {
                newAnimal.Traits.Add(trait);
            }
            // add new trait to animal traits' list
            newAnimal.Traits.Add(newTrait);

            // add new animal to animal list
            Animals.Add(newAnimal);
        }

        /// <summary>
        ///     Gives the win game message box or
        ///     asks for information on the new animal
        /// </summary>
        void EndGame(bool foundAnimal = false) {
            if (foundAnimal) {
                MessageBox.Show("I win again!", "Guessing Game", MessageBoxButtons.OK);
            } else {
                AskForNewAnimal();
            }
            usedTraits = new List<string>();
            gameOver = true;
        }
        
        /// <summary>
        ///     Debug method that returns a string with the current list of Animals with its traits
        /// </summary>
        public string ListAllAnimals() {
            string animalList = "";

            if (Animals != null && Animals.Count > 0) {
                for (int i = 0; i < Animals.Count; i++) {
                    Animal animal = Animals[i];

                    if (animal != null) {
                        animalList += i != 0 ? "\n" : "";
                        animalList += "\n" + (i + 1) + ":";
                        animalList += "\nName: " + animal.Name;
                        if (animal.Traits != null && animal.Traits.Count > 0) {
                            animalList += "\nTraits: ";
                            foreach (string trait in animal.Traits) {
                                animalList += "\n * " + trait;
                            }
                        } else {
                            animalList += "\nNo traits";
                        }
                    }
                }
            } else {
                animalList = "No Animals stored on list.";
            }

            return animalList;
        }

    }

}
