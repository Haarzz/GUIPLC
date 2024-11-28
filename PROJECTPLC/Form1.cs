using System;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace PROJECTPLC
{
    public partial class Form1 : Form
    {
        


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshComPorts();
            //serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
        }
       

        private void btn_connect_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            btn_connect.Text = "Connect";
            btn_connect.Enabled =false ;
            btn_discon.Enabled = true;

            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                //ToolastripstatusLabel1.ToString = serialPort1.PortName + " Disconnected.";
                btn_connect.Text = "Connect";
                timer1.Enabled = false;
            }
            else
            {
                try
                {
                    serialPort1.PortName = cmbPort.Text;
                    //serialPort1.BaudRate = cmbBaud.Text;
                    serialPort1.NewLine = "\r\n";
                    serialPort1.Open();
                    //ToolStripItem.Text = serialPort1.PortName + " Connected.";
                    timer1.Enabled = true;
                    SendData("00MS5e*CR");
                    toolStripLabel1.Text = serialPort1.PortName + " Connected.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void SendData(string dataToSend)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(dataToSend + Environment.NewLine);
            }
        }

        private delegate void DisplayDataDelegate(string item);

        private void DisplayData(string item)
        {
            if (InvokeRequired)
            {
                listBox1.Invoke(new DisplayDataDelegate(DisplayData), item);
            }
            else
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
                string itemWithTimestamp = $"{timestamp} -> {item}";

                listBox1.Items.Add(itemWithTimestamp);
            }
        }
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string data = serialPort1.ReadExisting();

            // Mengirim data yang diterima ke metode receivedData
            receivedData(data);
        }

        private void receivedData(string data)
        {
            // Misalnya menampilkan data yang diterima pada MessageBox
            
        }
        private void RefreshComPorts()
        {
            //cmbBaud.Items.Clear();
            cmbPort.Items.Clear();
            string[] portList = SerialPort.GetPortNames();
            foreach (string portName in portList)
            {
                cmbPort.Items.Add(portName);
                //cmbBaud.Items.Add(portName);
            }

            if (cmbPort.Items.Count > 0)
            {
                cmbPort.Text = cmbPort.Items[cmbPort.Items.Count - 1].ToString();
            }
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            receivedData("Data received at: " + DateTime.Now.ToString());
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write("@00WR00000002*" + '\r');
                }
                else
                {
                    MessageBox.Show("Serial Port belum Connect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending data: " + ex.Message);
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write("@00WR00000040*" + '\r');
                }
                else
                {
                    MessageBox.Show("Serial Port belum Connect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending data: " + ex.Message);
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write("@00WR00000020*" + '\r');
                }
                else
                {
                    MessageBox.Show("Serial Port belum Connect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending data: " + ex.Message);
            }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write("@00WR00000004*" + '\r');
                }
                else
                {
                    MessageBox.Show("Serial Port belum Connect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending data: " + ex.Message);
            }
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write("@00WR00000010*" + '\r');
                }
                else
                {
                    MessageBox.Show("Serial Port belum Connect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending data: " + ex.Message);
            }
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write("@00WR00000008*" + '\r');
                }
                else
                {
                    MessageBox.Show("Serial Port belum Connect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending data: " + ex.Message);
            }
        }

        private void btn_close_Click_1(object sender, EventArgs e)
        {
            Exit();
        }

        private void Exit()
        {
            this.Close();
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                RefreshComPorts();
            }
            else
            {
                MessageBox.Show("Harap disconnect serial komunikasi sebelum melakukan refresh.");
            }
        }

        private byte CalculateFCS(string inputText)
        {
            byte fcs = 0;

            foreach (char ch in inputText)
            {
                fcs ^= Convert.ToByte(ch);
            }

            return fcs;
        }

        private string ConvertFcsToAscii(byte fcs)
        {
            char highNibble = (char)((fcs >> 4) + '0');
            char lowNibble = (char)((fcs & 0xF) + '0');

            if (highNibble > '9') highNibble = (char)(highNibble + 7);
            if (lowNibble > '9') lowNibble = (char)(lowNibble + 7);

            return $"{highNibble}{lowNibble}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write("@00WR00000080*" + '\r');
                }
                else
                {
                    MessageBox.Show("Serial Port belum Connect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending data: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write("@00WR00000100*" + '\r');
                }
                else
                {
                    MessageBox.Show("Serial Port belum Connect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending data: " + ex.Message);
            }
        }



        private void btn_discon_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                btn_connect.Enabled = true;
                btn_discon.Enabled = false;
            }
            toolStripLabel1.Text = serialPort1.PortName + " Disconnected.";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string inputText = textBox1.Text;

            if (string.IsNullOrWhiteSpace(inputText))
            {
                MessageBox.Show("Harap masukkan teks pada kolom input.");
                return;
            }
            listBox2.Items.Clear();

            byte fcs = CalculateFCS(inputText);
            string fcsAscii = ConvertFcsToAscii(fcs);

            listBox2.Items.Add($"FCS: {fcsAscii}");
            listBox2.Items.Add($"Command: {inputText}{fcsAscii}*CR");
            SendData($"{inputText}{fcsAscii}*CR");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    }




