﻿using System;
using System.Collections.Generic;
using UltimateGalaxyRandomizer.Logic;

namespace UltimateGalaxyRandomizer.Resources
{
    public static class Moves
    {
        public static Dictionary<uint, Move> PlayerMoves = new Dictionary<uint, Move>()
        {
            {0xB4AE0D0B, new Move("Fire Tornado")},
            {0x2DA75CB1, new Move("Eternal Blizzard")},
            {0x5AA06C27, new Move("Zephyr Shot")},
            {0xC4C4F984, new Move("Doomsword Slash")},
            {0xB3C3C912, new Move("Emperor Penguin nº 2")},
            {0x2ACA98A8, new Move("Emperor Penguin nº 7")},
            {0x5DCDA83E, new Move("Jumping Jack")},
            {0xCD72B5AF, new Move("Katana Kick")},
            {0xBA758539, new Move("Bouncing Bomb")},
            {0xDAB20CDC, new Move("Headbanger")},
            {0xADB53C4A, new Move("Warhead")},
            {0x34BC6DF0, new Move("Ballista Barrage")},
            {0x43BB5D66, new Move("Triangle ZZ")},
            {0xDDDFC8C5, new Move("Optimal Trajectory")},
            {0xAAD8F853, new Move("Fortissimo Foot")},
            {0x33D1A9E9, new Move("Doom Dive Drive")},
            {0x44D6997F, new Move("Will-o'-the-Wisp Shot")},
            {0xD46984EE, new Move("Dinosaur Roar")},
            {0xA36EB478, new Move("Sonic Shot")},
            {0xF19F5F1F, new Move("Entropic Arrows")},
            {0x86986F89, new Move("Brimstone Rain")},
            {0x1F913E33, new Move("Flying Fish")},
            {0x68960EA5, new Move("Sidewinder")},
            {0xF6F29B06, new Move("Supernatural Strike")},
            {0x81F5AB90, new Move("White Hurricane")},
            {0x18FCFA2A, new Move("Zero Magnum")},
            {0x6FFBCABC, new Move("Dark Energy Star")},
            {0xFF44D72D, new Move("2DD744FF")},
            {0x8843E7BB, new Move("BBE74388")},
            {0xE8846E5E, new Move("5E6E84E8")},
            {0x9F835EC8, new Move("C85E839F")},
            {0x068A0F72, new Move("720F8A06")},
            {0x718D3FE4, new Move("E43F8D71")},
            {0xEFE9AA47, new Move("47AAE9EF")},
            {0x98EE9AD1, new Move("D19AEE98")},
            {0x01E7CB6B, new Move("6BCBE701")},
            {0x76E0FBFD, new Move("FDFBE076")},
            {0xE65FE66C, new Move("6CE65FE6")},
            {0x9158D6FA, new Move("FAD65891")},
            {0xA7C5F899, new Move("99F8C5A7")},
            {0xD0C2C80F, new Move("0FC8C2D0")},
            {0x49CB99B5, new Move("B599CB49")},
            {0x3ECCA923, new Move("23A9CC3E")},
            {0xA0A83C80, new Move("803CA8A0")},
            {0xD7AF0C16, new Move("160CAFD7")},
            {0x4EA65DAC, new Move("Asteride")},
            {0x39A16D3A, new Move("3A6DA139")},
            {0xA91E70AB, new Move("AB701EA9")},
            {0xDE19403D, new Move("3D4019DE")},
            {0xBEDEC9D8, new Move("Gaussian Goal")},
            {0xC9D9F94E, new Move("4EF9D9C9")},
            {0x50D0A8F4, new Move("F4A8D050")},
            {0x27D79862, new Move("6298D727")},
            {0xB9B30DC1, new Move("C10DB3B9")},
            {0xCEB43D57, new Move("573DB4CE")},
            {0x57BD6CED, new Move("ED6CBD57")},
            {0x20BA5C7B, new Move("7B5CBA20")},
            {0xB00541EA, new Move("EA4105B0")},
            {0xC702717C, new Move("7C7102C7")},
            {0x95F39A1B, new Move("1B9AF395")},
            {0xE2F4AA8D, new Move("Accelerator Gears")},
            {0x7BFDFB37, new Move("37FBFD7B")},
            {0x0CFACBA1, new Move("A1CBFA0C")},
            {0x929E5E02, new Move("025E9E92")},
            {0xE5996E94, new Move("946E99E5")},
            {0x7C903F2E, new Move("2E3F907C")},
            {0x0B970FB8, new Move("B80F970B")},
            {0x9B281229, new Move("2912289B")},
            {0xEC2F22BF, new Move("BF222FEC")},
            {0x8CE8AB5A, new Move("5AABE88C")},
            {0xFBEF9BCC, new Move("CC9BEFFB")},
            {0x62E6CA76, new Move("76CAE662")},
            {0x15E1FAE0, new Move("E0FAE115")},
            {0x8B856F43, new Move("436F858B")},
            {0xFC825FD5, new Move("D55F82FC")},
            {0x658B0E6F, new Move("6F0E8B65")},
            {0x128C3EF9, new Move("F93E8C12")},
            {0x82332368, new Move("68233382")},
            {0xF53413FE, new Move("FE1334F5")},
            {0x0B70B795, new Move("95B7700B")},
            {0x7C778703, new Move("0387777C")},
            {0xE57ED6B9, new Move("B9D67EE5")},
            {0x9279E62F, new Move("2FE67992")},
            {0x0C1D738C, new Move("8C731D0C")},
            {0x7B1A431A, new Move("1A431A7B")},
            {0xE21312A0, new Move("A01213E2")},
            {0x95142236, new Move("36221495")},
            {0x05AB3FA7, new Move("A73FAB05")},
            {0x72AC0F31, new Move("310FAC72")},
            {0x126B86D4, new Move("D4866B12")},
            {0x656CB642, new Move("Megalodon")},
            {0xFC65E7F8, new Move("F8E765FC")},
            {0x8B62D76E, new Move("6ED7628B")},
            {0x150642CD, new Move("CD420615")},
            {0x6201725B, new Move("5B720162")},
            {0xFB0823E1, new Move("E12308FB")},
            {0x8C0F1377, new Move("77130F8C")},
            {0x1CB00EE6, new Move("E60EB01C")},
            {0x6BB73E70, new Move("703EB76B")},
            {0xC26B57AA, new Move("AA576BC2")},
            {0xB56C673C, new Move("3C676CB5")},
            {0x2C653686, new Move("8636652C")},
            {0x5B620610, new Move("1006625B")},
            {0xC50693B3, new Move("B39306C5")},
            {0xB201A325, new Move("25A301B2")},
            {0x2B08F29F, new Move("9FF2082B")},
            {0x5C0FC209, new Move("09C20F5C")},
            {0xCCB0DF98, new Move("98DFB0CC")},
            {0xBBB7EF0E, new Move("0EEFB7BB")},
            {0xDB7066EB, new Move("EB6670DB")},
            {0xAC77567D, new Move("7D5677AC")},
            {0x357E07C7, new Move("C7077E35")},
            {0x42793751, new Move("51377942")},
            {0xDC1DA2F2, new Move("F2A21DDC")},
            {0xAB1A9264, new Move("64921AAB")},
            {0x3213C3DE, new Move("DEC31332")},
            {0x4514F348, new Move("48F31445")},
            {0xD5ABEED9, new Move("D9EEABD5")},
            {0xA2ACDE4F, new Move("4FDEACA2")},
            {0xF05D3528, new Move("28355DF0")},
            {0x875A05BE, new Move("BE055A87")},
            {0x1E535404, new Move("0454531E")},
            {0x69546492, new Move("92645469")},
            {0xF730F131, new Move("31F130F7")},
            {0x8037C1A7, new Move("A7C13780")},
            {0x193E901D, new Move("1D903E19")},
            {0xA0409D4F, new Move("4F9D40A0")},
            {0x3454CC3F, new Move("3FCC5434")},
            {0x11BE7788, new Move("8877BE11")},
            {0x88B72632, new Move("3226B788")},
            {0xFFB016A4, new Move("A416B0FF")},
            {0x61D48307, new Move("0783D461")},
            {0x16D3B391, new Move("91B3D316")},
            {0x8FDAE22B, new Move("2BE2DA8F")},
            {0xF8DDD2BD, new Move("BDD2DDF8")},
            {0x6862CF2C, new Move("Future Eye")},
            {0x1F65FFBA, new Move("BAFF651F")},
            {0x7FA2765F, new Move("5F76A27F")},
            {0x08A546C9, new Move("C946A508")},
            {0x91AC1773, new Move("7317AC91")},
            {0xE6AB27E5, new Move("E527ABE6")},
            {0x78CFB246, new Move("46B2CF78")},
            {0x0FC882D0, new Move("D082C80F")},
            {0x96C1D36A, new Move("6AD3C196")},
            {0xE1C6E3FC, new Move("FCE3C6E1")},
            {0x7179FE6D, new Move("6DFE7971")},
            {0x067ECEFB, new Move("FBCE7E06")},
            {0x548F259C, new Move("9C258F54")},
            {0x2388150A, new Move("0A158823")},
            {0xBA8144B0, new Move("B04481BA")},
            {0xCD867426, new Move("267486CD")},
            {0x53E2E185, new Move("85E1E253")},
            {0x24E5D113, new Move("13D1E524")},
            {0xBDEC80A9, new Move("A980ECBD")},
            {0xCAEBB03F, new Move("3FB0EBCA")},
            {0x5A54ADAE, new Move("AEAD545A")},
            {0x2D539D38, new Move("389D532D")},
            {0x4D9414DD, new Move("DD14944D")},
            {0x3A93244B, new Move("4B24933A")},
            {0xA39A75F1, new Move("F1759AA3")},
            {0xD49D4567, new Move("67459DD4")},
            {0x4AF9D0C4, new Move("C4D0F94A")},
            {0x3DFEE052, new Move("52E0FE3D")},
            {0xA4F7B1E8, new Move("E8B1F7A4")},
            {0xD3F0817E, new Move("7E81F0D3")},
            {0x434F9CEF, new Move("EF9C4F43")},
            {0x3448AC79, new Move("Black Tide Ride")},
            {0x02D5821A, new Move("1A82D502")},
            {0x75D2B28C, new Move("8CB2D275")},
            {0xECDBE336, new Move("36E3DBEC")},
            {0x9BDCD3A0, new Move("A0D3DC9B")},
            {0x05B84603, new Move("0346B805")},
            {0x72BF7695, new Move("9576BF72")},
            {0xEBB6272F, new Move("2F27B6EB")},
            {0x9CB117B9, new Move("B917B19C")},
            {0x0C0E0A28, new Move("280A0E0C")},
            {0x7B093ABE, new Move("BE3A097B")},
            {0x1BCEB35B, new Move("5BB3CE1B")},
            {0x6CC983CD, new Move("CD83C96C")},
            {0xF5C0D277, new Move("Menacing Glare")},
            {0x82C7E2E1, new Move("E1E2C782")},
            {0x1CA37742, new Move("4277A31C")},
            {0x6BA447D4, new Move("D447A46B")},
            {0xF2AD166E, new Move("6E16ADF2")},
            {0x85AA26F8, new Move("F826AA85")},
            {0x15153B69, new Move("693B1515")},
            {0x62120BFF, new Move("FF0B1262")},
            {0x30E3E098, new Move("98E0E330")},
            {0x47E4D00E, new Move("0ED0E447")},
            {0xDEED81B4, new Move("B481EDDE")},
            {0xA9EAB122, new Move("22B1EAA9")},
            {0x378E2481, new Move("81248E37")},
            {0x40891417, new Move("17148940")},
            {0xD98045AD, new Move("AD4580D9")},
            {0xAE87753B, new Move("3B7587AE")},
            {0x3E3868AA, new Move("AA68383E")},
            {0x493F583C, new Move("3C583F49")},
            {0x666E4699, new Move("The Wall")},
            {0xFF671723, new Move("Mystifying Mist")},
            {0x886027B5, new Move("B5276088")},
            {0x1604B216, new Move("16B20416")},
            {0x61038280, new Move("80820361")},
            {0xF80AD33A, new Move("3AD30AF8")},
            {0x8F0DE3AC, new Move("ACE30D8F")},
            {0x1FB2FE3D, new Move("Whirly-Whirly")},
            {0x68B5CEAB, new Move("ABCEB568")},
            {0x0872474E, new Move("4E477208")},
            {0x7F7577D8, new Move("D877757F")},
            {0xE67C2662, new Move("62267CE6")},
            {0x917B16F4, new Move("F4167B91")},
            {0x0F1F8357, new Move("57831F0F")},
            {0x7818B3C1, new Move("C1B31878")},
            {0xE111E27B, new Move("7BE211E1")},
            {0x9616D2ED, new Move("EDD21696")},
            {0x06A9CF7C, new Move("7CCFA906")},
            {0x71AEFFEA, new Move("EAFFAE71")},
            {0x235F148D, new Move("8D145F23")},
            {0x5458241B, new Move("1B245854")},
            {0xCD5175A1, new Move("A17551CD")},
            {0xBA564537, new Move("374556BA")},
            {0x2432D094, new Move("94D03224")},
            {0x5335E002, new Move("02E03553")},
            {0xCA3CB1B8, new Move("B8B13CCA")},
            {0xBD3B812E, new Move("2E813BBD")},
            {0x2D849CBF, new Move("BF9C842D")},
            {0x5A83AC29, new Move("29AC835A")},
            {0x3A4425CC, new Move("CC25443A")},
            {0x4D43155A, new Move("5A15434D")},
            {0xD44A44E0, new Move("E0444AD4")},
            {0xA34D7476, new Move("76744DA3")},
            {0x3D29E1D5, new Move("D5E1293D")},
            {0x4A2ED143, new Move("43D12E4A")},
            {0xD32780F9, new Move("F98027D3")},
            {0xA420B06F, new Move("6FB020A4")},
            {0x349FADFE, new Move("FEAD9F34")},
            {0x43989D68, new Move("689D9843")},
            {0x7505B30B, new Move("0BB30575")},
            {0x0202839D, new Move("9D830202")},
            {0x9B0BD227, new Move("27D20B9B")},
            {0xEC0CE2B1, new Move("B1E20CEC")},
            {0x72687712, new Move("12776872")},
            {0x056F4784, new Move("84476F05")},
            {0x9C66163E, new Move("3E16669C")},
            {0xEB6126A8, new Move("A82661EB")},
            {0x7BDE3B39, new Move("393BDE7B")},
            {0x0CD90BAF, new Move("AF0BD90C")},
            {0x6C1E824A, new Move("4A821E6C")},
            {0x1B19B2DC, new Move("DCB2191B")},
            {0x8210E366, new Move("Fancy-Footwork")},
            {0xF517D3F0, new Move("F0D317F5")},
            {0x6B734653, new Move("5346736B")},
            {0x1C7476C5, new Move("C576741C")},
            {0x857D277F, new Move("7F277D85")},
            {0xF27A17E9, new Move("E9177AF2")},
            {0x62C50A78, new Move("780AC562")},
            {0x15C23AEE, new Move("EE3AC215")},
            {0x4733D189, new Move("89D13347")},
            {0x3034E11F, new Move("1FE13430")},
            {0xA93DB0A5, new Move("A5B03DA9")},
            {0xDE3A8033, new Move("33803ADE")},
            {0x405E1590, new Move("90155E40")},
            {0x37592506, new Move("Gravel Gavel")},
            {0xAE5074BC, new Move("BC7450AE")},
            {0xD957442A, new Move("2A4457D9")},
            {0x49E859BB, new Move("BB59E849")},
            {0x3EEF692D, new Move("2D69EF3E")},
            {0xE43ED148, new Move("Anchors Aweigh")},
            {0x7D3780F2, new Move("Fingers of Gaia")},
            {0x0A30B064, new Move("Combustion Catch")},
            {0x945425C7, new Move("Close Counter")},
            {0xE3531551, new Move("Whip Crack")},
            {0x7A5A44EB, new Move("Crystal Barrier")},
            {0x0D5D747D, new Move("Shadow Catch")},
            {0x9DE269EC, new Move("Somersault Stamp")},
            {0xEAE5597A, new Move("Ultrasonic")},
            {0x8A22D09F, new Move("Power Spike")},
            {0xFD25E009, new Move("Shot Stopper")},
            {0x642CB1B3, new Move("God Hand")},
            {0x132B8125, new Move("God Hand V")},
            {0x8D4F1486, new Move("All-Star")},
            {0xFA482410, new Move("Snakebite")},
            {0x634175AA, new Move("Capable Hands")},
            {0x1446453C, new Move("Bridge to Nowhere")},
            {0x84F958AD, new Move("Gravity Point")},
            {0xF3FE683B, new Move("Stomper Stopper")},
            {0xA10F835C, new Move("Breath Taker")},
            {0xD608B3CA, new Move("White Hole")},
            {0x4F01E270, new Move("Flower Power")},
            {0x3806D2E6, new Move("Negative Feedback")},
            {0xA6624745, new Move("Gyro-Blocker")},
            {0xD16577D3, new Move("Wormhole")},
            {0x486C2669, new Move("Shot Pocket")},
            {0x3F6B16FF, new Move("Psychlone")},
            {0xAFD40B6E, new Move("Mugen The Hand")},
            {0xD8D33BF8, new Move("Fist Beam")},
            {0xB814B21D, new Move("God Hand X")},
            {0xCF13828B, new Move("Rotary Sander")},
            {0x561AD331, new Move("Power Shield")},
            {0x211DE3A7, new Move("Jumping Whack")},
            {0xBF797604, new Move("Dust Devil")},
            {0xC87E4692, new Move("Double God Hand")},
            {0x51771728, new Move("Song of the Three Kingdoms")},
            {0x267027BE, new Move("Crescent Cross")},
            {0xB6CF3A2F, new Move("Penguin the Hand")},
            {0xC1C80AB9, new Move("Rejection")},
            {0xF75524DA, new Move("Gigant Wall")},
            {0x8052144C, new Move("Utter Gutsiness Catch")},
            {0x195B45F6, new Move("Nitro Slap")},
            {0x6E5C7560, new Move("Capoeira Grab")},
            {0xF038E0C3, new Move("Shot Trap")},
            {0x873FD055, new Move("Blam Dunk")},
            {0x1E3681EF, new Move("Twist'n'Clout")},
            {0x6931B179, new Move("Crocodile Jaws")},
            {0xF98EACE8, new Move("Dry Blow")},
            {0x8E899C7E, new Move("Menace Elbow")},
            {0xEE4E159B, new Move("Sand Blaster")},
            {0x9949250D, new Move("Spout it Out")},
            {0x004074B7, new Move("Volcanic Header")},
            {0x77474421, new Move("Venus Balltrap")},
            {0xE923D182, new Move("Bounce Handler")},
            {0x9E24E114, new Move("Claw de Force")},
            {0x072DB0AE, new Move("Shot Devourer")},
            {0x702A8038, new Move("Portcullis Guard")},
            {0xE0959DA9, new Move("Sacrificial Switch")},
            {0x9792AD3F, new Move("We Have Liftoff!")},
            {0xC5634658, new Move("Dragon Rage Dunk")},
                        {0xF30E77DB, new Move("Catch Boost")},
            {0x6A072661, new Move("6126076A")},
            {0x1D0016F7, new Move("F716001D")},
            {0x83648354, new Move("54836483")},
            {0xF463B3C2, new Move("C2B363F4")},
            {0x6D6AE278, new Move("78E26A6D")},
            {0x1A6DD2EE, new Move("EED26D1A")},
            {0x8AD2CF7F, new Move("7FCFD28A")},
            {0xFDD5FFE9, new Move("E9FFD5FD")},
            {0x9D12760C, new Move("0C76129D")},
            {0xEA15469A, new Move("9A4615EA")},
            {0x731C1720, new Move("20171C73")},
            {0x041B27B6, new Move("B6271B04")},
            {0x9A7FB215, new Move("15B27F9A")},
            {0xED788283, new Move("Luck Boost Plus")},
            {0x7471D339, new Move("39D37174")},
            {0x0376E3AF, new Move("AFE37603")},
            {0x93C9FE3E, new Move("FP Boost Plus")},
            {0xE4CECEA8, new Move("A8CECEE4")},
            {0xB63F25CF, new Move("CF253FB6")},
            {0x583144E3, new Move("E3443158")},
            {0x2F367475, new Move("7574362F")},
            {0xB152E1D6, new Move("D6E152B1")},
            {0xC655D140, new Move("40D155C6")},
            {0xB8E4ADFD, new Move("FDADE4B8")},
            {0xD8232418, new Move("Void Boost")},
            {0x412A75A2, new Move("A2752A41")},
            {0x362D4534, new Move("34452D36")},
            {0xA849D097, new Move("Fire Boost!")},
            {0xDF4EE001, new Move("01E04EDF")},
            {0x4647B1BB, new Move("BBB14746")},
            {0x3140812D, new Move("2D814031")},
            {0xA1FF9CBC, new Move("BC9CFFA1")},
            {0xD6F8AC2A, new Move("Fair play!")},
            {0xE0658249, new Move("498265E0")},
            {0x9762B2DF, new Move("DFB26297")},
            {0x0E6BE365, new Move("65E36B0E")},
            {0x796CD3F3, new Move("F3D36C79")},
            {0xE7084650, new Move("504608E7")},
            {0x900F76C6, new Move("C6760F90")},
            {0x0906277C, new Move("7C270609")},
            {0x7E0117EA, new Move("EA17017E")},
            {0xEEBE0A7B, new Move("7B0ABEEE")},
            {0x99B93AED, new Move("ED3AB999")},
            {0xF97EB308, new Move("08B37EF9")},
            {0x8E79839E, new Move("9E83798E")},
            {0x1770D224, new Move("24D27017")},
            {0x6077E2B2, new Move("B2E27760")},
            {0xFE137711, new Move("117713FE")},
            {0x89144787, new Move("87471489")},
            {0x101D163D, new Move("3D161D10")},
            {0x671A26AB, new Move("Pronto Pass")},
            {0xF7A53B3A, new Move("3A3BA5F7")},
            {0x80A20BAC, new Move("AC0BA280")},
            {0xD253E0CB, new Move("Heavy Hurl")},
            {0xA554D05D, new Move("5DD054A5")},
            {0x3C5D81E7, new Move("E7815D3C")},
            {0x4B5AB171, new Move("Shock Absorber")},
            {0xD53E24D2, new Move("D2243ED5")},
            {0x4C377568, new Move("6875374C")},
            {0xDC8868F9, new Move("F96888DC")},
            {0xAB8F586F, new Move("6F588FAB")},
            {0xCB48D18A, new Move("8AD148CB")},
            {0xBC4FE11C, new Move("Blood Sweat and Tears!")},
            {0x52418030, new Move("30804152")},
            {0xCC251593, new Move("931525CC")},
            {0xBB222505, new Move("052522BB")},
            {0x222B74BF, new Move("BF742B22")},
            {0xC59359B8, new Move("B85993C5")},
            {0xB294692E, new Move("2E6994B2")},
            {0x4CD0CD45, new Move("45CDD04C")},
            {0x3BD7FDD3, new Move("D3FDD73B")},
            {0xA2DEAC69, new Move("69ACDEA2")},
            {0xD5D99CFF, new Move("FF9CD9D5")},
            {0x4BBD095C, new Move("5C09BD4B")},
            {0x3CBA39CA, new Move("CA39BA3C")},
            {0xA5B36870, new Move("7068B3A5")},
            {0xD2B458E6, new Move("E658B4D2")},
        };

