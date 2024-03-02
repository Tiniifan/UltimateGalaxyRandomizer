using System.Windows.Forms;
using System.Collections.Generic;

namespace UltimateGalaxyRandomizer.Randomizer.Utility
{
    public class Option
    {
        public string Name { get; set; }

        public Dictionary<string, CheckBox> CheckBoxes { get; set; }

        public Dictionary<string, NumericUpDown> NumericUpDowns { get; set; }

        public Option(List<RadioButton> radioButtons)
        {
            List<string> names = new List<string>();
            int index = 0;

            switch (radioButtons.Count)
            {
                case 0:
                    names = new List<string>() { "NoRadioButton"};
                    break;
                case 2:
                    names = new List<string>() { "Unchanged", "Random" };
                    index = radioButtons.FindIndex(x => x.Checked);
                    break;
                default:
                    names = new List<string>() { "Unchanged", "Swap", "Random" };
                    index = radioButtons.FindIndex(x => x.Checked);
                    break;
            }

            Name = names[index];
        }
    }
}
