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
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MQTTClient
{
	public partial class Form1 : Form
	{
		static MqttClient clientUser;
		static MqttClient clientscr;
		static MqttClient clientdst;
		DataTable dt = new DataTable();
		DataTable dt2 = new DataTable();
		private delegate void ShowCallBack(string myStr1, string myStr2, DataGridView dgv);
		private delegate void myUICallBack(string myStr, TextBox ctl);
		string startupPath = Application.StartupPath + @"\MqttClient.ini";
		string mc = "MqttClient";
		public Form1()
		{
			InitializeComponent();
			//폼 닫기 이벤트 선언
			this.FormClosed += Form_Closing;
			this.buttonClear.BringToFront();
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
			radioButton1.Checked = true;
			radioButton3.Checked = true;
			comboBoxQos.SelectedIndex = 0;
			mqttRecord();
			textBoxPort.Text = "1883";
			comboBoxQos.SelectedIndex = 1;
			iniload();
			this.dataGridViewMessage.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			tabControl2_SelectedIndexChanged(sender, e);
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
		private void iniload()
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
			StringBuilder autoPubTopic = new StringBuilder();
			StringBuilder autoPubMsg2 = new StringBuilder();

			// ini파일에서 데이터를 불러옴
			// GetPrivateProfileString("카테고리", "Key값", "기본값", "저장할 변수", "불러올 경로");
			GetPrivateProfileString(mc, "LastHostName", "", host, 3200, startupPath);
			GetPrivateProfileString(mc, "LastSubTopic", "", topic, 3200, startupPath);
			GetPrivateProfileString(mc, "LastPubTopic1", "", pub1, 3200, startupPath);
			GetPrivateProfileString(mc, "LastPubTopic2", "", pub2, 3200, startupPath);
			GetPrivateProfileString(mc, "LastPubTopic3", "", pub3, 3200, startupPath);
			GetPrivateProfileString(mc, "LastPubTopic4", "", pub4, 3200, startupPath);
			GetPrivateProfileString(mc, "LastPubTopic5", "", pub5, 3200, startupPath);
			GetPrivateProfileString(mc, "LastPubTopic6", "", pub6, 3200, startupPath);
			GetPrivateProfileString(mc, "LastPubTopic7", "", pub7, 3200, startupPath);
			GetPrivateProfileString(mc, "LastPubTopic8", "", pub8, 3200, startupPath);
			GetPrivateProfileString(mc, "LastPubTopic9", "", pub9, 3200, startupPath);
			GetPrivateProfileString(mc, "LastPubTopic10", "", pub10, 3200, startupPath);
			GetPrivateProfileString(mc, "LastMessage1", "", m1, 3200, startupPath);
			GetPrivateProfileString(mc, "LastMessage2", "", m2, 3200, startupPath);
			GetPrivateProfileString(mc, "LastMessage3", "", m3, 3200, startupPath);
			GetPrivateProfileString(mc, "LastMessage4", "", m4, 3200, startupPath);
			GetPrivateProfileString(mc, "LastMessage5", "", m5, 3200, startupPath);
			GetPrivateProfileString(mc, "LastMessage6", "", m6, 3200, startupPath);
			GetPrivateProfileString(mc, "LastMessage7", "", m7, 3200, startupPath);
			GetPrivateProfileString(mc, "LastMessage8", "", m8, 3200, startupPath);
			GetPrivateProfileString(mc, "LastMessage9", "", m9, 3200, startupPath);
			GetPrivateProfileString(mc, "LastMessage10", "", m10, 3200, startupPath);
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
			GetPrivateProfileString(mc, "autoPubTopic", "", autoPubTopic, 3200, startupPath);
		  GetPrivateProfileString(mc, "autoPubMsg", "", autoPubMsg2, 32000, startupPath);

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
			textBoxAutoPubTopic.Text = autoPubTopic.ToString().Trim();
	    textBoxAutoPubMsg.Text = autoPubMsg2.ToString();
		}

		private void initCloseMethod()
		{
			// ini파일에 등록
			// WritePrivateProfileString("카테고리", "Key값", "Value", "저장할 경로");
			WritePrivateProfileString(mc, "LastHostName", textBoxHost.Text, startupPath);
			WritePrivateProfileString(mc, "LastSubTopic", textBoxSubTopic.Text, startupPath);
			WritePrivateProfileString(mc, "LastPubTopic1", textBoxPT1.Text, startupPath);
			WritePrivateProfileString(mc, "LastPubTopic2", textBoxPT2.Text, startupPath);
			WritePrivateProfileString(mc, "LastPubTopic3", textBoxPT3.Text, startupPath);
			WritePrivateProfileString(mc, "LastPubTopic4", textBoxPT4.Text, startupPath);
			WritePrivateProfileString(mc, "LastPubTopic5", textBoxPT5.Text, startupPath);
			WritePrivateProfileString(mc, "LastPubTopic6", textBoxPT6.Text, startupPath);
			WritePrivateProfileString(mc, "LastPubTopic7", textBoxPT7.Text, startupPath);
			WritePrivateProfileString(mc, "LastPubTopic8", textBoxPT8.Text, startupPath);
			WritePrivateProfileString(mc, "LastPubTopic9", textBoxPT9.Text, startupPath);
			WritePrivateProfileString(mc, "LastPubTopic10", textBoxPT10.Text, startupPath);
			WritePrivateProfileString(mc, "LastMessage1", textBoxM1.Text, startupPath);
			WritePrivateProfileString(mc, "LastMessage2", textBoxM2.Text, startupPath);
			WritePrivateProfileString(mc, "LastMessage3", textBoxM3.Text, startupPath);
			WritePrivateProfileString(mc, "LastMessage4", textBoxM4.Text, startupPath);
			WritePrivateProfileString(mc, "LastMessage5", textBoxM5.Text, startupPath);
			WritePrivateProfileString(mc, "LastMessage6", textBoxM6.Text, startupPath);
			WritePrivateProfileString(mc, "LastMessage7", textBoxM7.Text, startupPath);
			WritePrivateProfileString(mc, "LastMessage8", textBoxM8.Text, startupPath);
			WritePrivateProfileString(mc, "LastMessage9", textBoxM9.Text, startupPath);
			WritePrivateProfileString(mc, "LastMessage10", textBoxM10.Text, startupPath);
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
			WritePrivateProfileString(mc, "autoPubTopic", textBoxAutoPubTopic.Text, startupPath);
		  WritePrivateProfileString(mc, "autoPubMsg", textBoxAutoPubMsg.Text, startupPath);
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

		private void buttonSubscribe2_Click(object sender, EventArgs e)
		{
			if (!listBoxSub.Items.Contains(textBoxSubTopic.Text))
			{
				this.listBoxSub.Items.Add(textBoxSubTopic.Text);
			}

			if (textBoxSubTopic.Text.Length == 0)
			{
				return;
			}
			else
			{
				try
				{
					foreach (var item in listBoxSub.Items)
					{
						//result += string.Format("{0} ", item); 
						clientUser.Subscribe(new string[] { item.ToString() }, new byte[] { (byte)comboBoxQos.SelectedIndex });
					}
					dataGridViewMessage.DoubleBuffered(true);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void ShowMessage(string topic, string payload, DataGridView dgv)
		{
			try {
				if (this.InvokeRequired)
				{
					ShowCallBack myUpdate = new ShowCallBack(ShowMessage);
					this.Invoke(myUpdate, topic, payload, dgv);
				}
				else
				{
					dataGridViewMessage.SuspendLayout();
					//dt.Rows.Add(DateTime.Now.ToString("HH:mm:ss:fff") + " - [MQTT] " + myStr1 + " - " + myStr2 + Environment.NewLine);
					//dgv.Rows.Add(myStr + Environment.NewLine);
					//	dataGridViewMessage.Rows.Add(DateTime.Now.ToString("HH:mm:ss:fff"), myStr1 + Environment.NewLine, myStr2 + Environment.NewLine);
					dt.Rows.Add(DateTime.Now.ToString("HH:mm:ss:fff"), topic + Environment.NewLine, payload + Environment.NewLine);
					//	dt.Rows.Add(DateTime.Now.ToString("HH:mm:ss:fff "), myStr1, myStr2 + Environment.NewLine);

					dataGridViewMessage.CurrentCell = null;
					//dataGridViewMessage.ClearSelection();
					dataGridViewMessage.DataSource = dt;

					//a
					if (topic.Contains("VALUE"))
					{
						try
						{
							dataGridViewMeter["2", 1].Value = "1";
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.ToString());
						}
					}
					//as

					logSave(topic, payload);
					//jsondata(topic, payload);
					jsonSave(topic, payload);

					dataGridViewMessage.ResumeLayout();

				}
			} catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				//로그 에러 저장
			}
		}
	
		public class json
		{
			public string Date { get; set; }
			public string Topic { get; internal set; }
			public string Payload { get; internal set; }
			public string QosLevel { get; internal set; }
			public string Retain { get; internal set; }
		}

		private void jsondata(string topic, string payload)
		{
			//json data = new json
			//{
			//	Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"),
			//	Topic = topic,
			//	Payload = new List<string> { payload },
			//	QosLevel = comboBoxQos.SelectedIndex.ToString(),
			//	Retain = checkBoxRetain.Checked.ToString().ToLower()
			//};

			////직렬화
			//string json1 = JsonConvert.SerializeObject(data, Formatting.Indented);
			//MessageBox.Show(json1 + Environment.NewLine);
	}

	private void jsonSave(string topic, string payload)
		{
			try
			{
				//내 폴더 위치 불러오기
				System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

				//폴더 있는지 확인하고 생성하기
				if (!Directory.Exists("Json"))
				{
					System.IO.Directory.CreateDirectory("Json");
				}


				//string line =
				//	"{\n"
				//	+ "\"Date\":" + "\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "\","
				//	+ Environment.NewLine
				//	+ "\"Topic\": " + "\"" + topic + "\","
				//	+ Environment.NewLine
				//	+ "\"Payload\": " + payload
				//	+ ","
				//	+ Environment.NewLine
				//	+ "\"QosLevel\": " + comboBoxQos.SelectedIndex.ToString() + ","
				//	+ Environment.NewLine
				//	+ "\"Retain\": " + checkBoxRetain.Checked.ToString().ToLower()
				//	+ Environment.NewLine
				//	+ "}";

				string currentPath = System.IO.Directory.GetCurrentDirectory();
				json data = new json
				{
					Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"),
					Topic = topic,
					Payload = payload,
					QosLevel = comboBoxQos.SelectedIndex.ToString(),
					Retain = checkBoxRetain.Checked.ToString().ToLower()
				};

				//직렬화
				string json1 = JsonConvert.SerializeObject(data, Formatting.Indented);
				string jsonName = @"" + currentPath + "\\Log\\" + "LogJson_" + DateTime.Now.ToString("yyyyMM") + ".json";
				string topicjsonName = @"" + currentPath + "\\Log\\" + topic.Replace("/", "") + DateTime.Now.ToString("_yyyyMMdd") + ".json";


				if (checkBoxTopicLog.Checked == true)
				{
					if (topicjsonName.Contains("�") == true)
					{
						return;
					}
					else
						using (FileStream topicfs = new FileStream(topicjsonName, FileMode.Append, FileAccess.Write))
						using (StreamWriter Write2 = new StreamWriter(topicfs))
						{
							Write2.WriteLine(json1.Replace("\\", ""));
						}
				}

				using (FileStream fs = new FileStream(jsonName, FileMode.Append, FileAccess.Write))
				using (StreamWriter Write = new StreamWriter(fs))
				{
					Write.WriteLine(json1.Replace("\\", ""));
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void logSave(string topic, string payload)
		{
			try
			{
				//string data = System.IO.Directory.GetCurrentDirectory();
				//string[] lines = { DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff - ") + topic + " - " + payload };
				// 파일모드
				// FileMode.Create              // 파일을 만든다. 있으면 덮어쓴다.
				// FileMode.CreateNew           // 파일을 만든다. 있으면 DirectoryNotFoundException 예외가 발생한다.
				// FileMode.Append              // 파일을 만든다. 있으면 뒤에서 추가로 쓴다.
				// FileMode.Open                // 파일을 연다. 없으면 FileNotFoundException 예외가 발생한다
				// FileMode.Truncate            // 파일을 연다. 없으면 만든다. (있든 없든 무조건 만든다.)

				// 파일권한 모드
				// FileAccess.Read              // 파일에 읽을 수 있는 권한만 준다.
				// FileAccess.Write             // 파일에 쓰기 권한만 준다.
				// FileAccess.ReadWrite         // 파일에 읽고 쓰는 권한을 둘다 준다.

				//log파일은 계속해서 추가되어야 하므로 append를 선택

				string currentPath = System.IO.Directory.GetCurrentDirectory();
				string line = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff - ") + topic + " - " + payload;
				string logName = @"" + currentPath + "\\Log\\" + "log-MQTT-" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
				string topiclogName = @"" + currentPath + "\\Log\\" + topic.Replace("/", "") + DateTime.Now.ToString("_yyyyMMdd") + ".log";

				if (checkBoxTopicLog.Checked == true)
				{
					if (topiclogName.Contains("�") == true)
					{
						return;
					}
					else
				  using (FileStream topicfs = new FileStream(topiclogName, FileMode.Append, FileAccess.Write))
					using (StreamWriter Write2 = new StreamWriter(topicfs))
					{
						Write2.WriteLine(line);
					}
				}

				using (FileStream fs = new FileStream(logName, FileMode.Append, FileAccess.Write))
				using (StreamWriter Write = new StreamWriter(fs))
				{
					Write.WriteLine(line);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

	

		public class CMD_POS_VALUE
		{
			public string CMD { get; set; }
			public string POS { get; set; }
			public string VALUE { get; set; }
		}

		public class CMD_POS
		{
			public string CMD { get; set; }
			public int POS { get; set; }
		}

		public class CMD_ID_COUNT
		{
			public string CMD { get; set; }
			public int ID { get; set; }
			public int COUNT { get; set; }
		}

		public class CMD_ID_POS
		{
			public string CMD { get; set; }
			public int ID { get; set; }
			public int POS { get; set; }
		}

		public class CMD_ID_POS_VALUE
		{
			public string CMD { get; set; }
			public int ID { get; set; }
			public int POS { get; set; }
			public int VALUE { get; set; }
		}

		private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs data)
		{
			//clientdst.Publish(textBoxPT1.Text+data.Topic, data.Message, (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);

			/// 1. 토픽 체크
			/// 2. payload한 메세지에서 cmd를 체크
			/// 3. 원하는 값 추출
			/// 데이터그리드뷰 보여주기
			if (data.Topic == "dawoon/Manual/3850/1/POLOR")
			{
				CMD_POS_VALUE cmd = JsonConvert.DeserializeObject<CMD_POS_VALUE>(Encoding.UTF8.GetString(data.Message));
				if (cmd.CMD == "RESP_MAIN_SET_READ")
				{
					int pos = Convert.ToInt32(cmd.POS);
					int value = Convert.ToInt32(cmd.VALUE);
					dataGridViewMain["2", pos-1].Value = value.ToString();
				}
				else if(cmd.CMD == "RESP_METER_COUNT")
				{
					CMD_ID_COUNT resp = JsonConvert.DeserializeObject<CMD_ID_COUNT>(Encoding.UTF8.GetString(data.Message));
				//	MessageBox.Show(resp.COUNT.ToString());
					string respText = resp.COUNT.ToString();
					//textBoxIdCount.Text = respText;
					myUI(respText, textBoxIdCount);
				
			
				}
				else if (cmd.CMD == "RESP_METER_SET_READ")
				{
					CMD_ID_POS_VALUE resp = JsonConvert.DeserializeObject<CMD_ID_POS_VALUE>(Encoding.UTF8.GetString(data.Message));
					int id = resp.ID;
					int pos = Convert.ToInt32(resp.POS);
					int value = Convert.ToInt32(resp.VALUE);
					dataGridViewMeter[id+1, pos - 1].Value = value.ToString();
				}
				else if (cmd.CMD == "RESP_IR_SET_READ")
				{
					CMD_ID_POS_VALUE resp = JsonConvert.DeserializeObject<CMD_ID_POS_VALUE>(Encoding.UTF8.GetString(data.Message));
					int id = resp.ID;
					int pos = Convert.ToInt32(resp.POS);
					int value = Convert.ToInt32(resp.VALUE);
					dataGridViewIR[id + 1, pos - 1].Value = value.ToString();
				}
			}

			//dynamic json = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data.Message));
			//MessageBox.Show(cmd.ToString());


			//			if (System.Text.Encoding.UTF8.GetString(data.Message).Contains(
			//				"\"CMD\":" + "\"RESP_MAIN_SET_READ\""
			//				))
			//			{
			//				// valu값을 포함하면 value값 가져오기
			//				//dataGridViewMeter["2", 0].Value = System.Text.Encoding.UTF8.GetString(data.Message);
			//				//포스가 35번이면 35번에 채우기 할 예정

			//				for (int i = 1; i < 41; i++)
			//				{
			//					if (System.Text.Encoding.UTF8.GetString(data.Message).Contains("POS\":" + i + ","))
			//					{
			//						string s = System.Text.Encoding.UTF8.GetString(data.Message);
			//						string word1 = "VALUE\":";
			//						string word2 = "}";
			//						string text = stringBetween(s, word1, word2);
			//					dataGridViewMeter["2", i - 1].Value = text;
			//						//데이터쓰기
			//						//	if(data.Topic == "dawoon/Manual/3850/1/POLOR") 
			//					}
			//				}
			//				//	"\"CMD\":" + "\"RESP_MAIN_SET_OK\""
			//			}
			//			else if (System.Text.Encoding.UTF8.GetString(data.Message).Contains(
			//"\"CMD\":" + "\"MAIN_SET_OK\""
			//))
			//			{
			//				// valu값을 포함하면 value값 가져오기
			//				//dataGridViewMeter["2", 0].Value = System.Text.Encoding.UTF8.GetString(data.Message);
			//				//포스가 35번이면 35번에 채우기 할 예정

			//				for (int i = 1; i < 41; i++)
			//				{
			//					if (System.Text.Encoding.UTF8.GetString(data.Message).Contains("POS\":" + i + ","))
			//					{
			//						string s = System.Text.Encoding.UTF8.GetString(data.Message);
			//						string word1 = "VALUE\":";
			//						string word2 = ",\"NOWTIME\"";
			//						string text = stringBetween(s, word1, word2);
			//					 dataGridViewMeter["2", i - 1].Value = text;

			//					}
			//	}
			//}
			ShowMessage(data.Topic, System.Text.Encoding.UTF8.GetString(data.Message), dataGridViewMessage);





			}
		
		public static string stringBetween(string Source, string Start, string End)
		{
			string result = "";
			if (Source.Contains(Start) && Source.Contains(End))
			{
				int StartIndex = Source.IndexOf(Start, 0) + Start.Length;
				int EndIndex = Source.IndexOf(End, StartIndex);
				result = Source.Substring(StartIndex, EndIndex - StartIndex);
				return result;
			}
			return result;
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
				catch (Exception ex)
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
					buttonUnscribe.Enabled = true;
					buttonDisconnect.Enabled = true;
					textBoxHost.Enabled = false;
					textBoxPort.Enabled = false;
					buttonConnect.Enabled = false;
					buttonIdCount.Enabled = true;
					buttonMain.Enabled = true;
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
					List<string> result = new List<string>();
					result.Add(textBoxSubTopic.Text);
					//this.listBoxSub.Items.Add(textBoxSubTopic.Text);

					//result.Add(textBoxSubTopic.Text);
					//////중복 제거
					//result = result.Distinct().ToList();
					//var _items = result.Distinct().ToArray();

					//this.listBoxSub.Items.Clear();

					//foreach (var item in _items)
					//{
					//	this.listBoxSub.Items.Add(textBoxSubTopic.Text);
					//}
				
					if (!listBoxSub.Items.Contains(textBoxSubTopic.Text))
					{
						this.listBoxSub.Items.Add(textBoxSubTopic.Text);
					}
				

					dataGridViewMessage.DoubleBuffered(true);
				}
				catch (Exception ex)
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
			buttonSubscribe.Enabled = false;
			buttonPublish.Enabled = false;
			buttonPublish2.Enabled = false;
			buttonPublish3.Enabled = false;
			buttonPublish4.Enabled = false;
			buttonPublish4.Enabled = false ;
			buttonPublish5.Enabled = false ;
			buttonPublish6.Enabled = false ;
			buttonPublish7.Enabled = false ;
			buttonPublish8.Enabled = false;
			buttonPublish9.Enabled = false ;
			buttonPublish10.Enabled = false ;
			buttonMeter.Enabled = false ;
			buttonIdCount.Enabled = false ;
			buttonIR.Enabled = false ;
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
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void buttonClear_Click(object sender, EventArgs e)
		{

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

		private void cell1(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try {
				string text = e.Value.ToString().Trim();
				string[] text2 = text.Split(',');

				if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
				{
					if ((e.Value != null))
					{
						string[] redSplit = textBoxRed.Text.Split(',');
						for (int i=0; i < redSplit.Length; i++)
						{
							if (redSplit[i].ToString().Trim().Length == 0)
							{
								continue;
							}
							if (text.Contains(redSplit[i].ToString().Trim()))
							{
								e.CellStyle.BackColor = Color.Red;
								e.CellStyle.ForeColor = Color.White;
							}
						}

						string[] greenSplit = textBoxGreen.Text.Split(',');
						for (int i = 0; i < greenSplit.Length; i++)
						{
							if (greenSplit[i].ToString().Trim().Length == 0)
							{
								continue;
							}
							if (text.Contains(greenSplit[i].ToString().Trim()))
							{
								e.CellStyle.BackColor = Color.Green;
								e.CellStyle.ForeColor= Color.White;
							}
						}

						string[] yellowSplit = textBoxYellow.Text.Split(',');
						for (int i = 0; i < yellowSplit.Length; i++)
						{
							if (yellowSplit[i].ToString().Trim().Length == 0)
							{
								continue;
							}
							if (text.Contains(yellowSplit[i].ToString().Trim()))
							{
								e.CellStyle.BackColor = Color.Yellow;
								e.CellStyle.ForeColor = Color.Black;
							}
						}


						string[] graySplit = textBoxGray.Text.Split(',');
						for (int i = 0; i < graySplit.Length; i++)
						{
							if (graySplit[i].ToString().Trim().Length == 0)
							{
								continue;
							}
							if (text.Contains(graySplit[i].ToString().Trim()))
							{
								e.CellStyle.BackColor = Color.Gray;
								e.CellStyle.ForeColor = Color.White;
							}
						}

						string[] NavySplit = textBoxNavy.Text.Split(',');
						for (int i = 0; i < NavySplit.Length; i++)
						{
							if (NavySplit[i].ToString().Trim().Length == 0)
							{
								continue;
							}
							if (text.Contains(NavySplit[i].ToString().Trim()))
							{
								e.CellStyle.BackColor = Color.Navy;
								e.CellStyle.ForeColor = Color.White;
							}
						}

						string[] PurpleSplit = textBoxPurple.Text.Split(',');
						for (int i = 0; i < PurpleSplit.Length; i++)
						{
							if (PurpleSplit[i].ToString().Trim().Length == 0)
							{
								continue;
							}
							if (text.Contains(PurpleSplit[i].ToString().Trim()))
							{
								e.CellStyle.BackColor = Color.Purple;
								e.CellStyle.ForeColor = Color.White;
							}
						}


						string[] LimeSplit = textBoxLime.Text.Split(',');
						for (int i = 0; i < LimeSplit.Length; i++)
						{
							if (LimeSplit[i].ToString().Trim().Length == 0)
							{
								continue;
							}
							if (text.Contains(LimeSplit[i].ToString().Trim()))
							{
								e.CellStyle.BackColor = Color.Lime;
								e.CellStyle.ForeColor = Color.White;
							}
						}

						string[] PinkSplit = textBoxPink.Text.Split(',');
						for (int i = 0; i < PinkSplit.Length; i++)
						{
							if (PinkSplit[i].ToString().Trim().Length == 0)
							{
								continue;
							}
							if (text.Contains(PinkSplit[i].ToString().Trim()))
							{
								e.CellStyle.BackColor = Color.Pink;
								e.CellStyle.ForeColor = Color.Black;
							}
						}


						string[] OrangeSplit = textBoxOrange.Text.Split(',');
						for (int i = 0; i < OrangeSplit.Length; i++)
						{
							if (OrangeSplit[i].ToString().Trim().Length == 0)
							{
								continue;
							}
							if (text.Contains(OrangeSplit[i].ToString().Trim()))
							{
								e.CellStyle.BackColor = Color.Orange;
								e.CellStyle.ForeColor = Color.Black;
							}
						}



						string[] BlueSplit = textBoxBlue.Text.Split(',');
						for (int i = 0; i < BlueSplit.Length; i++)
						{
							if (BlueSplit[i].ToString().Trim().Length == 0)
							{
								continue;
							}
							if (text.Contains(BlueSplit[i].ToString().Trim()))
							{
								e.CellStyle.BackColor = Color.Blue;
								e.CellStyle.ForeColor = Color.White;
							}
						}

						string[] BlackSplit = textBoxBlack.Text.Split(',');
						for (int i = 0; i < BlackSplit.Length; i++)
						{
							if (BlackSplit[i].ToString().Trim().Length == 0)
							{
								continue;
							}
							if (text.Contains(BlackSplit[i].ToString().Trim()))
							{
								e.CellStyle.BackColor = Color.Black;
								e.CellStyle.ForeColor = Color.White;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
		
			}
		}

		private void cell2(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try { 
			if (e.ColumnIndex == 2)
			{
				if (e.Value != null)
				{
					string text = e.Value.ToString();

					if (text.Contains(textBoxRed.Text) || text.Contains(textBoxRed.Text.Split(',')[0]) || text.Contains(textBoxRed.Text.Split(',')[1]))
					{
						if (textBoxRed.Text == "")
						{
							return;
						}
						else
							e.CellStyle.BackColor = Color.Red;
						  e.CellStyle.ForeColor = Color.White;
					}

					if (text.Contains(textBoxGreen.Text))
					{
						if (textBoxNavy.Text == "")
						{
							return;
						}
						else
						e.CellStyle.BackColor = Color.Green;
						e.CellStyle.ForeColor = Color.White;
					}


					if (text.Contains(textBoxYellow.Text) || text.Contains(textBoxYellow.Text.Split(',')[0]) || text.Contains(textBoxYellow.Text.Split(',')[1]))
					{
						if (textBoxYellow.Text == "")
						{
							return;
						}
						else
							e.CellStyle.BackColor = Color.Yellow;
						e.CellStyle.ForeColor = Color.Black;
					}

					if (text.Contains(textBoxGray.Text) || text.Contains(textBoxGray.Text.Split(',')[0]) || text.Contains(textBoxGray.Text.Split(',')[1]))
					{
						if (textBoxGray.Text == "")
						{
							return;
						}
						else
							e.CellStyle.BackColor = Color.Gray;
					}

					if (text.Contains(textBoxNavy.Text) || text.Contains(textBoxNavy.Text.Split(',')[0]) || text.Contains(textBoxNavy.Text.Split(',')[1]))
					{
						if (textBoxNavy.Text == "")
						{
							return;
						}
						else
							e.CellStyle.BackColor = Color.Navy;
						e.CellStyle.ForeColor = Color.White;
					}


					if (text.Contains(textBoxPurple.Text) || text.Contains(textBoxPurple.Text.Split(',')[0]) || text.Contains(textBoxPurple.Text.Split(',')[1]))
					{
						if (textBoxPurple.Text == "")
						{
							return;
						}
						else
							e.CellStyle.BackColor = Color.Purple;
						e.CellStyle.ForeColor = Color.White;
					}

					if (text.Contains(textBoxLime.Text) || text.Contains(textBoxLime.Text.Split(',')[0]) || text.Contains(textBoxLime.Text.Split(',')[1]))
					{
						if (textBoxLime.Text == "")
						{
							return;
						}
						else
							e.CellStyle.BackColor = Color.Lime;
						e.CellStyle.ForeColor = Color.White;
					}

					if (text.Contains(textBoxPink.Text) || text.Contains(textBoxPink.Text.Split(',')[0]) || text.Contains(textBoxPink.Text.Split(',')[1]))
					{
						if (textBoxPink.Text == "")
						{
							return;
						}
						else
							e.CellStyle.BackColor = Color.Pink;
						e.CellStyle.ForeColor = Color.White;
					}

					if (text.Contains(textBoxOrange.Text) || text.Contains(textBoxOrange.Text.Split(',')[0]) || text.Contains(textBoxOrange.Text.Split(',')[1]))
					{
						if (textBoxOrange.Text == "")
						{
							return;
						}
						else
							e.CellStyle.BackColor = Color.Orange;
						e.CellStyle.ForeColor = Color.White;
					}

					if (text.Contains(textBoxBlue.Text) || text.Contains(textBoxBlue.Text.Split(',')[0]) || text.Contains(textBoxBlue.Text.Split(',')[1]))
					{
						if (textBoxBlue.Text == "")
						{
							return;
						}
						else
							e.CellStyle.BackColor = Color.Blue;
						e.CellStyle.ForeColor = Color.White;
					}

					if (text.Contains(textBoxBlack.Text) || text.Contains(textBoxBlack.Text.Split(',')[0]) || text.Contains(textBoxBlack.Text.Split(',')[1]))
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
		}catch (Exception ex)
			{
			
			}
	}

		private void dataGridViewMessage_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			{
				// 특정값을 가진 열을 좀 다르게 보여주고 싶을 때
				//if (textBoxRed.Text.Contains(",") == true)
				//{
				//	cell1(sender, e);
				//	cell2(sender, e);
				//}
				//else
					cell1(sender,e);
					//cell2(sender, e);

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
			if (clientscr != null && clientUser.IsConnected) clientUser.Disconnect();
			if (clientdst != null && clientUser.IsConnected) clientUser.Disconnect();
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

		private static DateTime Delay(int MS)
		{
			DateTime ThisMoment = DateTime.Now;
			TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
			DateTime AfterWards = ThisMoment.Add(duration);
			while (AfterWards >= ThisMoment)
			{
				System.Windows.Forms.Application.DoEvents();
				ThisMoment = DateTime.Now;
			}
			return DateTime.Now;
		}

		private void buttonMain_Click(object sender, EventArgs e)
		{
			string topic = "dawoon/Manual/" + textBoxCode.Text.Trim() + "/1/" + "POLOR";
			try
			{
				for (int i = 1; i <= 40; i++)
					{
					// REQ CLASS 생성 2개를 가져오는
					// CMD ,POS 값
					CMD_POS req = new CMD_POS();
					req.CMD = "REQ_MAIN_SET_READ";
					req.POS = i;
					string reqStr = JsonConvert.SerializeObject(req, Formatting.Indented);
					clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
					Delay(500);
					}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	
		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton1.Checked == true)
			{
				if(dataGridViewMain.Columns.Count < 3)
				{
					return;
				}
				dataGridViewMain.Columns[3].HeaderText = "헤링본";
				int idx = 0;
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "192";
				dataGridViewMain["3", idx++].Value = "168";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "10";
				dataGridViewMain["3", idx++].Value = "192";
				dataGridViewMain["3", idx++].Value = "168";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "10";
				dataGridViewMain["3", idx++].Value = "12";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "60";
				dataGridViewMain["3", idx++].Value = "103";
				dataGridViewMain["3", idx++].Value = "60";
				dataGridViewMain["3", idx++].Value = "126";
				dataGridViewMain["3", idx++].Value = "23";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "3850";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "2";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "2000";
				dataGridViewMain["3", idx++].Value = "2";
				dataGridViewMain["3", idx++].Value = "120000";
				dataGridViewMain["3", idx++].Value = "2";
				dataGridViewMain["3", idx++].Value = "100";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "2000";
				dataGridViewMain["3", idx++].Value = "50000";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "0";

			
			}
		}
		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton2.Checked == true)
			{

				if (dataGridViewMain.Columns.Count < 3)
				{
					return;
				}

				dataGridViewMain.Columns[3].HeaderText = "텐덤";
				int idx = 0;

			
				dataGridViewMain["3", idx++].Value = "3";
				dataGridViewMain["3", idx++].Value = "172";
				dataGridViewMain["3", idx++].Value = "30";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "25";
				dataGridViewMain["3", idx++].Value = "172";
				dataGridViewMain["3", idx++].Value = "30";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "25";
				dataGridViewMain["3", idx++].Value = "6";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "60";
				dataGridViewMain["3", idx++].Value = "103";
				dataGridViewMain["3", idx++].Value = "60";
				dataGridViewMain["3", idx++].Value = "126";
				dataGridViewMain["3", idx++].Value = "23";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "3863";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "2000";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "30000";
				dataGridViewMain["3", idx++].Value = "3";
				dataGridViewMain["3", idx++].Value = "50";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "3000";
				dataGridViewMain["3", idx++].Value = "20000";
				dataGridViewMain["3", idx++].Value = "1";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "0";
				dataGridViewMain["3", idx++].Value = "0";



	
			}
		}

		private void checkBoxTopicLog_CheckedChanged(object sender, EventArgs e)
		{
		
		}

		private void tabControl2_Selected(object sender, TabControlEventArgs e)
		{

		}

		private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
		{
	
			buttonIR.Enabled = false;
			buttonMeter.Enabled = false;
			//	textBoxIdCount.Text = "0";
			if (tabControl2.SelectedTab == tabPageMain)
			{
				buttonIdCount.Enabled = true;
		
				//삭제
				dataGridViewMain.Columns.Clear();
				dataGridViewMain.ReadOnly = true;
				dataGridViewMain.RowHeadersVisible = false;
				dataGridViewMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				dataGridViewMain.Columns.Add("0", "POS");
				dataGridViewMain.Columns.Add("1", "DESC");
				dataGridViewMain.Columns.Add("2", "현재값");
				dataGridViewMain.Columns.Add("3", "텐덤/해링본");
				dataGridViewMain.Columns.Add("4", "버튼");
				dataGridViewMain.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				dataGridViewMain.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				dataGridViewMain.Columns[0].Width = 40;
				dataGridViewMain.Columns[1].Width = 350;
				dataGridViewMain.Columns[3].Width = 70;
				for (int i = 0; i < 40; i++)
				{
					dataGridViewMain.Rows.Add();
					dataGridViewMain["0", i].Value = i + 1;
					dataGridViewMain["4", i] = new DataGridViewButtonCell();
				}
				dataGridViewMain["1", 0].Value = "DEVICE_TYPE: 디바이스 종류";
				dataGridViewMain["1", 1].Value = "MQTT_IP1: 농장 PC MQTT IP 첫번째 번호";
				dataGridViewMain["1", 2].Value = "MQTT_IP2: 농장 PC MQTT IP 두번째 번호";
				dataGridViewMain["1", 3].Value = "MQTT_IP3: 농장 PC MQTT IP 세번째 번호";
				dataGridViewMain["1", 4].Value = "MQTT_IP4: 농장 PC MQTT IP 네번째 번호";
				dataGridViewMain["1", 5].Value = "HTTP_IP1: 농장 PC 프로그램 IP 첫번째 번호";
				dataGridViewMain["1", 6].Value = "HTTP_IP2: 농장 PC 프로그램 IP 두번째 번호";
				dataGridViewMain["1", 7].Value = "HTTP_IP3: 농장 PC 프로그램 IP 세번째 번호";
				dataGridViewMain["1", 8].Value = "HTTP_IP4: 농장 PC 프로그램 IP 네번째 번호";
				dataGridViewMain["1", 9].Value = "Milk_Device_Max: 착유 메터 설치 수";
				dataGridViewMain["1", 10].Value = "BackLight Auto:0(off)/1(on)";
				dataGridViewMain["1", 11].Value = "Milking NUM SET:0(RFID)/1(장비번호)";
				dataGridViewMain["1", 12].Value = "RFID Erase Minute: RFID 삭제 시간(기본 60분)";
				dataGridViewMain["1", 13].Value = "DW MQTT_IP1: 리눅스서버 MQTT IP 첫번째";
				dataGridViewMain["1", 14].Value = "DW MQTT_IP2: 리눅스서버 MQTT IP 두번째";
				dataGridViewMain["1", 15].Value = "DW MQTT_IP3: 리눅스서버 MQTT IP 세번째";
				dataGridViewMain["1", 16].Value = "DW MQTT_IP4: 리눅스서버 MQTT IP 네번째";
				dataGridViewMain["1", 17].Value = "DW MQTT STATUS INFO: 0(미사용)/1(사용)";
				dataGridViewMain["1", 18].Value = "DW MQTT MILK INFO: 0(미사용)/1(사용)";
				dataGridViewMain["1", 19].Value = "DATA_MQTT_MODE: 0:HTTP모드/1:MQTT모드";
				dataGridViewMain["1", 20].Value = "Farm Code: 농장코드(cowplan에 등록된 농장코드) DW2016과 같은 코드";
				dataGridViewMain["1", 21].Value = "Device Code: MQTT 번호(1번 설정)";
				dataGridViewMain["1", 22].Value = "RFID TYPE: (기본0 셋팅) 0:이전 착유소 인식하지않음 1:이전 착유소 읽을수 있게 변경 2:이전 착유소 지우되 각라인 마지막소는 지우지 않음";
				dataGridViewMain["1", 23].Value = "MILKING TYPE: 0:다운착유기 1:타사착유기(기본0 셋팅)";
				dataGridViewMain["1", 24].Value = "YIELD_LIMIT: (다운착유기 기본 0, 타사착유기 기본 2000) MILKING TYPE가 1일때 (타사착유기)착유량이 설정 값보다 적으면 착유정보 보내지 않음";
				dataGridViewMain["1", 25].Value = "RFID_LINE_RESET: 0:착유소 나가고 바로인식 1:착유라인 전체가 종료되야 RFID인식 시작(기본 1)";
				dataGridViewMain["1", 26].Value = "RFID_MQTT_SEND: 1:장비가 RFID인식시 MQTT정보 보냄, 0:보내지않음 1:보냄(기본1)";
				dataGridViewMain["1", 27].Value = "IR_RFID_READ_TIME: 한 IR장비 읽기 점유율시간 (1ms) (기본 2000)";
				dataGridViewMain["1", 28].Value = "IR_SENSOR_ENABLE: IR장비의 소입력 센서 사용유무 0:사용안함 1:사용함 (기본0) 2:전방 도어센서 인식 3:후방 도어센서 인식";
				dataGridViewMain["1", 29].Value = "29.항목이 0일때 착유 완료후 소 IRID 읽기 쉬는 시간(1ms) (기본 60000) 29.항목이 1일때 소로 인식되는 센서 입력 길이(1ms)";
				dataGridViewMain["1", 30].Value = "IR_READ_OK_COUNT: IRID인식 획수. 3일경우 같은 번호가 3번 연속 읽혀야 함";
				dataGridViewMain["1", 31].Value = "IR_READ_LIMIT: 착유진행됐을 경우 IRID 못읽은 장비의 시도 횟수(값이 읽혔으면 시도횟수 초기화)";
				dataGridViewMain["1", 32].Value = "IR_ONE_SEND: 0: 라인별 독립적으로 보냄 1: 한 개의 장비만 데이터보냄(특수한 경우가 아니면 1로 셋팅되어야함)";
				dataGridViewMain["1", 33].Value = "YEILD_SEND_ZERO: 착유량이 없더라도 서버에 데이터를 보냄";
				dataGridViewMain["1", 34].Value = "DOOR_OPEN_TIME: IR_SENSOR_ENABLE이 2일 경우 도어 오픈 연속 감지 시간(기본값:1000 -> 1초)";
				dataGridViewMain["1", 35].Value = "COW_SAME_ID_TIME: 0일때 사용하지 않음,0이 아니면 있으면 그만큼 쉬고 읽기 시작(기본값:20000->20초)";
				dataGridViewMain["1", 36].Value = "IRID_MILK_REREAD: 0일경우 착유시작시 다시 읽지 않음, 1일경우 착유 시작시 다시 읽음";
				dataGridViewMain["1", 37].Value = "IR_LINE_OK_COUNT: 입구 IRID 인식 횟수. 3일경우 같은 번호가 3번 연속 읽혀야함";
				dataGridViewMain["1", 38].Value = "IR_TX_MQTT_SEND: IR TX에 대한 MQTT 정보 표시 0:사용함, 1:사용안함";
				dataGridViewMain["1", 39].Value = "MAIN_RESET: 12시간이 넘었고 착유중이 아니면 자정에 메인 리셋 진행 0:사용함 1:사용안함";
				radioButton1_CheckedChanged(sender, e);
				radioButton2_CheckedChanged(sender, e);
			}

			if (tabControl2.SelectedTab == tabPageMeter)
			{
				buttonIdCount.Enabled = true;
				dataGridViewMeter.Columns.Clear();
				dataGridViewMeter.ReadOnly = true;
				dataGridViewMeter.RowHeadersVisible = false;
				dataGridViewMeter.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				dataGridViewMeter.Columns.Add("0", "POS");
				dataGridViewMeter.Columns.Add("1", "DESC");
				if (textBoxIdCount.Text == "")
				{
					return;
				}
				int bbbb = Convert.ToInt32(textBoxIdCount.Text);
				for (int i = 1; i <= bbbb; i++)
				{
					dataGridViewMeter.Columns.Add((i).ToString(), i.ToString());
				}
				dataGridViewMeter.Columns.Add((dataGridViewMeter.ColumnCount).ToString(), "텐덤/해링본");
				dataGridViewMeter.Columns.Add((dataGridViewMeter.ColumnCount + 1).ToString(), "버튼");
				for (int i = 0; i < 46; i++)
				{
					dataGridViewMeter.Rows.Add();
					dataGridViewMeter["0", i].Value = i + 1;
					dataGridViewMeter[dataGridViewMeter.Columns.Count - 1, i] = new DataGridViewButtonCell();
				}
				dataGridViewMeter["1", 0].Value = "사용안함";
				dataGridViewMeter["1", 1].Value = "장비 ID";
				dataGridViewMeter["1", 2].Value = "맥동분당횟수(20~80)";
				dataGridViewMeter["1", 3].Value = "탈착시간 초단위 계산";
				dataGridViewMeter["1", 4].Value = "초기 착유종료 탈착 초";
				dataGridViewMeter["1", 5].Value = "착유종료 탈착 초";
				dataGridViewMeter["1", 6].Value = "시작마사지속도(MAX1000)";
				dataGridViewMeter["1", 7].Value = "시작마사지 시간(1s)";
				dataGridViewMeter["1", 8].Value = "종료마사지속도(MAX1000)";
				dataGridViewMeter["1", 9].Value = "종료마사지 카운트 시작";
				dataGridViewMeter["1", 10].Value = "종료마사지 카운트 종료";
				dataGridViewMeter["1", 11].Value = "유방염 에러 밸류";
				dataGridViewMeter["1", 12].Value = "혈류 에러 벨류(mg/L)";
				dataGridViewMeter["1", 13].Value = "집유기 종류(다운:1/타사:0)";
				dataGridViewMeter["1", 14].Value = "우유 ADC 값(1ms)";
				dataGridViewMeter["1", 15].Value = "집유기 동작 시간";
				dataGridViewMeter["1", 16].Value = "세척 집유기 시간(1s)";
				dataGridViewMeter["1", 17].Value = "젖꼭지 탈착 시점(1ms)";
				dataGridViewMeter["1", 18].Value = "우유 수집 시간(1ms)";
				dataGridViewMeter["1", 19].Value = "우유 수집 시간 끝(1ms)";
				dataGridViewMeter["1", 20].Value = "종료 우유 수집(1ms)";
				dataGridViewMeter["1", 21].Value = "진공 개방시간(1ms)";
				dataGridViewMeter["1", 22].Value = "RF DB 값(MAX300)";

				dataGridViewMeter["1", 23].Value = "RFID 종류";
				dataGridViewMeter["1", 24].Value = "착유량 컬러: 흰색/녹색/노란색/적색";
				dataGridViewMeter["1", 25].Value = "RFID 변경: 뒤로밀기/현재자리/앞으로밀기";
				dataGridViewMeter["1", 26].Value = "도어 센서위치 닫힘/열림";
				dataGridViewMeter["1", 27].Value = "부저사용함/부저사용안함";
				dataGridViewMeter["1", 28].Value = "세척물 ADC값";
				dataGridViewMeter["1", 29].Value = "LCD오프타임(1s)";
				dataGridViewMeter["1", 30].Value = "세척맥동타임(1s)";
				dataGridViewMeter["1", 31].Value = "솔밸브초기 ON타임(1s)";
				dataGridViewMeter["1", 32].Value = "솔밸브 Pwm 주기(1ms)";
				dataGridViewMeter["1", 33].Value = "솔밸브 Pwm 퍼센트(1~100)";
				dataGridViewMeter["1", 34].Value = "맥동1 비율(20~80)";
				dataGridViewMeter["1", 35].Value = "맥동2 비율(20~80)";
				dataGridViewMeter["1", 36].Value = "소 ID 에러페이지 활성화";
				dataGridViewMeter["1", 37].Value = "유량 값mL (50~150)";
				dataGridViewMeter["1", 38].Value = "유량보정배율(1.100배)";
				dataGridViewMeter["1", 39].Value = "유량보정시작ms";
				dataGridViewMeter["1", 40].Value = "유량보정타입()";

				dataGridViewMeter["1", 41].Value = "집유기 전도도조정(기본:1000)";
				dataGridViewMeter["1", 42].Value = "종료카운트시작 유량값(100g)";
				dataGridViewMeter["1", 43].Value = "집유기 밸브 에러 시간(1ms)";
				dataGridViewMeter["1", 44].Value = "집유기 밸브 에러 횟수";
				dataGridViewMeter["1", 45].Value = "7,8 OUT 타입(): 다운 착유기시 0";

				radioButton3_CheckedChanged(sender, e);
				radioButton4_CheckedChanged(sender, e);
				dataGridViewMeter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

				dataGridViewMeter.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				dataGridViewMeter.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				dataGridViewMeter.Columns[(dataGridViewMeter.ColumnCount).ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				//dataGridViewMeter.Columns[(dataGridViewMeter.ColumnCount).ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				dataGridViewMeter.Columns[0].Width = 40;
				dataGridViewMeter.Columns[1].Width = 200;
				dataGridViewMeter.Columns[(dataGridViewMeter.ColumnCount).ToString()].Width = 35;

			}

			if (tabControl2.SelectedTab == tabPageIR)
			{
				buttonIdCount.Enabled = true;
				dataGridViewIR.Columns.Clear();
				dataGridViewIR.ReadOnly = true;
				dataGridViewIR.RowHeadersVisible = false;
				dataGridViewIR.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				dataGridViewIR.Columns.Add("0", "POS");
				dataGridViewIR.Columns.Add("1", "DESC");

				if (textBoxIdCount.Text == "")
				{
					return;
				}
				int bbbb = Convert.ToInt32(textBoxIdCount.Text);
				for (int i = 1; i <= bbbb; i++)
				{
					dataGridViewIR.Columns.Add((i).ToString(), i.ToString());
				}
			
				dataGridViewIR.Columns.Add((dataGridViewMeter.ColumnCount + 1).ToString(), "버튼");
				for (int i = 0; i < 10; i++)
				{
					dataGridViewIR.Rows.Add();
					dataGridViewIR["0", i].Value = i + 1;
					dataGridViewIR[dataGridViewIR.Columns.Count - 1, i] = new DataGridViewButtonCell();
				}
				int num = 0;
				dataGridViewIR["1", num++].Value = "ID: 0:232통신에 사용 1~24 CAN 통신 이용시 고유 ID";
				dataGridViewIR["1", num++].Value = "Slr: IRID Tag를 깨우기 위한 신호 시간(0.1ms). 기본값 1";
				dataGridViewIR["1", num++].Value = "Sdp: IRID Tag를 깨우기 위한 신호의 강도(1~100). 기본값 40";
				dataGridViewIR["1", num++].Value = "SA: IRID Tag를 깨우고 IR정보 보내는 대기 시간(0.1ms) 기본값 2000(바뀌면 안됨)";
				dataGridViewIR["1", num++].Value = "rA: IRID 송신후 수신 대기 시간 (0.1ms). 기본값 2000(바뀌면 안됨)";
				dataGridViewIR["1", num++].Value = "lrt: IRID 리더의 송수신 총시간(1ms). 기본값 2000 *정해진 시간만큼만 읽을 수 있음. 메인셋팅 28.에 의존";
				dataGridViewIR["1", num++].Value = "LSe: 레이져센서의 소감지 유지시간(1ms). Main 셋팅 29.이 2일때만 사용";
				dataGridViewIR["1", num++].Value = "rse: 리모콘사용 여부 0:사용안함 1:사용함";
				dataGridViewIR["1", num++].Value = "be: 0:부져 사용 1: 부져 사용안함. 기본값 0";
				dataGridViewIR["1", num++].Value = "tuc: 태그 깨어나는 IR 카운트수. IR 라이팅때 같이 사용됨";

				dataGridViewIR.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

				dataGridViewIR.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				dataGridViewIR.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				//dataGridViewIR.Columns[(dataGridViewMeter.ColumnCount).ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				//dataGridViewMeter.Columns[(dataGridViewMeter.ColumnCount).ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				dataGridViewIR.Columns[0].Width = 40;
				dataGridViewIR.Columns[1].Width = 200;
				//dataGridViewIR.Columns[(dataGridViewMeter.ColumnCount).ToString()].Width = 35;

				if (dataGridViewIR.Columns.Count < 3)
				{
					return;
				}
			}
			
		}

		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton3.Checked == true)
			{
				if (dataGridViewMeter.Columns.Count < 3)
				{
					return;
				}
				dataGridViewMeter.Columns[(dataGridViewMeter.Columns.Count-2).ToString()].HeaderText = "헤링본";

				int idx = 0;
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1~12";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "60";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "0";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "180";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "35";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "60";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "120";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "60";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "3";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "2";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "8000";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "200";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1000";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "500";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "5";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1500";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "3000";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "2000";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "2";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "6";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "2";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "0";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "2100";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "2100";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "0";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "2000";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "10";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "60";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "53";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "58";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "83";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1100";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "5000";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "2";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1000";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "20";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "50";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "5";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "0";
			}
		}
		private void panel11_Paint(object sender, PaintEventArgs e)
		{
		}


		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton4.Checked == true)
			{
				if (dataGridViewMeter.Columns.Count < 3)
				{
					return;
				}
				dataGridViewMeter.Columns[(dataGridViewMeter.ColumnCount - 2).ToString()].HeaderText = "텐덤";
				int idx = 0;
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1~6";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "60";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "0";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "180";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "35";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "60";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "120";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "60";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "5";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "2";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "8000(세이버없음)";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1000(세이버없음)";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1000";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "500";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "5";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1500";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "3000";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "2000";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "2";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "7";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "2";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "0";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "0";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "2100";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1200";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "10";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "2000";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "10";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "60";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "59";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "59";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "83";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1100";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "5000";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "2";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "1000";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "20";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "50";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "5";
				dataGridViewMeter[(dataGridViewMeter.ColumnCount - 2).ToString(), idx++].Value = "0";
			}
		}


		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			//tabControl2_SelectedIndexChanged(sender, e);
			radioButton1_CheckedChanged(sender,e);
			radioButton2_CheckedChanged(sender,e);
			radioButton3_CheckedChanged(sender,e);
			radioButton4_CheckedChanged(sender,e);
		}
		private void tabcontrols()
		{
			//tabControl2_SelectedIndexChanged(sender, e);
		}

		private void buttonIdCount_Click(object sender, EventArgs e)
		{
			buttonMeter.Enabled = true;
			buttonIR.Enabled = true;
			string topic = "dawoon/Manual/" + textBoxCode.Text.Trim() + "/1/POLOR";
			try
			{
				if ((tabControl2.SelectedTab == tabPageMeter)||(tabControl2.SelectedTab == tabPageIR))
				{
					textBoxIdCount.Text = "";
					CMD_ID_COUNT req = new CMD_ID_COUNT();
					req.CMD = "RESP_METER_COUNT";
					req.ID = 1;
					req.COUNT = 8;
					string reqStr = (JsonConvert.SerializeObject(req, Formatting.Indented)).Trim();
					clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
		
			
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
	
		}

		private void buttonMeter_Click(object sender, EventArgs e)
		{
			string topic = "dawoon/Manual/" + textBoxCode.Text.Trim() + "/1/" + "POLOR";
			try
			{
			  CMD_ID_POS req = new CMD_ID_POS();
		
					for (int k = 1; k <= 20; k++)
					{
					for (int i = 1; i <= 20; i++)
					{
						req.ID = k;
						req.CMD = "REQ_METER_SET_READ";
						req.POS = i;
					string reqStr = JsonConvert.SerializeObject(req, Formatting.Indented);
					clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
					}
					Delay(500);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void buttonIR_Click(object sender, EventArgs e)
		{
			string topic = "dawoon/Manual/" + textBoxCode.Text.Trim() + "/1/" + "POLOR";
			try
			{
				CMD_ID_POS req = new CMD_ID_POS();

				for (int k = 1; k <= 20; k++)
				{
					for (int i = 1; i <= 20; i++)
					{
						req.ID = k;
						req.CMD = "REQ_IR_SET_READ";
						req.POS = i;

						string reqStr = JsonConvert.SerializeObject(req, Formatting.Indented);
						clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
					}
					Delay(500);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void textBoxIdCount_TextChanged(object sender, EventArgs e)
		{
			tabControl2_SelectedIndexChanged(sender, e);
			buttonMeter.Enabled = true;
			buttonIR.Enabled = true;
			if (tabControl2.SelectedTab == tabPageMeter)
			{

				dataGridViewMeter.Columns.Clear();
				dataGridViewMeter.ReadOnly = true;
				dataGridViewMeter.RowHeadersVisible = false;
				dataGridViewMeter.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				dataGridViewMeter.Columns.Add("0", "POS");
				dataGridViewMeter.Columns.Add("1", "DESC");
				if (textBoxIdCount.Text == "")
				{
					return;
				}
				int bbbb = Convert.ToInt32(textBoxIdCount.Text);
				for (int i = 1; i <= bbbb; i++)
				{
					dataGridViewMeter.Columns.Add((i).ToString(), i.ToString());
				}
				dataGridViewMeter.Columns.Add((dataGridViewMeter.ColumnCount).ToString(), "텐덤/해링본");
				dataGridViewMeter.Columns.Add((dataGridViewMeter.ColumnCount + 1).ToString(), "버튼");
				for (int i = 0; i < 46; i++)
				{
					dataGridViewMeter.Rows.Add();
					dataGridViewMeter["0", i].Value = i + 1;
					dataGridViewMeter[dataGridViewMeter.Columns.Count - 1, i] = new DataGridViewButtonCell();
				}
				dataGridViewMeter["1", 0].Value = "사용안함";
				dataGridViewMeter["1", 1].Value = "장비 ID";
				dataGridViewMeter["1", 2].Value = "맥동분당횟수(20~80)";
				dataGridViewMeter["1", 3].Value = "탈착시간 초단위 계산";
				dataGridViewMeter["1", 4].Value = "초기 착유종료 탈착 초";
				dataGridViewMeter["1", 5].Value = "착유종료 탈착 초";
				dataGridViewMeter["1", 6].Value = "시작마사지속도(MAX1000)";
				dataGridViewMeter["1", 7].Value = "시작마사지 시간(1s)";
				dataGridViewMeter["1", 8].Value = "종료마사지속도(MAX1000)";
				dataGridViewMeter["1", 9].Value = "종료마사지 카운트 시작";
				dataGridViewMeter["1", 10].Value = "종료마사지 카운트 종료";
				dataGridViewMeter["1", 11].Value = "유방염 에러 밸류";
				dataGridViewMeter["1", 12].Value = "혈류 에러 벨류(mg/L)";
				dataGridViewMeter["1", 13].Value = "집유기 종류(다운:1/타사:0)";
				dataGridViewMeter["1", 14].Value = "우유 ADC 값(1ms)";
				dataGridViewMeter["1", 15].Value = "집유기 동작 시간";
				dataGridViewMeter["1", 16].Value = "세척 집유기 시간(1s)";
				dataGridViewMeter["1", 17].Value = "젖꼭지 탈착 시점(1ms)";
				dataGridViewMeter["1", 18].Value = "우유 수집 시간(1ms)";
				dataGridViewMeter["1", 19].Value = "우유 수집 시간 끝(1ms)";
				dataGridViewMeter["1", 20].Value = "종료 우유 수집(1ms)";
				dataGridViewMeter["1", 21].Value = "진공 개방시간(1ms)";
				dataGridViewMeter["1", 22].Value = "RF DB 값(MAX300)";

				dataGridViewMeter["1", 23].Value = "RFID 종류";
				dataGridViewMeter["1", 24].Value = "착유량 컬러: 흰색/녹색/노란색/적색";
				dataGridViewMeter["1", 25].Value = "RFID 변경: 뒤로밀기/현재자리/앞으로밀기";
				dataGridViewMeter["1", 26].Value = "도어 센서위치 닫힘/열림";
				dataGridViewMeter["1", 27].Value = "부저사용함/부저사용안함";
				dataGridViewMeter["1", 28].Value = "세척물 ADC값";
				dataGridViewMeter["1", 29].Value = "LCD오프타임(1s)";
				dataGridViewMeter["1", 30].Value = "세척맥동타임(1s)";
				dataGridViewMeter["1", 31].Value = "솔밸브초기 ON타임(1s)";
				dataGridViewMeter["1", 32].Value = "솔밸브 Pwm 주기(1ms)";
				dataGridViewMeter["1", 33].Value = "솔밸브 Pwm 퍼센트(1~100)";
				dataGridViewMeter["1", 34].Value = "맥동1 비율(20~80)";
				dataGridViewMeter["1", 35].Value = "맥동2 비율(20~80)";
				dataGridViewMeter["1", 36].Value = "소 ID 에러페이지 활성화";
				dataGridViewMeter["1", 37].Value = "유량 값mL (50~150)";
				dataGridViewMeter["1", 38].Value = "유량보정배율(1.100배)";
				dataGridViewMeter["1", 39].Value = "유량보정시작ms";
				dataGridViewMeter["1", 40].Value = "유량보정타입()";

				dataGridViewMeter["1", 41].Value = "집유기 전도도조정(기본:1000)";
				dataGridViewMeter["1", 42].Value = "종료카운트시작 유량값(100g)";
				dataGridViewMeter["1", 43].Value = "집유기 밸브 에러 시간(1ms)";
				dataGridViewMeter["1", 44].Value = "집유기 밸브 에러 횟수";
				dataGridViewMeter["1", 45].Value = "7,8 OUT 타입(): 다운 착유기시 0";

				radioButton3_CheckedChanged(sender, e);
				radioButton4_CheckedChanged(sender, e);
				dataGridViewMeter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

				dataGridViewMeter.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				dataGridViewMeter.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				dataGridViewMeter.Columns[(dataGridViewMeter.ColumnCount).ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				//dataGridViewMeter.Columns[(dataGridViewMeter.ColumnCount).ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				dataGridViewMeter.Columns[0].Width = 40;
				dataGridViewMeter.Columns[1].Width = 200;
				dataGridViewMeter.Columns[(dataGridViewMeter.ColumnCount).ToString()].Width = 35;

			}

			if (tabControl2.SelectedTab == tabPageIR)
			{

				dataGridViewIR.Columns.Clear();
				dataGridViewIR.ReadOnly = true;
				dataGridViewIR.RowHeadersVisible = false;
				dataGridViewIR.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				dataGridViewIR.Columns.Add("0", "POS");
				dataGridViewIR.Columns.Add("1", "DESC");

				if (textBoxIdCount.Text == "")
				{
					return;
				}
				int bbbb = Convert.ToInt32(textBoxIdCount.Text);
				for (int i = 1; i <= bbbb; i++)
				{
					dataGridViewIR.Columns.Add((i).ToString(), i.ToString());
				}
				dataGridViewIR.Columns.Add((dataGridViewMeter.ColumnCount + 1).ToString(), "버튼");
				for (int i = 0; i < 10; i++)
				{
					dataGridViewIR.Rows.Add();
					dataGridViewIR["0", i].Value = i + 1;
					dataGridViewIR[dataGridViewIR.Columns.Count - 1, i] = new DataGridViewButtonCell();
				}
				int num = 0;
				dataGridViewIR["1", num++].Value = "ID: 0:232통신에 사용 1~24 CAN 통신 이용시 고유 ID";
				dataGridViewIR["1", num++].Value = "Slr: IRID Tag를 깨우기 위한 신호 시간(0.1ms). 기본값 1";
				dataGridViewIR["1", num++].Value = "Sdp: IRID Tag를 깨우기 위한 신호의 강도(1~100). 기본값 40";
				dataGridViewIR["1", num++].Value = "SA: IRID Tag를 깨우고 IR정보 보내는 대기 시간(0.1ms) 기본값 2000(바뀌면 안됨)";
				dataGridViewIR["1", num++].Value = "rA: IRID 송신후 수신 대기 시간 (0.1ms). 기본값 2000(바뀌면 안됨)";
				dataGridViewIR["1", num++].Value = "lrt: IRID 리더의 송수신 총시간(1ms). 기본값 2000 *정해진 시간만큼만 읽을 수 있음. 메인셋팅 28.에 의존";
				dataGridViewIR["1", num++].Value = "LSe: 레이져센서의 소감지 유지시간(1ms). Main 셋팅 29.이 2일때만 사용";
				dataGridViewIR["1", num++].Value = "rse: 리모콘사용 여부 0:사용안함 1:사용함";
				dataGridViewIR["1", num++].Value = "be: 0:부져 사용 1: 부져 사용안함. 기본값 0";
				dataGridViewIR["1", num++].Value = "tuc: 태그 깨어나는 IR 카운트수. IR 라이팅때 같이 사용됨";

				dataGridViewIR.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

				dataGridViewIR.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				dataGridViewIR.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				//dataGridViewIR.Columns[(dataGridViewMeter.ColumnCount).ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				//dataGridViewMeter.Columns[(dataGridViewMeter.ColumnCount).ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				dataGridViewIR.Columns[0].Width = 40;
				dataGridViewIR.Columns[1].Width = 200;
			//	dataGridViewIR.Columns[(dataGridViewMeter.ColumnCount).ToString()].Width = 35;

				if (dataGridViewIR.Columns.Count < 3)
				{
					return;
				}
			}
		}

	

		private void buttonIRPublish_Click(object sender, EventArgs e)
		{
			string irTopic = "dawoon/meterset/" + textBoxCode.Text +"/1/POLOR";
			string irMessage = "{ \"CMD\":\"IR_SET\",\"POS\":" + textBoxIrPos.Text + ",\"ID\":"+ textBoxIrId.Text + ",\"VALUE\":"+ textBoxIrValue.Text +" }";
				clientUser.Publish(irTopic, Encoding.UTF8.GetBytes(irMessage), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
		}

		private void buttonMeterPublish_Click(object sender, EventArgs e)
		{
			string irTopic = "dawoon/meterset/" + textBoxCode.Text + "/1/POLOR";
			string irMessage = "{ \"CMD\":\"METER_SET\",\"POS\":" + textBoxMeterPos.Text + ",\"ID\":" + textBoxMeterId.Text + ",\"VALUE\":" + textBoxMeterValue.Text + " }";
			clientUser.Publish(irTopic, Encoding.UTF8.GetBytes(irMessage), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
		}

		private void buttonMainPulish_Click(object sender, EventArgs e)
		{
			string irTopic = "dawoon/meterset/" + textBoxCode.Text + "/1/POLOR";
			string irMessage = "{ \"CMD\":\"MAIN_SET\",\"POS\":" + textBoxMainPos.Text + ",\"VALUE\":" + textBoxMainValue.Text + " }";
			clientUser.Publish(irTopic, Encoding.UTF8.GetBytes(irMessage), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
		}

		bool a = true;
		private void buttonAutoPubStart_Click(object sender, EventArgs e)
		{
			string topic = textBoxAutoPubTopic.Text;


			while (a)
			{
				clientUser.Publish(topic, Encoding.UTF8.GetBytes(textBoxAutoPubMsg.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				Delay(Int32.Parse(textBoxDelay1.Text));
			}

			CMD_ID_POS req = new CMD_ID_POS();

			for (int k = 1; k <= 20; k++)
			{
				for (int i = 1; i <= 20; i++)
				{
					req.ID = k;
					req.CMD = "REQ_IR_SET_READ";
					req.POS = i;

					string reqStr = JsonConvert.SerializeObject(req, Formatting.Indented);
					clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				}
				Delay(500);
			}


		}
		private void buttonAutoPubStop_Click(object sender, EventArgs e)
		{
			a = false;
		}

		private void button9_Click(object sender, EventArgs e)
		{

		}


		private void button2_Click(object sender, EventArgs e)
		{

		}

		private void button7_Click(object sender, EventArgs e)
		{

		}

		private void label55_Click(object sender, EventArgs e)
		{

		}

		private void textBox51_TextChanged(object sender, EventArgs e)
		{

		}

		private void label50_Click(object sender, EventArgs e)
		{

		}

		private void textBox52_TextChanged(object sender, EventArgs e)
		{

		}

		private void panel13_Paint(object sender, PaintEventArgs e)
		{

		}

		private void textBoxAutoPubMsg_TextChanged(object sender, EventArgs e)
		{

		}

		private void textBoxAutoPubTopic_TextChanged(object sender, EventArgs e)
		{

		}

		private void label41_Click(object sender, EventArgs e)
		{

		}

		private void label58_Click(object sender, EventArgs e)
		{

		}

		private void tabPage5_Click(object sender, EventArgs e)
		{

		}

		private void buttonConnet2_Click(object sender, EventArgs e)
		{
			try
			{
				clientscr = new MqttClient(textBoxHost.Text);
				clientscr.Connect(Guid.NewGuid().ToString());
				clientscr.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(client_MqttMsgPublishReceived);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
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