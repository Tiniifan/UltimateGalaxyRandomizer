using System;
using System.Linq;
using UltimateGalaxyRandomizer.Logic;
using System.Collections.Generic;
using UltimateGalaxyRandomizer.Resources;
using UltimateGalaxyRandomizer.Randomizer.Utility;

namespace UltimateGalaxyRandomizer.Randomizer
{
    public static class Randomizer
    {
        public static RandomNumber Seed = new RandomNumber();

        private static void SwapPlayers(Dictionary<Player, bool> players, Dictionary<string, Option> options)
        {
            if (options["groupBoxSwapPlayer"].Name == "Random")
            {
                // Create Tempory Player Dic
                Dictionary<Player, bool> tempPlayers = players.ToDictionary(x => x.Key.Clone(), x => x.Value);

                foreach (KeyValuePair<Player, bool> player in players)
                {
                    Dictionary<Player, bool> possiblePlayers = new Dictionary<Player, bool>();

                    // Same Body
                    if (options["groupBoxSwapPlayer"].CheckBoxes["checkBoxSwapSameBody"].Checked == true)
                    {
                        possiblePlayers = tempPlayers.Where(x => x.Key.Base.Size >> 4 == player.Key.Base.Size >> 4).ToDictionary(x => x.Key, x => x.Value);
                    }
                    else
                    {
                        possiblePlayers = tempPlayers;
                    }

                    // Same Position
                    if (options["groupBoxSwapPlayer"].CheckBoxes["checkBoxSwapSamePosition"].Checked == true)
                    {
                        possiblePlayers = possiblePlayers.Where(x => x.Key.Param.Position == player.Key.Param.Position).ToDictionary(x => x.Key, x => x.Value);
                    }

                    // Important Character -> Important Character and Scout -> Scout
                    if (options["groupBoxSwapPlayer"].CheckBoxes["checkboxSwapFocus"].Checked == true)
                    {
                        possiblePlayers = possiblePlayers.Where(x => x.Value == player.Value).ToDictionary(x => x.Key, x => x.Value);
                    }

                    Player randomPlayer = possiblePlayers.ElementAt(Seed.Next(0, possiblePlayers.Count)).Key;
                    player.Key.Base.Swap(randomPlayer.Base);
                    player.Key.Param.Swap(randomPlayer.Param);
                    player.Key.Skills = randomPlayer.Skills;
                    tempPlayers.Remove(randomPlayer);
                }
            }
        }

