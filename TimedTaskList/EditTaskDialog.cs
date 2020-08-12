using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TimedTaskList
{
    public partial class EditTaskDialog : Form
    {
        public TimedTask taskUpdate;
        public bool update;

        public EditTaskDialog(string des, TimeSpan ts)
        {
            InitializeComponent();            
            taskUpdate = new TimedTask(des,ts);
            update = false;

            tbUDescription.Text = taskUpdate.description;
            tbUEstimatedTime.Text = taskUpdate.estimatedTime.TotalMinutes.ToString();
        }

        private void bUpdateTask_Click(object sender, EventArgs e)
        {
            int minutes;
            if (int.TryParse(tbUEstimatedTime.Text, out minutes))
            {
                taskUpdate.description = tbUDescription.Text;
                taskUpdate.estimatedTime = new TimeSpan(0, minutes, 0);
                update = true;
                this.Close();
            }
            else
            {
                tbUEstimatedTime.Text = "";
                tbUEstimatedTime.Select();
            }
        }

        private void tbUEstimatedTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) || e.KeyChar == '-')
            {
                e.Handled = true;
            }
        }
    }
}
