﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Xml;

namespace TTG_Tools
{
    public partial class MainMenu : Form
    {
        public static Settings settings = new Settings("", "", 1251, false, false, false, true, false, 0, false, false, false, false, false, false, 0, 0, "", "", "", false, false, false, false, 0, 0, false, false, false, false);

        [DllImport("kernel32.dll")]
        public static extern void SetProcessWorkingSetSize(IntPtr hWnd, int i, int j);
        
        public static List<keysEncryption> gamelist = new List<keysEncryption>(); //Список класса для сборки ключей шифрования

        public MainMenu()
        {
            InitializeComponent();
        }

        private void OpenAutopacker_Form_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<AutoPacker>().Count() == 0)
            {
                Form autopacker = new AutoPacker();
                autopacker.Show();
            }
        }

        private void RunFontEditor_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<FontEditor>().Count() == 0)
            {
                Form fonteditor = new FontEditor();
                fonteditor.Show();
            }
        }

        private void About_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<About>().Count() == 0)
            {
                Form about = new About();
                about.Show();
            }
        }

        public class keysEncryption
        {
            public byte[] key;
            public string gamename;

            public keysEncryption() { }

            public keysEncryption(byte[] _key,
                string _gamename)
            {
                this.key = _key;
                this.gamename = _gamename;
            }
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            #region Adding blowfish encryption keys
            byte[] TelltaleTexasHoldEm = { 0x8f, 0xd8, 0x98, 0x99, 0x96, 0xbc, 0xa2, 0xae, 0xd7, 0xde, 0xc5, 0xd3, 0x9d, 0xca, 0xc5, 0xa7, 0xd8, 0x95, 0x92, 0xe9, 0x8d, 0xe4, 0xa1, 0xd4, 0xd7, 0x71, 0xde, 0xc0, 0x9e, 0xde, 0xb1, 0xa3, 0xca, 0xaa, 0xa4, 0x9f, 0xd0, 0xce, 0x9e, 0xde, 0xc5, 0xe3, 0xe3, 0xd1, 0xa9, 0x82, 0xc1, 0xda, 0xaa, 0xd5, 0x76, 0xa2, 0xdb, 0xd7, 0xb1 };
            gamelist.Add(new keysEncryption(TelltaleTexasHoldEm, "Telltale Texas Holdem"));

            byte[] Bone1 = { 0x81, 0xd8, 0x9b, 0x99, 0x55, 0xe2, 0x65, 0x73, 0xb4, 0xdb, 0xe3, 0xc9, 0x63, 0xdb, 0x85, 0x87, 0xab, 0x99, 0x9b, 0xdc, 0x6e, 0xeb, 0x68, 0x9f, 0xa7, 0x90, 0xdd, 0xba, 0x6a, 0xe2, 0x93, 0x64, 0xa1, 0xb4, 0xa0, 0xb4, 0x92, 0xd9, 0x6b, 0x9c, 0xb7, 0xe3, 0xe6, 0xd1, 0x68, 0xa8, 0x84, 0x9f, 0x87, 0xd2, 0x94, 0x98, 0xa1, 0xe8, 0x71 };
            gamelist.Add(new keysEncryption(Bone1, "Bone: Out from Boneville"));

            byte[] csi3D = { 0x34, 0x24, 0x6C, 0x33, 0x43, 0x72, 0x6C, 0x75, 0x64, 0x32, 0x65, 0x53, 0x57, 0x69, 0x45, 0x32, 0x4F, 0x61, 0x63, 0x39, 0x6C, 0x75, 0x74, 0x78, 0x6C, 0x37, 0x32, 0x52, 0x2D, 0x2A, 0x38, 0x49, 0x31, 0x71, 0x4F, 0x34, 0x6F, 0x61, 0x6A, 0x6C, 0x5F, 0x24, 0x65, 0x23, 0x69, 0x61, 0x63, 0x70, 0x34, 0x2A, 0x75, 0x46, 0x6C, 0x65, 0x30 };
            gamelist.Add(new keysEncryption(csi3D, "CSI: 3 Dimensions of Murder"));

            byte[] bone2 = { 0x81, 0xD8, 0x9B, 0x99, 0x56, 0xE2, 0x65, 0x73, 0xB4, 0xDB, 0xE3, 0xC9, 0x64, 0xDB, 0x85, 0x87, 0xAB, 0x99, 0x9B, 0xDC, 0x6F, 0xEB, 0x68, 0x9F, 0xA7, 0x90, 0xDD, 0xBA, 0x6B, 0xE2, 0x93, 0x64, 0xA1, 0xB4, 0xA0, 0xB4, 0x93, 0xD9, 0x6B, 0x9C, 0xB7, 0xE3, 0xE6, 0xD1, 0x69, 0xA8, 0x84, 0x9F, 0x87, 0xD2, 0x94, 0x98, 0xA2, 0xE8, 0x71 };
            gamelist.Add(new keysEncryption(bone2, "Bone: The Great Cow Race"));

            byte[] SM101 = { 0x92, 0xCA, 0x9A, 0x81, 0x85, 0xE4, 0x64, 0x73, 0xA3, 0xBF, 0xD6, 0xD1, 0x7F, 0xC6, 0xCB, 0x88, 0x99, 0x5B, 0x80, 0xD8, 0xAA, 0xC2, 0x97, 0xE7, 0x96, 0x51, 0xA0, 0xA8, 0x9A, 0xD9, 0xAE, 0x95, 0xD7, 0x76, 0x62, 0x80, 0xB4, 0xC4, 0xA6, 0xB9, 0xD6, 0xEC, 0xA9, 0x9C, 0x68, 0x85, 0xB3, 0xDC, 0x92, 0xC4, 0x9E, 0x64, 0xA0, 0xA3, 0x92 };
            gamelist.Add(new keysEncryption(SM101, "Sam & Max 101: Culture Shock"));

            byte[] SM102 = { 0x92, 0xCA, 0x9A, 0x81, 0x85, 0xE4, 0x64, 0x73, 0xA4, 0xBF, 0xD6, 0xD1, 0x7F, 0xC6, 0xCB, 0x88, 0x99, 0x01, 0x80, 0xD8, 0xAA, 0xC2, 0x97, 0xE7, 0x96, 0x51, 0xA1, 0xA8, 0x9A, 0xD9, 0xAE, 0x95, 0xD7, 0x76, 0x62, 0x81, 0xB4, 0xC4, 0xA6, 0xB9, 0xD6, 0xEC, 0xA9, 0x9C, 0x69, 0x85, 0xB3, 0xDC, 0x92, 0xC4, 0x9E, 0x64, 0xA0, 0xA4, 0x92 };
            gamelist.Add(new keysEncryption(SM102, "Sam & Max 102: Situation Comedy"));

            byte[] SM103 = { 0x92, 0xca, 0x9a, 0x81, 0x85, 0xe4, 0x64, 0x73, 0xa5, 0xbf, 0xd6, 0xd1, 0x7f, 0xc6, 0xcb, 0x88, 0x99, 0x5d, 0x80, 0xd8, 0xaa, 0xc2, 0x97, 0xe7, 0x96, 0x51, 0xa2, 0xa8, 0x9a, 0xd9, 0xae, 0x95, 0xd7, 0x76, 0x62, 0x82, 0xb4, 0xc4, 0xa6, 0xb9, 0xd6, 0xec, 0xa9, 0x9c, 0x6a, 0x85, 0xb3, 0xdc, 0x92, 0xc4, 0x9e, 0x64, 0xa0, 0xa5, 0x92 };
            gamelist.Add(new keysEncryption(SM103, "Sam & Max 103: The Mole, The Mob, and the Meatball"));

            byte[] SM104 = { 0x92, 0xCA, 0x9A, 0x81, 0x85, 0xE4, 0x64, 0x73, 0xA6, 0xBF, 0xD6, 0xD1, 0x7F, 0xC6, 0xCB, 0x88, 0x99, 0x5E, 0x80, 0xD8, 0xAA, 0xC2, 0x97, 0xE7, 0x96, 0x51, 0xA3, 0xA8, 0x9A, 0xD9, 0xAE, 0x95, 0xD7, 0x76, 0x62, 0x83, 0xB4, 0xC4, 0xA6, 0xB9, 0xD6, 0xEC, 0xA9, 0x9C, 0x6B, 0x85, 0xB3, 0xDC, 0x92, 0xC4, 0x9E, 0x64, 0xA0, 0xA6, 0x92 };
            gamelist.Add(new keysEncryption(SM104, "Sam & Max 104: Abe Lincoln Must Die!"));

            byte[] SM105 = { 0x92, 0xCA, 0x9A, 0x81, 0x85, 0xE4, 0x64, 0x73, 0xA7, 0xBF, 0xD6, 0xD1, 0x7F, 0xC6, 0xCB, 0x88, 0x99, 0x5F, 0x80, 0xD8, 0xAA, 0xC2, 0x97, 0xE7, 0x96, 0x51, 0xA4, 0xA8, 0x9A, 0xD9, 0xAE, 0x95, 0xD7, 0x76, 0x62, 0x84, 0xB4, 0xC4, 0xA6, 0xB9, 0xD6, 0xEC, 0xA9, 0x9C, 0x6C, 0x85, 0xB3, 0xDC, 0x92, 0xC4, 0x9E, 0x64, 0xA0, 0xA7, 0x92 };
            gamelist.Add(new keysEncryption(SM105, "Sam & Max 105: Reality 2.0"));

            byte[] SM106 = { 0x92, 0xCA, 0x9A, 0x81, 0x85, 0xE4, 0x64, 0x73, 0xA8, 0xBF, 0xD6, 0xD1, 0x7F, 0xC6, 0xCB, 0x88, 0x99, 0x60, 0x80, 0xD8, 0xAA, 0xC2, 0x97, 0xE7, 0x96, 0x51, 0xA5, 0xA8, 0x9A, 0xD9, 0xAE, 0x95, 0xD7, 0x76, 0x62, 0x85, 0xB4, 0xC4, 0xA6, 0xB9, 0xD6, 0xEC, 0xA9, 0x9C, 0x6D, 0x85, 0xB3, 0xDC, 0x92, 0xC4, 0x9E, 0x64, 0xA0, 0xA8, 0x92 };
            gamelist.Add(new keysEncryption(SM106, "Sam & Max 106: Bright Side of the Moon"));

            byte[] CSI4 = { 0x82, 0xbc, 0x76, 0x68, 0x67, 0xbf, 0x7c, 0x77, 0xb5, 0xbf, 0xbe, 0x98, 0x75, 0xb8, 0x9c, 0x8b, 0xac, 0x7d, 0x76, 0xab, 0x80, 0xc8, 0x7f, 0xa3, 0xa8, 0x74, 0xb8, 0x89, 0x7c, 0xbf, 0xaa, 0x68, 0xa2, 0x98, 0x7b, 0x83, 0xa4, 0xb6, 0x82, 0xa0, 0xb8, 0xc7, 0xc1, 0xa0, 0x7a, 0x85, 0x9b, 0xa3, 0x88, 0xb6, 0x6f, 0x67, 0xb3, 0xc5, 0x88 };
            gamelist.Add(new keysEncryption(CSI4, "CSI: Hard Evidence"));

            byte[] SM201 = { 0x92, 0xca, 0x9a, 0x81, 0x85, 0xe4, 0x65, 0x73, 0xa3, 0xbf, 0xd6, 0xd1, 0x7f, 0xc6, 0xcb, 0x89, 0x99, 0x5b, 0x80, 0xd8, 0xaa, 0xc2, 0x97, 0xe7, 0x97, 0x51, 0xa0, 0xa8, 0x9a, 0xd9, 0xae, 0x95, 0xd7, 0x77, 0x62, 0x80, 0xb4, 0xc4, 0xa6, 0xb9, 0xd6, 0xec, 0xaa, 0x9c, 0x68, 0x85, 0xb3, 0xdc, 0x92, 0xc4, 0x9e, 0x65, 0xa0, 0xa3, 0x92 };
            gamelist.Add(new keysEncryption(SM201, "Sam & Max 201: Ice Station Santa"));

            byte[] SM202 = { 0x92, 0xCA, 0x9A, 0x81, 0x85, 0xE4, 0x65, 0x73, 0xA4, 0xBF, 0xD6, 0xD1, 0x7F, 0xC6, 0xCB, 0x89, 0x99, 0x01, 0x80, 0xD8, 0xAA, 0xC2, 0x97, 0xE7, 0x97, 0x51, 0xA1, 0xA8, 0x9A, 0xD9, 0xAE, 0x95, 0xD7, 0x77, 0x62, 0x81, 0xB4, 0xC4, 0xA6, 0xB9, 0xD6, 0xEC, 0xAA, 0x9C, 0x69, 0x85, 0xB3, 0xDC, 0x92, 0xC4, 0x9E, 0x65, 0xA0, 0xA4, 0x92 };
            gamelist.Add(new keysEncryption(SM202, "Sam & Max 202: Moai Better Blues"));

            byte[] SM203 = { 0x92, 0xCA, 0x9A, 0x81, 0x85, 0xE4, 0x65, 0x73, 0xA5, 0xBF, 0xD6, 0xD1, 0x7F, 0xC6, 0xCB, 0x89, 0x99, 0x5D, 0x80, 0xD8, 0xAA, 0xC2, 0x97, 0xE7, 0x97, 0x51, 0xA2, 0xA8, 0x9A, 0xD9, 0xAE, 0x95, 0xD7, 0x77, 0x62, 0x82, 0xB4, 0xC4, 0xA6, 0xB9, 0xD6, 0xEC, 0xAA, 0x9C, 0x6A, 0x85, 0xB3, 0xDC, 0x92, 0xC4, 0x9E, 0x65, 0xA0, 0xA5, 0x92 };
            gamelist.Add(new keysEncryption(SM203, "Sam & Max 203: Night of the Raving Dead"));

            byte[] SM204 = { 0x92, 0xCA, 0x9A, 0x81, 0x85, 0xE4, 0x65, 0x73, 0xA6, 0xBF, 0xD6, 0xD1, 0x7F, 0xC6, 0xCB, 0x89, 0x99, 0x5E, 0x80, 0xD8, 0xAA, 0xC2, 0x97, 0xE7, 0x97, 0x51, 0xA3, 0xA8, 0x9A, 0xD9, 0xAE, 0x95, 0xD7, 0x77, 0x62, 0x83, 0xB4, 0xC4, 0xA6, 0xB9, 0xD6, 0xEC, 0xAA, 0x9C, 0x6B, 0x85, 0xB3, 0xDC, 0x92, 0xC4, 0x9E, 0x65, 0xA0, 0xA6, 0x92 };
            gamelist.Add(new keysEncryption(SM204, "Sam & Max 204: Chariots of the Dogs"));

            byte[] SM205 = { 0x92, 0xca, 0x9a, 0x81, 0x85, 0xe4, 0x65, 0x73, 0xa7, 0xbf, 0xd6, 0xd1, 0x7f, 0xc6, 0xcb, 0x89, 0x99, 0x5f, 0x80, 0xd8, 0xaa, 0xc2, 0x97, 0xe7, 0x97, 0x51, 0xa4, 0xa8, 0x9a, 0xd9, 0xae, 0x95, 0xd7, 0x77, 0x62, 0x84, 0xb4, 0xc4, 0xa6, 0xb9, 0xd6, 0xec, 0xaa, 0x9c, 0x6c, 0x85, 0xb3, 0xdc, 0x92, 0xc4, 0x9e, 0x65, 0xa0, 0xa7, 0x92 };
            gamelist.Add(new keysEncryption(SM205, "Sam & Max 205: What's New, Beelzebub"));

            byte[] SB101 = { 0x87, 0xD8, 0x9A, 0x99, 0x97, 0xE0, 0x94, 0xB5, 0xA3, 0x9C, 0xA6, 0xAC, 0xA1, 0xD2, 0xB8, 0xCA, 0xDD, 0x8B, 0x9F, 0xA8, 0x6D, 0xA6, 0x7E, 0xDE, 0xD2, 0x86, 0xE2, 0xC9, 0x9A, 0xDE, 0x92, 0x64, 0x90, 0x8D, 0xA1, 0xBC, 0xC6, 0xD6, 0xAD, 0xCD, 0xE7, 0xA5, 0xA8, 0x9D, 0x7F, 0xA1, 0xBF, 0xD4, 0xB8, 0xD7, 0x87, 0xA5, 0xA1, 0xA2, 0x70 };
            gamelist.Add(new keysEncryption(SB101, "Strong Bad: Homestar Ruiner"));

            byte[] SB102 = { 0x87, 0xd8, 0x9a, 0x99, 0x97, 0xe0, 0x94, 0xb5, 0xa3, 0x9c, 0xa7, 0xac, 0xa1, 0xd2, 0xb8, 0xca, 0xdd, 0x8b, 0x9f, 0xa8, 0x6d, 0xa7, 0x7e, 0xde, 0xd2, 0x86, 0xe2, 0xc9, 0x9a, 0xde, 0x92, 0x64, 0x91, 0x8d, 0xa1, 0xbc, 0xc6, 0xd6, 0xad, 0xcd, 0xe7, 0xa5, 0xa8, 0x9e, 0x7f, 0xa1, 0xbf, 0xd4, 0xb8, 0xd7, 0x87, 0xa5, 0xa1, 0xa2, 0x71 };
            gamelist.Add(new keysEncryption(SB102, "Strong Bad: Strongbadia the free"));

            byte[] SB103 = { 0x87, 0xD8, 0x9A, 0x99, 0x97, 0xE0, 0x94, 0xB5, 0xA3, 0x9C, 0xA8, 0xAC, 0xA1, 0xD2, 0xB8, 0xCA, 0xDD, 0x8B, 0x9F, 0xA8, 0x6D, 0xA8, 0x7E, 0xDE, 0xD2, 0x86, 0xE2, 0xC9, 0x9A, 0xDE, 0x92, 0x64, 0x92, 0x8D, 0xA1, 0xBC, 0xC6, 0xD6, 0xAD, 0xCD, 0xE7, 0xA5, 0xA8, 0x9F, 0x7F, 0xA1, 0xBF, 0xD4, 0xB8, 0xD7, 0x87, 0xA5, 0xA1, 0xA2, 0x72 };
            gamelist.Add(new keysEncryption(SB103, "Strong Bad: Baddest of the Bands"));

            byte[] SB104 = { 0x87, 0xd8, 0x9a, 0x99, 0x97, 0xe0, 0x94, 0xb5, 0xa3, 0x9c, 0xa9, 0xac, 0xa1, 0xd2, 0xb8, 0xca, 0xdd, 0x8b, 0x9f, 0xa8, 0x6d, 0xa9, 0x7e, 0xde, 0xd2, 0x86, 0xe2, 0xc9, 0x9a, 0xde, 0x92, 0x64, 0x93, 0x8d, 0xa1, 0xbc, 0xc6, 0xd6, 0xad, 0xcd, 0xe7, 0xa5, 0xa8, 0xa0, 0x7f, 0xa1, 0xbf, 0xd4, 0xb8, 0xd7, 0x87, 0xa5, 0xa1, 0xa2, 0x73 };
            gamelist.Add(new keysEncryption(SB104, "Strong Bad: Daneresque 3"));

            byte[] SB105 = { 0x87, 0xd8, 0x9a, 0x99, 0x97, 0xe0, 0x94, 0xb5, 0xa3, 0x9c, 0xaa, 0xac, 0xa1, 0xd2, 0xb8, 0xca, 0xdd, 0x8b, 0x9f, 0xa8, 0x6d, 0xaa, 0x7e, 0xde, 0xd2, 0x86, 0xe2, 0xc9, 0x9a, 0xde, 0x92, 0x64, 0x94, 0x8d, 0xa1, 0xbc, 0xc6, 0xd6, 0xad, 0xcd, 0xe7, 0xa5, 0xa8, 0xa1, 0x7f, 0xa1, 0xbf, 0xd4, 0xb8, 0xd7, 0x87, 0xa5, 0xa1, 0xa2, 0x74 };
            gamelist.Add(new keysEncryption(SB105, "Strong Bad: 8-bit is Enough"));

            byte[] WG101 = { 0x96, 0xCA, 0x99, 0xA0, 0x85, 0xCF, 0x98, 0x8A, 0xE4, 0xDB, 0xE2, 0xCD, 0xA6, 0x96, 0x83, 0x88, 0xC0, 0x8B, 0x99, 0xE3, 0x9E, 0xD8, 0x9B, 0xB6, 0xD7, 0x90, 0xDC, 0xBE, 0xAD, 0x9D, 0x91, 0x65, 0xB6, 0xA6, 0x9E, 0xBB, 0xC2, 0xC6, 0x9E, 0xB3, 0xE7, 0xE3, 0xE5, 0xD5, 0xAB, 0x63, 0x82, 0xA0, 0x9C, 0xC4, 0x92, 0x9F, 0xD1, 0xD5, 0xA4 };
            gamelist.Add(new keysEncryption(WG101, "Wallace & Gromit's grand adventures: Fright of the Bumblebees"));

            byte[] WG102 = { 0x96, 0xCA, 0x99, 0xA0, 0x85, 0xCF, 0x98, 0x8A, 0xE4, 0xDB, 0xE2, 0xCD, 0xA6, 0x96, 0x83, 0x89, 0xC0, 0x8B, 0x99, 0xE3, 0x9E, 0xD8, 0x9B, 0xB6, 0xD7, 0x90, 0xDC, 0xBE, 0xAD, 0x9D, 0x91, 0x66, 0xB6, 0xA6, 0x9E, 0xBB, 0xC2, 0xC6, 0x9E, 0xB3, 0xE7, 0xE3, 0xE5, 0xD5, 0xAB, 0x63, 0x82, 0xA1, 0x9C, 0xC4, 0x92, 0x9F, 0xD1, 0xD5, 0xA4 };
            gamelist.Add(new keysEncryption(WG102, "Wallace & Gromit's grand adventures: The Last Resort"));

            byte[] WG103 = { 0x96, 0xCA, 0x99, 0xA0, 0x85, 0xCF, 0x98, 0x8A, 0xE4, 0xDB, 0xE2, 0xCD, 0xA6, 0x96, 0x83, 0x8A, 0xC0, 0x8B, 0x99, 0xE3, 0x9E, 0xD8, 0x9B, 0xB6, 0xD7, 0x90, 0xDC, 0xBE, 0xAD, 0x9D, 0x91, 0x67, 0xB6, 0xA6, 0x9E, 0xBB, 0xC2, 0xC6, 0x9E, 0xB3, 0xE7, 0xE3, 0xE5, 0xD5, 0xAB, 0x63, 0x82, 0xA2, 0x9C, 0xC4, 0x92, 0x9F, 0xD1, 0xD5, 0xA4 };
            gamelist.Add(new keysEncryption(WG103, "Wallace & Gromit's grand adventures: Muzzled"));

            byte[] WG104 = { 0x96, 0xCA, 0x99, 0xA0, 0x85, 0xCF, 0x98, 0x8A, 0xE4, 0xDB, 0xE2, 0xCD, 0xA6, 0x96, 0x83, 0x8B, 0xC0, 0x8B, 0x99, 0xE3, 0x9E, 0xD8, 0x9B, 0xB6, 0xD7, 0x90, 0xDC, 0xBE, 0xAD, 0x9D, 0x91, 0x68, 0xB6, 0xA6, 0x9E, 0xBB, 0xC2, 0xC6, 0x9E, 0xB3, 0xE7, 0xE3, 0xE5, 0xD5, 0xAB, 0x63, 0x82, 0xA3, 0x9C, 0xC4, 0x92, 0x9F, 0xD1, 0xD5, 0xA4 };
            gamelist.Add(new keysEncryption(WG104, "Wallace & Gromit's grand adventures: The Bogey Man"));

            byte[] ToMI101 = { 0x8C, 0xD8, 0x9B, 0x9F, 0x89, 0xE5, 0x7C, 0xB6, 0xDE, 0xCD, 0xE3, 0xC8, 0x63, 0x95, 0x84, 0xA4, 0xD8, 0x98, 0x98, 0xDC, 0xB6, 0xBE, 0xA9, 0xDB, 0xC6, 0x8F, 0xD3, 0x86, 0x69, 0x9D, 0xAE, 0xA3, 0xCD, 0xB0, 0x97, 0xC8, 0xAA, 0xD6, 0xA5, 0xCD, 0xE3, 0xD8, 0xA9, 0x9C, 0x68, 0x7F, 0xC1, 0xDD, 0xB0, 0xC8, 0x9F, 0x7C, 0xE3, 0xDE, 0xA0 };
            gamelist.Add(new keysEncryption(ToMI101, "Tales of Monkey Island 101"));

            byte[] ToMI102 = { 0x8C, 0xD8, 0x9B, 0x9F, 0x89, 0xE5, 0x7C, 0xB6, 0xDE, 0xCD, 0xE3, 0xC8, 0x63, 0x95, 0x85, 0xA4, 0xD8, 0x98, 0x98, 0xDC, 0xB6, 0xBE, 0xA9, 0xDB, 0xC6, 0x8F, 0xD3, 0x86, 0x69, 0x9E, 0xAE, 0xA3, 0xCD, 0xB0, 0x97, 0xC8, 0xAA, 0xD6, 0xA5, 0xCD, 0xE3, 0xD8, 0xA9, 0x9C, 0x69, 0x7F, 0xC1, 0xDD, 0xB0, 0xC8, 0x9F, 0x7C, 0xE3, 0xDE, 0xA0 };
            gamelist.Add(new keysEncryption(ToMI102, "Tales of Monkey Island 102"));

            byte[] ToMI103 = { 0x8C, 0xD8, 0x9B, 0x9F, 0x89, 0xE5, 0x7C, 0xB6, 0xDE, 0xCD, 0xE3, 0xC8, 0x63, 0x95, 0x86, 0xA4, 0xD8, 0x98, 0x98, 0xDC, 0xB6, 0xBE, 0xA9, 0xDB, 0xC6, 0x8F, 0xD3, 0x86, 0x69, 0x9F, 0xAE, 0xA3, 0xCD, 0xB0, 0x97, 0xC8, 0xAA, 0xD6, 0xA5, 0xCD, 0xE3, 0xD8, 0xA9, 0x9C, 0x6A, 0x7F, 0xC1, 0xDD, 0xB0, 0xC8, 0x9F, 0x7C, 0xE3, 0xDE, 0xA0 };
            gamelist.Add(new keysEncryption(ToMI103, "Tales of Monkey Island 103"));

            byte[] ToMI104 = { 0x8c, 0xd8, 0x9b, 0x9f, 0x89, 0xe5, 0x7c, 0xb6, 0xde, 0xcd, 0xe3, 0xc8, 0x63, 0x95, 0x87, 0xa4, 0xd8, 0x98, 0x98, 0xdc, 0xb6, 0xbe, 0xa9, 0xdb, 0xc6, 0x8f, 0xd3, 0x86, 0x69, 0xa0, 0xae, 0xa3, 0xcd, 0xb0, 0x97, 0xc8, 0xaa, 0xd6, 0xa5, 0xcd, 0xe3, 0xd8, 0xa9, 0x9c, 0x6b, 0x7f, 0xc1, 0xdd, 0xb0, 0xc8, 0x9f, 0x7c, 0xe3, 0xde, 0xa0 };
            gamelist.Add(new keysEncryption(ToMI104, "Tales of Monkey Island 104"));

            byte[] ToMI105 = { 0x8c, 0xd8, 0x9b, 0x9f, 0x89, 0xe5, 0x7c, 0xb6, 0xde, 0xcd, 0xe3, 0xc8, 0x63, 0x95, 0x88, 0xa4, 0xd8, 0x98, 0x98, 0xdc, 0xb6, 0xbe, 0xa9, 0xdb, 0xc6, 0x8f, 0xd3, 0x86, 0x69, 0xa1, 0xae, 0xa3, 0xcd, 0xb0, 0x97, 0xc8, 0xaa, 0xd6, 0xa5, 0xcd, 0xe3, 0xd8, 0xa9, 0x9c, 0x6c, 0x7f, 0xc1, 0xdd, 0xb0, 0xc8, 0x9f, 0x7c, 0xe3, 0xde, 0xa0 };
            gamelist.Add(new keysEncryption(ToMI105, "Tales of Monkey Island 105"));

            byte[] CSI5 = { 0x82, 0xBC, 0x76, 0x69, 0x54, 0x9C, 0x86, 0x90, 0xD7, 0xDA, 0xEA, 0xA7, 0x85, 0xAE, 0x88, 0x87, 0x99, 0x7D, 0x7A, 0xDC, 0xAB, 0xEA, 0x79, 0xC2, 0xAE, 0x56, 0x9F, 0x85, 0x8C, 0xB9, 0xC6, 0xA2, 0xD4, 0x88, 0x85, 0x98, 0x96, 0x93, 0x69, 0xBF, 0xC2, 0xD9, 0xE6, 0xE1, 0x7A, 0x85, 0x9B, 0xA4, 0x75, 0x93, 0x79, 0x80, 0xD5, 0xE0, 0xB4 };
            gamelist.Add(new keysEncryption(CSI5, "CSI: Deadly Intent"));

            byte[] SM301 = { 0x92, 0xCA, 0x9A, 0x81, 0x85, 0xE4, 0x66, 0x73, 0xA3, 0xBF, 0xD6, 0xD1, 0x7F, 0xC6, 0xCB, 0x8A, 0x99, 0x5B, 0x80, 0xD8, 0xAA, 0xC2, 0x97, 0xE7, 0x98, 0x51, 0xA0, 0xA8, 0x9A, 0xD9, 0xAE, 0x95, 0xD7, 0x78, 0x62, 0x80, 0xB4, 0xC4, 0xA6, 0xB9, 0xD6, 0xEC, 0xAB, 0x9C, 0x68, 0x85, 0xB3, 0xDC, 0x92, 0xC4, 0x9E, 0x66, 0xA0, 0xA3, 0x92 };
            gamelist.Add(new keysEncryption(SM301, "Sam & Max 301: The Penal Zone"));

            byte[] SM302 = { 0x92, 0xCA, 0x9A, 0x81, 0x85, 0xE4, 0x66, 0x73, 0xA4, 0xBF, 0xD6, 0xD1, 0x7F, 0xC6, 0xCB, 0x8A, 0x99, 0x01, 0x80, 0xD8, 0xAA, 0xC2, 0x97, 0xE7, 0x98, 0x51, 0xA1, 0xA8, 0x9A, 0xD9, 0xAE, 0x95, 0xD7, 0x78, 0x62, 0x81, 0xB4, 0xC4, 0xA6, 0xB9, 0xD6, 0xEC, 0xAB, 0x9C, 0x69, 0x85, 0xB3, 0xDC, 0x92, 0xC4, 0x9E, 0x66, 0xA0, 0xA4, 0x92 };
            gamelist.Add(new keysEncryption(SM302, "Sam & Max 302: The Tomb of Sammun-Mak"));

            byte[] SM303 = { 0x92, 0xCA, 0x9A, 0x81, 0x85, 0xE4, 0x66, 0x73, 0xA5, 0xBF, 0xD6, 0xD1, 0x7F, 0xC6, 0xCB, 0x8A, 0x99, 0x5D, 0x80, 0xD8, 0xAA, 0xC2, 0x97, 0xE7, 0x98, 0x51, 0xA2, 0xA8, 0x9A, 0xD9, 0xAE, 0x95, 0xD7, 0x78, 0x62, 0x82, 0xB4, 0xC4, 0xA6, 0xB9, 0xD6, 0xEC, 0xAB, 0x9C, 0x6A, 0x85, 0xB3, 0xDC, 0x92, 0xC4, 0x9E, 0x66, 0xA0, 0xA5, 0x92 };
            gamelist.Add(new keysEncryption(SM303, "Sam & Max 303: They Stole Max's Brain"));

            byte[] SM304 = { 0x92, 0xCA, 0x9A, 0x81, 0x85, 0xE4, 0x66, 0x73, 0xA6, 0xBF, 0xD6, 0xD1, 0x7F, 0xC6, 0xCB, 0x8A, 0x99, 0x5E, 0x80, 0xD8, 0xAA, 0xC2, 0x97, 0xE7, 0x98, 0x51, 0xA3, 0xA8, 0x9A, 0xD9, 0xAE, 0x95, 0xD7, 0x78, 0x62, 0x83, 0xB4, 0xC4, 0xA6, 0xB9, 0xD6, 0xEC, 0xAB, 0x9C, 0x6B, 0x85, 0xB3, 0xDC, 0x92, 0xC4, 0x9E, 0x66, 0xA0, 0xA6, 0x92 };
            gamelist.Add(new keysEncryption(SM304, "Sam & Max 304: Beyond the Alley of the Dolls"));

            byte[] SM305 = { 0x92, 0xCA, 0x9A, 0x81, 0x85, 0xE4, 0x66, 0x73, 0xA7, 0xBF, 0xD6, 0xD1, 0x7F, 0xC6, 0xCB, 0x8A, 0x99, 0x5F, 0x80, 0xD8, 0xAA, 0xC2, 0x97, 0xE7, 0x98, 0x51, 0xA4, 0xA8, 0x9A, 0xD9, 0xAE, 0x95, 0xD7, 0x78, 0x62, 0x84, 0xB4, 0xC4, 0xA6, 0xB9, 0xD6, 0xEC, 0xAB, 0x9C, 0x6C, 0x85, 0xB3, 0xDC, 0x92, 0xC4, 0x9E, 0x66, 0xA0, 0xA7, 0x92 };
            gamelist.Add(new keysEncryption(SM305, "Sam & Max 305: The City That Dares Not Sleep"));

            byte[] Hector101 = { 0x87, 0xCE, 0x90, 0xA8, 0x93, 0xDE, 0x64, 0x73, 0xA3, 0xB4, 0xDA, 0xC7, 0xA6, 0xD4, 0xC5, 0x88, 0x99, 0x5B, 0x75, 0xDC, 0xA0, 0xE9, 0xA5, 0xE1, 0x96, 0x51, 0xA0, 0x9D, 0x9E, 0xCF, 0xD5, 0xA3, 0xD1, 0x76, 0x62, 0x80, 0xA9, 0xC8, 0x9C, 0xE0, 0xE4, 0xE6, 0xA9, 0x9C, 0x68, 0x7A, 0xB7, 0xD2, 0xB9, 0xD2, 0x98, 0x64, 0xA0, 0xA3, 0x87 };
            gamelist.Add(new keysEncryption(Hector101, "Hector. Episode 1"));

            byte[] Hector102 = { 0x87, 0xCE, 0x90, 0xA8, 0x93, 0xDE, 0x64, 0x73, 0xA4, 0xB4, 0xDA, 0xC7, 0xA6, 0xD4, 0xC5, 0x88, 0x99, 0x01, 0x75, 0xDC, 0xA0, 0xE9, 0xA5, 0xE1, 0x96, 0x51, 0xA1, 0x9D, 0x9E, 0xCF, 0xD5, 0xA3, 0xD1, 0x76, 0x62, 0x81, 0xA9, 0xC8, 0x9C, 0xE0, 0xE4, 0xE6, 0xA9, 0x9C, 0x69, 0x7A, 0xB7, 0xD2, 0xB9, 0xD2, 0x98, 0x64, 0xA0, 0xA4, 0x87 };
            gamelist.Add(new keysEncryption(Hector102, "Hector. Episode 2"));

            byte[] Hector103 = { 0x87, 0xCE, 0x90, 0xA8, 0x93, 0xDE, 0x64, 0x73, 0xA5, 0xB4, 0xDA, 0xC7, 0xA6, 0xD4, 0xC5, 0x88, 0x99, 0x5D, 0x75, 0xDC, 0xA0, 0xE9, 0xA5, 0xE1, 0x96, 0x51, 0xA2, 0x9D, 0x9E, 0xCF, 0xD5, 0xA3, 0xD1, 0x76, 0x62, 0x82, 0xA9, 0xC8, 0x9C, 0xE0, 0xE4, 0xE6, 0xA9, 0x9C, 0x6A, 0x7A, 0xB7, 0xD2, 0xB9, 0xD2, 0x98, 0x64, 0xA0, 0xA5, 0x87 };
            gamelist.Add(new keysEncryption(Hector103, "Hector. Episode 3"));

            byte[] PA1 = { 0x86, 0xDB, 0x96, 0x97, 0x8F, 0xD8, 0x98, 0x74, 0xA2, 0x9D, 0xBC, 0xD6, 0x9B, 0xC8, 0xBE, 0xC3, 0xCE, 0x5B, 0x5D, 0xA8, 0x84, 0xE7, 0x9F, 0xD2, 0xD0, 0x8D, 0xD4, 0x86, 0x69, 0x9D, 0xA8, 0xA6, 0xC8, 0xA8, 0x9D, 0xBB, 0xC6, 0x94, 0x69, 0x9D, 0xBC, 0xE6, 0xE1, 0xCF, 0xA2, 0x9E, 0xB7, 0xA0, 0x75, 0x94, 0x6D, 0xA5, 0xD9, 0xD5, 0xAA };
            gamelist.Add(new keysEncryption(PA1, "Nelson Tethers: Puzzle Agent"));

            byte[] CSI6 = { 0x82, 0xBC, 0x76, 0x6A, 0x54, 0x9C, 0x76, 0x96, 0xBB, 0xA2, 0xA5, 0x94, 0x75, 0xB8, 0x9C, 0x8D, 0x99, 0x5A, 0x70, 0xCA, 0x86, 0xAB, 0x66, 0x9F, 0xA8, 0x74, 0xB8, 0x8B, 0x69, 0x9C, 0xA4, 0x87, 0xA8, 0x7B, 0x62, 0x7F, 0xA4, 0xB6, 0x82, 0xA2, 0xA5, 0xA4, 0xBB, 0xBF, 0x80, 0x68, 0x82, 0x9F, 0x88, 0xB6, 0x6F, 0x69, 0xA0, 0xA2, 0x82 };
            gamelist.Add(new keysEncryption(CSI6, "CSI: Fatal Conspiracy"));

            byte[] PN1 = { 0x82, 0xCE, 0x99, 0x99, 0x86, 0xDE, 0x9C, 0xB7, 0xEB, 0xBC, 0xE4, 0xCF, 0x97, 0xD7, 0x96, 0xBC, 0xD5, 0x8F, 0x8F, 0xE9, 0xA6, 0xE9, 0xAF, 0xBF, 0xD4, 0x8C, 0xD4, 0xC7, 0x7C, 0xD1, 0xCD, 0x99, 0xC1, 0xB7, 0x9B, 0xC3, 0xDA, 0xB3, 0xA8, 0xD7, 0xDA, 0xE6, 0xBB, 0xD1, 0xA3, 0x97, 0xB4, 0xE1, 0xAE, 0xD7, 0x9F, 0x83, 0xDF, 0xDD, 0xA4 };
            gamelist.Add(new keysEncryption(PN1, "Poker Night at the Inventory"));

            byte[] BTTF101 = { 0x81, 0xCA, 0x90, 0x9F, 0x78, 0xDB, 0x87, 0xAB, 0xD7, 0xB2, 0xEA, 0xD8, 0xA7, 0xD7, 0xB8, 0x88, 0x99, 0x5B, 0x6F, 0xD8, 0xA0, 0xE0, 0x8A, 0xDE, 0xB9, 0x89, 0xD4, 0x9B, 0xAE, 0xE0, 0xD6, 0xA6, 0xC4, 0x76, 0x62, 0x80, 0xA3, 0xC4, 0x9C, 0xD7, 0xC9, 0xE3, 0xCC, 0xD4, 0x9C, 0x78, 0xC7, 0xE3, 0xBA, 0xD5, 0x8B, 0x64, 0xA0, 0xA3, 0x81 };
            gamelist.Add(new keysEncryption(BTTF101, "Back To The Future: Episode 1 - It's About Time"));

            byte[] BTTF102 = { 0x81, 0xca, 0x90, 0x9f, 0x78, 0xdb, 0x87, 0xab, 0xd7, 0xb2, 0xea, 0xd8, 0xa7, 0xd7, 0xb8, 0x88, 0x99, 0x01, 0x6f, 0xd8, 0xa0, 0xe0, 0x8a, 0xde, 0xb9, 0x89, 0xd4, 0x9b, 0xae, 0xe0, 0xd6, 0xa6, 0xc4, 0x76, 0x62, 0x81, 0xa3, 0xc4, 0x9c, 0xd7, 0xc9, 0xe3, 0xcc, 0xd4, 0x9c, 0x78, 0xc7, 0xe3, 0xba, 0xd5, 0x8b, 0x64, 0xa0, 0xa4, 0x81 };
            gamelist.Add(new keysEncryption(BTTF102, "Back To The Future: Episode 2 - Get Tannen!"));

            byte[] BTTF103 = { 0x81, 0xCA, 0x90, 0x9F, 0x78, 0xDB, 0x87, 0xAB, 0xD7, 0xB2, 0xEA, 0xD8, 0xA7, 0xD7, 0xB8, 0x88, 0x99, 0x5D, 0x6F, 0xD8, 0xA0, 0xE0, 0x8A, 0xDE, 0xB9, 0x89, 0xD4, 0x9B, 0xAE, 0xE0, 0xD6, 0xA6, 0xC4, 0x76, 0x62, 0x82, 0xA3, 0xC4, 0x9C, 0xD7, 0xC9, 0xE3, 0xCC, 0xD4, 0x9C, 0x78, 0xC7, 0xE3, 0xBA, 0xD5, 0x8B, 0x64, 0xA0, 0xA5, 0x81 };
            gamelist.Add(new keysEncryption(BTTF103, "Back To The Future: Episode 3 - Citizen Brown"));

            byte[] BTTF104 = { 0x81, 0xCA, 0x90, 0x9F, 0x78, 0xDB, 0x87, 0xAB, 0xD7, 0xB2, 0xEA, 0xD8, 0xA7, 0xD7, 0xB8, 0x88, 0x99, 0x5E, 0x6F, 0xD8, 0xA0, 0xE0, 0x8A, 0xDE, 0xB9, 0x89, 0xD4, 0x9B, 0xAE, 0xE0, 0xD6, 0xA6, 0xC4, 0x76, 0x62, 0x83, 0xA3, 0xC4, 0x9C, 0xD7, 0xC9, 0xE3, 0xCC, 0xD4, 0x9C, 0x78, 0xC7, 0xE3, 0xBA, 0xD5, 0x8B, 0x64, 0xA0, 0xA6, 0x81 };
            gamelist.Add(new keysEncryption(BTTF104, "Back To The Future: Episode 4 - Double Visions"));

            byte[] BTTF105 = { 0x81, 0xCA, 0x90, 0x9F, 0x78, 0xDB, 0x87, 0xAB, 0xD7, 0xB2, 0xEA, 0xD8, 0xA7, 0xD7, 0xB8, 0x88, 0x99, 0x5F, 0x6F, 0xD8, 0xA0, 0xE0, 0x8A, 0xDE, 0xB9, 0x89, 0xD4, 0x9B, 0xAE, 0xE0, 0xD6, 0xA6, 0xC4, 0x76, 0x62, 0x84, 0xA3, 0xC4, 0x9C, 0xD7, 0xC9, 0xE3, 0xCC, 0xD4, 0x9C, 0x78, 0xC7, 0xE3, 0xBA, 0xD5, 0x8B, 0x64, 0xA0, 0xA7, 0x81 };
            gamelist.Add(new keysEncryption(BTTF105, "Back To The Future: Episode 5 - OUTATIME"));

            byte[] PA2 = { 0x86, 0xDB, 0x96, 0x97, 0x8F, 0xD8, 0x98, 0x74, 0xA2, 0x9E, 0xBC, 0xD6, 0x9B, 0xC8, 0xBE, 0xC3, 0xCE, 0x5B, 0x5D, 0xA9, 0x84, 0xE7, 0x9F, 0xD2, 0xD0, 0x8D, 0xD4, 0x86, 0x69, 0x9E, 0xA8, 0xA6, 0xC8, 0xA8, 0x9D, 0xBB, 0xC6, 0x94, 0x69, 0x9E, 0xBC, 0xE6, 0xE1, 0xCF, 0xA2, 0x9E, 0xB7, 0xA0, 0x75, 0x95, 0x6D, 0xA5, 0xD9, 0xD5, 0xAA };
            gamelist.Add(new keysEncryption(PA2, "Puzzle Agent 2"));

            byte[] JP100 = { 0x89, 0xde, 0x9f, 0x95, 0x97, 0xdf, 0x9c, 0xa6, 0xc2, 0xcd, 0xe7, 0xcf, 0x63, 0x95, 0x83, 0xa1, 0xde, 0x9c, 0x8e, 0xea, 0xb0, 0xde, 0x99, 0xbf, 0xc6, 0x93, 0xda, 0x86, 0x69, 0x9c, 0xab, 0xa9, 0xd1, 0xa6, 0xa5, 0xc2, 0xca, 0xc6, 0x89, 0xcd, 0xe7, 0xdf, 0xa9, 0x9c, 0x67, 0x7c, 0xc7, 0xe1, 0xa6, 0xd6, 0x99, 0x9c, 0xd3, 0xc2, 0xa0 };
            gamelist.Add(new keysEncryption(JP100, "Jurassic Park: the game"));

            byte[] LOL = { 0x8B, 0xCA, 0xA4, 0x75, 0x92, 0xD0, 0x82, 0xB5, 0xD6, 0xD1, 0xE7, 0x95, 0x62, 0x95, 0x9F, 0xB8, 0xE0, 0x6B, 0x9B, 0xDB, 0x8C, 0xE7, 0x9A, 0xD4, 0xD7, 0x52, 0x9F, 0x85, 0x85, 0xCD, 0xD8, 0x75, 0xCD, 0xA9, 0x81, 0xC1, 0xC5, 0xC8, 0xAB, 0x9D, 0xA5, 0xA4, 0xC4, 0xCD, 0xAE, 0x73, 0xC0, 0xD3, 0x94, 0xD5, 0x8A, 0x98, 0xE2, 0xA3, 0x6F };
            gamelist.Add(new keysEncryption(LOL, "Law & Order: Legacies"));

            byte[] WD100 = { 0x96, 0xca, 0x99, 0x9f, 0x8d, 0xda, 0x9a, 0x87, 0xd7, 0xcd, 0xd9, 0x95, 0x62, 0x95, 0xaa, 0xb8, 0xd5, 0x95, 0x96, 0xe5, 0xa4, 0xb9, 0x9b, 0xd0, 0xc9, 0x52, 0x9f, 0x85, 0x90, 0xcd, 0xcd, 0x9f, 0xc8, 0xb3, 0x99, 0x93, 0xc6, 0xc4, 0x9d, 0x9d, 0xa5, 0xa4, 0xcf, 0xcd, 0xa3, 0x9d, 0xbb, 0xdd, 0xac, 0xa7, 0x8b, 0x94, 0xd4, 0xa3, 0x6f };
            gamelist.Add(new keysEncryption(WD100, "Walking Dead: the game"));

            byte[] PN2 = { 0x82, 0xCE, 0x99, 0x99, 0x86, 0xDE, 0x9C, 0xB7, 0xEB, 0xBC, 0xE4, 0xCF, 0x97, 0xD7, 0x85, 0x9A, 0xCE, 0x96, 0x92, 0xD9, 0xAF, 0xDE, 0xAA, 0xE8, 0xB5, 0x90, 0xDA, 0xBA, 0xAB, 0x9E, 0xA4, 0x99, 0xCB, 0xAA, 0x94, 0xC1, 0xCA, 0xD7, 0xB2, 0xBC, 0xE4, 0xDF, 0xDD, 0xDE, 0x69, 0x75, 0xB7, 0xDB, 0xAA, 0xC5, 0x98, 0x9C, 0xE4, 0xEB, 0x8F };
            gamelist.Add(new keysEncryption(PN2, "Poker Night 2"));

            byte[] WAU100 = { 0x85, 0xca, 0x8f, 0xa0, 0x89, 0xdf, 0x64, 0x73, 0xa2, 0xb2, 0xd6, 0xc6, 0x9e, 0xca, 0xc6, 0x88, 0x99, 0x5a, 0x73, 0xd8, 0x9f, 0xe1, 0x9b, 0xe2, 0x96, 0x51, 0x9f, 0x9b, 0x9a, 0xce, 0xcd, 0x99, 0xd2, 0x76, 0x62, 0x7f, 0xa7, 0xc4, 0x9b, 0xd8, 0xda, 0xe7, 0xa9, 0x9c, 0x67, 0x78, 0xb3, 0xd1, 0xb1, 0xc8, 0x99, 0x64, 0xa0, 0xa2, 0x85 };
            gamelist.Add(new keysEncryption(WAU100, "The Wolf Among Us"));

            byte[] WD200 = { 0x96, 0xCA, 0x99, 0x9F, 0x8D, 0xDA, 0x9A, 0x87, 0xD7, 0xCD, 0xD9, 0x96, 0x62, 0x95, 0xAA, 0xB8, 0xD5, 0x95, 0x96, 0xE5, 0xA4, 0xB9, 0x9B, 0xD0, 0xC9, 0x53, 0x9F, 0x85, 0x90, 0xCD, 0xCD, 0x9F, 0xC8, 0xB3, 0x99, 0x93, 0xC6, 0xC4, 0x9D, 0x9E, 0xA5, 0xA4, 0xCF, 0xCD, 0xA3, 0x9D, 0xBB, 0xDD, 0xAC, 0xA7, 0x8B, 0x94, 0xD4, 0xA4, 0x6F };
            gamelist.Add(new keysEncryption(WD200, "The Walking Dead: Season Two"));

            byte[] TftB100 = { 0x81, 0xD8, 0x9F, 0x98, 0x89, 0xDE, 0x9F, 0xA4, 0xE0, 0xD0, 0xE8, 0x95, 0x62, 0x95, 0x95, 0xC6, 0xDB, 0x8E, 0x92, 0xE9, 0xA9, 0xD6, 0xA4, 0xD3, 0xD8, 0x52, 0x9F, 0x85, 0x7B, 0xDB, 0xD3, 0x98, 0xC4, 0xB7, 0x9E, 0xB0, 0xCF, 0xC7, 0xAC, 0x9D, 0xA5, 0xA4, 0xBA, 0xDB, 0xA9, 0x96, 0xB7, 0xE1, 0xB1, 0xC4, 0x94, 0x97, 0xE3, 0xA3, 0x6F };
            gamelist.Add(new keysEncryption(TftB100, "Tales From the Borderlands"));

            byte[] GoT100 = { 0x86, 0xCA, 0x9A, 0x99, 0x73, 0xD2, 0x87, 0xAB, 0xE4, 0xDB, 0xE3, 0xC9, 0xA5, 0x96, 0x83, 0x87, 0xB0, 0x8B, 0x9A, 0xDC, 0x8C, 0xDB, 0x8A, 0xD7, 0xD7, 0x90, 0xDD, 0xBA, 0xAC, 0x9D, 0x91, 0x64, 0xA6, 0xA6, 0x9F, 0xB4, 0xB0, 0xC9, 0x8D, 0xD4, 0xE7, 0xE3, 0xE6, 0xD1, 0xAA, 0x63, 0x82, 0x9F, 0x8C, 0xC4, 0x93, 0x98, 0xBF, 0xD8, 0x93 };
            gamelist.Add(new keysEncryption(GoT100, "Game of Thrones"));

            byte[] MSM100 = { 0x8c, 0xd2, 0x9b, 0x99, 0x87, 0xde, 0x94, 0xa9, 0xe6, 0x9d, 0xa5, 0x94, 0x7f, 0xce, 0xc1, 0xbc, 0xcc, 0x9c, 0x8e, 0xdd, 0xb1, 0xa6, 0x66, 0x9f, 0xb2, 0x8a, 0xdd, 0xba, 0x9c, 0xde, 0xc2, 0x9a, 0xd3, 0x76, 0x62, 0x7f, 0xae, 0xcc, 0xa7, 0xd1, 0xd8, 0xe6, 0xd9, 0xd2, 0xab, 0x63, 0x82, 0x9f, 0x92, 0xcc, 0x94, 0x98, 0xd3, 0xe4, 0xa0 };
            gamelist.Add(new keysEncryption(MSM100, "Minecraft: Story Mode"));

            byte[] WDM = { 0x96, 0xCA, 0x99, 0x9F, 0x8D, 0xDA, 0x9A, 0x87, 0xD7, 0xCD, 0xD9, 0xB1, 0x63, 0x95, 0x83, 0xAE, 0xCA, 0x96, 0x98, 0xE0, 0xAB, 0xDC, 0x7A, 0xD4, 0xC6, 0x85, 0xBC, 0x86, 0x69, 0x9C, 0xB8, 0x95, 0xCB, 0xB0, 0x9B, 0xBD, 0xC8, 0xA7, 0x9E, 0xCD, 0xD9, 0xC1, 0xA9, 0x9C, 0x67, 0x89, 0xB3, 0xDB, 0xB0, 0xCC, 0x94, 0x9A, 0xB4, 0xD7, 0xA0 };
            gamelist.Add(new keysEncryption(WDM, "Walking Dead: Michonne"));

            byte[] Batman = { 0x81, 0xca, 0xa1, 0xa1, 0x85, 0xda, 0x64, 0x73, 0xa2, 0xae, 0xd6, 0xd8, 0x9f, 0xc6, 0xc1, 0x88, 0x99, 0x5a, 0x6f, 0xd8, 0xb1, 0xe2, 0x97, 0xdd, 0x96, 0x51, 0x9f, 0x97, 0x9a, 0xe0, 0xce, 0x95, 0xcd, 0x76, 0x62, 0x7f, 0xa3, 0xc4, 0xad, 0xd9, 0xd6, 0xe2, 0xa9, 0x9c, 0x67, 0x74, 0xb3, 0xe3, 0xb2, 0xc4, 0x94, 0x64, 0xa0, 0xa2, 0x81 };
            gamelist.Add(new keysEncryption(Batman, "Batman: Telltale Series"));

            byte[] WD300 = { 0x96, 0xCA, 0x99, 0x9F, 0x8D, 0xDA, 0x9A, 0x87, 0xD7, 0xCD, 0xD9, 0x97, 0x62, 0x95, 0xAA, 0xB8, 0xD5, 0x95, 0x96, 0xE5, 0xA4, 0xB9, 0x9B, 0xD0, 0xC9, 0x54, 0x9F, 0x85, 0x90, 0xCD, 0xCD, 0x9F, 0xC8, 0xB3, 0x99, 0x93, 0xC6, 0xC4, 0x9D, 0x9F, 0xA5, 0xA4, 0xCF, 0xCD, 0xA3, 0x9D, 0xBB, 0xDD, 0xAC, 0xA7, 0x8B, 0x94, 0xD4, 0xA5, 0x6F };
            gamelist.Add(new keysEncryption(WD300, "Walking Dead: A New Frontier"));

            byte[] Guardians = { 0x86, 0xDE, 0x8E, 0xA6, 0x88, 0xD5, 0x94, 0xB1, 0xE5, 0x9D, 0xA5, 0x94, 0x79, 0xDA, 0xB4, 0xC9, 0xCD, 0x93, 0x8E, 0xE5, 0xB0, 0xA6, 0x66, 0x9F, 0xAC, 0x96, 0xD0, 0xC7, 0x9D, 0xD5, 0xC2, 0xA2, 0xD2, 0x76, 0x62, 0x7F, 0xA8, 0xD8, 0x9A, 0xDE, 0xD9, 0xDD, 0xD9, 0xDA, 0xAA, 0x63, 0x82, 0x9F, 0x8C, 0xD8, 0x87, 0xA5, 0xD4, 0xDB, 0xA0 };
            gamelist.Add(new keysEncryption(Guardians, "Guardians of the galaxy"));

            byte[] Batman_2 = { 0x81, 0xCA, 0xA1, 0xA1, 0x85, 0xDA, 0x65, 0x73, 0xA2, 0xAE, 0xD6, 0xD8, 0x9F, 0xC6, 0xC1, 0x89, 0x99, 0x5A, 0x6F, 0xD8, 0xB1, 0xE2, 0x97, 0xDD, 0x97, 0x51, 0x9F, 0x97, 0x9A, 0xE0, 0xCE, 0x95, 0xCD, 0x77, 0x62, 0x7F, 0xA3, 0xC4, 0xAD, 0xD9, 0xD6, 0xE2, 0xAA, 0x9C, 0x67, 0x74, 0xB3, 0xE3, 0xB2, 0xC4, 0x94, 0x65, 0xA0, 0xA2, 0x81 };
            gamelist.Add(new keysEncryption(Batman_2, "Batman: The Enemy Within"));

            byte[] MSM200 = { 0x8c, 0xd2, 0x9b, 0x99, 0x87, 0xde, 0x94, 0xa9, 0xe6, 0x9e, 0xa5, 0x94, 0x7f, 0xce, 0xc1, 0xbc, 0xcc, 0x9c, 0x8e, 0xdd, 0xb1, 0xa7, 0x66, 0x9f, 0xb2, 0x8a, 0xdd, 0xba, 0x9c, 0xde, 0xc2, 0x9a, 0xd3, 0x77, 0x62, 0x7f, 0xae, 0xcc, 0xa7, 0xd1, 0xd8, 0xe6, 0xd9, 0xd2, 0xab, 0x64, 0x82, 0x9f, 0x92, 0xcc, 0x94, 0x98, 0xd3, 0xe4, 0xa0 };
            gamelist.Add(new keysEncryption(MSM200, "Minecraft: Season Two"));

            byte[] WD4Demo = { 0x96, 0xCA, 0x99, 0x9F, 0x8D, 0xDA, 0x9A, 0x87, 0xD7, 0xCD, 0xD9, 0x98, 0x62, 0x95, 0xAA, 0xB8, 0xD5, 0x95, 0x96, 0xE5, 0xA4, 0xB9, 0x9B, 0xD0, 0xC9, 0x55, 0x9F, 0x85, 0x90, 0xCD, 0xCD, 0x9F, 0xC8, 0xB3, 0x99, 0x93, 0xC6, 0xC4, 0x9D, 0xA0, 0xA5, 0xA4, 0xCF, 0xCD, 0xA3, 0x9D, 0xBB, 0xDD, 0xAC, 0xA7, 0x8B, 0x94, 0xD4, 0xA6, 0x6F };
            gamelist.Add(new keysEncryption(WD4Demo, "Walking Dead: The Final Season"));

            byte[] WDTDS = { 0x96, 0xCA, 0x99, 0x9F, 0x8D, 0xDA, 0x9A, 0x87, 0xD7, 0xCD, 0xD9, 0xBB, 0x93, 0xD1, 0xBE, 0xC0, 0xD7, 0x91, 0x71, 0xDC, 0x9E, 0xD9, 0x8D, 0xD0, 0xD1, 0x8C, 0xD8, 0xC3, 0xA0, 0xB0, 0xC6, 0x95, 0xC3, 0x9C, 0x93, 0xBB, 0xCC, 0xCC, 0xA7, 0xD3, 0xB9, 0xD9, 0xD9, 0xD0, 0x8E, 0x93, 0xBE, 0xDA, 0xAE, 0xD1, 0x8D, 0x77, 0xD5, 0xD3, 0xA3 };
            gamelist.Add(new keysEncryption(WDTDS, "The Walking Dead: The Telltale Definitive Series"));

            byte[] SMSTWR = { 0x92, 0xCA, 0x9A, 0x81, 0x85, 0xE4, 0x64, 0x73, 0xA2, 0xBF, 0xD6, 0xD1, 0x7F, 0xC6, 0xCB, 0x88, 0x99, 0x5A, 0x80, 0xD8, 0xAA, 0xC2, 0x97, 0xE7, 0x96, 0x51, 0x9F, 0xA8, 0x9A, 0xD9, 0xAE, 0x95, 0xD7, 0x76, 0x62, 0x7F, 0xB4, 0xC4, 0xA6, 0xB9, 0xD6, 0xEC, 0xA9, 0x9C, 0x67, 0x85, 0xB3, 0xDC, 0x92, 0xC4, 0x9E, 0x64, 0xA0, 0xA2, 0x92 };
            gamelist.Add(new keysEncryption(SMSTWR, "Sam & Max Save the World Remastered"));

            byte[] SMBTaS = { 0x92, 0xCA, 0x9A, 0x81, 0x85, 0xE4, 0x65, 0x73, 0xA2, 0xBF, 0xD6, 0xD1, 0x7F, 0xC6, 0xCB, 0x89, 0x99, 0x5A, 0x80, 0xD8, 0xAA, 0xC2, 0x97, 0xE7, 0x97, 0x51, 0x9F, 0xA8, 0x9A, 0xD9, 0xAE, 0x95, 0xD7, 0x77, 0x62, 0x7F, 0xB4, 0xC4, 0xA6, 0xB9, 0xD6, 0xEC, 0xAA, 0x9C, 0x67, 0x85, 0xB3, 0xDC, 0x92, 0xC4, 0x9E, 0x65, 0xA0, 0xA2, 0x92 };
            gamelist.Add(new keysEncryption(SMBTaS, "Sam & Max Beyond Time and Space Remastered"));

            #endregion

            string xmlPath = Application.StartupPath + "\\config.xml";
            XmlReader reader = new XmlTextReader(xmlPath);
            XmlSerializer settingsDeserializer = new System.Xml.Serialization.XmlSerializer(typeof(Settings));
            settings = (Settings)settingsDeserializer.Deserialize(reader);
            reader.Close();

            SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
        }

        private void MainMenu_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.Visible = true;
                Hide();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<TextEditor>().Count() == 0)
            {
                Form txteditor = new TextEditor();
                txteditor.Show();
            }
        }
        private void buttonSettings_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<FormSettings>().Count() == 0)
            {
                Form settings = new FormSettings();
                settings.Show(this);
            }
        }

        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>
        public class Glossariy_for_Credits
        {
            public string originalName;
            public string translatedName;
            public Glossariy_for_Credits() { }
            public Glossariy_for_Credits(string _originalName, string _translatedName)
            {
                this.originalName = _originalName;
                this.translatedName = _translatedName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<ArchivePacker>().Count() == 0)
            {
                Form archiveForm = new ArchivePacker();
                archiveForm.Show();
            }
        }
    }
}