        public static void RandomizePlayers(Dictionary<string, Option> options)
        {
            // Merge Player Dictionaries
            Dictionary<Player, bool> players = new Dictionary<Player, bool>();
            Players.Story.Values.ToList().ForEach(x => players.Add(x, false));
            Players.Normal.Values.ToList().ForEach(x => players.Add(x, false));
            Players.Scout.Values.ToList().ForEach(x => players.Add(x, true));

            // Do Swap Player First
            SwapPlayers(players, options);

            // Initialise List ID
            List<UInt32> names = players.Keys.Select(x => x.Base.NameID).ToList();
            List<UInt32> nicknames = players.Keys.Select(x => x.Base.NicknameID).ToList();
            List<UInt32> descriptions = players.Keys.Select(x => x.Base.DescriptionID).ToList();

            // Randomize Each Player
            foreach (Player player in players.Keys)
            {
                if (options["groupBoxName"].Name == "Random")
                {
                    int randomIndex = Seed.Next(0, names.Count);
                    player.Base.NameID = names[randomIndex];
                    names.RemoveAt(randomIndex);
                }

                if (options["groupBoxNickname"].Name == "Random")
                {
                    int randomIndex = Seed.Next(0, nicknames.Count);
                    player.Base.NicknameID = nicknames[randomIndex];
                    nicknames.RemoveAt(randomIndex);
                }

                if (options["groupBoxDescription"].Name == "Random")
                {
                    int randomIndex = Seed.Next(0, descriptions.Count);
                    player.Base.DescriptionID = descriptions[randomIndex];
                    descriptions.RemoveAt(randomIndex);
                }

                if (options["groupBoxBody"].Name == "Random")
                {
                    player.Base.Size = (byte)((Seed.Next(1, 9) << 4) | (player.Base.Size & 0x0F));
                }

                if (options["groupBoxYear"].Name == "Random")
                {
                    player.Base.Identity = (byte)((Seed.Next(0, 6) << 4) | (player.Base.Identity & 0x0F));
                }

                if (options["groupBoxGender"].Name == "Random")
                {
                    player.Base.Identity = (byte)((player.Base.Identity << 4) | (Seed.Next(1, 4) & 0x0F));
                }

                if (options["groupBoxStyle"].Name == "Random")
                {
                    player.Base.Style = (byte)Seed.Next(0, 6);
                }

                if (options["groupBoxElement"].Name == "Random")
                {
                    player.Param.Element = (byte)Seed.Next(1, 5 + Convert.ToInt32(options["groupBoxElement"].CheckBoxes["checkBoxElementAllowVoid"].Checked));
                }

                if (options["groupBoxPosition"].Name == "Random")
                {
                    player.Param.Position = (byte)Seed.Next(1, 4);
                }

                if (options["groupBoxBaseStats"].Name == "Swap")
                {
                    // Shuffle
                    List<int> tempStat = player.Param.BaseStat.Values.Select(x => x.Value).ToList();
                    for (int s = 0; s < player.Param.BaseStat.Values.Count; s++)
                    {
                        int getRandomIndex = Seed.Next(0, tempStat.Count);
                        player.Param.BaseStat.Values[player.Param.BaseStat.Values.ElementAt(s).Key] = tempStat[getRandomIndex];
                    }
                }
                else if (options["groupBoxBaseStats"].Name == "Random")
                {
                    // Generate Random Base Stat
                    Position playerPosition = Positions.Player[player.Param.Position];
                    Element playerElement = Elements.Values[player.Param.Element];
                    Gender playerGender = Identity.Genders[(byte)(player.Base.Identity & 0x0F)];

                    for (int s = 0; s < player.Param.BaseStat.Values.Count; s++)
                    {
                        int baseStat = Seed.Next(24, 45);
                        int finalStat = (playerPosition.StatBuff[s] * baseStat / 100) + (playerElement.StatBuff[s] * baseStat / 100) + (playerGender.StatBuff[s] * baseStat / 100) + baseStat;
                        player.Param.BaseStat.Values[player.Param.BaseStat.Values.ElementAt(s).Key] = finalStat;
                    }
                }

                if (options["groupBoxGrownStats"].Name == "Random")
                {
                    int[] grownStats = new int[5] { 0, 1, 2, 254, 255 };
                    for (int s = 0; s < player.Param.BaseStat.Values.Count; s++)
                    {
                        player.Param.GrownStat.Values[player.Param.BaseStat.Values.ElementAt(s).Key] = grownStats[Seed.Next(0, grownStats.Count())];
                    }
                }

                if (options["groupBoxFreedom"].Name == "Random")
                {
                    player.Param.Freedom = Convert.ToInt16(Seed.Next(20, 41) * 10);
                }

                if (options["groupBoxMoveset"].Name == "Random")
                {
                    // Get Position and Element Type Probability
                    List<UInt32> moveset = player.GetRandomMoveset(player.Param.SkillCount);

                    for (int s = 0; s < player.Param.SkillCount; s++)
                    {
                        player.Skills[s].SkillID = moveset[s];
                    }
                }

                if (options["groupBoxAvatar"].Name == "Random")
                {
                    Invoke invoke = Invokes.Values[player.Param.Invoke];

                    UInt32 avatarID = 0x00;

                    if (invoke.CanInvoke == true)
                    {
                        int[] invokerProbability = new int[3] {
                        Convert.ToInt32(options["groupBoxInvokerUser"].NumericUpDowns["numericUpDownFightingSpirit"].Value),
                        Convert.ToInt32(options["groupBoxInvokerUser"].NumericUpDowns["numericUpDownTotem"].Value),
                        Convert.ToInt32(options["groupBoxInvokerUser"].NumericUpDowns["numericUpDownNoneInvoker"].Value)
                        };

                        int invokerStatus = new Probability(invokerProbability).GetRandomIndex();
                        switch (invokerStatus)
                        {
                            case 0:
                                avatarID = player.GetRandomFightingSpirit();
                                break;
                            case 1:
                                avatarID = player.GetRandomTotem();
                                break;
                            case 2:
                                avatarID = 0x00;
                                break;
                        }
                    }

                    player.Param.Avatar = avatarID;
                }

                if (options["groupBoxInvokerUser"].Name == "Random")
                {
                    int[] invokerProbability = new int[3] {
                        Convert.ToInt32(options["groupBoxInvokerUser"].NumericUpDowns["numericUpDownFightingSpirit"].Value),
                        Convert.ToInt32(options["groupBoxInvokerUser"].NumericUpDowns["numericUpDownTotem"].Value),
                        Convert.ToInt32(options["groupBoxInvokerUser"].NumericUpDowns["numericUpDownNoneInvoker"].Value)
                    };

                    // Get random invoker status according to user invoker probability
                    int invokerStatus = new Probability(invokerProbability).GetRandomIndex();
                    switch (invokerStatus)
                    {
                        case 0:
                            player.Param.Invoke = 0x04;
                            player.Param.Avatar = player.GetRandomFightingSpirit();
                            break;
                        case 1:
                            player.Param.Invoke = 0x44;
                            player.Param.Avatar = player.GetRandomTotem();
                            break;
                        case 2:
                            player.Param.Invoke = 0x00;
                            player.Param.Avatar = 0x00;
                            break;
                    }
                }

                if (options["groupBoxArmoufiedUser"].Name == "Random")
                {
                    Invoke invoke = Invokes.Values[player.Param.Invoke];

                    if (invoke.CanInvoke == true && invoke.IsFightingSpirit == false)
                    {
                        int armourflyPercentage = Convert.ToInt32(options["groupBoxArmoufiedUser"].NumericUpDowns["numericUpDownArmoufly"].Value);
                        Probability armoufly = new Probability(new int[2] { 100 - armourflyPercentage, armourflyPercentage });
                        switch (armoufly.GetRandomIndex())
                        {
                            case 0:
                                player.Param.Invoke = 0x04;
                                break;
                            case 1:
                                player.Param.Invoke = 0x0C;
                                break;
                        }
                    }
                }

                if (options["groupBoxExperienceBar"].Name == "Random")
                {
                    player.Param.ExperienceBar = Convert.ToByte(Seed.Next(1, 8));
                }
            }
        }
    }
}
