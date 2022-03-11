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
using System.Threading;
using System.Threading.Tasks;

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

			iniload();

			this.dataGridViewMessage.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

			dataGridView2.Columns.Clear();
			dataGridView2.ReadOnly = true;
			dataGridView2.RowHeadersVisible = false;
			dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridView2.Columns.Add("0", "POS");
			dataGridView2.Columns.Add("1", "DESC");
			dataGridView2.Columns.Add("2", "현재값");
			dataGridView2.Columns.Add("3", "텐덤/해링본");
			dataGridView2.Columns.Add("4", "버튼");
			dataGridView2.Columns[dataGridView2.ColumnCount - 4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			dataGridView2.Columns[0].Width = 40;
			dataGridView2.Columns[2].Width = 350;
			dataGridView2.Columns[3].Width = 70;
			for (int i = 0; i < 40; i++)
			{
				dataGridView2.Rows.Add();
				dataGridView2["0", i].Value = i + 1;
				dataGridView2["4", i] = new DataGridViewButtonCell();
			}

			dataGridView2["1", 0].Value = "DEVICE_TYPE: 디바이스 종류";
			dataGridView2["1", 1].Value = "MQTT_IP1: 농장 PC MQTT IP 첫번째 번호";
			dataGridView2["1", 2].Value = "MQTT_IP2: 농장 PC MQTT IP 두번째 번호";
			dataGridView2["1", 3].Value = "MQTT_IP3: 농장 PC MQTT IP 세번째 번호";
			dataGridView2["1", 4].Value = "MQTT_IP4: 농장 PC MQTT IP 네번째 번호";
			dataGridView2["1", 5].Value = "HTTP_IP1: 농장 PC 프로그램 IP 첫번째 번호";
			dataGridView2["1", 6].Value = "HTTP_IP2: 농장 PC 프로그램 IP 두번째 번호";
			dataGridView2["1", 7].Value = "HTTP_IP3: 농장 PC 프로그램 IP 세번째 번호";
			dataGridView2["1", 8].Value = "HTTP_IP4: 농장 PC 프로그램 IP 네번째 번호";
			dataGridView2["1", 9].Value = "Milk_Device_Max: 착유 메터 설치 수";
			dataGridView2["1", 10].Value = "BackLight Auto:0(off)/1(on)";
			dataGridView2["1", 11].Value = "Milking NUM SET:0(RFID)/1(장비번호)";
			dataGridView2["1", 12].Value = "RFID Erase Minute: RFID 삭제 시간(기본 60분)";
			dataGridView2["1", 13].Value = "DW MQTT_IP1: 리눅스서버 MQTT IP 첫번째";
			dataGridView2["1", 14].Value = "DW MQTT_IP2: 리눅스서버 MQTT IP 두번째";
			dataGridView2["1", 15].Value = "DW MQTT_IP3: 리눅스서버 MQTT IP 세번째";
			dataGridView2["1", 16].Value = "DW MQTT_IP4: 리눅스서버 MQTT IP 네번째";
			dataGridView2["1", 17].Value = "DW MQTT STATUS INFO: 0(미사용)/1(사용)";
			dataGridView2["1", 18].Value = "DW MQTT MILK INFO: 0(미사용)/1(사용)";
			dataGridView2["1", 19].Value = "DATA_MQTT_MODE: 0:HTTP모드/1:MQTT모드";
			dataGridView2["1", 20].Value = "Farm Code: 농장코드(cowplan에 등록된 농장코드) DW2016과 같은 코드";
			dataGridView2["1", 21].Value = "Device Code: MQTT 번호(1번 설정)";
			dataGridView2["1", 22].Value = "RFID TYPE: (기본0 셋팅) 0:이전 착유소 인식하지않음 1:이전 착유소 읽을수 있게 변경 2:이전 착유소 지우되 각라인 마지막소는 지우지 않음";
			dataGridView2["1", 23].Value = "MILKING TYPE: 0:다운착유기 1:타사착유기(기본0 셋팅)";
			dataGridView2["1", 24].Value = "YIELD_LIMIT: (다운착유기 기본 0, 타사착유기 기본 2000) MILKING TYPE가 1일때 (타사착유기)착유량이 설정 값보다 적으면 착유정보 보내지 않음";
			dataGridView2["1", 25].Value = "RFID_LINE_RESET: 0:착유소 나가고 바로인식 1:착유라인 전체가 종료되야 RFID인식 시작(기본 1)";
			dataGridView2["1", 26].Value = "RFID_MQTT_SEND: 1:장비가 RFID인식시 MQTT정보 보냄, 0:보내지않음 1:보냄(기본1)";
			dataGridView2["1", 27].Value = "IR_RFID_READ_TIME: 한 IR장비 읽기 점유율시간 (1ms) (기본 2000)";
			dataGridView2["1", 28].Value = "IR_SENSOR_ENABLE: IR장비의 소입력 센서 사용유무 0:사용안함 1:사용함 (기본0) 2:전방 도어센서 인식 3:후방 도어센서 인식";
			dataGridView2["1", 29].Value = "29.항목이 0일때 착유 완료후 소 IRID 읽기 쉬는 시간(1ms) (기본 60000) 29.항목이 1일때 소로 인식되는 센서 입력 길이(1ms)";
			dataGridView2["1", 30].Value = "IR_READ_OK_COUNT: IRID인식 획수. 3일경우 같은 번호가 3번 연속 읽혀야 함";
			dataGridView2["1", 31].Value = "IR_READ_LIMIT: 착유진행됐을 경우 IRID 못읽은 장비의 시도 횟수(값이 읽혔으면 시도횟수 초기화)";
			dataGridView2["1", 32].Value = "IR_ONE_SEND: 0: 라인별 독립적으로 보냄 1: 한 개의 장비만 데이터보냄(특수한 경우가 아니면 1로 셋팅되어야함)";
			dataGridView2["1", 33].Value = "YEILD_SEND_ZERO: 착유량이 없더라도 서버에 데이터를 보냄";
			dataGridView2["1", 34].Value = "DOOR_OPEN_TIME: IR_SENSOR_ENABLE이 2일 경우 도어 오픈 연속 감지 시간(기본값:1000 -> 1초)";
			dataGridView2["1", 35].Value = "COW_SAME_ID_TIME: 0일때 사용하지 않음,0이 아니면 있으면 그만큼 쉬고 읽기 시작(기본값:20000->20초)";
			dataGridView2["1", 36].Value = "IRID_MILK_REREAD: 0일경우 착유시작시 다시 읽지 않음, 1일경우 착유 시작시 다시 읽음";
			dataGridView2["1", 37].Value = "IR_LINE_OK_COUNT: 입구 IRID 인식 횟수. 3일경우 같은 번호가 3번 연속 읽혀야함";
			dataGridView2["1", 38].Value = "IR_TX_MQTT_SEND: IR TX에 대한 MQTT 정보 표시 0:사용함, 1:사용안함";
			dataGridView2["1", 39].Value = "MAIN_RESET: 12시간이 넘었고 착유중이 아니면 자정에 메인 리셋 진행 0:사용함 1:사용안함";

			radioButton1.Checked = true;

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
					logSave(topic, payload);
					dataGridViewMessage.ResumeLayout();

				}
			} catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
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

		private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs data)
		{
			ShowMessage(data.Topic, System.Text.Encoding.UTF8.GetString(data.Message), dataGridViewMessage);
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
					////중복 제거
					result = result.Distinct().ToList();
					var _items = result.Distinct().ToArray();
					this.listBoxSub.Items.Clear();
					foreach (var item in _items)
					{
						this.listBoxSub.Items.Add(item);
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
					string text = e.Value.ToString().Trim();
					
			if (e.ColumnIndex == 1)
			{
				if ((e.Value != null))
				{
					int id = text.IndexOf(",");
					int end = text.Length;
				//	string asd = text.Remove(1, id);



					if (text.Contains(textBoxRed.Text) && text.Contains(","))
					{
						string[] text2 = text.Split(',');
						if (textBoxRed.Text == "")
						{
							return;
						}
					
						else if(text2[0].ToString() == "")
						{
							return;
						}
						else
						e.CellStyle.BackColor = Color.Red;
						e.CellStyle.ForeColor = Color.White;
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

		private void cell2(object sender, DataGridViewCellFormattingEventArgs e)
		{
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
					cell2(sender, e);

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


		private void dataGridView2_CellDoubleClick(object sender, EventArgs e)
		{
			MessageBox.Show("선택한 row의 0번째cell의 값 == " + dataGridView2.Rows[0].Cells[0].FormattedValue.ToString());
		}






		private void buttonMeter_Click(object sender, EventArgs e)
		{
			//if (radioButton1.Checked == true)
			//{
			//	string topic = "dawoon/Manual/" + textBoxCode.Text.Trim() + "/1/POLOR";
			//}
			//else if (radioButton2.Checked == true)
			//{
			//	string topic = "dawoon/Manual/" + textBoxCode.Text.Trim() + "/1/TENDOM";
			//}
			string POLORTENDOM = "";
			if (radioButton1.Checked == true)
			{
				POLORTENDOM = "POLOR";
			}
			else if (radioButton2.Checked == true)
			{
				POLORTENDOM = "TENDOM";
			}

			string topic = "dawoon/Manual/" + textBoxCode.Text.Trim() + "/1/" + POLORTENDOM;




			try
			{
				for (int i = 1; i < 41; i++)
					{
						string REQ = "{" + "\"CMD\"" + ":" + "\"REQ_MAIN_SET_READ\"" + "," + "\"POS\"" + ":" + i + "}";
						clientUser.Publish(topic, Encoding.UTF8.GetBytes(REQ), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);

					string RESP = "{" + "\"CMD\"" + ":" + "\"RESP_MAIN_SET_READ\"" + "," + "\"POS\"" + ":" 
						+ dataGridView2.Rows[i-1].Cells[0].FormattedValue.ToString() + ",\"VALUE\":" + i + "}";
					clientUser.Publish(topic, Encoding.UTF8.GetBytes(RESP), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
					if (RESP.Contains("RESP") == true)
					{
						dataGridView2["2", i-1].Value = RESP;
					}

						Delay(3000);
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
				if(dataGridView2.Columns.Count < 3)
				{
					return;
				}
				dataGridView2.Columns[3].HeaderText = "헤링본";

				int idx = 0;
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "192";
				dataGridView2["3", idx++].Value = "168";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "10";
				dataGridView2["3", idx++].Value = "192";
				dataGridView2["3", idx++].Value = "168";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "10";
				dataGridView2["3", idx++].Value = "12";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "60";
				dataGridView2["3", idx++].Value = "103";
				dataGridView2["3", idx++].Value = "60";
				dataGridView2["3", idx++].Value = "126";
				dataGridView2["3", idx++].Value = "23";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "3850";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "2";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "2000";
				dataGridView2["3", idx++].Value = "2";
				dataGridView2["3", idx++].Value = "120000";
				dataGridView2["3", idx++].Value = "2";
				dataGridView2["3", idx++].Value = "100";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "2000";
				dataGridView2["3", idx++].Value = "50000";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "0";
		
			}
		}

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton2.Checked == true)
			{

				if (dataGridView2.Columns.Count < 3)
				{
					return;
				}

				dataGridView2.Columns[3].HeaderText = "텐덤";
				int idx = 0;

			
				dataGridView2["3", idx++].Value = "3";
				dataGridView2["3", idx++].Value = "172";
				dataGridView2["3", idx++].Value = "30";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "25";
				dataGridView2["3", idx++].Value = "172";
				dataGridView2["3", idx++].Value = "30";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "25";
				dataGridView2["3", idx++].Value = "6";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "60";
				dataGridView2["3", idx++].Value = "103";
				dataGridView2["3", idx++].Value = "60";
				dataGridView2["3", idx++].Value = "126";
				dataGridView2["3", idx++].Value = "23";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "3863";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "2000";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "30000";
				dataGridView2["3", idx++].Value = "3";
				dataGridView2["3", idx++].Value = "50";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "3000";
				dataGridView2["3", idx++].Value = "20000";
				dataGridView2["3", idx++].Value = "1";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "0";
				dataGridView2["3", idx++].Value = "0";



	
			}
		}

		private void checkBoxTopicLog_CheckedChanged(object sender, EventArgs e)
		{
		
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