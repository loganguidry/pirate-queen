using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PirateQueen
{
    class Saving
    {
        // Attributes:
        static private string dataFile = "C:/pirate-queen/data.dat";
        static private string settingsFile = "C:/pirate-queen/settings.dat";

        // Saving data:
        static public void SaveData()
        {
            try
            {
                // Open the data file:
                Directory.CreateDirectory("C:/pirate-queen");
                BinaryWriter writer = new BinaryWriter(File.OpenWrite(dataFile));

                // Fill file with data:
                // level, score, whatever
                //writer.Write(Game1.PLAYER_WALKING_SPEED); //int

                // Close the file:
                writer.Close();
            }
            catch { }
        }

        // Loading data:
        static public void LoadData()
        {
            try
            {
                // Open the settings file:
                Directory.CreateDirectory("C:/pirate-queen");
                BinaryReader reader = new BinaryReader(File.OpenRead(settingsFile));

                // Get settings:
                Game1.PLAYER_WALKING_SPEED = reader.ReadInt32();
                Game1.PLAYER_RUNNING_SPEED = reader.ReadInt32();

                // Close the file:
                reader.Close();
            }
            catch
            {
                // Error loading settings, reset variables:
                Game1.PLAYER_WALKING_SPEED = 3;
                Game1.PLAYER_RUNNING_SPEED = 5;

                // Try creating the settings file:
                Directory.CreateDirectory("C:/pirate-queen");
                BinaryWriter writer = new BinaryWriter(File.OpenWrite(settingsFile));
                writer.Close();
            }

            try
            {
                // Open the data file:
                Directory.CreateDirectory("C:/pirate-queen");
                BinaryReader reader = new BinaryReader(File.OpenRead(dataFile));

                // Get data:
                // level, score, whatever
                //Game1.PLAYER_WALKING_SPEED = reader.ReadInt32();

                // Close the file:
                reader.Close();
            }
            catch
            {
                // Error loading data, reset variables:
                // level, score, whatever

                // Try creating the data file:
                Directory.CreateDirectory("C:/pirate-queen");
                BinaryWriter writer = new BinaryWriter(File.OpenWrite(dataFile));
                writer.Close();
            }
        }
    }
}
