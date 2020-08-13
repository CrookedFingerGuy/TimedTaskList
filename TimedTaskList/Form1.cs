using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimedTaskList
{
   
    public partial class Form1 : Form
    {
        TimeSpan theSecond;
        TimeSpan startTime;
        TimeSpan currentTime;
        List<TimedTask> TTlist;
        int fontSize;
        string timeFormattingEstimated;
        string timeFormattingActual;
        string timeFormattingStatus;
        string fontName;

        public Form1()
        {
            InitializeComponent();

            theSecond = new TimeSpan(0, 0, 1);
            startTime = new TimeSpan(0, 0, 0);
            currentTime = new TimeSpan(0, 0, 0);
            TTlist = new List<TimedTask>();
            fontSize = 16;
            timeFormattingEstimated = "{0:##}"; //"{0:#0}:{1:00}";
            timeFormattingActual = "{0:#0}:{1:00}";
            timeFormattingStatus = "{0:#0}:{1:00}";
            fontName = "Tahoma";

            lvTasks.Columns.Add("Description", 295);
            lvTasks.Columns.Add("Est. Time", 100);
            lvTasks.Columns[1].TextAlign = HorizontalAlignment.Right;
            lvTasks.Columns.Add("Time", 80);
            lvTasks.Columns[2].TextAlign = HorizontalAlignment.Right;
            lvTasks.View = View.Details;
            lvTasks.CheckBoxes = true;
            lvTasks.InsertionMark.Color = Color.Red;
            lvTasks.AllowDrop = true;
            lvTasks.Font = new Font(fontName, fontSize);
        }

        private void StartTimer_ButtonClicked(object sender, EventArgs e)
        {
            if(TTlist.Count==0)
            {
                MessageBox.Show("Add a task before you start the timer");
                return;
            }
            timer1.Start();
        }

        private void PauseTimer_ButtonClicked(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int i = 0;
            while (i < lvTasks.Items.Count - 1 && lvTasks.Items[i].Checked == true)
            {
                i++;
            }

            if (TTlist[i].actualTime != currentTime)
                currentTime = currentTime.Add(TTlist[i].actualTime);

            currentTime = currentTime.Add(theSecond);
            if (lvTasks.Items.Count > 0 && lvTasks.Items[i].Checked != true)
            {
                lvTasks.Items[i].SubItems[2].Text = String.Format(timeFormattingActual, currentTime.Minutes, currentTime.Seconds);
                TTlist[i].actualTime = currentTime;
            }


            statusStrip1.Items[0].Text = "Total Estimated Time: "+ CalculateTotalEstimatedTime() + " || Time remaing for current task: " + String.Format(timeFormattingStatus,
                    TTlist[i].estimatedTime.Subtract(currentTime).Minutes,
                    TTlist[i].estimatedTime.Subtract(currentTime).Seconds);
        }

        public string CalculateTotalEstimatedTime()
        {
            TimeSpan totalTime = new TimeSpan(0,0,0);
            foreach (TimedTask tt in TTlist)
            {
                totalTime =totalTime.Add(tt.estimatedTime);
            }

            return String.Format(timeFormattingStatus, totalTime.Hours, totalTime.Minutes); ;
        }
        private void DeleteTask_ButtonClicked(object sender, EventArgs e)
        {
            if (lvTasks.SelectedIndices != null)
            {
                foreach (int x in lvTasks.SelectedIndices)
                {
                    TTlist.RemoveAt(x);
                    lvTasks.Items.RemoveAt(x);
                }
                statusStrip1.Items[0].Text = "Total Estimated Time: " + CalculateTotalEstimatedTime();
            }
        }

        private void EditTask_ButtonClicked(object sender, EventArgs e)
        {
            if (lvTasks.SelectedIndices != null && lvTasks.SelectedIndices.Count > 0)
            {
                EditTaskDialog editTD = new EditTaskDialog(TTlist[lvTasks.SelectedIndices[0]].description, TTlist[lvTasks.SelectedIndices[0]].estimatedTime);
                editTD.ShowDialog();
                if (editTD.update)
                {
                    int i = lvTasks.SelectedIndices[0];
                    TTlist[i].description = editTD.taskUpdate.description;
                    TTlist[i].estimatedTime = editTD.taskUpdate.estimatedTime;
                    lvTasks.Items[i] = new ListViewItem(new string[]
                            { TTlist[i].description,
                                String.Format(timeFormattingEstimated, TTlist[i].estimatedTime.Minutes, TTlist[i].estimatedTime.Seconds),
                                String.Format(timeFormattingActual, TTlist[i].actualTime.Minutes, TTlist[i].actualTime.Seconds)});
                    lvTasks.Items[i].Checked = TTlist[i].isChecked;
                }
                HandleListVewStrikout();
                statusStrip1.Items[0].Text = "Total Estimated Time: " + CalculateTotalEstimatedTime();
            }
        }

        private void SaveTasks_ButtonClicked(object sender, EventArgs e)
        {
            SaveFileDialog sDialog = new SaveFileDialog();
            if (sDialog.ShowDialog() == DialogResult.OK)
            {
                FileUtils.WriteToXmlFile<List<TimedTask>>(sDialog.FileName, TTlist);
                this.Text = "Timed Task List - " + sDialog.FileName;
            }
        }

        public void LoadTasks_ButtonClicked(object sender, EventArgs e)
        {
            OpenFileDialog lDialog = new OpenFileDialog();
            if (lDialog.ShowDialog() == DialogResult.OK)
            {
                LoadFromFile(lDialog.FileName);
            }
        }

        public void LoadFromFile(string fn)
        {
            TTlist.Clear();
            lvTasks.Items.Clear();
            TTlist = FileUtils.ReadFromXmlFile<List<TimedTask>>(fn);

            for (int i = 0; i < TTlist.Count; i++)
            {
                lvTasks.Items.Add(new ListViewItem(
                    new string[] { TTlist[i].description,
                            String.Format(timeFormattingEstimated, TTlist[i].estimatedTime.Minutes,TTlist[i].estimatedTime.Seconds),
                            String.Format(timeFormattingActual,TTlist[i].actualTime.Minutes,TTlist[i].actualTime.Seconds)}));
                lvTasks.Items[i].Checked = TTlist[i].isChecked;
            }
            HandleListVewStrikout();
            statusStrip1.Items[0].Text = "Total Estimated Time: " + CalculateTotalEstimatedTime();
        }

        public void FontChange_ButtonClicked(object sender,EventArgs e)
        {
            FontDialog fDialog = new FontDialog();
            if(fDialog.ShowDialog()==DialogResult.OK)
            {
                fontName = fDialog.Font.Name;
                HandleListVewStrikout();
            }
        }
        public void FontIncrease_ButtonClicked(object sender, EventArgs e)
        {
            fontSize += 2;
            lvTasks.Font = new Font(fontName, fontSize);
            foreach (ListViewItem item in lvTasks.Items)
            {
                if (item.Checked == true)
                {
                    item.SubItems[0].Font = new Font(fontName, fontSize, FontStyle.Strikeout);
                }
                else
                {
                    item.SubItems[0].Font = new Font(fontName, fontSize);
                }
                item.SubItems[1].Font = new Font(fontName, fontSize);
                item.SubItems[2].Font = new Font(fontName, fontSize);
            }
        }

        public void FontDecrease_ButtonClicked(object sender, EventArgs e)
        {
            if (fontSize > 10)
            {
                fontSize -= 2;
                lvTasks.Font = new Font(fontName, fontSize);
                foreach (ListViewItem item in lvTasks.Items)
                {
                    if (item.Checked == true)
                    {
                        item.SubItems[0].Font = new Font(fontName, fontSize, FontStyle.Strikeout);
                    }
                    else
                    {
                        item.SubItems[0].Font = new Font(fontName, fontSize);
                    }
                    item.SubItems[1].Font = new Font(fontName, fontSize);
                    item.SubItems[2].Font = new Font(fontName, fontSize);
                }
            }
        }

        public void About_ButtonClicked(object sender,EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog();
        }

        public void HandleListVewStrikout()
        {
            for (int i = 0; i < TTlist.Count; i++)
            {
                lvTasks.Items[i].UseItemStyleForSubItems = false;

                if (lvTasks.Items[i].Checked)
                {
                    lvTasks.Items[i].SubItems[0].Font = new Font(fontName, fontSize, FontStyle.Strikeout);
                }
                else
                {
                    lvTasks.Items[i].SubItems[0].Font = new Font(fontName, fontSize);
                }

                lvTasks.Items[i].SubItems[1].Font = new Font(fontName, fontSize);
                lvTasks.Items[i].SubItems[2].Font = new Font(fontName, fontSize);
            }
        }

        private void bAddTask_Click(object sender, EventArgs e)
        {
            int minutes;
            if (int.TryParse(tbEstimatedTime.Text, out minutes))
            {
                TimedTask temp = new TimedTask(tbDescription.Text, new TimeSpan(0, minutes, 0));
                TTlist.Add(temp);
                lvTasks.Items.Add(new ListViewItem(new string[]
                    { temp.description,
                        String.Format(timeFormattingEstimated,temp.estimatedTime.Minutes,temp.estimatedTime.Seconds),
                        String.Format(timeFormattingActual,temp.actualTime.Minutes,temp.actualTime.Seconds) }));

                tbDescription.Text = "";
                HandleListVewStrikout();
                statusStrip1.Items[0].Text = "Total Estimated Time: " + CalculateTotalEstimatedTime();
            }
            else
            {
                tbEstimatedTime.Text = "";
                tbEstimatedTime.Select();
            }

        }

        private void lvTasks_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!e.Item.Focused)
                return;

            int i = lvTasks.Items.IndexOf(lvTasks.FindItemWithText(e.Item.SubItems[0].Text));
            if (e.Item.Checked==true)
            {
                e.Item.UseItemStyleForSubItems = false;
                TTlist[i].isChecked = true;
                lvTasks.Items[i].SubItems[2].Text = String.Format(timeFormattingActual, TTlist[i].actualTime.Minutes, TTlist[i].actualTime.Seconds);
                currentTime = new TimeSpan(0, 0, 0);
            }
            else
            {
                //currentI is the index of the task that was accruing time before index i was unchecked
                int currentI = 0;
                while ((currentI < lvTasks.Items.Count - 1 && lvTasks.Items[currentI].Checked == true) || currentI==i)
                {
                    currentI++;
                }
                TTlist[currentI].actualTime = new TimeSpan(0, 0, 0);
                lvTasks.Items[currentI].SubItems[2].Text = String.Format(timeFormattingActual, TTlist[currentI].actualTime.Minutes, TTlist[currentI].actualTime.Seconds);

                e.Item.UseItemStyleForSubItems = false;
                TTlist[i].actualTime=TTlist[i].actualTime.Add(currentTime);
                TTlist[i].isChecked = false;
                lvTasks.Items[i].SubItems[2].Text = String.Format(timeFormattingActual, TTlist[i].actualTime.Minutes, TTlist[i].actualTime.Seconds);
                currentTime = TTlist[i].actualTime;
            }
            HandleListVewStrikout();
        }

        private void ResetTask_ButtonClicked(object sender, EventArgs e)
        {
            if (lvTasks.SelectedIndices != null && lvTasks.SelectedIndices.Count > 0)
            {
                int i = lvTasks.SelectedIndices[0];
                TTlist[i].actualTime = new TimeSpan(0, 0, 0);
                currentTime = new TimeSpan(0, 0, 0);
                lvTasks.Items[i] = new ListViewItem(new string[]
                    { TTlist[i].description,
                          String.Format(timeFormattingEstimated, TTlist[i].estimatedTime.Minutes, TTlist[i].estimatedTime.Seconds),
                          String.Format(timeFormattingActual, TTlist[i].actualTime.Minutes, TTlist[i].actualTime.Seconds)});
                lvTasks.Items[i].Checked = TTlist[i].isChecked;
                HandleListVewStrikout();
            }
        }

        private void tbEstimatedTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((!char.IsControl(e.KeyChar)&&!char.IsDigit(e.KeyChar))||e.KeyChar=='-')
            {
                e.Handled = true;
            }
        }

        private void lvTasks_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void lvTasks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lvTasks_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvTasks.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void lvTasks_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void lvTasks_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = lvTasks.PointToClient(new Point(e.X, e.Y));
            int i = lvTasks.InsertionMark.NearestIndex(targetPoint);

            if(i>-1)
            {
                Rectangle itemBounds = lvTasks.GetItemRect(i);
                if(targetPoint.X>itemBounds.Left+(itemBounds.Width/2))
                {
                    lvTasks.InsertionMark.AppearsAfterItem = true;
                }
                else 
                {
                    lvTasks.InsertionMark.AppearsAfterItem = false;
                }
            }
            lvTasks.InsertionMark.Index = i;
        }

        private void lvTasks_DragLeave(object sender, EventArgs e)
        {
            lvTasks.InsertionMark.Index = -1;
        }

        private void lvTasks_DragDrop(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                LoadFromFile(fileNames[0]);
            }

            int i = lvTasks.InsertionMark.Index;
            if (i == -1)
                return;

            if(lvTasks.InsertionMark.AppearsAfterItem)
            {
                i++;
            }            

            ListViewItem draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
            int index = lvTasks.Items.IndexOf(lvTasks.FindItemWithText(draggedItem.SubItems[0].Text));
            TimedTask temp = TTlist[index];
            TTlist.Insert(i, temp);
            if (i <= index)
                TTlist.RemoveAt(index + 1);
            else
                TTlist.RemoveAt(index);
            lvTasks.Items.Insert(i, (ListViewItem)draggedItem.Clone());
            lvTasks.Items.Remove(draggedItem);
        }
    }
}
