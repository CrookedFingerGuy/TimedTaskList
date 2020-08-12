using System;
using System.Collections.Generic;
using System.Text;

namespace TimedTaskList
{    
    [Serializable]
    public class TimedTask
    {
        public string description;
        public TimeSpan estimatedTime;
        public TimeSpan actualTime;
        public bool isChecked;

        public TimedTask(string des,TimeSpan ts)
        {
            description = des;
            estimatedTime = ts;
            actualTime = new TimeSpan(0, 0, 0);
            isChecked = false;
        }

        public TimedTask()
        {
        }
    }
}
