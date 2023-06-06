using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.GUI
{
    internal class Config
    {
        public bool IsWakeSimpleEEW = true;
        public bool IsTopSimpleEEW = true;
        public Struct.ConfigBox.Notification_EEW_Nationwide.Enums NoticeNationWide = Struct.ConfigBox.Notification_EEW_Nationwide.Enums.ALL;
        public Struct.ConfigBox.Notification_EEW_Area.Enums NoticeArea = Struct.ConfigBox.Notification_EEW_Area.Enums.Int1;
    }
}
