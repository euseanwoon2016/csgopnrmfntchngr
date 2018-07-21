﻿using System;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;
using System.Drawing.Text;
using System.Drawing;
using System.Linq;
using System.Text;

namespace csgopnrmfntchngr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (!File.Exists("fonts.zip"))
            {
                MessageBox.Show("Font file missing!");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string csdir = txtCSDir.Text;
            string font = txtFont.Text;
            if (String.IsNullOrEmpty(csdir) || String.IsNullOrEmpty(font))
            {
                MessageBox.Show("Directory and font must not be empty.");
            }
            else if (!Directory.Exists(csdir))
            {
                MessageBox.Show("Directory does not exist.");
            }
            else
            {
                string csfonts = Path.Combine(csdir, "csgo/panorama/fonts");
                Console.WriteLine(csfonts);
                string csfontsbckp = Path.Combine(csdir, "csgo/panorama/fonts-backup");
                Console.WriteLine(csfontsbckp);
                if (!Directory.Exists(csfontsbckp))
                {
                    Directory.Move(csfonts, csfontsbckp);
                }
                else
                {
                    Directory.Delete(csfonts, true);
                }

                string zipPath = "fonts.zip";
                string extractPath = csfonts;

                ZipFile.ExtractToDirectory(zipPath, extractPath);
                File.Copy(font, Path.Combine(csfonts, Path.GetFileName(font)));

                PrivateFontCollection fontCol = new PrivateFontCollection();
                fontCol.AddFontFile(font);
                string fontName = fontCol.Families[0].Name;
                string fontFileName = Path.GetFileName(font);
                Console.WriteLine(fontName);

                string fontext = Path.GetExtension(font);
                string filename = Path.GetFileNameWithoutExtension(font);

                string fontsconf = File.ReadAllText(Path.Combine(csfonts, "fonts.conf"));
                Console.WriteLine(Path.GetExtension(font));
                StringBuilder b = new StringBuilder(fontsconf);
                b.Replace("RuneScape UF", fontName);
                b.Replace("runescape_uf", filename);
                b.Replace(".ttf", fontext);
                b.Replace("<double>1.2", "<double>1.0");
                File.WriteAllText(Path.Combine(csfonts, "fonts.conf"), b.ToString());

                string confd20 = File.ReadAllText(Path.Combine(csfonts, "conf.d/20-aliases-default-win.conf"));
                string confd20Patched = confd20.Replace("RuneScape UF", fontName);
                File.WriteAllText(Path.Combine(csfonts, "conf.d/20-aliases-default-win.conf"), confd20Patched);

                string confd30 = File.ReadAllText(Path.Combine(csfonts, "conf.d/30-non-latin-inf-win.conf"));
                string confd30Patched = confd30.Replace("RuneScape UF", fontName);
                File.WriteAllText(Path.Combine(csfonts, "conf.d/30-non-latin-inf-win.conf"), confd30Patched);

                string confd95 = File.ReadAllText(Path.Combine(csfonts, "conf.d/95-valve.conf"));
                string confd95Patched = confd95.Replace("RuneScape UF", fontName);
                File.WriteAllText(Path.Combine(csfonts, "conf.d/95-valve.conf"), confd95Patched);

                MessageBox.Show("Done Installing " + fontName + ". To uninstall, delete " + csfonts + " and rename " + csfontsbckp + " to 'fonts'.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFont.Text = openFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtCSDir.Text = folderBrowserDialog1.SelectedPath.ToString();
            }
        }
    }
}
