using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace TxtTask
{
    public partial class Form1 : Form
    {
        String CONFIGFILE = "config.txt";
        String CONFIG;
        String PYPATH;
        String IPATH;

        public Form1()
        {
            InitializeComponent();
            init();
        }

        public void init()
        {
            try { loadConfig(); }
            catch (Exception e) { say("Error while loading config: " + e.Message); }
        }

        public void displayFile(String file)
        {
            listView1.Clear();
            StreamReader sr = new StreamReader(file);
            String[] items = sr.ReadToEnd().Split('\n');
            foreach (String item in items)
            {
                if (item.Contains('\n')) { item.Remove(item.IndexOf('\n')); }
                if (item.Contains('\r')) { item.Remove(item.IndexOf('\r')); }
                listView1.Items.Add(item);
            }
            sr.Close();
            toolStripStatusLabel1.Text = "Loaded text file";
        }

        public String[,] parseConfig()
        {
            if (File.Exists(CONFIGFILE))
            {
                using (StreamReader configFile = new StreamReader(CONFIGFILE))
                {
                    String raw = configFile.ReadToEnd();
                    String[] pre = raw.Split('\n');
                    String[,] config = new String[pre.Length, 2];
                    for (int i = 0; i < config.GetLength(0); i++)
                    {
                        config[i, 0] = pre[i].Split('=')[0];
                        config[i, 1] = pre[i].Split('=')[1];
                    }
                    for (int i = 0; i < config.GetLength(0); i++)
                    {
                        listView1.Items.Add(config[i, 0] + "=" + config[i, 1]);
                    }
                    return config;
                }
            }
            else
            {
                say("Config doesn't exist, creating a default...");
                say("Creating config...");
                using (StreamWriter configFile = File.CreateText(CONFIGFILE))
                {
                    configFile.WriteLine("file=C:\\Sample\\Todo\\File.txt");
                    configFile.WriteLine("pypath=C:\\Path\\To\\Python27\\Python.exe");
                    configFile.WriteLine("ipath=C:\\Path\\To\\GTasks\\Interface.py");
                }
                say("Created config, please customize it and press refresh to continue");
                return null;
            }
        }

        public void loadConfig()
        {
            String[,] config = parseConfig();
            for (int i = 0; i < config.GetLength(0); i++)
            {
                if (config[i, 0] == "file")
                {
                    CONFIG = config[i, 1].Remove(config[i, 1].Length-1);
                    if (File.Exists(config[i, 1]))
                    {
                        displayFile(CONFIG);
                    }
                    else
                    {
                        say("Non-existant file specified in config");
                        Form2 form2 = new Form2();
                        form2.ShowDialog();
                        if (form2.DialogResult == DialogResult.Yes)
                        {
                            say("Creating todo file...");
                            using (StreamWriter configFile = File.CreateText(CONFIG))
                            {
                                configFile.WriteLine("#Auto-generated todo file, created by TxtTask by Isaac Grant");
                                configFile.WriteLine("Example todo");
                            }
                            displayFile(CONFIG);
                            say("Loaded todo");
                        }
                        else
                        {
                            say("Temporarily load a new text file, create a new one, or change the one specified in the config file and click \"Refresh\"");
                            CONFIG = "";
                        }
                    }
                }
                else if (config[i, 0] == "pypath")
                {
                    if (File.Exists(config[i, 1]))
                    {
                        PYPATH = config[i, 1].Remove(config[i, 1].Length-1);
                        say("Loaded Python2.7 path");
                    }
                    else
                    {
                        PYPATH = "";
                        say("Invalid Python path");
                    }
                }
                else if (config[i, 0] == "ipath")
                {
                    if (File.Exists(config[i, 1]))
                    {
                        IPATH = config[i, 1];
                        say("Loaded Python GTasks interface");
                    }
                    else
                    { 
                        IPATH = "";
                        say("Invalid interface path");
                    }
                }
            }
        }

        public void say(String msg) { toolStripStatusLabel1.Text = msg; }

        public void refresh()
        {
            say("Refreshing list...");
            Process pyInterface = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = PYPATH;
            startInfo.Arguments = IPATH + " -refresh";
            pyInterface.StartInfo = startInfo;
            pyInterface.Start();
            pyInterface.WaitForExit();
            displayFile(CONFIG);
            say("Success! Refreshed text file");
        }

        public void addTask(String task)
        {
            say("Adding new task...");
            Process pyInterface = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = PYPATH;
            startInfo.Arguments = IPATH + " -create \"" + task + "\"";
            pyInterface.StartInfo = startInfo;
            pyInterface.Start();
            pyInterface.WaitForExit();
            say("Success! Added new task");
        }

        public void removeTask(String task)
        {
            say("Removing task...");
            Process pyInterface = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = PYPATH;
            startInfo.Arguments = IPATH + " -remove \"" + task + "\"";
            pyInterface.StartInfo = startInfo;
            pyInterface.Start();
            pyInterface.WaitForExit();
            say("Success!");
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                displayFile(openFileDialog1.FileName);
                CONFIG = openFileDialog1.FileName;
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void add_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
            if (form3.DialogResult == DialogResult.OK)
            {
                addTask(form3.TODO);
            }
            refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            say("Removing selected items...");
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                removeTask(item.Text);
            }
            say("Success!");
            refresh();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void queue_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.ShowDialog();
            if (form4.DialogResult == DialogResult.OK)
            {
                for (int i = 0; i < form4.tasks.Items.Count; i++)
                {
                    addTask(form4.tasks.Items[i].Text);
                }
            }
        }
    }
}
