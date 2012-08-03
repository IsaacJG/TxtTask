using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TxtTask
{
    public partial class Form4 : Form
    {
        public ListView tasks;
        public Form4()
        {
            InitializeComponent();
        }

        public void refresh()
        {
            if (listView1.Items.Count > 0)
            {
                remove.Enabled = true;
                done.Enabled = true;
            }
            else
            {
                remove.Enabled = false;
                done.Enabled = false;
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
            if (form3.DialogResult == DialogResult.OK)
            {
                listView1.Items.Add(form3.TODO);
            }
            refresh();
        }

        private void remove_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    listView1.Items.Remove(listView1.SelectedItems[i]);
                }
            }
            refresh();
        }

        private void done_Click(object sender, EventArgs e)
        {
            tasks = listView1;
        }
    }
}
