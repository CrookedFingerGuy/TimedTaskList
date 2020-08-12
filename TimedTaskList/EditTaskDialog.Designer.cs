namespace TimedTaskList
{
    partial class EditTaskDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bUpdateTask = new System.Windows.Forms.Button();
            this.tbUEstimatedTime = new System.Windows.Forms.TextBox();
            this.tbUDescription = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // bUpdateTask
            // 
            this.bUpdateTask.Location = new System.Drawing.Point(397, 11);
            this.bUpdateTask.Name = "bUpdateTask";
            this.bUpdateTask.Size = new System.Drawing.Size(75, 23);
            this.bUpdateTask.TabIndex = 2;
            this.bUpdateTask.Text = "Update Task";
            this.bUpdateTask.UseVisualStyleBackColor = true;
            this.bUpdateTask.Click += new System.EventHandler(this.bUpdateTask_Click);
            // 
            // tbUEstimatedTime
            // 
            this.tbUEstimatedTime.Location = new System.Drawing.Point(351, 12);
            this.tbUEstimatedTime.Name = "tbUEstimatedTime";
            this.tbUEstimatedTime.Size = new System.Drawing.Size(40, 23);
            this.tbUEstimatedTime.TabIndex = 1;
            this.tbUEstimatedTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbUEstimatedTime_KeyPress);
            // 
            // tbUDescription
            // 
            this.tbUDescription.Location = new System.Drawing.Point(12, 12);
            this.tbUDescription.Name = "tbUDescription";
            this.tbUDescription.Size = new System.Drawing.Size(333, 23);
            this.tbUDescription.TabIndex = 0;
            // 
            // EditTaskDialog
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 47);
            this.Controls.Add(this.tbUDescription);
            this.Controls.Add(this.tbUEstimatedTime);
            this.Controls.Add(this.bUpdateTask);
            this.Name = "EditTaskDialog";
            this.Text = "Edit Task Dialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bUpdateTask;
        private System.Windows.Forms.TextBox tbUEstimatedTime;
        private System.Windows.Forms.TextBox tbUDescription;
    }
}