using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NModbus;
using NModbus.Serial;

namespace FourLED
{
    class AnalogInputViewer : INotifyPropertyChanged
    {       

        private AnalogInput selectedInput;
        private SerialPort sp = null;           //инициализация хендлера порта
        Thread thread = null;
        public ObservableCollection<string> Ports { get; set; }
       
        public ObservableCollection<AnalogInput> AnalogInputs { get; set; }

        public AnalogInput SelectedInput
        {
            get { return selectedInput; }
            set
            {
                selectedInput = value;
                OnPropertyChanged("SelectedInput");
            }
        }

        public AnalogInputViewer()
        {
            AnalogInputs = new ObservableCollection<AnalogInput>
            { new AnalogInput() };
            
            string[] Ports = SerialPort.GetPortNames();
            //try
            {
                sp = new SerialPort(Ports[0], 9600, Parity.None, 8, StopBits.One);
                if (sp != null)
                {
                    sp.Open();

                    sp.ReadTimeout = 1000;
                }
            }
            //catch { }
            
            if (Ports.Length > 0)
            {
                thread = new Thread(new ThreadStart(GetAnalogInput));
                thread.Start();
            }
        }

        public void GetAnalogInput()
        {   
            while (true)
            {
                //try
                {
                    ushort[] analogCode = ReturnMaster().ReadInputRegisters(0, 0, 1);
                    ushort[] analogPhysical = ReturnMaster().ReadHoldingRegisters(0, 0, 11);
                    if (analogCode.Length > 0)
                        AnalogInputs[0].Code = analogCode[0];                    
                   else
                        AnalogInputs[0].Code = 0;

                    if (analogPhysical.Length > 0)
                    {
                        AnalogInputs[0].Physical = UshortToFloat(analogPhysical[10], analogPhysical[9]);
                        AnalogInputs[0].LEDAmount = analogPhysical[8];
                        AnalogInputs[0].HighCode = analogPhysical[0];
                        AnalogInputs[0].LowCode = analogPhysical[1];
                    }
                    else
                    {
                        AnalogInputs[0].Physical = 0;
                        AnalogInputs[0].LEDAmount = 0;
                        AnalogInputs[0].HighCode = 0;
                        AnalogInputs[0].LowCode = 0;
                    }
                        
                }
                /*catch
                {
                    return;
                }*/
            }
            
        }

        private IModbusMaster ReturnMaster()
        {
            try
            {
                var adapter = new SerialPortAdapter(sp);
                // create modbus master
                var factory = new ModbusFactory();
                return factory.CreateRtuMaster(adapter);
            }
            catch
            {
                return null;
            }
        }
        private float UshortToFloat(ushort a, ushort b)
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(a & 0xFF);
            bytes[1] = (byte)(a >> 8);
            bytes[2] = (byte)(b & 0xFF);
            bytes[3] = (byte)(b >> 8);
            return BitConverter.ToSingle(bytes, 0);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
