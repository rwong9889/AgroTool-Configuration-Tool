using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;


namespace test2
{
    public partial class Form1 : Form
    {
        //Variable Definition
        Timer tm;
        String timestamp = DateTime.Now.ToString();
        public SerialPort serial_port1; //Serial Port 1 Variable
        public string DataReceived; //Variable to store RX data
        public string DataReceivedString; //Varialbe to store RX data converted to string
        public Form1()
        {
            InitializeComponent();
        }

        //Button to go to configuration page
        private void configure_button_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.DiscardOutBuffer();
                serialPort1.DiscardInBuffer();
                Form2 form2 = new Form2(serialPort1);
                form2.ShowDialog();
            }
        }

        //These Items will load when form is load.
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            com_port.Items.AddRange(ports);
            //not connected enable comboboxes
            com_port.Enabled = true;
            //disable configure button if not connected
            configure_button.Enabled = true;
            disconnect_button.Enabled = false;
            //Enter ASCII Encoding
          

        }

        //Button to establish connection to COM port
        private void connect_button_Click(object sender, EventArgs e)
        {
            try
            {
                timer1.Start();
                serial_port1 = new SerialPort(com_port.Text, Convert.ToInt32(baudrate.Text), (Parity)Enum.Parse(typeof(Parity), parity.Text), Convert.ToInt32(databits.Text), (StopBits)Enum.Parse(typeof(StopBits), stopbits.Text));
                serialPort1.PortName = com_port.Text;
                serialPort1.BaudRate = Convert.ToInt32(baudrate.Text);
                serialPort1.DataBits = Convert.ToInt32(databits.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopbits.Text);
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), parity.Text);
                serialPort1.Open();
                //check whether the port is connected then function executes
                if (serialPort1.IsOpen)
                {
                    serial_port1.Encoding = System.Text.Encoding.GetEncoding(1252);
                    textBox1.Text += DataReceivedString + Environment.NewLine;
                    connection_bar.Visible = true;
                    connection_bar.Style = ProgressBarStyle.Marquee;
                    com_connection_status.Text = "CONNECTING";
                    com_connection_status.ForeColor = Color.Red;
                    //disable combobox if it is connected
                    com_port.Enabled = false;
                    baudrate.Enabled = false;
                    stopbits.Enabled = false;
                    databits.Enabled = false;
                    parity.Enabled = false;
                    configure_button.Enabled = false;
                    refresh_button.Enabled = false;

                    // When software established connection with Agromon hardware
                    // Send 0xAA 0x55 \r\n to Agromon to enter Configuration mode automatically before the 10s timeout.
                    //    if (DataReceivedString == "OK\r\n")
                    //    {
                    //        textBox1.Text += "<TX> "+ timestamp + " " + "ªU" + "<CR><LF>"+ Environment.NewLine;
                    //        if (DataReceivedString == "CONFIGURATION MODE\r\nOK\r\n")
                    //        {
                    //            textBox1.Text += "<RX> " + timestamp + " " + "CONFIGURATION MODE " + "<CR><LF>" + Environment.NewLine;
                    //            textBox1.Text += "<RX> " + timestamp + " " + "OK " + "<CR><LF>" + Environment.NewLine;
                    //            //Recieved the data from agromon 
                    //            if (DataReceivedString == "COM\r\n")
                    //            {
                    //                network_set.Text = DataReceivedString;
                    //                textBox1.Text += "<RX> " + timestamp + " " + "COM " + "<CR><LF>" + Environment.NewLine;
                    //                network_set.Text = DataReceivedString;
                    //                textBox1.Text += "<RX> " + timestamp + " " + DataReceivedString + " <CR><LF>" + Environment.NewLine;

                    //                if (DataReceivedString == "SEN1\r\n")
                    //                {
                    //                    sensor1.Text = DataReceivedString;
                    //                    textBox1.Text += "<RX> " + timestamp + " " + "SEN1 " + "<CR><LF>" + Environment.NewLine;
                    //                    textBox1.Text += "<RX> " + timestamp + " " + DataReceivedString + " <CR><LF>" + Environment.NewLine;

                    //                    if (DataReceivedString == "SEN2\r\n")
                    //                    {
                    //                        sensor2.Text = DataReceivedString;
                    //                        textBox1.Text += "<RX> " + timestamp + " " + "SEN2 " + "<CR><LF>" + Environment.NewLine;
                    //                        textBox1.Text += "<RX> " + timestamp + " " + DataReceivedString + " <CR><LF>" + Environment.NewLine;

                    //                        if (DataReceivedString == "SEN3\r\n")
                    //                        {
                    //                            sensor3.Text = DataReceivedString;
                    //                            textBox1.Text += "<RX> " + timestamp + " " + "SEN3 " + "<CR><LF>" + Environment.NewLine;
                    //                            textBox1.Text += "<RX> " + timestamp + " " + DataReceivedString + " <CR><LF>" + Environment.NewLine;
                    //                        }
                    //                    }
                    //                }
                    //                network_status.Text = "CONNECTED";
                    //                network_status.ForeColor = Color.Green;
                    //            }

                    //        }
                    //    }
                    //    else
                    //    {
                    //        //When COM FRESH recieved
                    //        network_set.Text = "NOT AVAILABLE";
                    //        sensor1.Text = "NOT AVAILABLE";
                    //        sensor2.Text = "NOT AVAILABLE";
                    //        sensor3.Text = "NOT AVAILABLE";
                    //        network_status.Text = "NOT CONNECTED";
                    //        network_status.ForeColor = Color.Red;
                    //    }
                }
                configure_button.Enabled = true;
                disconnect_button.Enabled = true;
                connect_button.Enabled = false;
                com_connection_status.Text = "CONNECTED";
                com_connection_status.ForeColor = Color.Green;
                connection_bar.Style = ProgressBarStyle.Blocks;
            }
            catch (Exception)
            {
                MessageBox.Show("Please select the serial PORT or refresh if none serial PORT detected", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                com_connection_status.ForeColor = Color.Red;
                com_connection_status.Text = "NOT CONNECTED";
                //Enable Comboboxes when disconnected to COM Port.
                com_port.Enabled = true;
                baudrate.Enabled = true;
                databits.Enabled = true;
                stopbits.Enabled = true;
                parity.Enabled = true;
                connect_button.Enabled = true;
                refresh_button.Enabled = true;
                configure_button.Enabled = false;
                disconnect_button.Enabled = false;

            }
        }
        //private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        //{

        //    try
        //    {
        //        DataReceived = serialPort1.ReadLine();
        //        this.Invoke(new Action(ProcessingData));
        //    }
        //    catch (Exception err)
        //    {
        //        MessageBox.Show(err.Message);
        //    }
        //}

        //private void ProcessingData()
        //{
        //    DataReceivedString = DataReceived.ToString();
        //}

        //Timer to read always read data
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            try
            {
                DataReceived = serialPort1.ReadLine();
                String j = DataReceived.ToString();
                string[] DataReceivedStrings = j.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                textBox1.Text += "<RX> " + timestamp + " " + DataReceivedStrings[0] + " <CR><LF> " + Environment.NewLine;
                textBox1.Text += "<RX> " + timestamp + " " + DataReceivedStrings[1] + " <CR><LF> " + Environment.NewLine;
                if (DataReceivedStrings[1] == "OK")
                {
                    serialPort1.DiscardOutBuffer();
                    serialPort1.DiscardInBuffer();
                }

            }
            finally
            {

                serialPort1.Write("ªU\r\n");
                wait(2000);
                String m = serialPort1.ReadLine().ToString();
                string[] DataReceivedStringss = m.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (DataReceivedString == null)
                {

                }
                else
                {
                    if (DataReceivedStringss[0] == "START NORMAL MODE")
                    {
                        textBox1.Text += "<RX> " + timestamp + " " + "Cannot Enter CONFIGURATION MODE" + Environment.NewLine;
                        textBox1.Text += "<RX> " + timestamp + " " + "Please restart AGROMON" + Environment.NewLine;
                    }
                    if (DataReceivedStringss[0] == "\r")
                    {

                        textBox1.Text += "<TX> " + timestamp + " " + "0xAA 0x55" + "<CR><LF>" + Environment.NewLine;
                        textBox1.Text += "<RX> " + timestamp + "Hai " + DataReceivedString + "\r\n" + " <CR> <LF>" + Environment.NewLine;
                    }
                    timer1.Start();
                }
            }
        }

        //wait function
        private void wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }

        //Button to refresh COM port available.
        private void refresh_button_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString();
            if (!serialPort1.IsOpen)
            {

                try
                {
                    com_port.Items.Clear();
                    com_port.Text = "Select Port";
                    textBox1.Clear();
                    getAvailablePorts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Serial Port is Busy Or Open", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Close The Current Port Before Refreshing", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Button to disconnect COM port connection
        private void disconnect_button_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                DateTime dateTime = DateTime.Now;
                String timestamp = dateTime.ToString();
                serialPort1.DiscardOutBuffer();
                serialPort1.DiscardInBuffer();
                serialPort1.Close();
                connection_bar.Value = 0;

                com_connection_status.ForeColor = Color.Red;
                com_connection_status.Text = "NOT CONNECTED";
                network_status.ForeColor = Color.Red;
                network_status.Text = "NOT CONNECTED";

                //not connected enable comboboxes
                com_port.Enabled = true;
                baudrate.Enabled = true;
                databits.Enabled = true;
                stopbits.Enabled = true;
                parity.Enabled = true;

                //disable configure button if not connected
                configure_button.Enabled = false;
                disconnect_button.Enabled = false;
                connect_button.Enabled = true;
                refresh_button.Enabled = true;
                com_connection_status.Text = "Not Available";
                network_set.Text = "Not Available";
            }
            else
            {
                DateTime dateTime = DateTime.Now;
                String timestamp = dateTime.ToString();
                textBox1.Text += timestamp + " " + "No Serial Port is connected!" + Environment.NewLine;

            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.DiscardOutBuffer();
                serialPort1.DiscardInBuffer();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
            if (serialPort1.IsOpen)
            {
                //close connection when application exit
                serialPort1.Close();

            }
        }
        // Check available port when refresh button push
        void getAvailablePorts()
        {
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString();
            //this code gets the name of the port and port number
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
            {
                var portnames = SerialPort.GetPortNames();
                var ports2 = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());

                var portList = portnames.Select(n => n + " - " + ports2.FirstOrDefault(s => s.Contains(n))).ToList();

                foreach (var s in portList)
                {
                    textBox1.Text += timestamp + " " + s.ToString() + "\r\n" + "detected " + Environment.NewLine;
                }
            }
            //this code only gets the com port number
            string[] ports = SerialPort.GetPortNames();
            com_port.Items.AddRange(ports);
        }
    }
}


