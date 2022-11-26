using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using UltimateGalaxyRandomizer.Logic;
using UltimateGalaxyRandomizer.Resources;
using UltimateGalaxyRandomizer.Randomizer;
using UltimateGalaxyRandomizer.Randomizer.Utility;

namespace UltimateGalaxyRandomizer
{
    public partial class RandomizerWindow : Form
    {
        Galaxy Game = null;

        public RandomizerWindow()
        {
            InitializeComponent();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
            {
                // Check If It's Valid Path
                if (Directory.Exists(ofd.SelectedPath + "/ie6_a_fa/gds_pack_decomp_pck"))
                {
                    Game = new Galaxy(ofd.SelectedPath);
                }
                else
                {
                    Game = null;
                    MessageBox.Show("Unrecognized Game Folder");
                }
            }

            if (Game != null)
            {
                tabControl1.Enabled = true;
                randomizeSaveToolStripMenuItem.Enabled = true;
            }
            else
            {
                tabControl1.Enabled = false;
                randomizeSaveToolStripMenuItem.Enabled = false;
            }
        }

        private Option GroupBoxToRandomizerOption(GroupBox groupBox)
        {
            Option randomizerOption = new Option(groupBox.Controls.OfType<RadioButton>().OrderBy(x => x.Name).ToList());
            randomizerOption.CheckBoxes = groupBox.Controls.OfType<CheckBox>().ToDictionary(x => x.Name, x => x);
            randomizerOption.NumericUpDowns = groupBox.Controls.OfType<NumericUpDown>().ToDictionary(x => x.Name, x => x);

            return randomizerOption;
        }

        private Dictionary<string, Option> TabControlToDictOption(TabControl tabControl)
        {
            Dictionary<string, Option> options = new Dictionary<string, Option>();

            foreach (Control control in tabControl.Controls)
            {
                if (control is TabPage)
                {
                    foreach (Control subControl in control.Controls)
                    {
                        if (subControl is GroupBox)
                        {
                            options.Add(subControl.Name, GroupBoxToRandomizerOption(subControl as GroupBox));
                        }
                    }
                }
            }

            return options;
        }

        private void RandomizeSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Game.RandomizeAvatars(TabControlToDictOption(tabControl4));
            Game.RandomizeMoves(TabControlToDictOption(tabControl3));
            Game.RandomizePlayers(TabControlToDictOption(tabControl2));
            Game.RandomizeTeams(TabControlToDictOption(tabControl5));
            Game.Miscellaneous(TabControlToDictOption(tabControl1));

            MessageBox.Show("Done!");
        }

        private void Option_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            GroupBox groupbox = radioButton.Parent as GroupBox;

            foreach (NumericUpDown numericUpDown in groupbox.Controls.OfType<NumericUpDown>())
            {
                numericUpDown.Enabled = radioButton.Checked;
            }

            foreach (CheckBox checkBox in groupbox.Controls.OfType<CheckBox>())
            {
                if (radioButton.Checked == false)
                {
                    checkBox.Checked = false;
                }

                checkBox.Enabled = radioButton.Checked;
            }
        }

        private void InvokerProbabilityChange(object sender, EventArgs e)
        {
            NumericUpDown numericUpDownFocused = sender as NumericUpDown;

            if (!numericUpDownFocused.Focused) return;        

            List<NumericUpDown> numericUpDowns = new List<NumericUpDown>(){ numericUpDownFightingSpirit, numericUpDownTotem, numericUpDownNoneInvoker };
            numericUpDowns.Remove(numericUpDownFocused);

            decimal sum = numericUpDownFocused.Value;
            for (int i = 0; i < numericUpDowns.Count; i++)
            {   
                if (sum + numericUpDowns[i].Value > 100)
                {
                    numericUpDowns[i].Value = 100 - sum;
                } 
                else if (i == 1)
                {
                    if (sum + numericUpDowns[i].Value < 100)
                    {
                        numericUpDowns[i].Value = 100 - sum;
                    }
                }

                sum += numericUpDowns[i].Value;
            }
        }
    }
}
