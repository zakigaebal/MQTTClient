using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Reflection;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace MQTTClient
{
	public partial class Form1 : Form
	{
		static MqttClient clientUser;
		DataTable dt = new DataTable();
		DataTable dt2 = new DataTable();
		private delegate void ShowCallBack(string myStr1, string myStr2, DataGridView dgv);
		private delegate void myUICallBack(string myStr, TextBox ctl);
		string startupPath = Application.StartupPath + @"\MqttClient.ini";
		public Form1()
		{
			InitializeComponent();
			//폼 닫기 이벤트 선언
			this.FormClosed += Form_Closing;
		}

		#region ini 입력 메소드
		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
		#endregion

		private void Form1_Load(object sender, EventArgs e)
		{
			this.AcceptButton = this.buttonConnect;
			comboBoxQos.SelectedIndex = 0;
			mqttRecord();
			textBoxPort.Text = "1883";
			comboBoxQos.SelectedIndex = 1;
			radioButton1.Checked = true;
			buttonMeter_Click(sender, e);
			initload();
		}

		private void buttonTopicSave_Click(object sender, EventArgs e)
		{
			StreamWriter sw;
			sw = new StreamWriter("Sub.txt");
			int nCount = listBoxSub.Items.Count;
			for (int i = 0; i < nCount; i++)
			{
				listBoxSub.Items[i] += "\r\n";
				sw.Write(listBoxSub.Items[i]);
			}
			sw.Close();
		}
		private void buttonTopicOpen_Click(object sender, EventArgs e)
		{
			listBoxSub.Items.Clear();
			string currentPath = System.IO.Directory.GetCurrentDirectory();
			StreamReader file = new StreamReader(currentPath + "\\Sub.txt", Encoding.Default); 
			string s = ""; 
			while (s != null)
			{ 
				s = file.ReadLine(); 
				if (!string.IsNullOrEmpty(s)) listBoxSub.Items.Add(s); 
			}
			file.Close();
		}
		private void initload()
		{
			// ini값을 집어넣을 변수 선언
			StringBuilder host = new StringBuilder();
			StringBuilder topic = new StringBuilder();
			StringBuilder pub1 = new StringBuilder();
			StringBuilder pub2 = new StringBuilder();
			StringBuilder pub3 = new StringBuilder();
			StringBuilder pub4 = new StringBuilder();
			StringBuilder pub5 = new StringBuilder();
			StringBuilder pub6 = new StringBuilder();
			StringBuilder pub7 = new StringBuilder();
			StringBuilder pub8 = new StringBuilder();
			StringBuilder pub9 = new StringBuilder();
			StringBuilder pub10 = new StringBuilder();
			StringBuilder m1 = new StringBuilder();
			StringBuilder m2 = new StringBuilder();
			StringBuilder m3 = new StringBuilder();
			StringBuilder m4 = new StringBuilder();
			StringBuilder m5 = new StringBuilder();
			StringBuilder m6 = new StringBuilder();
			StringBuilder m7 = new StringBuilder();
			StringBuilder m8 = new StringBuilder();
			StringBuilder m9 = new StringBuilder();
			StringBuilder m10 = new StringBuilder();
			StringBuilder red = new StringBuilder();
			StringBuilder green = new StringBuilder();
			StringBuilder yellow = new StringBuilder();
			StringBuilder gray = new StringBuilder();
			StringBuilder navy = new StringBuilder();
			StringBuilder purple = new StringBuilder();
			StringBuilder lime = new StringBuilder();
			StringBuilder pink = new StringBuilder();
			StringBuilder orange = new StringBuilder();
			StringBuilder blue = new StringBuilder();
			StringBuilder black = new StringBuilder();

			// ini파일에서 데이터를 불러옴
			// GetPrivateProfileString("카테고리", "Key값", "기본값", "저장할 변수", "불러올 경로");
			GetPrivateProfileString("MqttClient", "LastHostName", "", host, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastSubTopic", "", topic, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastPubTopic1", "", pub1, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastPubTopic2", "", pub2, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastPubTopic3", "", pub3, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastPubTopic4", "", pub4, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastPubTopic5", "", pub5, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastPubTopic6", "", pub6, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastPubTopic7", "", pub7, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastPubTopic8", "", pub8, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastPubTopic9", "", pub9, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastPubTopic10", "", pub10, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastMessage1", "", m1, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastMessage2", "", m2, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastMessage3", "", m3, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastMessage4", "", m4, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastMessage5", "", m5, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastMessage6", "", m6, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastMessage7", "", m7, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastMessage8", "", m8, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastMessage9", "", m9, 3200, startupPath);
			GetPrivateProfileString("MqttClient", "LastMessage10", "", m10, 3200, startupPath);
			GetPrivateProfileString("Color", "Red", "", red, 3200, startupPath);
			GetPrivateProfileString("Color", "Green", "", green, 3200, startupPath);
			GetPrivateProfileString("Color", "Yellow", "", yellow, 3200, startupPath);
			GetPrivateProfileString("Color", "Gray", "", gray, 3200, startupPath);
			GetPrivateProfileString("Color", "Navy", "", navy, 3200, startupPath);
			GetPrivateProfileString("Color", "Purple", "", purple, 3200, startupPath);
			GetPrivateProfileString("Color", "Lime", "", lime, 3200, startupPath);
			GetPrivateProfileString("Color", "Pink", "", pink, 3200, startupPath);
			GetPrivateProfileString("Color", "Orange", "", orange, 3200, startupPath);
			GetPrivateProfileString("Color", "Blue", "", blue, 3200, startupPath);
			GetPrivateProfileString("Color", "Black", "", black, 3200, startupPath);

			// 텍스트박스에 ini파일에서 가져온 데이터를 넣는다
			textBoxHost.Text = host.ToString();
			textBoxSubTopic.Text = topic.ToString();
			textBoxPT1.Text = pub1.ToString();
			textBoxPT2.Text = pub2.ToString();
			textBoxPT3.Text = pub3.ToString();
			textBoxPT4.Text = pub4.ToString();
			textBoxPT5.Text = pub5.ToString();
			textBoxPT6.Text = pub6.ToString();
			textBoxPT7.Text = pub7.ToString();
			textBoxPT8.Text = pub8.ToString();
			textBoxPT9.Text = pub9.ToString();
			textBoxPT10.Text = pub10.ToString();
			textBoxM1.Text = m1.ToString();
			textBoxM2.Text = m2.ToString();
			textBoxM3.Text = m3.ToString();
			textBoxM4.Text = m4.ToString();
			textBoxM5.Text = m5.ToString();
			textBoxM6.Text = m6.ToString();
			textBoxM7.Text = m7.ToString();
			textBoxM8.Text = m8.ToString();
			textBoxM9.Text = m9.ToString();
			textBoxM10.Text = m10.ToString();
			textBoxRed.Text = red.ToString();
			textBoxGreen.Text = green.ToString();
			textBoxYellow.Text = yellow.ToString();
			textBoxGray.Text = gray.ToString();
			textBoxNavy.Text = navy.ToString();
			textBoxPurple.Text = purple.ToString();
			textBoxLime.Text = lime.ToString();
			textBoxPink.Text = pink.ToString();
			textBoxOrange.Text = orange.ToString();
			textBoxBlue.Text = blue.ToString();
			textBoxBlack.Text = black.ToString();
		}

		

		private void initCloseMethod()
		{
			// ini파일에 등록
			// WritePrivateProfileString("카테고리", "Key값", "Value", "저장할 경로");
			WritePrivateProfileString("MqttClient", "LastHostName", textBoxHost.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastSubTopic", textBoxSubTopic.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastPubTopic1", textBoxPT1.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastPubTopic2", textBoxPT2.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastPubTopic3", textBoxPT3.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastPubTopic4", textBoxPT4.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastPubTopic5", textBoxPT5.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastPubTopic6", textBoxPT6.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastPubTopic7", textBoxPT7.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastPubTopic8", textBoxPT8.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastPubTopic9", textBoxPT9.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastPubTopic10", textBoxPT10.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastMessage1", textBoxM1.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastMessage2", textBoxM2.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastMessage3", textBoxM3.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastMessage4", textBoxM4.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastMessage5", textBoxM5.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastMessage6", textBoxM6.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastMessage7", textBoxM7.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastMessage8", textBoxM8.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastMessage9", textBoxM9.Text, startupPath);
			WritePrivateProfileString("MqttClient", "LastMessage10", textBoxM10.Text, startupPath);
			WritePrivateProfileString("Color", "Red", textBoxRed.Text, startupPath);
			WritePrivateProfileString("Color", "Green", textBoxGreen.Text, startupPath);
			WritePrivateProfileString("Color", "Yellow", textBoxYellow.Text, startupPath);
			WritePrivateProfileString("Color", "Gray", textBoxGray.Text, startupPath);
			WritePrivateProfileString("Color", "Navy", textBoxNavy.Text, startupPath);
			WritePrivateProfileString("Color", "Purple", textBoxPurple.Text, startupPath);
			WritePrivateProfileString("Color", "Lime", textBoxLime.Text, startupPath);
			WritePrivateProfileString("Color", "Pink", textBoxPink.Text, startupPath);
			WritePrivateProfileString("Color", "Orange", textBoxOrange.Text, startupPath);
			WritePrivateProfileString("Color", "Blue", textBoxBlue.Text, startupPath);
			WritePrivateProfileString("Color", "Black", textBoxBlack.Text, startupPath);
		}

		private void mqttRecord()
		{
			dt.Columns.Add("Time", typeof(string));
			dt.Columns.Add("Topic");
			dt.Columns.Add("Message");
			dt.DefaultView.Sort = "Time desc";
			dataGridViewMessage.DataSource = dt;


			//dataGridViewMessage.Columns.Add("0", "Time");
			//dataGridViewMessage.Columns.Add("1", "Topic");
			//dataGridViewMessage.Columns.Add("2", "Message");

			dataGridViewMessage.ReadOnly = true;
			dataGridViewMessage.RowHeadersVisible = false;
			dataGridViewMessage.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridViewMessage.Columns[dataGridViewMessage.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewMessage.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			dataGridViewMessage.Columns[1].Width = 200;

		}

		private void logs()
		{
			//텍스트 파일 쓰기
			//StreamWriter 객체를 생성한다. 입력파라미터로 File Path나 파일스트림을 사용한다
			using (StreamWriter wr = new StreamWriter(@"C:\Temp\data/txt"))
			{
				//WriteLine()을 써서 한 라인 씩 문자열을 쓴다
				wr.WriteLine("Line 1");
				wr.WriteLine("Line 2");

				//숫자를 쓰기
				decimal val = 1024.19M;
				wr.WriteLine(val);

				//문자배열을 쓰기
				char[] array = new char[5] { 'A', 'B', 'C', 'D', 'E' };
				wr.WriteLine(array);
			}
		}

		private void logSave()
		{
			//폴더 있는지 확인하고 생성하기
			if (!Directory.Exists("Log"))
			{
				System.IO.Directory.CreateDirectory("Log");
			}
			string currentPath = System.IO.Directory.GetCurrentDirectory();
			//This line of code creates a text file for the data export.
			System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + currentPath + "\\Log\\" + "log-MQTT-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");

			try
			{
				string Line = "";
				for (int r = 0; r <= dataGridViewMessage.Rows.Count - 1; r++)
				{

					// 한 라인씩 읽기
					//IEnumerable<string> lines = File.ReadAllLines(@"" + currentPath + "\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
					//foreach (string line in lines)
					//{
				    	
				//	}

					for (int c = 0; c <= dataGridViewMessage.Columns.Count - 1; c++)
					{
						Line = Line + dataGridViewMessage.Rows[r].Cells[c].Value;
						if (c != dataGridViewMessage.Columns.Count - 1)
						{
							Line = Line + "";
						}
						file.Write(Line);
					}

				}
					file.Close();
			}
			catch (System.Exception err)
			{
				System.Windows.Forms.MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				file.Close();
			}
		}


		private void buttonSubscribe2_Click(object sender, EventArgs e)
		{
			if (textBoxSubTopic.Text.Length == 0)
			{
				return;
			}
			else
			{
				try
				{
					clientUser.Subscribe(new string[] { listBoxSub.SelectedItem.ToString() }, new byte[] { (byte)comboBoxQos.SelectedIndex });
					dataGridViewMessage.DoubleBuffered(true);
					dataGridViewMessage.SuspendLayout();
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void ShowMessage(string myStr1, string myStr2, DataGridView dgv)
		{
			if (this.InvokeRequired)
			{
				ShowCallBack myUpdate = new ShowCallBack(ShowMessage);
				this.Invoke(myUpdate, myStr1, myStr2, dgv);
			}
			else
			{
					dt.Rows.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " - [MQTT] " + myStr1 + " - " + myStr2 + Environment.NewLine);
				//dgv.Rows.Add(myStr + Environment.NewLine);
				//	dataGridViewMessage.Rows.Add(DateTime.Now.ToString("HH:mm:ss:fff"), myStr1 + Environment.NewLine, myStr2 + Environment.NewLine);
				//dt.Rows.Add(DateTime.Now.ToString("HH:mm:ss:fff"), myStr1 + Environment.NewLine, myStr2 + Environment.NewLine);
			//	dt.Rows.Add(DateTime.Now.ToString("HH:mm:ss:fff "), myStr1, myStr2 + Environment.NewLine);
				dataGridViewMessage.CurrentCell = dataGridViewMessage.Rows[0].Cells[0];
				dataGridViewMessage.DataSource = dt;
				logSave();
				for (int i = 0; i < 10; i++)
				{
					//string value = dataGridViewMessage.Rows[i][0].ToString();
					//string newValue = value.Substring(1, 2);
					//dataGridViewMessage.Rows[i].Cells[0].Value = newValue;
				}

			}
		}

		private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs data)
		{
			ShowMessage(data.Topic, System.Text.Encoding.UTF8.GetString(data.Message), dataGridViewMessage);
			myUI(System.Text.Encoding.UTF8.GetString(data.Message), MessageTextBox);
		}

		private void myUI(string myStr, TextBox ctl)
		{
			if (this.InvokeRequired)
			{
				myUICallBack myUpdate = new myUICallBack(myUI);
				this.Invoke(myUpdate, myStr, ctl);
			}
			else
			{
				ctl.AppendText(myStr + Environment.NewLine);
			}
		}

		private void buttonLocal_Click(object sender, EventArgs e)
		{
			textBoxHost.Text = "127.0.0.1";
		}

		private void buttonMqttServer_Click(object sender, EventArgs e)
		{
			textBoxHost.Text = "103.60.126.23";
		}

		private void buttonConnect_Click(object sender, EventArgs e)
		{
			int port;
			if (textBoxHost.Text.Length == 0)
			{
				return;
			}
			else if (!Int32.TryParse(textBoxPort.Text, out port))
			{
				return;
			}
			else
			{
				try
				{
					clientUser = new MqttClient(textBoxHost.Text);
					clientUser.Connect(Guid.NewGuid().ToString());
					clientUser.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(client_MqttMsgPublishReceived);
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
				if (clientUser != null && clientUser.IsConnected)
				{
					this.AcceptButton = this.buttonPublish;
					buttonSubscribe.Enabled = true;
					buttonPublish.Enabled = true;
					buttonPublish2.Enabled = true;
					buttonPublish3.Enabled = true;
					buttonPublish4.Enabled = true;
					buttonPublish5.Enabled = true;
					buttonPublish6.Enabled = true;
					buttonPublish7.Enabled = true;
					buttonPublish8.Enabled = true;
					buttonPublish9.Enabled = true;
					buttonPublish10.Enabled = true;
					MessageTextBox.Clear();
					buttonUnscribe.Enabled = true;
					buttonDisconnect.Enabled = true;
					textBoxHost.Enabled = false;
					textBoxPort.Enabled = false;
					buttonConnect.Enabled = false;
				}
			}
		}

		private void buttonSubscribe_Click(object sender, EventArgs e)
		{
		
			if (textBoxSubTopic.Text.Length == 0)
			{
				return;
			}
			else
			{
				try
				{
				
					clientUser.Subscribe(new string[] { textBoxSubTopic.Text }, new byte[] { (byte)comboBoxQos.SelectedIndex });
				
				

					//

					List<string> result = new List<string>();

					result.Add(textBoxSubTopic.Text);
					////중복 제거
					result = result.Distinct().ToList();

					//List<string> vals = new List<string>();




					//if (listBoxSub.Items.Contains(listBoxSub.Items.ToString()))
					//{
					//	return;
					//}
					//	else
					//{ 
					//	listBoxSub.Items.Add(textBoxSubTopic.Text);
					//  textBoxSubTopic.Text = "";
					//}

			
				  var _items = result.Distinct().ToArray();
					this.listBoxSub.Items.Clear();
					foreach (var item in _items)
						{
						this.listBoxSub.Items.Add(item);
						}

					//var _items = this.listBoxSub.Items.Cast<string>().Distinct().ToArray();
					//this.listBoxSub.Items.Clear();
					//foreach (var item in result)
					//{
					//	this.listBoxSub.Items.Add(item);
					//}






					dataGridViewMessage.DoubleBuffered(true);
					dataGridViewMessage.SuspendLayout();
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}



		private void buttonDisconnect_Click(object sender, EventArgs e)
		{
			if (clientUser != null && clientUser.IsConnected) clientUser.Disconnect();
			listBoxSub.Items.Clear();
			textBoxPort.Enabled = true;
			textBoxHost.Enabled = true;
			buttonConnect.Enabled = true;
			buttonDisconnect.Enabled = false;
			buttonUnscribe.Enabled = false;
			buttonPublish.Enabled = false;
			buttonSubscribe.Enabled = false;
		}

		private void buttonUnscribe_Click(object sender, EventArgs e)
		{
			try 
			{ 
			if (listBoxSub.SelectedItem == null)
			{
				return;
			}
			else
			{
				//listBoxSub.Items.Clear();
				clientUser.Unsubscribe(new string[] { listBoxSub.SelectedItem.ToString() });
				listBoxSub.Items.Remove(listBoxSub.SelectedItem);
			}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void buttonClear_Click(object sender, EventArgs e)
		{
			MessageTextBox.Clear();
			((DataTable)dataGridViewMessage.DataSource).Rows.Clear();
		}

		private void checkBoxTopicpub_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBoxTopicpub.Checked == true)
			{
				textBoxPT2.Enabled = false;
				textBoxPT3.Enabled = false;
				textBoxPT4.Enabled = false;
				textBoxPT5.Enabled = false;
				textBoxPT6.Enabled = false;
				textBoxPT7.Enabled = false;
				textBoxPT8.Enabled = false;
				textBoxPT9.Enabled = false;
				textBoxPT10.Enabled = false;
			}
			else if (checkBoxTopicpub.Checked == false)
			{
				textBoxPT2.Enabled = true;
				textBoxPT3.Enabled = true;
				textBoxPT4.Enabled = true;
				textBoxPT5.Enabled = true;
				textBoxPT6.Enabled = true;
				textBoxPT7.Enabled = true;
				textBoxPT8.Enabled = true;
				textBoxPT9.Enabled = true;
				textBoxPT10.Enabled = true;
			}
		}

		private void buttonPublish_Click(object sender, EventArgs e)
		{
			try
			{
				if (textBoxM1.Text.Length == 0)
				{
					return;
				}
				else if (textBoxSubTopic.Text.Length == 0)
				{
					return;
				}
				else if (textBoxPT1.Text.IndexOf('#') != -1 || textBoxPT1.Text.IndexOf('+') != -1)
				{
					return;
				}
				else
					clientUser.Publish(textBoxPT1.Text, Encoding.UTF8.GetBytes(textBoxM1.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
			}

			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		private void buttonPublish2_Click(object sender, EventArgs e)
		{
			if (checkBoxTopicpub.Checked == true)
			{
				clientUser.Publish(textBoxPT1.Text, Encoding.UTF8.GetBytes(textBoxM2.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
			}
			else if (checkBoxTopicpub.Checked == false)
			{
				clientUser.Publish(textBoxPT2.Text, Encoding.UTF8.GetBytes(textBoxM2.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
			}
		}

		private void buttonPublish3_Click(object sender, EventArgs e)
		{
			if (checkBoxTopicpub.Checked == true)
			{
				clientUser.Publish(textBoxPT1.Text, Encoding.UTF8.GetBytes(textBoxM3.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
			}
			else if (checkBoxTopicpub.Checked == false)
			{
				clientUser.Publish(textBoxPT3.Text, Encoding.UTF8.GetBytes(textBoxM3.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
			}
		}

		private void buttonPublish4_Click(object sender, EventArgs e)
		{
			{
				if (checkBoxTopicpub.Checked == true)
				{
					clientUser.Publish(textBoxPT1.Text, Encoding.UTF8.GetBytes(textBoxM4.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
				else if (checkBoxTopicpub.Checked == false)
				{
					clientUser.Publish(textBoxPT4.Text, Encoding.UTF8.GetBytes(textBoxM4.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
			}
		}


		private void dataGridViewMessage_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			{
				// 특정값을 가진 열을 좀 다르게 보여주고 싶을 때
				if (e.ColumnIndex == 2)
				{
					if (e.Value != null)
					{
						string text = e.Value.ToString();
						if (text.Contains(textBoxRed.Text))
						{
							if (textBoxRed.Text == "")
							{
								return;
							}
							else
								// if(text.Contains(",") == false)
								e.CellStyle.BackColor = Color.Red;

						}
						if (text.Contains(textBoxGreen.Text))
						{
							if (textBoxGreen.Text == "")
							{
								return;
							}
							else
								e.CellStyle.BackColor = Color.Green;
							  e.CellStyle.ForeColor = Color.White;
						}
						if (text.Contains(textBoxYellow.Text))
						{
							if (textBoxYellow.Text == "")
							{
								return;
							}
							else
								e.CellStyle.BackColor = Color.Yellow;
						}
						if (text.Contains(textBoxGray.Text))
						{
							if (textBoxGray.Text == "")
							{
								return;
							}
							else
								e.CellStyle.BackColor = Color.Gray;
						}
						if (text.Contains(textBoxNavy.Text))
						{
							if (textBoxNavy.Text == "")
							{
								return;
							}
							else
								e.CellStyle.BackColor = Color.Navy;
							e.CellStyle.ForeColor = Color.White;
						}
						if (text.Contains(textBoxPurple.Text))
						{
							if (textBoxPurple.Text == "")
							{
								return;
							}
							else
								e.CellStyle.BackColor = Color.Purple;
							e.CellStyle.ForeColor = Color.White;
						}
						if (text.Contains(textBoxLime.Text))
						{
							if (textBoxLime.Text == "")
							{
								return;
							}
							else
								e.CellStyle.BackColor = Color.Lime;
						}
						if (text.Contains(textBoxPink.Text))
						{
							if (textBoxPink.Text == "")
							{
								return;
							}
							else
								e.CellStyle.BackColor = Color.Pink;
						}
						if (text.Contains(textBoxOrange.Text))
						{
							if (textBoxOrange.Text == "")
							{
								return;
							}
							else
								e.CellStyle.BackColor = Color.Orange;
						}
						if (text.Contains(textBoxBlue.Text))
						{
							if (textBoxBlue.Text == "")
							{
								return;
							}
							else
								e.CellStyle.BackColor = Color.Blue;
							e.CellStyle.ForeColor = Color.White;
						}
						if (text.Contains(textBoxBlack.Text))
						{
							if (textBoxBlack.Text == "")
							{
								return;
							}
							else
								e.CellStyle.BackColor = Color.Black;
							e.CellStyle.ForeColor = Color.White;
						}
					}
				}
			}
		}

		/// <summary>

		/// 폼 닫기 이벤트 핸들러 선언

		/// </summary>

		/// <param name="sender"></param>

		/// <param name="e"></param>

		public void Form_Closing(object sender, FormClosedEventArgs e)
		{
			initCloseMethod();
			if (clientUser != null && clientUser.IsConnected) clientUser.Disconnect();
		}

		private void buttonLogFolder_Click(object sender, EventArgs e)
		{
			//내 폴더 위치 불러오기
			System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			//폴더 있는지 확인하고 생성하기
			if (!Directory.Exists("Log"))
			{
				System.IO.Directory.CreateDirectory("Log");
			}
			//폴더 열기
			System.Diagnostics.Process.Start("Log");
		}

		private void buttonPublish5_Click(object sender, EventArgs e)
		{
			{
				if (checkBoxTopicpub.Checked == true)
				{
					clientUser.Publish(textBoxPT1.Text, Encoding.UTF8.GetBytes(textBoxM5.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
				else if (checkBoxTopicpub.Checked == false)
				{
					clientUser.Publish(textBoxPT5.Text, Encoding.UTF8.GetBytes(textBoxM5.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
			}
		}

		private void buttonPublish6_Click(object sender, EventArgs e)
		{
			{
				if (checkBoxTopicpub.Checked == true)
				{
					clientUser.Publish(textBoxPT1.Text, Encoding.UTF8.GetBytes(textBoxM6.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
				else if (checkBoxTopicpub.Checked == false)
				{
					clientUser.Publish(textBoxPT6.Text, Encoding.UTF8.GetBytes(textBoxM6.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
			}
		}

		private void buttonPublish7_Click(object sender, EventArgs e)
		{
			{
				if (checkBoxTopicpub.Checked == true)
				{
					clientUser.Publish(textBoxPT1.Text, Encoding.UTF8.GetBytes(textBoxM7.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
				else if (checkBoxTopicpub.Checked == false)
				{
					clientUser.Publish(textBoxPT7.Text, Encoding.UTF8.GetBytes(textBoxM7.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
			}
		}

		private void buttonPublish8_Click(object sender, EventArgs e)
		{
			{
				if (checkBoxTopicpub.Checked == true)
				{
					clientUser.Publish(textBoxPT1.Text, Encoding.UTF8.GetBytes(textBoxM8.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
				else if (checkBoxTopicpub.Checked == false)
				{
					clientUser.Publish(textBoxPT8.Text, Encoding.UTF8.GetBytes(textBoxM8.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
			}
		}

		private void buttonPublish9_Click(object sender, EventArgs e)
		{
			{
				if (checkBoxTopicpub.Checked == true)
				{
					clientUser.Publish(textBoxPT1.Text, Encoding.UTF8.GetBytes(textBoxM9.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
				else if (checkBoxTopicpub.Checked == false)
				{
					clientUser.Publish(textBoxPT9.Text, Encoding.UTF8.GetBytes(textBoxM9.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
			}
		}

		private void buttonPublish10_Click(object sender, EventArgs e)
		{
			{
				if (checkBoxTopicpub.Checked == true)
				{
					clientUser.Publish(textBoxPT1.Text, Encoding.UTF8.GetBytes(textBoxM10.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
				else if (checkBoxTopicpub.Checked == false)
				{
					clientUser.Publish(textBoxPT10.Text, Encoding.UTF8.GetBytes(textBoxM10.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
			}
		}

		private void buttonMeter_Click(object sender, EventArgs e)
		{
			if(radioButton1.Checked == true)
			{
				dataGridView2.Rows.Clear();
				dataGridView2.Columns.Clear();
				dataGridView2.ReadOnly = true;
				dataGridView2.RowHeadersVisible = false;
				dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				dataGridView2.Columns.Add("0", "POS");
				dataGridView2.Columns.Add("1", "DESC");
				dataGridView2.Columns.Add("2", "현재값");
				dataGridView2.Columns.Add("3", "텐덤/해링본");
				dataGridView2.Columns.Add("4", "버튼");
				for (int i = 0; i < 40; i++)
				{
					dataGridView2.Rows.Add();
					dataGridView2["0", i].Value = i + 1;
					dataGridView2["1", i].Value = "맥동분당횟수(20~80)";

					dataGridView2["3", i].Value = "해링본";
					dataGridView2["4", i] = new DataGridViewButtonCell();
				}
				dataGridView2["2", 0].Value = "dawoon/Manual/3850/1/POLOR";
				dataGridView2["2", 1].Value = "dawoon/Manual/3850/192/POLOR";
				dataGridView2["2", 2].Value = "dawoon/Manual/3850/168/POLOR";
				dataGridView2["2", 3].Value = "dawoon/Manual/3850/1/POLOR";
				dataGridView2["2", 4].Value = "dawoon/Manual/3850/10/POLOR";
				dataGridView2["2", 5].Value = "dawoon/Manual/3850/192/POLOR";
				dataGridView2["2", 6].Value = "dawoon/Manual/3850/168/POLOR";
				dataGridView2["2", 7].Value = "dawoon/Manual/3850/1/POLOR";
				dataGridView2["2", 8].Value = "dawoon/Manual/3850/10/POLOR";
				dataGridView2["2", 9].Value = "dawoon/Manual/3850/12/POLOR";
				dataGridView2["2", 10].Value = "dawoon/Manual/3850/0/POLOR";
				dataGridView2["2", 11].Value = "dawoon/Manual/3850/0/POLOR";
				dataGridView2["2", 12].Value = "dawoon/Manual/3850/60/POLOR";
				dataGridView2["2", 13].Value = "dawoon/Manual/3850/103/POLOR";
				dataGridView2["2", 14].Value = "dawoon/Manual/3850/60/POLOR";
				dataGridView2["2", 15].Value = "dawoon/Manual/3850/126/POLOR";
				dataGridView2["2", 16].Value = "dawoon/Manual/3850/23/POLOR";
				dataGridView2["2", 17].Value = "dawoon/Manual/3850/1/POLOR";
				dataGridView2["2", 18].Value = "dawoon/Manual/3850/0/POLOR";
				dataGridView2["2", 19].Value = "dawoon/Manual/3850/1/POLOR";
				dataGridView2["2", 20].Value = "dawoon/Manual/3850/3850/POLOR";
				dataGridView2["2", 21].Value = "dawoon/Manual/3850/1/POLOR";
				dataGridView2["2", 22].Value = "dawoon/Manual/3850/2/POLOR";
				dataGridView2["2", 23].Value = "dawoon/Manual/3850/0/POLOR";
				dataGridView2["2", 24].Value = "dawoon/Manual/3850/0/POLOR";
				dataGridView2["2", 25].Value = "dawoon/Manual/3850/0/POLOR";
				dataGridView2["2", 26].Value = "dawoon/Manual/3850/1/POLOR";
				dataGridView2["2", 27].Value = "dawoon/Manual/3850/2000/POLOR";
				dataGridView2["2", 28].Value = "dawoon/Manual/3850/2/POLOR";
				dataGridView2["2", 29].Value = "dawoon/Manual/3850/120000/POLOR";
				dataGridView2["2", 30].Value = "dawoon/Manual/3850/2/POLOR";
				dataGridView2["2", 31].Value = "dawoon/Manual/3850/100/POLOR";
				dataGridView2["2", 32].Value = "dawoon/Manual/3850/1/POLOR";
				dataGridView2["2", 33].Value = "dawoon/Manual/3850/1/POLOR";
				dataGridView2["2", 34].Value = "dawoon/Manual/3850/2000/POLOR";
				dataGridView2["2", 35].Value = "dawoon/Manual/3850/50000/POLOR";
				dataGridView2["2", 36].Value = "dawoon/Manual/3850/1/POLOR";
				dataGridView2["2", 37].Value = "dawoon/Manual/3850/1/POLOR";
				dataGridView2["2", 38].Value = "dawoon/Manual/3850/0/POLOR";
				dataGridView2["2", 39].Value = "dawoon/Manual/3850/0/POLOR";
				dataGridView2.Columns[dataGridViewMessage.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				dataGridView2.Columns[1].Width = 200;
				dataGridView2.Columns[3].Width = 200;
			}
		  else if(radioButton2.Checked == true)
			{
				dataGridView2.Rows.Clear();
				dataGridView2.Columns.Clear();
				dataGridView2.ReadOnly = true;
				dataGridView2.RowHeadersVisible = false;
				dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


				dataGridView2.Columns.Add("0", "POS");
				dataGridView2.Columns.Add("1", "DESC");
				dataGridView2.Columns.Add("2", "현재값");
				dataGridView2.Columns.Add("3", "텐덤/해링본");
				dataGridView2.Columns.Add("4", "버튼");

				for (int i = 0; i < 40; i++)
				{
					dataGridView2.Rows.Add();
					dataGridView2["0", i].Value = i + 1;
					dataGridView2["1", i].Value = "맥동분당횟수(20~80)";

					dataGridView2["3", i].Value = "텐덤";
					dataGridView2["4", i] = new DataGridViewButtonCell();
				}
				dataGridView2["2", 0].Value = "dawoon/Manual/3863/3/TENDOM";
				dataGridView2["2", 1].Value = "dawoon/Manual/3863/172/TENDOM";
				dataGridView2["2", 2].Value = "dawoon/Manual/3863/30/TENDOM";
				dataGridView2["2", 3].Value = "dawoon/Manual/3863/1/TENDOM";
				dataGridView2["2", 4].Value = "dawoon/Manual/3863/25/TENDOM";
				dataGridView2["2", 5].Value = "dawoon/Manual/3863/172/TENDOM";
				dataGridView2["2", 6].Value = "dawoon/Manual/3863/30/TENDOM";
				dataGridView2["2", 7].Value = "dawoon/Manual/3863/1/TENDOM";
				dataGridView2["2", 8].Value = "dawoon/Manual/3863/25/TENDOM";
				dataGridView2["2", 9].Value = "dawoon/Manual/3863/6/TENDOM";
				dataGridView2["2", 10].Value = "dawoon/Manual/3863/0/TENDOM";
				dataGridView2["2", 11].Value = "dawoon/Manual/3863/0/TENDOM";
				dataGridView2["2", 12].Value = "dawoon/Manual/3863/60/TENDOM";
				dataGridView2["2", 13].Value = "dawoon/Manual/3863/103/TENDOM";
				dataGridView2["2", 14].Value = "dawoon/Manual/3863/60/TENDOM";
				dataGridView2["2", 15].Value = "dawoon/Manual/3863/126/TENDOM";
				dataGridView2["2", 16].Value = "dawoon/Manual/3863/23/TENDOM";
				dataGridView2["2", 17].Value = "dawoon/Manual/3863/1/TENDOM";
				dataGridView2["2", 18].Value = "dawoon/Manual/3863/0/TENDOM";
				dataGridView2["2", 19].Value = "dawoon/Manual/3863/1/TENDOM";
				dataGridView2["2", 20].Value = "dawoon/Manual/3863/3863/TENDOM";
				dataGridView2["2", 21].Value = "dawoon/Manual/3863/1/TENDOM";
				dataGridView2["2", 22].Value = "dawoon/Manual/3863/0/TENDOM";
				dataGridView2["2", 23].Value = "dawoon/Manual/3863/0/TENDOM";
				dataGridView2["2", 24].Value = "dawoon/Manual/3863/0/TENDOM";
				dataGridView2["2", 25].Value = "dawoon/Manual/3863/1/TENDOM";
				dataGridView2["2", 26].Value = "dawoon/Manual/3863/1/TENDOM";
				dataGridView2["2", 27].Value = "dawoon/Manual/3863/2000/TENDOM";
				dataGridView2["2", 28].Value = "dawoon/Manual/3863/0/TENDOM";
				dataGridView2["2", 29].Value = "dawoon/Manual/3863/30000/TENDOM";
				dataGridView2["2", 30].Value = "dawoon/Manual/3863/3/TENDOM";
				dataGridView2["2", 31].Value = "dawoon/Manual/3863/50/TENDOM";
				dataGridView2["2", 32].Value = "dawoon/Manual/3863/1/TENDOM";
				dataGridView2["2", 33].Value = "dawoon/Manual/3863/1/TENDOM";
				dataGridView2["2", 34].Value = "dawoon/Manual/3863/3000/TENDOM";
				dataGridView2["2", 35].Value = "dawoon/Manual/3863/20000/TENDOM";
				dataGridView2["2", 36].Value = "dawoon/Manual/3863/1/TENDOM";
				dataGridView2["2", 37].Value = "dawoon/Manual/3863/0/TENDOM";
				dataGridView2["2", 38].Value = "dawoon/Manual/3863/0/TENDOM";
				dataGridView2["2", 39].Value = "dawoon/Manual/3863/0/TENDOM";



				dataGridView2.Columns[dataGridViewMessage.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				dataGridView2.Columns[1].Width = 200;
				dataGridView2.Columns[3].Width = 200;
			}
		}
	}

	//Put this class at the end of the main class or you will have problems.
	public static class ExtensionMethods    // DoubleBuffered 메서드를 확장 시켜주자..
	{
		public static void DoubleBuffered(this DataGridView dgv, bool setting)
		{
			Type dgvType = dgv.GetType();
			PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty);
			pi.SetValue(dgv, setting, null);
		}
	}
}