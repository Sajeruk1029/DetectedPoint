using System;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace DetectedPoint
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        bool conn = false;

        static SerialPort port;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel = new System.Windows.Forms.TableLayoutPanel()
            {
                RowCount = 3,
                ColumnCount = 5,
                AutoSize = true,
                Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom,
                Dock = System.Windows.Forms.DockStyle.Fill
            };


            this.findArduino = new System.Windows.Forms.Button()
            {
                Text = "Найти ардуину",
                AutoSize = true
            };

            this.connectArduino = new System.Windows.Forms.Button()
            {
                Text = "Подключиться к ардуине",
                AutoSize = true,
                BackColor = System.Drawing.Color.Red
        };

            this.substionOneOn = new System.Windows.Forms.Button()
            {
                Text = "Включить подстанцию 1",
                AutoSize = true
            };

            this.substionOneOff = new System.Windows.Forms.Button()
            {
                Text = "Выключить подстанцию 1",
                AutoSize = true
            };
            
            this.substionTwoOn = new System.Windows.Forms.Button()
            {
                Text = "Включить подстанцию 2",
                AutoSize = true
            };
            
            this.substionTwoOff = new System.Windows.Forms.Button()
            {
                Text = "Выключить подстанцию 2",
                AutoSize = true
            };
            
            this.alarmOff = new System.Windows.Forms.Button()
            {
                Text = "Выключить сигнализацию",
                AutoSize = true
            };
            
            this.portsList = new System.Windows.Forms.ComboBox();

            this.components = new System.ComponentModel.Container();

            findArduino.Click += FindArduino_Click;
            connectArduino.Click += ConnectArduino_Click;
            substionOneOn.Click += SubstionOneOn_Click;
            substionOneOff.Click += SubstionOneOff_Click;
            substionTwoOn.Click += SubstionTwoOn_Click;
            substionTwoOff.Click += SubstionTwoOff_Click;
            alarmOff.Click += AlarmOff_Click;

            this.panel.Controls.Add(connectArduino, 0, 0);
            this.panel.Controls.Add(findArduino, 2, 0);
            this.panel.Controls.Add(substionOneOn, 0, 3);
            this.panel.Controls.Add(substionOneOff, 1, 3);
            this.panel.Controls.Add(substionTwoOn, 2, 3);
            this.panel.Controls.Add(substionTwoOff, 3, 3);
            this.panel.Controls.Add(alarmOff, 4, 3);

            this.panel.Controls.Add(portsList, 4, 0);

            this.Controls.Add(panel);
            this.AutoSize = true;

            this.Text = "DetectPoint";
        }

        private void AlarmOff_Click(object sender, System.EventArgs e)
        {
            if(port == null)
            {
                return;
            }    

            if(!port.IsOpen)
            {
                return;
            }

            try
            {
                port.WriteLine("ALARMOFF");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void SubstionTwoOff_Click(object sender, System.EventArgs e)
        {
            if (port == null)
            {
                return;
            }

            if (!port.IsOpen)
            {
                return;
            }

            try
            {
                port.WriteLine("LED2OFF");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void SubstionTwoOn_Click(object sender, System.EventArgs e)
        {
            if (port == null)
            {
                return;
            }

            if (!port.IsOpen)
            {
                return;
            }

            try
            {
                port.WriteLine("LED2ON");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void SubstionOneOff_Click(object sender, System.EventArgs e)
        {
            if (port == null)
            {
                return;
            }

            if (!port.IsOpen)
            {
                return;
            }

            try
            {
                port.WriteLine("LED1OFF");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void SubstionOneOn_Click(object sender, System.EventArgs e)
        {
            if (port == null)
            {
                return;
            }

            if (!port.IsOpen)
            {
                return;
            }

            try
            {
                port.WriteLine("LED1ON");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void ConnectArduino_Click(object sender, System.EventArgs e)
        {
            if(portsList.Items.Count <= 0)
            {
                MessageBox.Show("Вы должны сначала найти ардуину!", "Ошибка");
            }
            else
            {
                if(conn)
                {
                    port.Close();

                    connectArduino.BackColor = System.Drawing.Color.Red;
                    connectArduino.Text = "Подключиться";
                }
                else
                {
                    if(arduinoDetect(new SerialPort(portsList.SelectedItem.ToString())))
                    {
                        MessageBox.Show("Успех!", "Информация");
                    }
                    else
                    {
                        MessageBox.Show("Это не ардуина!", "Информация");

                        return;
                    }

                    port.BaudRate = 9600;
                    port.DataBits = 8;
                    port.StopBits = StopBits.One;
                    port.Handshake = Handshake.None;
                    port.DtrEnable = true;
                    port.Parity = Parity.None;

                    port.DataReceived += Port_DataReceived;

                    port.Open();
                    

                    connectArduino.BackColor = System.Drawing.Color.Green;
                    connectArduino.Text = "Отключиться";
                }

                conn = !conn;
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            if(sp.ReadExisting().ToString().Contains("A"))
            {
                DialogResult dialogResult = MessageBox.Show("Кажется у нас гости! Включить сигнализацию?", "Вопрос", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    port.WriteLine("ALARMON");
                }
            }
        }

        private void FindArduino_Click(object sender, System.EventArgs e)
        {
            portsList.Items.Clear();

            portsList.Items.AddRange(SerialPort.GetPortNames());

            if(portsList.Items.Count > 0)
            {
                portsList.SelectedIndex = 0;
            }
        }

        bool arduinoDetect(SerialPort serialPort)
        {
            string msg = "";

            try
            {
                serialPort.Open();

                //System.Threading.Thread.Sleep(1000);

                msg = serialPort.ReadLine();

                if (msg.Contains("N"))
                {
                    serialPort.WriteLine("C");

                    port = new SerialPort();
                    port.PortName = serialPort.PortName;

                    serialPort.Close();

                    return true;
                }
                else
                {
                    serialPort.Close();

                    return false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");

                serialPort.Close();

                return false;
            }

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel panel;
        
        private System.Windows.Forms.Button findArduino;
        private System.Windows.Forms.Button connectArduino;
        private System.Windows.Forms.Button substionOneOn;
        private System.Windows.Forms.Button substionOneOff;
        private System.Windows.Forms.Button substionTwoOn;
        private System.Windows.Forms.Button substionTwoOff;
        private System.Windows.Forms.Button alarmOff;

        private System.Windows.Forms.ComboBox portsList;
    }
}