        public static Dictionary<uint, Move> FightingSpiritMoves = new Dictionary<uint, Move>()
        {
            {0x323A7FA5, new Move("Fireball Storm")},
            {0xAB332E1F, new Move("1F2E33AB")},
            {0xDC341E89, new Move("891E34DC")},
            {0x42508B2A, new Move("2A8B5042")},
            {0x3557BBBC, new Move("BCBB5735")},
            {0xAC5EEA06, new Move("06EA5EAC")},
            {0xDB59DA90, new Move("90DA59DB")},
            {0x4BE6C701, new Move("01C7E64B")},
            {0x3CE1F797, new Move("97F7E13C")},
            {0x5C267E72, new Move("727E265C")},
            {0x2B214EE4, new Move("E44E212B")},
            {0xB2281F5E, new Move("5E1F28B2")},
            {0xC52F2FC8, new Move("C82F2FC5")},
            {0x5B4BBA6B, new Move("6BBA4B5B")},
            {0x2C4C8AFD, new Move("FD8A4C2C")},
            {0xB545DB47, new Move("47DB45B5")},
            {0xC242EBD1, new Move("D1EB42C2")},
            {0x52FDF640, new Move("40F6FD52")},
            {0x25FAC6D6, new Move("D6C6FA25")},
            {0x770B2DB1, new Move("B12D0B77")},
            {0x000C1D27, new Move("271D0C00")},
            {0x99054C9D, new Move("9D4C0599")},
            {0xEE027C0B, new Move("0B7C02EE")},
            {0x7066E9A8, new Move("A8E96670")},
            {0x0761D93E, new Move("3ED96107")},
            {0x9E688884, new Move("8488689E")},
            {0x972A0526, new Move("26052A97")},
            {0x0E23549C, new Move("9C54230E")},
            {0x7924640A, new Move("0A642479")},
            {0xE740F1A9, new Move("A9F140E7")},
            {0x9047C13F, new Move("3FC14790")},
            {0x094E9085, new Move("85904E09")},
            {0x7E49A013, new Move("13A0497E")},
            {0xEEF6BD82, new Move("82BDF6EE")},
            {0x99F18D14, new Move("148DF199")},
            {0xF93604F1, new Move("F10436F9")},
            {0x8E313467, new Move("6734318E")},
            {0xE0FA3437, new Move("3734FAE0")},
            {0x79F3658D, new Move("8D65F379")},
            {0x0EF4551B, new Move("1B55F40E")},
            {0x9090C0B8, new Move("B8C09090")},
            {0xE797F02E, new Move("2EF097E7")},
            {0x7E9EA194, new Move("94A19E7E")},
            {0x09999102, new Move("02919909")},
            {0x99268C93, new Move("938C2699")},
            {0xEE21BC05, new Move("05BC21EE")},
            {0x8EE635E0, new Move("E035E68E")},
            {0xF9E10576, new Move("7605E1F9")},
            {0x60E854CC, new Move("CC54E860")},
            {0x62AAA3E6, new Move("E6A3AA62")},
            {0xFBA3F25C, new Move("5CF2A3FB")},
            {0x8CA4C2CA, new Move("CAC2A48C")},
            {0x12C05769, new Move("6957C012")},
            {0x65C767FF, new Move("FF67C765")},
            {0xFCCE3645, new Move("4536CEFC")},
            {0x8BC906D3, new Move("D306C98B")},
            {0x1B761B42, new Move("421B761B")},
            {0x6C712BD4, new Move("D42B716C")},
            {0x0CB6A231, new Move("31A2B60C")},
            {0x7BB192A7, new Move("A792B17B")},
            {0xE2B8C31D, new Move("Superior Crayon Power")},
            {0x759A0575, new Move("Omnivitesse")},
            {0xEC9354CF, new Move("CF5493EC")},
            {0x9B946459, new Move("5964949B")},
            {0x05F0F1FA, new Move("FAF1F005")},
            {0x72F7C16C, new Move("6CC1F772")},
            {0xEBFE90D6, new Move("D690FEEB")},
            {0x9CF9A040, new Move("40A0F99C")},
            {0x0C46BDD1, new Move("D1BD460C")},
            {0x7B418D47, new Move("478D417B")},
            {0x1B8604A2, new Move("A204861B")},
            {0x6C813434, new Move("3434816C")},
            {0xF588658E, new Move("8E6588F5")},
            {0x828F5518, new Move("18558F82")},
            {0x1CEBC0BB, new Move("BBC0EB1C")},
            {0x6BECF02D, new Move("2DF0EC6B")},
            {0xF2E5A197, new Move("97A1E5F2")},
            {0x85E29101, new Move("0191E285")},
            {0x155D8C90, new Move("908C5D15")},
            {0x625ABC06, new Move("06BC5A62")},
            {0x30AB5761, new Move("6157AB30")},
            {0x47AC67F7, new Move("F767AC47")},
            {0xDEA5364D, new Move("4D36A5DE")},
            {0xA9A206DB, new Move("DB06A2A9")},
            {0x37C69378, new Move("7893C637")},
            {0x40C1A3EE, new Move("EEA3C140")},
            {0xD9C8F254, new Move("54F2C8D9")},
            {0xAECFC2C2, new Move("C2C2CFAE")},
            {0x3E70DF53, new Move("53DF703E")},
            {0x4977EFC5, new Move("C5EF7749")},
            {0x29B06620, new Move("2066B029")},
            {0x5EB756B6, new Move("B656B75E")},
            {0xC7BE070C, new Move("0C07BEC7")},
            {0xB0B9379A, new Move("9A37B9B0")},
            {0x2EDDA239, new Move("39A2DD2E")},
            {0x59DA92AF, new Move("AF92DA59")},
            {0xC0D3C315, new Move("15C3D3C0")},
            {0xB7D4F383, new Move("83F3D4B7")},
            {0x276BEE12, new Move("12EE6B27")},
            {0x506CDE84, new Move("84DE6C50")},
            {0x66F1F0E7, new Move("E7F0F166")},
            {0x11F6C071, new Move("71C0F611")},
            {0x88FF91CB, new Move("CB91FF88")},
            {0xFFF8A15D, new Move("5DA1F8FF")},
            {0x619C34FE, new Move("FE349C61")},
            {0x169B0468, new Move("68049B16")},
            {0x8F9255D2, new Move("D255928F")},
            {0xF8956544, new Move("446595F8")},
            {0x682A78D5, new Move("D5782A68")},
            {0x1F2D4843, new Move("43482D1F")},
            {0x7FEAC1A6, new Move("A6C1EA7F")},
            {0x08EDF130, new Move("30F1ED08")},
            {0x91E4A08A, new Move("8AA0E491")},
            {0xE6E3901C, new Move("1C90E3E6")},
            {0x788705BF, new Move("BF058778")},
            {0x0F803529, new Move("2935800F")},
            {0x96896493, new Move("93648996")},
            {0xE18E5405, new Move("05548EE1")},
            {0x71314994, new Move("94493171")},
            {0x06367902, new Move("02793606")},
            {0x54C79265, new Move("6592C754")},
            {0x23C0A2F3, new Move("F3A2C023")},
            {0xBAC9F349, new Move("49F3C9BA")},
            {0xCDCEC3DF, new Move("DFC3CECD")},
            {0x53AA567C, new Move("7C56AA53")},
        };

