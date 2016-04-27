using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

/*
 * Pirate Queen
 * Team LASR
 * External Tool
 *
 * Milestone 2
 * Created by Logan Guidry
*/

namespace OptionSetter
{
    public partial class Form1 : Form
    {
        // Attributes:
        int walkingSpeed = 5;
        int runningSpeed = 8;

        // Constructor:
        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        // User drags the walking speed slider:
        private void sliderSpeed_ValueChanged(object sender, EventArgs e)
        {
            walkingSpeed = sliderWalkingSpeed.Value + 1;
            lblWalkingSpeedData.Text = walkingSpeed.ToString();

            if (runningSpeed < walkingSpeed)
            {
                runningSpeed = walkingSpeed;
                sliderRunningSpeed.Value = runningSpeed - 1;
                lblRunningSpeedData.Text = runningSpeed.ToString();
            }

            SaveData();
        }

        // User drags the running speed slider:
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            runningSpeed = sliderRunningSpeed.Value + 1;
            lblRunningSpeedData.Text = runningSpeed.ToString();

            if (runningSpeed < walkingSpeed)
            {
                walkingSpeed = runningSpeed;
                sliderWalkingSpeed.Value = walkingSpeed - 1;
            }

            SaveData();
        }

        // Load the settings from a binary file:
        private void LoadData()
        {
            try
            {
                // Open the file:
                Directory.CreateDirectory("C:/pirate-queen");
                BinaryReader reader = new BinaryReader(File.OpenRead("C:/pirate-queen/settings.dat"));
                
                // Get data:
                walkingSpeed = reader.ReadInt32();
                runningSpeed = reader.ReadInt32();

                // Close the file:
                reader.Close();
            }
            catch
            {
                // Error loading data, reset variables and try creating file:
                walkingSpeed = 3;
                runningSpeed = 5;
                if(Directory.Exists("C:/pirate-queen") != true)
                {
                    Directory.CreateDirectory("C:/pirate-queen");
                    Stream stream = File.OpenWrite("C:/pirate-queen/settings.dat");
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Close();
                    stream.Close();
                }
            }

            // Set elements:
            sliderWalkingSpeed.Value = walkingSpeed - 1;
            lblWalkingSpeedData.Text = walkingSpeed.ToString();
            sliderRunningSpeed.Value = runningSpeed - 1;
            lblRunningSpeedData.Text = runningSpeed.ToString();
        }

        // Write the settings to a binary file:
        private void SaveData ()
        {
            // Open the file:
            Directory.CreateDirectory("C:/pirate-queen");
            BinaryWriter writer = new BinaryWriter(File.OpenWrite("C:/pirate-queen/settings.dat"));

            // Fill file with data:
            writer.Write(walkingSpeed); //int
            writer.Write(runningSpeed); //int

            // Close the file:
            writer.Close();
        }
    }
}
