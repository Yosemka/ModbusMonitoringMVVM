using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace FourLED
{
    class AnalogInput : INotifyPropertyChanged
    {
        private int highCode;
        private int lowCode;
        private double highPhysical;
        private double lowPhysical;
        private double physical;
        private int code;
        private int LEDamount;

        public int HighCode
        {
            get { return highCode; }
            set { highCode = value; OnPropertyChanged("HighCode"); }
        }
        public int LowCode
        {
            get { return lowCode; }
            set { lowCode = value; OnPropertyChanged("LowCode"); }
        }
        public double HighPhysical
        {
            get { return highPhysical; }
            set { highPhysical = value; OnPropertyChanged("HighPhysical"); }
        }
        public double LowPhysical
        {
            get { return lowPhysical; }
            set { lowPhysical = value; OnPropertyChanged("LowPhysical"); }
        }
        public double Physical
        {
            get { return physical; }
            set { physical = value; OnPropertyChanged("Physical"); }
        }
        public int Code
        {
            get { return code; }
            set { code = value; OnPropertyChanged("Code"); }
        }
        public int LEDAmount
        {
            get { return LEDamount; }
            set { LEDamount = value; OnPropertyChanged("LEDAmount"); }
        }
      
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