        public static Dictionary<uint, Move> TotemMoves = new Dictionary<uint, Move>()
        {
            {0xDDBFFF53, new Move("Totem Strike: Falcon")},
            {0x44B6AEE9, new Move("Totem Strike: Wolf")},
            {0x33B19E7F, new Move("Totem Strike: Lion")},
            {0xADD50BDC, new Move("Totem Strike: Belion")},
            {0xDAD23B4A, new Move("Totem Strike: Ixaal")},
            {0x43DB6AF0, new Move("Totem Strike: Dohma")},
            {0x78AF85D0, new Move("Totem Strike: Horse")},
            {0xE1A6D46A, new Move("6AD4A6E1")},
            {0x96A1E4FC, new Move("FCE4A196")},
            {0x08C5715F, new Move("5F71C508")},
            {0x7FC241C9, new Move("C941C27F")},
            {0xE6CB1073, new Move("7310CBE6")},
            {0x91CC20E5, new Move("E520CC91")},
            {0x0F7FB4C1, new Move("C1B47F0F")},
            {0x9676E57B, new Move("7BE57696")},
            {0xE171D5ED, new Move("EDD571E1")},
            {0x7F15404E, new Move("4E40157F")},
            {0x081270D8, new Move("D8701208")},
            {0x911B2162, new Move("62211B91")},
            {0x8D2F2310, new Move("10232F8D")},
            {0x142672AA, new Move("AA722614")},
            {0x6321423C, new Move("3C422163")},
            {0xFD45D79F, new Move("9FD745FD")},
            {0x8A42E709, new Move("09E7428A")},
            {0x134BB6B3, new Move("B3B64B13")},
            {0x9A1F8583, new Move("Miss")},
            {0x0316D439, new Move("39D41603")},
            {0x7411E4AF, new Move("AFE41174")},
            {0xEA75710C, new Move("0C7175EA")},
            {0x9D72419A, new Move("9A41729D")},
            {0x047B1020, new Move("20107B04")},
            {0x737C20B6, new Move("B6207C73")},
            {0xE3C33D27, new Move("273DC3E3")},
            {0x94C40DB1, new Move("B10DC494")},
            {0xF4038454, new Move("548403F4")},
            {0x8304B4C2, new Move("C2B40483")},
            {0x1A0DE578, new Move("78E50D1A")},
            {0x6D0AD5EE, new Move("EED50A6D")},
            {0xF36E404D, new Move("4D406EF3")},
            {0x846970DB, new Move("DB706984")},
            {0x1D602161, new Move("6121601D")},
            {0x6A6711F7, new Move("F711676A")},
            {0xFAD80C66, new Move("660CD8FA")},
            {0x8DDF3CF0, new Move("F03CDF8D")},
            {0xDF2ED797, new Move("97D72EDF")},
            {0xA829E701, new Move("01E729A8")},
            {0x3120B6BB, new Move("BBB62031")},
            {0x4627862D, new Move("2D862746")},
            {0xD843138E, new Move("8E1343D8")},
            {0xAF442318, new Move("182344AF")},
            {0x364D72A2, new Move("A2724D36")},
            {0x414A4234, new Move("34424A41")},
            {0xD1F55FA5, new Move("A55FF5D1")},
            {0xA6F26F33, new Move("336FF2A6")},
            {0xC635E6D6, new Move("D6E635C6")},
            {0xB132D640, new Move("40D632B1")},
            {0x283B87FA, new Move("FA873B28")},
            {0x5F3CB76C, new Move("6CB73C5F")},
            {0xC15822CF, new Move("CF2258C1")},
            {0xB65F1259, new Move("59125FB6")},
            {0x2F5643E3, new Move("E343562F")},
            {0x58517375, new Move("75735158")},
            {0xC8EE6EE4, new Move("E46EEEC8")},
            {0xBFE95E72, new Move("725EE9BF")},
            {0x89747011, new Move("11707489")},
            {0xFE734087, new Move("874073FE")},
            {0x677A113D, new Move("3D117A67")},
            {0x107D21AB, new Move("AB217D10")},
        };

        public static List<MoveUltimate> MovesUltimate = new List<MoveUltimate>()
        {
            new MoveUltimate("Ultimate Evolution 1"),
            new MoveUltimate("Ultimate Evolution 2"),
            new MoveUltimate("Ultimate Evolution 3"),
            new MoveUltimate("Ultimate Evolution 4"),
            new MoveUltimate("Ultimate Evolution 5"),
            new MoveUltimate("Ultimate Evolution 6"),
            new MoveUltimate("Ultimate Evolution 7"),
            new MoveUltimate("Ultimate Evolution 8"),
            new MoveUltimate("Ultimate Evolution 9"),
            new MoveUltimate("Ultimate Evolution 10"),
            new MoveUltimate("Ultimate Evolution 11"),
            new MoveUltimate("Ultimate Evolution 12"),
            new MoveUltimate("Ultimate Evolution 13"),
            new MoveUltimate("Ultimate Evolution 14"),
            new MoveUltimate("Ultimate Evolution 15"),
            new MoveUltimate("Ultimate Evolution 16"),
        };
    }
}
