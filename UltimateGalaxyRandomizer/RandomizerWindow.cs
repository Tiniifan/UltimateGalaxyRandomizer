using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
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
            var ofd = new FolderBrowserDialog();
            var result = ofd.ShowDialog();

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

        private static Option GroupBoxToRandomizerOption(GroupBox groupBox) => new([.. groupBox.Controls.OfType<RadioButton>().OrderBy(x => x.Name)])
        {
            CheckBoxes = groupBox.Controls.OfType<CheckBox>().ToDictionary(x => x.Name, x => x),
            NumericUpDowns = groupBox.Controls.OfType<NumericUpDown>().ToDictionary(x => x.Name, x => x)
        };

        private static Dictionary<string, Option> TabControlToDictOption(TabControl tabControl)
        {
            var options = new Dictionary<string, Option>();

            foreach (Control control in tabControl.Controls)
            {
                if (control is not TabPage) continue;

                foreach (Control subControl in control.Controls)
                {
                    if (subControl is GroupBox box)
                    {
                        options.Add(box.Name, GroupBoxToRandomizerOption(box));
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
            if (sender is not RadioButton { Parent: GroupBox groupbox } radioButton) return;

            foreach (NumericUpDown numericUpDown in groupbox.Controls.OfType<NumericUpDown>())
            {
                numericUpDown.Enabled = radioButton.Checked;
            }

            foreach (CheckBox checkBox in groupbox.Controls.OfType<CheckBox>())
            {
                if (!radioButton.Checked)
                {
                    checkBox.Checked = false;
                }

                checkBox.Enabled = radioButton.Checked;
            }
        }

        private void InvokerProbabilityChange(object sender, EventArgs e)
        {
            if (sender is not NumericUpDown { Focused: true } numericUpDownFocused) return;

            var numericUpDowns = new List<NumericUpDown> { numericUpDownFightingSpirit, numericUpDownTotem, numericUpDownNoneInvoker };
            numericUpDowns.Remove(numericUpDownFocused);

            decimal sum = numericUpDownFocused.Value;
            for (int i = 0; i < numericUpDowns.Count; i++)
            {   
                if (sum + numericUpDowns[i].Value > 100)
                {
                    numericUpDowns[i].Value = 100 - sum;
                } 
                else if (i == 1 && sum + numericUpDowns[i].Value < 100)
                {
                    numericUpDowns[i].Value = 100 - sum;
                }

                sum += numericUpDowns[i].Value;
            }
        }
    }
}
