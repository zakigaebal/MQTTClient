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
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using uPLibrary.Networking.M2Mqtt.Internal;
using uPLibrary.Networking.M2Mqtt.Session;
using uPLibrary.Networking.M2Mqtt.Utility;

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

			checkBox5.Checked = true;
			dataGridViewMain.RowTemplate.Height = 18;
			dataGridViewMeter.RowTemplate.Height = 18;
			dataGridViewIR.RowTemplate.Height = 18;

			this.AcceptButton = this.buttonConnect;
			radioButton1.Checked = true;
			radioButton3.Checked = true;
			mqttRecord();
			textBoxPort.Text = "1883";
			comboBoxQos.SelectedIndex = 1;
			comboBoxQos2.SelectedIndex = 1;
			comboBoxQos3.SelectedIndex = 1;
			iniload();
			this.dataGridViewMessage.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			//tabControl2_SelectedIndexChanged(sender, e);

			textBox52.AllowDrop = true;
			//textBox52.DragDrop += textBox52_DragDrop;

			dataGridViewMain.Columns.Add("0", "POS");
			dataGridViewMain.Columns.Add("1", "DESC");
			dataGridViewMain.Columns.Add("2", "현재값");
			dataGridViewMain.Columns.Add("3", "텐덤/해링본");
			dataGridViewMain.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

			dataGridViewMain.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewMain.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			dataGridViewMain.Columns[0].Width = 40;
			dataGridViewMain.Columns[2].Width = 70;
			dataGridViewMain.Columns[3].Width = 70;

			for (int i = 0; i < 40; i++)
			{
				dataGridViewMain.Rows.Add();
				dataGridViewMain["0", i].Value = i;
				dataGridViewMain.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
				dataGridViewMain.AllowUserToResizeRows = false;
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
			//radioButton1_CheckedChanged(sender, e);
			//radioButton2_CheckedChanged(sender, e);
			dataGridViewMain.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

			dataGridViewMeter.Columns.Add("0", "POS");
			dataGridViewMeter.Columns.Add("1", "DESC");
			//if (textBoxIdCount.Text == "")
			//{
			//	return;
			//}
			//if (textBoxIdCount.Text.Trim().Length <= 0)
			//{
			//	return;
			//}
			if (textBoxIdCount.Text == "")
			{
				int bbbb = 0;
			}
			else
			{
				int bbbb = Convert.ToInt32(textBoxIdCount.Text);
				for (int i = 1; i <= bbbb; i++)
				{
					dataGridViewMeter.Columns.Add((i).ToString(), i.ToString());
				}
			}
			dataGridViewMeter.Columns.Add((dataGridViewMeter.ColumnCount).ToString(), "텐덤/해링본");
			for (int i = 0; i < 46; i++)
			{
				dataGridViewMeter.Rows.Add();
				dataGridViewMeter["0", i].Value = i + 1;
				dataGridViewMeter.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
				dataGridViewMeter.AllowUserToResizeRows = false;
			}
			dataGridViewMeter["1", 0].Value = "사용안함";
			dataGridViewMeter["1", 1].Value = "장비 ID(1~20)";
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
			dataGridViewMeter.Columns[0].Width = 40;
			dataGridViewMeter.Columns[1].Width = 250;
			dataGridViewMeter.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

			buttonIdCount.Enabled = true;
			dataGridViewIR.RowHeadersVisible = false;
			dataGridViewIR.Columns.Add("0", "POS");
			dataGridViewIR.Columns.Add("1", "DESC");

			if (textBoxIdCount.Text == "")
			{
				int bbbb = 0;
			}
			else
			{
				int bbbb = Convert.ToInt32(textBoxIdCount.Text);
				for (int i = 1; i <= bbbb; i++)
				{
					dataGridViewIR.Columns.Add((i).ToString(), i.ToString());
				}
			}
			for (int i = 0; i < 10; i++)
			{
				dataGridViewIR.Rows.Add();
				dataGridViewIR["0", i].Value = i + 1;
				dataGridViewIR.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
				dataGridViewIR.AllowUserToResizeRows = false;
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
			dataGridViewIR.Columns[1].Width = 600;
			//dataGridViewIR.Columns[(dataGridViewMeter.ColumnCount).ToString()].Width = 35;
			dataGridViewIR.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewMain.CurrentCell = null;
			dataGridViewMeter.CurrentCell = null;
			dataGridViewIR.CurrentCell = null;

			if (dataGridViewIR.Columns.Count < 3)
			{
				return;
			}

			//컬럼 수정 못하게 하기
			//this.dataGridView1.Columns[1].ReadOnly = true;

			//마우스로 row header width 조절 못하게 하기.
			dataGridViewMain.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewMeter.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewIR.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

			//마우스로 column size 조절 못하게 하기
			//this.dataGridView1.Columns[0].Resizable = DataGridViewTriState.False;

			//dataGridView1.ReadOnly = true;
			//dataGridView1.RowHeadersVisible = false;
			textBoxIdCount_TextChanged(sender, e);

		}
		private void buttonTopicSave_Click(object sender, EventArgs e)
		{
			try
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
			catch (Exception ex)
			{
				LogMgr.ExceptionLog(ex);
			}

		}
		private void buttonTopicOpen_Click(object sender, EventArgs e)
		{
			try
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
			catch (Exception ex)
			{
				LogMgr.ExceptionLog(ex);
			}

		}
		private void iniload()
		{
			try
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
				StringBuilder host2 = new StringBuilder();
				StringBuilder host3 = new StringBuilder();
				StringBuilder topicHeader = new StringBuilder();
				StringBuilder scrTopic = new StringBuilder();
				StringBuilder delLocation = new StringBuilder();
				StringBuilder searchString = new StringBuilder();
				StringBuilder seperateString = new StringBuilder();
				StringBuilder allStartLine = new StringBuilder();
				StringBuilder allDelay = new StringBuilder();

				StringBuilder autoDelay = new StringBuilder();
				StringBuilder autotextStr1 = new StringBuilder();
				StringBuilder autotextStr2 = new StringBuilder();
				StringBuilder autotextStr3 = new StringBuilder();
				StringBuilder autotextStr4 = new StringBuilder();
				StringBuilder autotextStr5 = new StringBuilder();
				StringBuilder autotextStr6 = new StringBuilder();
				StringBuilder autotextStr7 = new StringBuilder();
				StringBuilder autotextStr8 = new StringBuilder();
				StringBuilder autotextStr9 = new StringBuilder();
				StringBuilder autotextStr10 = new StringBuilder();
				StringBuilder autotextfirst1 = new StringBuilder();
				StringBuilder autotextfirst2 = new StringBuilder();
				StringBuilder autotextfirst3 = new StringBuilder();
				StringBuilder autotextfirst4 = new StringBuilder();
				StringBuilder autotextfirst5 = new StringBuilder();
				StringBuilder autotextfirst6 = new StringBuilder();
				StringBuilder autotextfirst7 = new StringBuilder();
				StringBuilder autotextfirst8 = new StringBuilder();
				StringBuilder autotextfirst9 = new StringBuilder();
				StringBuilder autotextfirst10 = new StringBuilder();
				StringBuilder autotextPlus1 = new StringBuilder();
				StringBuilder autotextPlus2 = new StringBuilder();
				StringBuilder autotextPlus3 = new StringBuilder();
				StringBuilder autotextPlus4 = new StringBuilder();
				StringBuilder autotextPlus5 = new StringBuilder();
				StringBuilder autotextPlus6 = new StringBuilder();
				StringBuilder autotextPlus7 = new StringBuilder();
				StringBuilder autotextPlus8 = new StringBuilder();
				StringBuilder autotextPlus9 = new StringBuilder();
				StringBuilder autotextPlus10 = new StringBuilder();
				StringBuilder atextBoxCode = new StringBuilder();
				//StringBuilder atextBoxIdCount = new StringBuilder();
				StringBuilder atextBoxDelay = new StringBuilder();


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
				GetPrivateProfileString(mc, "LastHostName2", "", host2, 3200, startupPath);
				GetPrivateProfileString(mc, "LastHostName3", "", host3, 3200, startupPath);
				GetPrivateProfileString(mc, "topicHeader", "", topicHeader, 3200, startupPath);
				GetPrivateProfileString(mc, "scrTopic", "", scrTopic, 3200, startupPath);
				GetPrivateProfileString(mc, "delLocation", "", delLocation, 3200, startupPath);
				GetPrivateProfileString(mc, "searchString", "", searchString, 3200, startupPath);
				GetPrivateProfileString(mc, "seperateString", "", seperateString, 3200, startupPath);
				GetPrivateProfileString(mc, "allStartLine", "", allStartLine, 3200, startupPath);
				GetPrivateProfileString(mc, "allDelay", "", allDelay, 3200, startupPath);
				GetPrivateProfileString(mc, "autoDelay", "", autoDelay, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxStr1", "", autotextStr1, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxStr2", "", autotextStr2, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxStr3", "", autotextStr3, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxStr4", "", autotextStr4, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxStr5", "", autotextStr5, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxStr6", "", autotextStr6, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxStr7", "", autotextStr7, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxStr8", "", autotextStr8, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxStr9", "", autotextStr9, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxStr10", "", autotextStr10, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxfirst1", "", autotextfirst1, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxfirst2", "", autotextfirst2, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxfirst3", "", autotextfirst3, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxfirst4", "", autotextfirst4, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxfirst5", "", autotextfirst5, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxfirst6", "", autotextfirst6, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxfirst7", "", autotextfirst7, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxfirst8", "", autotextfirst8, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxfirst9", "", autotextfirst9, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxfirst10", "", autotextfirst10, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxPlus1", "", autotextPlus1, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxPlus2", "", autotextPlus2, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxPlus3", "", autotextPlus3, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxPlus4", "", autotextPlus4, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxPlus5", "", autotextPlus5, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxPlus6", "", autotextPlus6, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxPlus7", "", autotextPlus7, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxPlus8", "", autotextPlus8, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxPlus9", "", autotextPlus9, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxPlus10", "", autotextPlus10, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxCode", "", atextBoxCode, 3200, startupPath);
				//GetPrivateProfileString(mc, "textBoxIdCount", "", atextBoxIdCount, 3200, startupPath);
				GetPrivateProfileString(mc, "textBoxDelay", "", atextBoxDelay, 3200, startupPath);

				// 텍스트박스에 ini파일에서 가져온 데이터를 넣는다
				textBoxHost.Text = host.ToString();
				textBoxHost2.Text = host2.ToString();
				textBoxHost3.Text = host3.ToString();
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
				textBoxTopicHeader.Text = topicHeader.ToString();
				textBoxScrTopic.Text = scrTopic.ToString();
				textBoxDelLocation.Text = delLocation.ToString();
				textBoxSearchString.Text = searchString.ToString();
				textBoxSeperateString.Text = seperateString.ToString();
				textBoxAllStartline.Text = allStartLine.ToString();
				textBoxAllDelay.Text = allDelay.ToString();
				textBoxDelay1.Text = autoDelay.ToString();
				textBoxStr1.Text = autotextStr1.ToString();
				textBoxStr2.Text = autotextStr2.ToString();
				textBoxStr3.Text = autotextStr3.ToString();
				textBoxStr4.Text = autotextStr4.ToString();
				textBoxStr5.Text = autotextStr5.ToString();
				textBoxStr6.Text = autotextStr6.ToString();
				textBoxStr7.Text = autotextStr7.ToString();
				textBoxStr8.Text = autotextStr8.ToString();
				textBoxStr9.Text = autotextStr9.ToString();
				textBoxStr10.Text = autotextStr10.ToString();
				textBoxfirst1.Text = autotextfirst1.ToString();
				textBoxfirst2.Text = autotextfirst2.ToString();
				textBoxfirst3.Text = autotextfirst3.ToString();
				textBoxfirst4.Text = autotextfirst4.ToString();
				textBoxfirst5.Text = autotextfirst5.ToString();
				textBoxfirst6.Text = autotextfirst6.ToString();
				textBoxfirst7.Text = autotextfirst7.ToString();
				textBoxfirst8.Text = autotextfirst8.ToString();
				textBoxfirst9.Text = autotextfirst9.ToString();
				textBoxfirst10.Text = autotextfirst10.ToString();
				textBoxPlus1.Text = autotextPlus1.ToString();
				textBoxPlus2.Text = autotextPlus2.ToString();
				textBoxPlus3.Text = autotextPlus3.ToString();
				textBoxPlus4.Text = autotextPlus4.ToString();
				textBoxPlus5.Text = autotextPlus5.ToString();
				textBoxPlus6.Text = autotextPlus6.ToString();
				textBoxPlus7.Text = autotextPlus7.ToString();
				textBoxPlus8.Text = autotextPlus8.ToString();
				textBoxPlus9.Text = autotextPlus9.ToString();
				textBoxPlus10.Text = autotextPlus10.ToString();
				textBoxCode.Text = atextBoxCode.ToString();
				//textBoxIdCount.Text = atextBoxIdCount.ToString();
				textBoxDelay.Text = atextBoxDelay.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());

			}

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
			WritePrivateProfileString(mc, "LastHostName2", textBoxHost2.Text, startupPath);
			WritePrivateProfileString(mc, "LastHostName3", textBoxHost3.Text, startupPath);
			WritePrivateProfileString(mc, "topicHeader", textBoxTopicHeader.Text, startupPath);
			WritePrivateProfileString(mc, "scrTopic", textBoxScrTopic.Text, startupPath);
			WritePrivateProfileString(mc, "delLocation", textBoxDelLocation.Text, startupPath);
			WritePrivateProfileString(mc, "searchString", textBoxSearchString.Text, startupPath);
			WritePrivateProfileString(mc, "seperateString", textBoxSeperateString.Text, startupPath);
			WritePrivateProfileString(mc, "allStartLine", textBoxAllStartline.Text, startupPath);
			WritePrivateProfileString(mc, "allDelay", textBoxAllDelay.Text, startupPath);
			WritePrivateProfileString(mc, "autoDelay", textBoxDelay1.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxStr1", textBoxStr1.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxStr2", textBoxStr2.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxStr3", textBoxStr3.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxStr4", textBoxStr4.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxStr5", textBoxStr5.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxStr6", textBoxStr6.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxStr7", textBoxStr7.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxStr8", textBoxStr8.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxStr9", textBoxStr9.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxStr10", textBoxStr10.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxfirst1", textBoxfirst1.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxfirst2", textBoxfirst2.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxfirst3", textBoxfirst3.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxfirst4", textBoxfirst4.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxfirst5", textBoxfirst5.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxfirst6", textBoxfirst6.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxfirst7", textBoxfirst7.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxfirst8", textBoxfirst8.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxfirst9", textBoxfirst9.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxfirst10", textBoxfirst10.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxPlus1", textBoxPlus1.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxPlus2", textBoxPlus2.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxPlus3", textBoxPlus3.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxPlus4", textBoxPlus4.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxPlus5", textBoxPlus5.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxPlus6", textBoxPlus6.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxPlus7", textBoxPlus7.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxPlus8", textBoxPlus8.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxPlus9", textBoxPlus9.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxPlus10", textBoxPlus10.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxCode", textBoxCode.Text, startupPath);
			//WritePrivateProfileString(mc, "textBoxIdCount", textBoxIdCount.Text, startupPath);
			WritePrivateProfileString(mc, "textBoxDelay", textBoxDelay.Text, startupPath);
		}

		private void mqttRecord()
		{
			dt.Columns.Add("Time", typeof(string));
			dt.Columns.Add("Topic");
			dt.Columns.Add("Message");
			dt.DefaultView.Sort = "Time desc";
			dataGridViewMessage.DataSource = dt;
			dataGridViewMessage.ReadOnly = true;
			dataGridViewMessage.RowHeadersVisible = false;
			//dataGridViewMessage.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridViewMessage.Columns[dataGridViewMessage.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewMessage.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			dataGridViewMessage.Columns[1].Width = 200;
			dataGridViewMessage.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			dataGridViewMessage.AllowUserToResizeRows = false;

			dt2.Columns.Add("Time", typeof(string));
			dt2.Columns.Add("Topic");
			dt2.Columns.Add("Message");
			dt2.DefaultView.Sort = "Time desc";
			dataGridViewDelivery.DataSource = dt2;
			dataGridViewDelivery.ReadOnly = true;
			dataGridViewDelivery.RowHeadersVisible = false;
			//dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridViewDelivery.Columns[dataGridViewDelivery.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewDelivery.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			dataGridViewDelivery.Columns[1].Width = 200;
			dataGridViewDelivery.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			dataGridViewDelivery.AllowUserToResizeRows = false;
		}

		private void buttonSubscribe2_Click(object sender, EventArgs e)
		{

			try
			{
				foreach (var item in listBoxSub.Items)
				{
					clientUser.Subscribe(new string[] { item.ToString() }, new byte[] { (byte)comboBoxQos.SelectedIndex });
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void ShowMessage(string topic, string payload, DataGridView dgv)
		{
			try
			{
				if (topic.Contains("dawoon/MqttLive") && checkBoxMqttLiveHide.Checked)
				{
					return;
				}
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

					//jsondata(topic, payload);
					logSave(topic, payload);
					jsonSave(topic, payload);
					// 한 칸 스크롤
					dataGridViewMessage.FirstDisplayedScrollingRowIndex = 0;
					//int rowIndex = dataGridViewMessage.FirstDisplayedScrollingRowIndex;

					//// Refresh your DGV.

					//dataGridViewMessage.FirstDisplayedScrollingRowIndex = rowIndex;

					dataGridViewMessage.ResumeLayout();

				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				//로그 에러 저장
			}
		}


		private void ShowMessage2(string topic, string payload, DataGridView dgv)
		{
			try
			{
				if (this.InvokeRequired)
				{
					ShowCallBack myUpdate = new ShowCallBack(ShowMessage2);
					this.Invoke(myUpdate, topic, payload, dgv);
				}
				else
				{
					dataGridViewDelivery.SuspendLayout();
					dt.Rows.Add(DateTime.Now.ToString("HH:mm:ss:fff"), topic + Environment.NewLine, payload + Environment.NewLine);
					dataGridViewDelivery.CurrentCell = null;
					dataGridViewDelivery.DataSource = dt;
					logSave(topic, payload);
					jsonSave(topic, payload);
					dataGridViewDelivery.ResumeLayout();
				}
			}
			catch (Exception ex)
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

		private void jsonSave(string topic, string payload)
		{
			try
			{
				//내 폴더 위치 불러오기
				System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

				//폴더 있는지 확인하고 생성하기
				//if (!Directory.Exists("Json"))
				//{
				//	System.IO.Directory.CreateDirectory("Json");
				//}


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


				//내 폴더 위치 불러오기
				//System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

				//폴더 있는지 확인하고 생성하기
				if (!Directory.Exists("Log"))
				{
					System.IO.Directory.CreateDirectory("Log");
				}

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

		public class CMD_ID_VALUE
		{
			public string CMD { get; set; }
			public string ID { get; set; }
			public string VALUE { get; set; }
		}

		public class CMD_POS_VALUE
		{
			public string CMD { get; set; }
			public int POS { get; set; }
			public int VALUE { get; set; }
		}

		public class CMD_POS
		{
			public string CMD { get; set; }
			public int POS { get; set; }
		}
		public class COMMAND
		{
			public string CMD { get; set; }
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
			try
			{
				//clientdst.Publish(textBoxPT1.Text+data.Topic, data.Message, (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);

				/// 1. 토픽 체크
				/// 2. payload한 메세지에서 cmd를 체크
				/// 3. 원하는 값 추출
				/// 데이터그리드뷰 보여주기
				/// string topic = "dawoon/Manual/" + textBoxCode.Text.Trim() + "/1/" + "POLOR";
				/// 
				if (data.Topic == "dawoon/Manual/" + textBoxCode.Text.Trim() + "/1/" + "POLOR")
				{
					CMD_POS_VALUE cmd = JsonConvert.DeserializeObject<CMD_POS_VALUE>(Encoding.UTF8.GetString(data.Message));
					if (cmd.CMD == "RESP_MAIN_SET_READ" || cmd.CMD == "MAIN_SET_OK")
					{
						int pos = Convert.ToInt32(cmd.POS);
						int value = Convert.ToInt32(cmd.VALUE);
						dataGridViewMain["2", pos].Value = value.ToString();
					}
					else if (cmd.CMD == "RESP_METER_COUNT")
					{
						CMD_ID_COUNT resp = JsonConvert.DeserializeObject<CMD_ID_COUNT>(Encoding.UTF8.GetString(data.Message));
						//	MessageBox.Show(resp.COUNT.ToString());
						string respText = resp.COUNT.ToString();
						//textBoxIdCount.Text = respText;
						myUI(respText, textBoxIdCount);
					}
				}
				else if (data.Topic == "dawoon/meterset/" + textBoxCode.Text.Trim() + "/1/" + "POLOR")
				{
					CMD_POS_VALUE cmd = JsonConvert.DeserializeObject<CMD_POS_VALUE>(Encoding.UTF8.GetString(data.Message));
					if (cmd.CMD == "RESP_METER_SET_READ")
					{
						CMD_ID_POS_VALUE resp = JsonConvert.DeserializeObject<CMD_ID_POS_VALUE>(Encoding.UTF8.GetString(data.Message));
						int id = resp.ID;
						int pos = Convert.ToInt32(resp.POS);
						int value = Convert.ToInt32(resp.VALUE);
						dataGridViewMeter[id + 1, pos - 1].Value = value.ToString();
					}
					else if (cmd.CMD == "RESP_IR_SET_READ")
					{
						CMD_ID_POS_VALUE resp = JsonConvert.DeserializeObject<CMD_ID_POS_VALUE>(Encoding.UTF8.GetString(data.Message));
						int id = resp.ID;
						int pos = Convert.ToInt32(resp.POS);
						int value = Convert.ToInt32(resp.VALUE);
						dataGridViewIR[id + 1, pos - 1].Value = value.ToString();
					}

					else if (cmd.CMD == "METER_SET")
					{
						CMD_ID_POS_VALUE resp = JsonConvert.DeserializeObject<CMD_ID_POS_VALUE>(Encoding.UTF8.GetString(data.Message));
						int id = resp.ID;
						int pos = Convert.ToInt32(resp.POS);
						int value = Convert.ToInt32(resp.VALUE);
						dataGridViewMeter[id + 1, pos - 1].Value = value.ToString();
					}

					else if (cmd.CMD == "IR_SET")
					{
						CMD_ID_POS_VALUE resp = JsonConvert.DeserializeObject<CMD_ID_POS_VALUE>(Encoding.UTF8.GetString(data.Message));
						int id = resp.ID;
						int pos = Convert.ToInt32(resp.POS);
						int value = Convert.ToInt32(resp.VALUE);
						dataGridViewIR[id + 1, pos - 1].Value = value.ToString();
					}
					else if (cmd.CMD == "MAIN_SET")
					{
						CMD_POS_VALUE resp = JsonConvert.DeserializeObject<CMD_POS_VALUE>(Encoding.UTF8.GetString(data.Message));
						int pos = Convert.ToInt32(resp.POS);
						int value = Convert.ToInt32(resp.VALUE);
						dataGridViewMain[2, pos].Value = value.ToString();
					}
				}
				ShowMessage(data.Topic, System.Text.Encoding.UTF8.GetString(data.Message), dataGridViewMessage);
			}
			catch (Exception ex)
			{
				LogMgr.ExceptionLog(ex);
			}
		}
		private void clientsrc_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs data)
		{
			if (textBoxTopicHeader.Text.Trim().Length <= 0)
			{
				clientdst.Publish(data.Topic, data.Message, (byte)qosSelectedIndex, checkBox1.Checked);
				ShowMessage2(data.Topic, System.Text.Encoding.UTF8.GetString(data.Message), dataGridViewDelivery);
			}
			else
			{
				clientdst.Publish(textBoxTopicHeader.Text.Trim() + data.Topic, data.Message, (byte)qosSelectedIndex, checkBox1.Checked);
				ShowMessage2(textBoxTopicHeader.Text.Trim() + data.Topic, System.Text.Encoding.UTF8.GetString(data.Message), dataGridViewDelivery);
			}
		}
		private void clientdst_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs data)
		{

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
				ctl.Text = myStr;
				//ctl.AppendText(myStr + Environment.NewLine);
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




		public class LogMgr

		{
			static string prefixMsg = "\r\n-------------------------------------------";
			// 일반 로그 남기기
			static public void Log(string msg)
			{
				try
				{
					string currentPath = System.IO.Directory.GetCurrentDirectory();
					string logName = @"" + currentPath + "\\Log\\" + "Error-MQTT-" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
					string logFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
					StreamWriter tw = File.AppendText(logName);
					tw.WriteLine(prefixMsg);
					tw.WriteLine(msg);
					tw.Close();
				}
				catch (Exception)
				{
				}
			}
			// 예외 로그
			static public void ExceptionLog(Exception ex, string titleString = null)
			{
				string msg = "";
				if (!string.IsNullOrEmpty(msg))
				{
					msg = titleString + "\r\n";
				}
				msg += GetExceptionMessage(ex);
				msg = "예외 발생 일자 : " + DateTime.Now.ToString() + msg + "\r\n";
				Log(msg);
			}
			// 예외 메세지 만들기
			static private string GetExceptionMessage(Exception ex)
			{
				string err = "\r\n 에러 발생 원인 : " + ex.Source +
					"\r\n 에러 메시지 :" + ex.Message +
					"\r\n Stack Trace : " + ex.StackTrace.ToString();
				if (ex.InnerException != null)
				{
					err += GetExceptionMessage(ex.InnerException);
				}
				return err;
			}

			static public void mainLog(string msg)
			{
				try
				{
					string currentPath = System.IO.Directory.GetCurrentDirectory();
					string logName = @"" + currentPath + "\\Log\\" + "Main-" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
					string logFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
					StreamWriter tw = File.AppendText(logName);
					//tw.WriteLine(prefixMsg);
					tw.WriteLine(msg);
					tw.Close();
				}
				catch (Exception)
				{
				}
			}
			static public void mainLogInfo(string msg, string titleString = null)
			{

				if (!string.IsNullOrEmpty(msg))
				{
					msg = titleString;
				}
				msg = "예외 발생 일자 : " + DateTime.Now.ToString() + msg + "\r\n";
				mainLog(msg);
			}

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
					clientUser = new MqttClient(textBoxHost.Text, Int32.Parse(textBoxPort.Text), false, null, null, MqttSslProtocols.None);
					clientUser.Connect(Guid.NewGuid().ToString());
					clientUser.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(client_MqttMsgPublishReceived);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					LogMgr.ExceptionLog(ex);
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
			buttonSubscribe_Click(sender, e);
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
					LogMgr.ExceptionLog(ex);
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
			buttonPublish4.Enabled = false;
			buttonPublish5.Enabled = false;
			buttonPublish6.Enabled = false;
			buttonPublish7.Enabled = false;
			buttonPublish8.Enabled = false;
			buttonPublish9.Enabled = false;
			buttonPublish10.Enabled = false;
			buttonMeter.Enabled = false;
			buttonIdCount.Enabled = false;
			buttonIR.Enabled = false;
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
				LogMgr.ExceptionLog(ex);
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
				LogMgr.ExceptionLog(ex);
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
			try
			{
				string text = e.Value.ToString().Trim();
				string[] text2 = text.Split(',');

				if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
				{
					if ((e.Value != null))
					{
						string[] redSplit = textBoxRed.Text.Split(',');
						for (int i = 0; i < redSplit.Length; i++)
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
								e.CellStyle.ForeColor = Color.White;
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
				LogMgr.ExceptionLog(ex);
			}
		}

		private void cell2(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
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
			}
			catch (Exception ex)
			{
				LogMgr.ExceptionLog(ex);
			}
		}

		private void dataGridViewMessage_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			cell1(sender, e);
		}



		/// <summary>

		/// 폼 닫기 이벤트 핸들러 선언

		/// </summary>

		/// <param name="sender"></param>

		/// <param name="e"></param>

		public void Form_Closing(object sender, FormClosedEventArgs e)
		{
			try
			{
				initCloseMethod();
				if (clientUser != null && clientUser.IsConnected) clientUser.Disconnect();
				if (clientscr != null && clientscr.IsConnected) clientscr.Disconnect();
				if (clientdst != null && clientdst.IsConnected) clientdst.Disconnect();

			}
			catch (Exception error)
			{
				MessageBox.Show(error.ToString());
				LogMgr.ExceptionLog(error);
			}
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
				for (int i = 0; i < 40; i++)
				{
					Application.DoEvents();

					if (checkBoxStop.Checked)
					{
						break;
					}
					// REQ CLASS 생성 2개를 가져오는
					// CMD ,POS 값
					CMD_POS req = new CMD_POS();
					req.CMD = "REQ_MAIN_SET_READ";
					req.POS = i;
					string reqStr = JsonConvert.SerializeObject(req);
					clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
					Delay(Convert.ToInt32(textBoxDelay.Text));
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
				if (dataGridViewMain.Columns.Count < 3)
				{
					return;
				}
				dataGridViewMain.Columns[3].HeaderText = "헤링본";
				dataGridViewMain.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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


		private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (tabControl2.SelectedTab == tabPageMain)
				{
					//dataGridViewMain.Columns.Clear();
					buttonIdCount.Enabled = true;
					dataGridViewMain.RowHeadersVisible = false;


				}
				else if (tabControl2.SelectedTab == tabPageMeter)
				{
					/*
					 * 	buttonIdCount.Enabled = true;
				dataGridViewMeter.Columns.Clear();
				dataGridViewMeter.ReadOnly = true;
				dataGridViewMeter.RowHeadersVisible = false;
				//dataGridViewMeter.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				dataGridViewMeter.Columns.Add("0", "POS");
				dataGridViewMeter.Columns.Add("1", "DESC");
				if (textBoxIdCount.Text == "")
				{
					return;
				}
				if (textBoxIdCount.Text.Trim().Length <= 0)
				{
					return;
				}
				int bbbb = Convert.ToInt32(textBoxIdCount.Text);
				for (int i = 1; i <= bbbb; i++)
				{
					dataGridViewMeter.Columns.Add((i).ToString(), i.ToString());
				}
				dataGridViewMeter.Columns.Add((dataGridViewMeter.ColumnCount).ToString(), "텐덤/해링본");
					for (int i = 0; i < 46; i++)
					{
						dataGridViewMeter.Rows.Add();
						dataGridViewMeter["0", i].Value = i + 1;
					}
					dataGridViewMeter["1", 0].Value = "사용안함";
					dataGridViewMeter["1", 1].Value = "장비 ID(1~20)";
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
					dataGridViewMeter.Columns[0].Width = 40;
					dataGridViewMeter.Columns[1].Width = 250;
					dataGridViewMeter.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
					 */
				}
				else if (tabControl2.SelectedTab == tabPageIR)
				{
					/*
					 * buttonIdCount.Enabled = true;
				dataGridViewIR.Columns.Clear();
				dataGridViewIR.ReadOnly = true;
				dataGridViewIR.RowHeadersVisible = false;
				dataGridViewIR.Columns.Add("0", "POS");
				dataGridViewIR.Columns.Add("1", "DESC");
				if (textBoxIdCount.Text == "")
				{
					return;
				}
				//int bbbb = Convert.ToInt32(textBoxIdCount.Text);
				for (int i = 1; i <= bbbb; i++)
				{
					dataGridViewIR.Columns.Add((i).ToString(), i.ToString());
				}
				for (int i = 0; i < 10; i++)
				{
					dataGridViewIR.Rows.Add();
					dataGridViewIR["0", i].Value = i + 1;
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
				dataGridViewIR.Columns[1].Width = 600;
				//dataGridViewIR.Columns[(dataGridViewMeter.ColumnCount).ToString()].Width = 35;
				dataGridViewIR.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
					 */
				}

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				LogMgr.ExceptionLog(ex);
			}
		}

		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				int bbbb;
				if (radioButton3.Checked == true)
				{
					if (dataGridViewMeter.Columns.Count < 3)
					{
						return;
					}
					int dgvHeader = dataGridViewMeter.Columns.Count - 1;

					if (textBoxIdCount.Text == "")
					{
						bbbb = 0;
					}
					else
					{
						bbbb = Convert.ToInt32(textBoxIdCount.Text);
					}
					int dgvValue = bbbb + 2;
					dataGridViewMeter.Columns[dgvHeader].HeaderText = "헤링본";
					int idx = 0;
					dataGridViewMeter["1", 1].Value = "장비 ID(1~12)";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "60";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "0";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "180";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "35";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "60";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "120";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "60";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "3";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "2";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "8000";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "200";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1000";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "500";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "5";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1500";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "3000";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "2000";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "2";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "6";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "2";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "0";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "2100";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "2100";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "0";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "2000";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "10";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "60";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "53";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "58";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "83";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1100";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "5000";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "2";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1000";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "20";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "50";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "5";
					dataGridViewMeter[dgvValue.ToString(), idx++].Value = "0";
				}
			}

			catch (Exception ex)
			{
				LogMgr.ExceptionLog(ex);
			}
		}
		private void panel11_Paint(object sender, PaintEventArgs e)
		{
		}


		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			int bbbb;
			if (radioButton4.Checked == true)
			{
				if (dataGridViewMeter.Columns.Count < 1)
				{
					return;
				}
				int dgvHeader = dataGridViewMeter.Columns.Count - 1;
				if (textBoxIdCount.Text == "")
				{
					bbbb = 0;
				}
				else
				{
					bbbb = Convert.ToInt32(textBoxIdCount.Text);
				}
				int dgvValue = bbbb + 2;
				dataGridViewMeter.Columns[dgvHeader].HeaderText = "텐덤";

				int idx = 0;

				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "60";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "0";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "180";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "35";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "60";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "120";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "60";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "5";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "2";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "8000";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1000";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1000";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "500";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "5";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1500";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "3000";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "2000";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "2";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "7";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "2";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "0";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "0";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "2100";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1200";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "10";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "2000";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "10";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "60";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "59";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "59";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "83";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1100";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "5000";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "2";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "1000";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "20";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "50";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "5";
				dataGridViewMeter[dgvValue.ToString(), idx++].Value = "0";
				dataGridViewMeter["1", 1].Value = "장비 ID(1~6)";
				dataGridViewMeter["1", 11].Value = "유방염 에러 밸류 (세이버없음)";
				dataGridViewMeter["1", 12].Value = "혈류 에러 벨류(mg/L) (세이버없음)";
			}
		}


		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			//tabControl2_SelectedIndexChanged(sender, e);
			radioButton1_CheckedChanged(sender, e);
			radioButton2_CheckedChanged(sender, e);
			radioButton3_CheckedChanged(sender, e);
			radioButton4_CheckedChanged(sender, e);
		}
		private void tabcontrols()
		{
			//tabControl2_SelectedIndexChanged(sender, e);
		}

		private void buttonIdCount_Click(object sender, EventArgs e)
		{

			//MessageBox.Show("데이타가 변경됩니다. 하시겠습니까?","변경여부확인",MessageBoxButtons.OK,MessageBoxIcon.Warning);
			if (MessageBox.Show("데이타가 변경됩니다. 하시겠습니까?", "변경여부확인", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}

			buttonMeter.Enabled = true;
			buttonIR.Enabled = true;
			string topic = "dawoon/Manual/" + textBoxCode.Text.Trim() + "/1/POLOR";
			try
			{
				//	if (clientUser.IsConnected)
				//{
				//	clientUser = new MqttClient(textBoxHost.Text);
				//	clientUser.Connect(Guid.NewGuid().ToString());
				//	clientUser.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(client_MqttMsgPublishReceived);
				//}
				//if ((tabControl2.SelectedTab == tabPageMeter) || (tabControl2.SelectedTab == tabPageIR))
				//{
				textBoxIdCount.Text = "";
				//COMMAND req = new COMMAND();
				//req.CMD = "REQ_METER_COUNT";
				CMD_ID_COUNT req = new CMD_ID_COUNT();
				req.CMD = "REQ_METER_COUNT";
				req.ID = 1;
				req.COUNT = 10;
				string reqStr = (JsonConvert.SerializeObject(req)).Trim();
				clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				//}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				LogMgr.ExceptionLog(ex);
			}

		}

		private void buttonMeter_Click(object sender, EventArgs e)
		{
			string topic = "dawoon/meterset/" + textBoxCode.Text.Trim() + "/1/" + "POLOR";
			try
			{
				CMD_ID_POS req = new CMD_ID_POS();
				for (int i = 1; i <= 46; i++)

				{
					Application.DoEvents();
					if (checkBoxStop.Checked)
					{
						break;
					}
					for (int k = 1; k <= Convert.ToInt32(textBoxIdCount.Text); k++)

					{
						Application.DoEvents();
						if (checkBoxStop.Checked)
						{
							break;
						}
						req.ID = k;
						req.CMD = "REQ_METER_SET_READ";
						req.POS = i;
						string reqStr = JsonConvert.SerializeObject(req);
						clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
						Delay(Convert.ToInt32(textBoxDelay.Text));
					}


				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void buttonIR_Click(object sender, EventArgs e)
		{
			string topic = "dawoon/meterset/" + textBoxCode.Text.Trim() + "/1/" + "POLOR";
			try
			{
				CMD_ID_POS req = new CMD_ID_POS();
				for (int i = 1; i <= 10; i++)

				{
					Application.DoEvents();

					if (checkBoxStop.Checked)
					{
						break;
					}
					for (int k = 1; k <= Convert.ToInt32(textBoxIdCount.Text); k++)

					{
						Application.DoEvents();

						if (checkBoxStop.Checked)
						{
							break;
						}
						req.ID = k;
						req.CMD = "REQ_IR_SET_READ";
						req.POS = i;

						string reqStr = JsonConvert.SerializeObject(req);
						clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
						Delay(Convert.ToInt32(textBoxDelay.Text));
					}

				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void textBoxIdCount_TextChanged(object sender, EventArgs e)
		{
			int bbbb;
			//tabControl2_SelectedIndexChanged(sender, e);
			buttonMeter.Enabled = true;
			buttonIR.Enabled = true;
			//if (tabControl2.SelectedTab == tabPageMeter)
			//{

			dataGridViewMeter.Columns.Clear();
			dataGridViewMeter.RowHeadersVisible = false;
			//dataGridViewMeter.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridViewMeter.Columns.Add("0", "POS");
			dataGridViewMeter.Columns.Add("1", "DESC");
			if (textBoxIdCount.Text == "")
			{
				bbbb = 0;
			}
			else
			{
				bbbb = Convert.ToInt32(textBoxIdCount.Text);
				for (int i = 1; i <= bbbb; i++)
				{
					dataGridViewMeter.Columns.Add((i).ToString(), i.ToString());
				}
			}
			dataGridViewMeter.Columns.Add((dataGridViewMeter.ColumnCount).ToString(), "텐덤/해링본");

			for (int i = 0; i < 46; i++)
			{
				dataGridViewMeter.Rows.Add();
				dataGridViewMeter["0", i].Value = i + 1;

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
			dataGridViewMeter.Columns[0].Width = 40;
			dataGridViewMeter.Columns[1].Width = 250;
			dataGridViewMeter.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			//}

			//if (tabControl2.SelectedTab == tabPageIR)
			//{

			dataGridViewIR.Columns.Clear();
			dataGridViewIR.RowHeadersVisible = false;
			//dataGridViewIR.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridViewIR.Columns.Add("0", "POS");
			dataGridViewIR.Columns.Add("1", "DESC");

			if (textBoxIdCount.Text == "")
			{
				bbbb = 0;
			}
			else
			{
				bbbb = Convert.ToInt32(textBoxIdCount.Text);
				for (int i = 1; i <= bbbb; i++)
				{
					dataGridViewIR.Columns.Add((i).ToString(), i.ToString());
				}
			}
			for (int i = 0; i < 10; i++)
			{
				dataGridViewIR.Rows.Add();
				dataGridViewIR["0", i].Value = i + 1;
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
			dataGridViewIR.Columns[0].Width = 40;
			dataGridViewIR.Columns[1].Width = 600;
			dataGridViewIR.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			if (dataGridViewIR.Columns.Count < 3)
			{
				return;
			}

			//	}
		}



		private void buttonIRPublish_Click(object sender, EventArgs e)
		{
			string irTopic = "dawoon/meterset/" + textBoxCode.Text + "/1/POLOR";
			CMD_ID_POS_VALUE message = new CMD_ID_POS_VALUE();
			message.CMD = "IR_SET";
			message.ID = Convert.ToInt32(textBoxIrId.Text);
			message.POS = Convert.ToInt32(textBoxIrPos.Text);
			message.VALUE = Convert.ToInt32(textBoxIrValue.Text);
			string irMessage = JsonConvert.SerializeObject(message);
			clientUser.Publish(irTopic, Encoding.UTF8.GetBytes(irMessage.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);

			//string irTopic = "dawoon/meterset/" + textBoxCode.Text + "/1/POLOR";
			//string irMessage = "{ \"CMD\":\"IR_SET\",\"POS\":" + textBoxIrPos.Text + ",\"ID\":" + textBoxIrId.Text + ",\"VALUE\":" + textBoxIrValue.Text + " }";
			//clientUser.Publish(irTopic, Encoding.UTF8.GetBytes(irMessage), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
		}

		private void buttonMeterPublish_Click(object sender, EventArgs e)
		{
			string metersetTopic = "dawoon/meterset/" + textBoxCode.Text + "/1/POLOR";
			CMD_ID_POS_VALUE message = new CMD_ID_POS_VALUE();
			message.CMD = "METER_SET";
			message.ID = Convert.ToInt32(textBoxMeterId.Text);
			message.POS = Convert.ToInt32(textBoxMeterPos.Text);
			message.VALUE = Convert.ToInt32(textBoxMeterValue.Text);
			string metersetMessage = JsonConvert.SerializeObject(message);
			clientUser.Publish(metersetTopic, Encoding.UTF8.GetBytes(metersetMessage.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);

			//string metersetTopic = "dawoon/meterset/" + textBoxCode.Text + "/1/POLOR";
			//string metersetMessage = "{ \"CMD\":\"METER_SET\",\"POS\":" + textBoxMeterPos.Text + ",\"ID\":" + textBoxMeterId.Text + ",\"VALUE\":" + textBoxMeterValue.Text + " }";
			//clientUser.Publish(metersetTopic, Encoding.UTF8.GetBytes(metersetMessage), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
		}

		private void buttonMainPulish_Click(object sender, EventArgs e)
		{
			string mainTopic = "dawoon/meterset/" + textBoxCode.Text + "/1/POLOR";
			CMD_POS_VALUE message = new CMD_POS_VALUE();
			message.CMD = "MAIN_SET";
			message.POS = Convert.ToInt32(textBoxMainPos.Text);
			message.VALUE = Convert.ToInt32(textBoxMainValue.Text);
			string mainMessage = JsonConvert.SerializeObject(message);
			clientUser.Publish(mainTopic, Encoding.UTF8.GetBytes(mainMessage.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);


			//string mainTopic = "dawoon/Manual/" + textBoxCode.Text + "/1/POLOR";
			//string mainMessage = "{ \"CMD\":\"MAIN_SET\",\"POS\":" + textBoxMainPos.Text + ",\"VALUE\":" + textBoxMainValue.Text + " }";
			//clientUser.Publish(mainTopic, Encoding.UTF8.GetBytes(mainMessage), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
		}

		bool a = true;
		private void buttonAutoPubStart_Click(object sender, EventArgs e)
		{
			a = true;
			string topic = textBoxAutoPubTopic.Text;
			string newMsg = "";
			string msg = textBoxAutoPubMsg.Text;
			string[] varName = new string[10];
			int[] varInit = new int[10];
			int[] varAdd = new int[10];
			varName[0] = textBoxStr1.Text.Trim();
			varName[1] = textBoxStr2.Text.Trim();
			varName[2] = textBoxStr3.Text.Trim();
			varName[3] = textBoxStr4.Text.Trim();
			varName[4] = textBoxStr5.Text.Trim();
			varName[5] = textBoxStr6.Text.Trim();
			varName[6] = textBoxStr7.Text.Trim();
			varName[7] = textBoxStr8.Text.Trim();
			varName[8] = textBoxStr9.Text.Trim();
			varName[9] = textBoxStr10.Text.Trim();
			int i = 0;
			if (textBoxfirst1.Text.Trim().Length <= 0) varInit[i++] = 0; else varInit[i++] = Convert.ToInt32(textBoxfirst1.Text.Trim());
			if (textBoxfirst2.Text.Trim().Length <= 0) varInit[i++] = 0; else varInit[i++] = Convert.ToInt32(textBoxfirst2.Text.Trim());
			if (textBoxfirst3.Text.Trim().Length <= 0) varInit[i++] = 0; else varInit[i++] = Convert.ToInt32(textBoxfirst3.Text.Trim());
			if (textBoxfirst4.Text.Trim().Length <= 0) varInit[i++] = 0; else varInit[i++] = Convert.ToInt32(textBoxfirst4.Text.Trim());
			if (textBoxfirst5.Text.Trim().Length <= 0) varInit[i++] = 0; else varInit[i++] = Convert.ToInt32(textBoxfirst5.Text.Trim());
			if (textBoxfirst6.Text.Trim().Length <= 0) varInit[i++] = 0; else varInit[i++] = Convert.ToInt32(textBoxfirst6.Text.Trim());
			if (textBoxfirst7.Text.Trim().Length <= 0) varInit[i++] = 0; else varInit[i++] = Convert.ToInt32(textBoxfirst7.Text.Trim());
			if (textBoxfirst8.Text.Trim().Length <= 0) varInit[i++] = 0; else varInit[i++] = Convert.ToInt32(textBoxfirst8.Text.Trim());
			if (textBoxfirst9.Text.Trim().Length <= 0) varInit[i++] = 0; else varInit[i++] = Convert.ToInt32(textBoxfirst9.Text.Trim());
			if (textBoxfirst10.Text.Trim().Length <= 0) varInit[i++] = 0; else varInit[i++] = Convert.ToInt32(textBoxfirst10.Text.Trim());
			i = 0;
			if (textBoxPlus1.Text.Trim().Length <= 0) varInit[i++] = 0; else varAdd[i++] = Convert.ToInt32(textBoxPlus1.Text.Trim());
			if (textBoxPlus2.Text.Trim().Length <= 0) varInit[i++] = 0; else varAdd[i++] = Convert.ToInt32(textBoxPlus2.Text.Trim());
			if (textBoxPlus3.Text.Trim().Length <= 0) varInit[i++] = 0; else varAdd[i++] = Convert.ToInt32(textBoxPlus3.Text.Trim());
			if (textBoxPlus4.Text.Trim().Length <= 0) varInit[i++] = 0; else varAdd[i++] = Convert.ToInt32(textBoxPlus4.Text.Trim());
			if (textBoxPlus5.Text.Trim().Length <= 0) varInit[i++] = 0; else varAdd[i++] = Convert.ToInt32(textBoxPlus5.Text.Trim());
			if (textBoxPlus6.Text.Trim().Length <= 0) varInit[i++] = 0; else varAdd[i++] = Convert.ToInt32(textBoxPlus6.Text.Trim());
			if (textBoxPlus7.Text.Trim().Length <= 0) varInit[i++] = 0; else varAdd[i++] = Convert.ToInt32(textBoxPlus7.Text.Trim());
			if (textBoxPlus8.Text.Trim().Length <= 0) varInit[i++] = 0; else varAdd[i++] = Convert.ToInt32(textBoxPlus8.Text.Trim());
			if (textBoxPlus9.Text.Trim().Length <= 0) varInit[i++] = 0; else varAdd[i++] = Convert.ToInt32(textBoxPlus9.Text.Trim());
			if (textBoxPlus10.Text.Trim().Length <= 0) varInit[i++] = 0; else varAdd[i++] = Convert.ToInt32(textBoxPlus10.Text.Trim());

			int[] varInitAdd = new int[10];
			for (int j = 0; j < 10; j++)
			{
				varInitAdd[j] = varInit[j];
			}
			while (a)
			{
				for (int j = 0; j < 10; j++)
				{
					if (varName[j].Trim().Length <= 0)
					{
						continue;
					}
					varInitAdd[j] += varAdd[j];
					newMsg = msg.Replace(varName[j], varInitAdd[j].ToString());
				}

				clientUser.Publish(topic, Encoding.UTF8.GetBytes(newMsg), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				Delay(Int32.Parse(textBoxDelay1.Text));
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
		int qosSelectedIndex = 0;

		private void buttonConnet2_Click(object sender, EventArgs e)
		{
			qosSelectedIndex = comboBoxQos2.SelectedIndex;
			int port;
			if (textBoxHost2.Text.Length == 0)
			{
				return;
			}
			else if (!Int32.TryParse(textBoxPort2.Text, out port))
			{
				//로그잡기.메세지박스잡기
				return;
			}
			else
			{
				try
				{
					clientscr = new MqttClient(textBoxHost2.Text, Int32.Parse(textBoxPort2.Text), false, null, null, MqttSslProtocols.None);
					clientscr.Connect(Guid.NewGuid().ToString());
					clientscr.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(clientsrc_MqttMsgPublishReceived);
					//button2_Click_1(sender, e);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
				if (clientscr != null && clientscr.IsConnected)
				{
					buttonSubScribe3.Enabled = true;
					buttonUnscribe2.Enabled = true;
					buttonDisconnect2.Enabled = true;
					textBoxHost2.Enabled = false;
					textBoxHost3.Enabled = false;
					textBoxPort3.Enabled = false;
					textBoxPort2.Enabled = false;
					buttonConnect2.Enabled = false;
					button14.Enabled = true;
				}
			}
			int port2;
			if (textBoxHost3.Text.Length == 0)
			{
				return;
			}
			else if (!Int32.TryParse(textBoxPort3.Text, out port2))
			{
				return;
			}
			else
			{
				try
				{

					clientdst = new MqttClient(textBoxHost3.Text, Int32.Parse(textBoxPort3.Text), false, null, null, MqttSslProtocols.None);
					clientdst.Connect(Guid.NewGuid().ToString());
					clientdst.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(clientdst_MqttMsgPublishReceived);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}

			}
		}

		private void label55_Click_1(object sender, EventArgs e)
		{

		}

		private void label50_Click_1(object sender, EventArgs e)
		{

		}

		private void textBox52_TextChanged_1(object sender, EventArgs e)
		{

		}

		private void button6_Click(object sender, EventArgs e)
		{
			a = true;
			string[] topic = textBox51.Lines;
			string[] msg = textBox52.Lines;
			string[] varName = new string[10];
			int startLine = Convert.ToInt32(textBoxAllStartline.Text);
			int delayLine = Convert.ToInt32(textBoxAllDelay.Text);


			for (int j = startLine; j < topic.Length; j++)
			{
				if (a == false)
				{
					break;
				}
				if (topic[j].Trim().Length <= 0)
				{
					continue;
				}
				if (j >= msg.Length)
				{
					break;
				}
				if (msg[j].Trim().Length <= 0)
				{
					continue;
				}

				clientUser.Publish(topic[j], Encoding.UTF8.GetBytes(msg[j]), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
				Delay(delayLine);
			}

		}

		private void button5_Click(object sender, EventArgs e)
		{
			a = false;
		}

		private void panel17_Paint(object sender, PaintEventArgs e)
		{

		}



		private void textBox7_TextChanged(object sender, EventArgs e)
		{

		}

		private void panel18_Paint(object sender, PaintEventArgs e)
		{

		}

		private void panel20_Paint(object sender, PaintEventArgs e)
		{

		}

		private void label71_Click(object sender, EventArgs e)
		{

		}

		private void textBox51_TextChanged_1(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				//폴더 있는지 확인하고 생성하기
				//if (!Directory.GetCurrentDirectory().Contains("subSend.txt"))
				//{
				//	StreamWriter sw;
				//	sw = new StreamWriter("subSend.txt");
				//	sw.Close();
				//}

				listBoxSub2.Items.Clear();
				string currentPath = System.IO.Directory.GetCurrentDirectory();
				StreamReader file = new StreamReader(currentPath + "\\subSend.txt", Encoding.Default);
				string s = "";
				while (s != null)
				{
					s = file.ReadLine();
					if (!string.IsNullOrEmpty(s)) listBoxSub2.Items.Add(s);
				}
				file.Close();
			}
			catch (Exception ex)
			{
				LogMgr.ExceptionLog(ex);
			}

		}

		private void button4_Click(object sender, EventArgs e)
		{
			try
			{
				StreamWriter sw;
				sw = new StreamWriter("subSend.txt");
				int nCount = listBoxSub2.Items.Count;
				for (int i = 0; i < nCount; i++)
				{
					listBoxSub2.Items[i] += "\r\n";
					sw.Write(listBoxSub2.Items[i]);
				}
				sw.Close();
			}
			catch (Exception ex)
			{
				LogMgr.ExceptionLog(ex);
			}
		}

		private void button2_Click_1(object sender, EventArgs e)
		{
			try
			{
				foreach (var item in listBoxSub2.Items)
				{
					clientscr.Subscribe(new string[] { item.ToString() }, new byte[] { (byte)comboBoxQos2.SelectedIndex });
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void button8_Click(object sender, EventArgs e)
		{
			try
			{
				if (listBoxSub2.SelectedItem == null)
				{
					return;
				}
				else
				{
					clientscr.Unsubscribe(new string[] { listBoxSub2.SelectedItem.ToString() });
					listBoxSub2.Items.Remove(listBoxSub2.SelectedItem);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void button9_Click_1(object sender, EventArgs e)
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

		private void button12_Click(object sender, EventArgs e)
		{
			try
			{
				if (clientscr != null && clientscr.IsConnected) clientscr.Disconnect();
				if (clientdst != null && clientdst.IsConnected) clientdst.Disconnect();
				listBoxSub2.Items.Clear();
				textBoxPort3.Enabled = true;
				textBoxPort2.Enabled = true;
				textBoxHost2.Enabled = true;
				textBoxHost3.Enabled = true;
				buttonConnect2.Enabled = true;
				buttonDisconnect2.Enabled = false;
				buttonUnscribe2.Enabled = false;
				buttonSubScribe3.Enabled = false;
				button14.Enabled = false;

			}
			catch (Exception ex)
			{
				//메세지박스
				MessageBox.Show(ex.ToString());
				//로그저장

			}
		}

		private void button7_Click_1(object sender, EventArgs e)
		{
			textBoxHost2.Text = "103.60.126.23";

		}

		private void button11_Click(object sender, EventArgs e)
		{
			if (textBoxHost2.Text == "103.60.126.23")
			{
				MessageBox.Show("동일한 서버에 전송할수 없습니다.");
				return;
			}
			textBoxHost3.Text = "103.60.126.23";

		}

		private void button10_Click(object sender, EventArgs e)
		{
			textBoxHost2.Text = "127.0.0.1";

		}

		private void button13_Click(object sender, EventArgs e)
		{
			textBoxHost3.Text = "127.0.0.1";

		}

		private void button14_Click(object sender, EventArgs e)
		{
			if (textBoxScrTopic.Text.Length == 0)
			{
				return;
			}
			else
			{
				try
				{
					clientscr.Subscribe(new string[] { textBoxScrTopic.Text }, new byte[] { (byte)comboBoxQos2.SelectedIndex });
					List<string> result = new List<string>();
					result.Add(textBoxScrTopic.Text);
					if (!listBoxSub2.Items.Contains(textBoxScrTopic.Text))
					{
						this.listBoxSub2.Items.Add(textBoxScrTopic.Text);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void button15_Click(object sender, EventArgs e)
		{
			string[] msg = textBox52.Lines;
			string[] newMsg = textBox52.Lines;

			for (int i = 0; i < msg.Length; i++)
			{
				if (msg[i].Length < Convert.ToInt32(textBoxDelLocation.Text)) continue;

				newMsg[i] = msg[i].Substring(Convert.ToInt32(textBoxDelLocation.Text) - 1, msg[i].Length - Convert.ToInt32(textBoxDelLocation.Text) + 1);
			}
			textBox52.Lines = newMsg;
		}

		private void button16_Click(object sender, EventArgs e)
		{
			if (textBoxSearchString.Text == "")
			{
				return;
			}
			else
			{
				string[] key = textBoxSearchString.Text.Split(',');
				string[] msg = textBox52.Lines;

				List<string> list = new List<string>();
				for (int i = 0; i < msg.Length; i++)
				{
					for (int j = 0; j < key.Length; j++)
					{
						if (key[j].Trim().Length <= 0)
						{
							continue;
						}
						if (msg[i].Contains(key[j].Trim()))
						{
							list.Add(msg[i]);
							break;
						}
					}
				}
				textBox52.Lines = list.ToArray();
			}
		}

		private void button17_Click(object sender, EventArgs e)
		{
			string key = textBoxSeperateString.Text;
			string[] msg = textBox52.Lines;

			List<string> listTopic = new List<string>();
			List<string> listMessage = new List<string>();
			for (int i = 0; i < msg.Length; i++)
			{
				string[] tm = new string[2];
				int pos = msg[i].IndexOf(key);
				if (pos == -1) continue;
				tm[0] = msg[i].Substring(0, pos);
				tm[1] = msg[i].Substring(pos + key.Length);
				listTopic.Add(tm[0]);
				listMessage.Add(tm[1]);

			}
			textBox51.Lines = listTopic.ToArray();
			textBox52.Lines = listMessage.ToArray();
		}

		private void textBox9_TextChanged(object sender, EventArgs e)
		{

		}

		private void button18_Click(object sender, EventArgs e)
		{
			textBox51.Text = "";
		}

		private void button19_Click(object sender, EventArgs e)
		{
			textBox52.Text = "";
		}

		private void dataGridViewMain_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				return;
			}
			textBoxMainPos.Text = dataGridViewMain.Rows[e.RowIndex].Cells[0].Value.ToString();
			if (dataGridViewMain.Rows[e.RowIndex].Cells[2].Value == null)
			{
				textBoxMainValue.Text = "";
			}
			else
				textBoxMainValue.Text = dataGridViewMain.Rows[e.RowIndex].Cells[2].Value.ToString();
		}

		private void panel19_Paint(object sender, PaintEventArgs e)
		{

		}

		private void label59_Click(object sender, EventArgs e)
		{

		}

		private void label60_Click(object sender, EventArgs e)
		{

		}

		private void label61_Click(object sender, EventArgs e)
		{

		}

		private void dataGridViewMeter_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				return;
			}
			textBoxMeterPos.Text = dataGridViewMeter.Rows[e.RowIndex].Cells[0].Value.ToString();

			if ((dataGridViewMeter.Columns[e.ColumnIndex].HeaderText == "헤링본") || (dataGridViewMeter.Columns[e.ColumnIndex].HeaderText == "DESC"))
			{
				return;
			}
			else textBoxMeterId.Text = (dataGridViewMeter.Columns[e.ColumnIndex].HeaderText);

			if (dataGridViewMeter.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
			{
				textBoxMeterValue.Text = "";
			}
			else textBoxMeterValue.Text = dataGridViewMeter.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
		}

		private void dataGridViewIR_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				return;
			}
			textBoxIrPos.Text = dataGridViewIR.Rows[e.RowIndex].Cells[0].Value.ToString();

			if (dataGridViewIR.Columns[e.ColumnIndex].HeaderText == "DESC")
			{
				return;
			}
			else textBoxIrId.Text = (dataGridViewIR.Columns[e.ColumnIndex].HeaderText);

			if (dataGridViewIR.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
			{
				textBoxIrValue.Text = "";
			}
			else textBoxIrValue.Text = dataGridViewIR.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
		}

		private void textBoxHost2_TextChanged(object sender, EventArgs e)
		{

		}

		private void label63_Click(object sender, EventArgs e)
		{

		}

		private void textBox3_TextChanged(object sender, EventArgs e)
		{

		}

		private void label64_Click(object sender, EventArgs e)
		{

		}

		private void comboBoxQos2_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void label62_Click(object sender, EventArgs e)
		{

		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void label66_Click(object sender, EventArgs e)
		{

		}

		private void dataGridViewDelivery_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void textBox52_DragDrop(object sender, DragEventArgs e)
		{
			textBox52.Text = "";
			string[] strFileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
			foreach (string strFilePath in strFileNames)
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
				{
					textBox52.AppendText(System.IO.File.ReadAllText(strFilePath));
					//textBox52.Text += System.IO.File.ReadAllText(strFilePath);
				}
			}
			//	// 파일의 내용을 텍스트박스에 추가
			//	//txtMain.AppendText( System.IO.File.ReadAllText(strFilePath));
			//	// 파일의 내용만 텍스트박스에 출력
			//	textBox52.Text += System.IO.File.ReadAllText(strFilePath);
			//	// 하나만 출력하고 그대로 빠져나감
			//	//break;
			// DragDrop은 리스트 컨트롤에 마우스를 드래그해서 놓았을때 발생하는 함수
			button16_Click(sender, e);
		}

		private void textBox52_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}

		private void button2_Click_2(object sender, EventArgs e)
		{
			string[] array = textBox52.Lines;




			List<string> listMessage = new List<string>();
			listMessage.AddRange(array);
			listMessage.Sort();
			textBox52.Lines = listMessage.ToArray();
		}

		private void buttonMainRead1_Click(object sender, EventArgs e)
		{
			string topic = "dawoon/Manual/" + textBoxCode.Text.Trim() + "/1/" + "POLOR";
			try
			{
				// REQ CLASS 생성 2개를 가져오는
				// CMD ,POS 값
				CMD_POS_VALUE req = new CMD_POS_VALUE();
				req.CMD = "REQ_MAIN_SET_READ";
				req.POS = Convert.ToInt32(textBoxMainPos.Text);

				string reqStr = JsonConvert.SerializeObject(req);
				clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				LogMgr.ExceptionLog(ex);
			}
		}



		private void buttonMeterRead_Click(object sender, EventArgs e)
		{
			string topic = "dawoon/meterset/" + textBoxCode.Text.Trim() + "/1/" + "POLOR";
			try
			{
				CMD_ID_POS_VALUE req = new CMD_ID_POS_VALUE();
				req.CMD = "REQ_METER_SET_READ";
				req.ID = Convert.ToInt32(textBoxMeterId.Text);
				req.POS = Convert.ToInt32(textBoxMeterPos.Text);
				string reqStr = JsonConvert.SerializeObject(req);
				clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				LogMgr.ExceptionLog(ex);
			}
		}

		private void buttonIrRead_Click(object sender, EventArgs e)
		{
			string topic = "dawoon/meterset/" + textBoxCode.Text.Trim() + "/1/" + "POLOR";
			try
			{
				CMD_ID_POS_VALUE req = new CMD_ID_POS_VALUE();
				req.CMD = "REQ_IR_SET_READ";
				req.ID = Convert.ToInt32(textBoxIrId.Text);
				req.POS = Convert.ToInt32(textBoxIrPos.Text);
				string reqStr = JsonConvert.SerializeObject(req);
				clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				LogMgr.ExceptionLog(ex);
			}
		}

		private void button8_Click_1(object sender, EventArgs e)
		{
			string topic = "dawoon/meterset/" + textBoxCode.Text.Trim() + "/1/" + "POLOR";
			try
			{
				CMD_ID_POS req = new CMD_ID_POS();

				for (int i = 1; i <= 46; i++)
				{
					Application.DoEvents();
					if (checkBoxStop.Checked)
					{
						break;
					}
					req.ID = Convert.ToInt32(textBoxMeterId.Text);
					req.CMD = "REQ_METER_SET_READ";
					req.POS = i;
					string reqStr = JsonConvert.SerializeObject(req);
					clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
					Delay(Convert.ToInt32(textBoxDelay.Text));
				}


			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void button12_Click_1(object sender, EventArgs e)
		{
			string topic = "dawoon/meterset/" + textBoxCode.Text.Trim() + "/1/" + "POLOR";
			try
			{
				CMD_ID_POS req = new CMD_ID_POS();


				for (int i = 1; i <= 14; i++)
				{
					Application.DoEvents();

					if (checkBoxStop.Checked)
					{
						break;
					}
					req.ID = Convert.ToInt32(textBoxIrId.Text);
					req.CMD = "REQ_IR_SET_READ";
					req.POS = i;

					string reqStr = JsonConvert.SerializeObject(req);
					clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
					Delay(Convert.ToInt32(textBoxDelay.Text));
				}


			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void buttonIdAllWrite_Click(object sender, EventArgs e)
		{
			string mainTopic = "dawoon/meterset/" + textBoxCode.Text + "/1/POLOR";
			CMD_POS_VALUE message = new CMD_POS_VALUE();
			message.CMD = "MAIN_SET";
			message.POS = Convert.ToInt32(textBoxMainPos.Text);
			message.VALUE = Convert.ToInt32(textBoxMainValue.Text);
			string mainMessage = JsonConvert.SerializeObject(message);
			clientUser.Publish(mainTopic, Encoding.UTF8.GetBytes(mainMessage.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);


			//string mainTopic = "dawoon/Manual/" + textBoxCode.Text + "/1/POLOR";
			//string mainMessage = "{ \"CMD\":\"MAIN_SET\",\"POS\":" + textBoxMainPos.Text + ",\"VALUE\":" + textBoxMainValue.Text + " }";
			//clientUser.Publish(mainTopic, Encoding.UTF8.GetBytes(mainMessage), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
		}

		private void buttonIdAllWrite_Click_1(object sender, EventArgs e)
		{
			for (int i = 1; i <= Convert.ToInt32(textBoxIdCount.Text); i++)
			{
				string metersetTopic = "dawoon/meterset/" + textBoxCode.Text + "/1/POLOR";
				CMD_ID_POS_VALUE message = new CMD_ID_POS_VALUE();
				message.CMD = "METER_SET";
				message.ID = i;
				message.POS = Convert.ToInt32(textBoxMeterPos.Text);
				message.VALUE = Convert.ToInt32(textBoxMeterValue.Text);
				string metersetMessage = JsonConvert.SerializeObject(message);
				clientUser.Publish(metersetTopic, Encoding.UTF8.GetBytes(metersetMessage.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
			}


			//string metersetTopic = "dawoon/meterset/" + textBoxCode.Text + "/1/POLOR";
			//string metersetMessage = "{ \"CMD\":\"METER_SET\",\"POS\":" + textBoxMeterPos.Text + ",\"ID\":" + textBoxMeterId.Text + ",\"VALUE\":" + textBoxMeterValue.Text + " }";
			//clientUser.Publish(metersetTopic, Encoding.UTF8.GetBytes(metersetMessage), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
		}

		private void buttonIdAllWrite2_Click(object sender, EventArgs e)
		{
			for (int i = 1; i <= Convert.ToInt32(textBoxIdCount.Text); i++)
			{
				string irTopic = "dawoon/meterset/" + textBoxCode.Text + "/1/POLOR";
				CMD_ID_POS_VALUE message = new CMD_ID_POS_VALUE();
				message.CMD = "IR_SET";
				message.ID = i;
				message.POS = Convert.ToInt32(textBoxIrPos.Text);
				message.VALUE = Convert.ToInt32(textBoxIrValue.Text);
				string irMessage = JsonConvert.SerializeObject(message);
				clientUser.Publish(irTopic, Encoding.UTF8.GetBytes(irMessage.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
			}
		}

		private void buttonMainClear_Click(object sender, EventArgs e)
		{
			button27_Click(sender, e);
			for (int i = 0; i < 40; i++)
			{
				dataGridViewMain[2, i].Value = "";

			}
		}

		private void buttonMeterClear_Click(object sender, EventArgs e)
		{
			button16_Click_1(sender, e);
			//CMD_ID_POS req = new CMD_ID_POS();
			for (int k = 1; k <= Convert.ToInt32(textBoxIdCount.Text); k++)
			{
				for (int i = 1; i <= 46; i++)
				{
					dataGridViewMeter[k + 1, i - 1].Value = "";
				}
			}
			//DragEnter는 마우스로 리스트컨트롤 안으로 들어왔을 때 발생하는 함수
		}

		private void buttonIrClear_Click(object sender, EventArgs e)
		{
			//CMD_ID_POS req = new CMD_ID_POS();
			for (int k = 1; k <= Convert.ToInt32(textBoxIdCount.Text); k++)
			{
				for (int i = 1; i <= 10; i++)
				{
					dataGridViewIR[k + 1, i - 1].Value = "";
				}
			}
			//DragEnter는 마우스로 리스트컨트롤 안으로 들어왔을 때 발생하는 함수
			button24_Click(sender, e);
		}

		private void buttonMainSave_Click(object sender, EventArgs e)
		{
			string fileName = "";

			SaveFileDialog saveFile = new SaveFileDialog();

			// 다이얼 로그가 Open되었을 때 최초의 경로 설정
			//saveFile.InitialDirectory = @"C:";   

			// 다이얼 로그의 제목
			saveFile.Title = "저장 위치 지정";

			// 기본 확장자
			saveFile.DefaultExt = "main";

			// 파일 목록 필터링
			saveFile.Filter = "main files(*.main)|*.main";

			saveFile.FileName = textBoxCode.Text + "-main-" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss");


			// OK버튼을 눌렀을때의 동작
			if (saveFile.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			// 경로와 파일명을 fileName에 저장
			fileName = saveFile.FileName.ToString();



			try
			{
				//StreamWriter sw;
				//sw = new StreamWriter(fileName, append: false);

				//sw.WriteLine(textBoxIdCount.Text);

				//for (int i = 1; i <= 46; i++)
				//{
				//	string str = "";
				//	for (int k = 1; k <= Convert.ToInt32(textBoxIdCount.Text); k++)
				//	{
				//		str += dataGridViewMeter[k + 1, i - 1].Value + " | ";
				//	}
				//	sw.WriteLine(str);

				//}
				//sw.Close();



				StreamWriter sw;
				sw = new StreamWriter(fileName, append: false);
				for (int i = 0; i < 40; i++)
				{
					string str = "";
					//for (int k = 1; k <= Convert.ToInt32(textBoxIdCount.Text); k++)
					//{
					str += dataGridViewMain[2, i].Value;
					//}
					sw.WriteLine(str);

				}
				sw.Close();

				for (int i = 0; i < 40; i++)
				{
					sw.WriteLine(dataGridViewMain[2, i].Value);
				}
				sw.Close();
			}
			catch (Exception ex)
			{
				LogMgr.ExceptionLog(ex);
			}

		}

		private void buttonMeterSave_Click(object sender, EventArgs e)
		{
			string fileName = "";

			SaveFileDialog saveFile = new SaveFileDialog();

			// 다이얼 로그가 Open되었을 때 최초의 경로 설정
			//saveFile.InitialDirectory = @"C:";   

			// 다이얼 로그의 제목
			saveFile.Title = "저장 위치 지정";

			// 기본 확장자
			saveFile.DefaultExt = "meter";

			// 파일 목록 필터링
			saveFile.Filter = "meter files(*.meter)|*.meter";

			saveFile.FileName = textBoxCode.Text + "-meter-" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss");

			// OK버튼을 눌렀을때의 동작
			if (saveFile.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			// 경로와 파일명을 fileName에 저장
			fileName = saveFile.FileName.ToString();
			try
			{
				StreamWriter sw;
				sw = new StreamWriter(fileName, append: false);

				sw.WriteLine(textBoxIdCount.Text);

				for (int i = 1; i <= 46; i++)
				{
					string str = "";
					for (int k = 1; k <= Convert.ToInt32(textBoxIdCount.Text); k++)
					{
						str += dataGridViewMeter[k + 1, i - 1].Value + " | ";
					}
					sw.WriteLine(str);

				}
				sw.Close();
			}
			catch (Exception ex)
			{
				LogMgr.ExceptionLog(ex);
			}


		}



		private void buttonIrSave_Click(object sender, EventArgs e)
		{
			string fileName = "";

			SaveFileDialog saveFile = new SaveFileDialog();

			// 다이얼 로그가 Open되었을 때 최초의 경로 설정
			//saveFile.InitialDirectory = @"C:";   

			// 다이얼 로그의 제목
			saveFile.Title = "저장 위치 지정";

			// 기본 확장자
			saveFile.DefaultExt = "ir";

			// 파일 목록 필터링
			saveFile.Filter = "ir files(*.ir)|*.ir";

			saveFile.FileName = textBoxCode.Text + "-ir-" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss");

			// OK버튼을 눌렀을때의 동작
			if (saveFile.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			// 경로와 파일명을 fileName에 저장
			fileName = saveFile.FileName.ToString();





			try
			{
				StreamWriter sw;
				sw = new StreamWriter(fileName, append: false);

				sw.WriteLine(textBoxIdCount.Text);

				for (int i = 1; i <= 10; i++)
				{
					string str = "";
					for (int k = 1; k <= Convert.ToInt32(textBoxIdCount.Text); k++)
					{
						str += dataGridViewIR[k + 1, i - 1].Value + " | ";
					}
					sw.WriteLine(str);

				}
				sw.Close();
			}
			catch (Exception ex)
			{
				LogMgr.ExceptionLog(ex);
			}
			//string currentPath = System.IO.Directory.GetCurrentDirectory();
			//try
			//{
			//	string save = @"" + currentPath + "\\Log\\" + "Ir-" + DateTime.Now.ToString("yyyy-MM-dd") + ".ir";
			//	StreamWriter sw;
			//	sw = new StreamWriter(save, append: false);
			//	for (int i = 1; i <= 10; i++)
			//	{
			//		for (int k = 1; k <= Convert.ToInt32(textBoxIdCount.Text); k++)
			//		{
			//			sw.WriteLine(dataGridViewIR[k + 1, i - 1].Value);
			//		}
			//	}
			//	sw.Close();
			//}
			//catch (Exception ex)
			//{
			//	LogMgr.ExceptionLog(ex);
			//}
		}
		private void dataGridViewMeter_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (dataGridViewMeter.Rows.Count <= 0)
			{
				return;
			}
			if (e.ColumnIndex == 0)
			{
				for (int i = 1; i <= Convert.ToInt32(textBoxIdCount.Text); i++)
				{
					string topic = "dawoon/meterset/" + textBoxCode.Text.Trim() + "/1/" + "POLOR";
					try
					{
						CMD_ID_POS_VALUE req = new CMD_ID_POS_VALUE();
						req.CMD = "REQ_METER_SET_READ";
						req.ID = i;
						req.POS = Convert.ToInt32(textBoxMeterPos.Text);
						string reqStr = JsonConvert.SerializeObject(req);
						clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
						LogMgr.ExceptionLog(ex);
					}
				}
			}
		}

		private void dataGridViewIR_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (dataGridViewIR.Rows.Count <= 0)
			{
				return;
			}
			if (e.ColumnIndex == 0)
			{
				for (int i = 1; i <= Convert.ToInt32(textBoxIdCount.Text); i++)
				{
					string topic = "dawoon/meterset/" + textBoxCode.Text.Trim() + "/1/" + "POLOR";
					try
					{
						CMD_ID_POS_VALUE req = new CMD_ID_POS_VALUE();
						req.CMD = "REQ_IR_SET_READ";
						req.ID = i;
						req.POS = Convert.ToInt32(textBoxIrPos.Text);
						string reqStr = JsonConvert.SerializeObject(req);
						clientUser.Publish(topic, Encoding.UTF8.GetBytes(reqStr.Replace(" ", "")), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
						LogMgr.ExceptionLog(ex);
					}
				}
			}
		}

		private void buttonMainLoad_Click(object sender, EventArgs e)
		{
			string fileName = "";

			OpenFileDialog openFile = new OpenFileDialog();

			// 다이얼 로그가 Open되었을 때 최초의 경로 설정
			//saveFile.InitialDirectory = @"C:";   

			// 다이얼 로그의 제목
			openFile.Title = "저장 위치 지정";

			// 기본 확장자
			openFile.DefaultExt = "main";

			// 파일 목록 필터링
			openFile.Filter = "main files(*.main)|*.main";

			openFile.FileName = textBoxCode.Text + "-main-" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss");


			// OK버튼을 눌렀을때의 동작
			if (openFile.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			// 경로와 파일명을 fileName에 저장
			fileName = openFile.FileName.ToString();







			//try
			//{
			//	StreamReader sr;
			//	sr = new StreamReader(fileName);

			//	for (int i = 1; i <= 40; i++)
			//	{
			//		string s2 = sr.ReadLine();
			//		dataGridViewMain[2, i - 1].Value = s2.Trim();
			//	}
			//	sr.Close();
			//}
			//catch (Exception ex)
			//{
			//	LogMgr.ExceptionLog(ex);
			//}


			try
			{
				StreamReader sr;
				sr = new StreamReader(fileName);

				for (int i = 1; i <= 40; i++)
				{
					string s2 = sr.ReadLine();
					if (checkBoxLoad.Checked)
					{
						string s3 = dataGridViewMain[2, i - 1].Value.ToString().Trim();
						if (s2.Trim() == s3.Trim())
						{
							dataGridViewMain[2, i - 1].Value = s2.Trim();
						}
						else
						{
							dataGridViewMain[2, i - 1].Value = s2 + " | " + s3;
						}
					}
					else
						dataGridViewMain[2, i - 1].Value = s2.Trim();

				}
				sr.Close();
			}
			catch (Exception ex)
			{
				LogMgr.ExceptionLog(ex);
			}


		}

		private void buttonMeterLoad_Click(object sender, EventArgs e)
		{
			string fileName = "";

			OpenFileDialog openFile = new OpenFileDialog();

			// 다이얼 로그가 Open되었을 때 최초의 경로 설정
			//openFile.InitialDirectory = @"C:";   

			// 다이얼 로그의 제목
			openFile.Title = "저장 위치 지정";

			// 기본 확장자
			openFile.DefaultExt = "meter";

			// 파일 목록 필터링
			openFile.Filter = "meter files(*.meter)|*.meter";

			openFile.FileName = textBoxCode.Text + "-meter-" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss");

			// OK버튼을 눌렀을때의 동작
			if (openFile.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			// 경로와 파일명을 fileName에 저장
			fileName = openFile.FileName.ToString();



			try
			{
				StreamReader sr;
				sr = new StreamReader(fileName);
				string s = sr.ReadLine();
				textBoxIdCount.Text = s;

				for (int i = 1; i <= 46; i++)
				{
					string s2 = sr.ReadLine();
					string[] s3 = s2.Split('|');
					string str = "";
					for (int k = 0; k < s3.Length - 1; k++)
					{
						if (checkBox3.Checked)
						{
							string s4 = dataGridViewMeter[k + 2, i - 1].Value.ToString().Trim();
							if (s4 == s3[k].Trim())
							{
								dataGridViewMeter[k + 2, i - 1].Value = s3[k].Trim();
							}
							else
							{
								dataGridViewMeter[k + 2, i - 1].Value = s4 + " | " + s3[k].Trim();
							}
						}
						else
							dataGridViewMeter[k + 2, i - 1].Value = s3[k].Trim();
					}

				}
				sr.Close();
			}
			catch (Exception ex)
			{
				LogMgr.ExceptionLog(ex);
			}
		}

		private void buttonIrLoad_Click(object sender, EventArgs e)
		{
			string fileName = "";

			OpenFileDialog openFile = new OpenFileDialog();

			// 다이얼 로그가 Open되었을 때 최초의 경로 설정
			//openFile.InitialDirectory = @"C:";   

			// 다이얼 로그의 제목
			openFile.Title = "저장 위치 지정";

			// 기본 확장자
			openFile.DefaultExt = "ir";

			// 파일 목록 필터링
			openFile.Filter = "ir files(*.ir)|*.ir";

			openFile.FileName = textBoxCode.Text + "-ir-" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss");

			// OK버튼을 눌렀을때의 동작
			if (openFile.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			// 경로와 파일명을 fileName에 저장
			fileName = openFile.FileName.ToString();



			try
			{
				StreamReader sr;
				sr = new StreamReader(fileName);
				string s = sr.ReadLine();
				textBoxIdCount.Text = s;

				for (int i = 1; i <= 10; i++)
				{
					string s2 = sr.ReadLine();
					string[] s3 = s2.Split('|');
					string str = "";
					for (int k = 0; k < s3.Length - 1; k++)
					{
						if (checkBox4.Checked)
						{
							string s4 = dataGridViewIR[k + 2, i - 1].Value.ToString().Trim();
							if (s4 == s3[k].Trim())
							{
								dataGridViewIR[k + 2, i - 1].Value = s3[k].Trim();
							}
							else
							{
								dataGridViewIR[k + 2, i - 1].Value = s4 + " | " + s3[k].Trim();
							}
						}
						else
							dataGridViewIR[k + 2, i - 1].Value = s3[k].Trim();
					}

				}
				sr.Close();
			}
			catch (Exception ex)
			{
				LogMgr.ExceptionLog(ex);
			}
		}

		private void checkBoxMqttLiveHide_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void buttonMainDefault_Click(object sender, EventArgs e)
		{
			button27_Click(sender, e);
			for (int i = 0; i < dataGridViewMain.Rows.Count; i++)
			{
				if (dataGridViewMeter.Rows[i].Cells[2].Value.ToString() == "")
				{
					//continue;
				}
				if (dataGridViewMain.Rows[i].Cells[2].Value.ToString() == dataGridViewMain.Rows[i].Cells[3].Value.ToString())
				{
					dataGridViewMain.Rows[i].Cells[2].Style.BackColor = Color.White;

				}
				else if (dataGridViewMain.Rows[i].Cells[2].Value.ToString() == "")
				{
					dataGridViewMain.Rows[i].Cells[2].Style.BackColor = Color.Orange;
				}
				else
				{
					dataGridViewMain.Rows[i].Cells[2].Style.BackColor = Color.Aqua;

				}
			}
		}

		private void dataGridViewMain_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			//헤링본값이 같으면 기본값에 색상추가
			//if (dataGridViewMain.Rows[e.RowIndex].Cells[dataGridViewMain.Columns.Count - 2].Value == null)
			//	return;
			//if (dataGridViewMain.Rows[e.RowIndex].Cells[dataGridViewMain.Columns.Count - 1].Value.ToString() == dataGridViewMain.Rows[e.RowIndex].Cells[dataGridViewMain.Columns.Count - 2].Value.ToString())
			//{
			//	e.CellStyle.BackColor = Color.Aqua;
			//	e.CellStyle.ForeColor = Color.Black;
			//}
			//이전값이 같으면 기본값에 색상추가
			//// 그 외의 경우 기본 디자인으로 보여준다
			//else
			//{
			//	e.CellStyle.BackColor = Color.Orange;
			//	e.CellStyle.ForeColor = Color.Black;
			//}
		}

		private void dataGridViewMeter_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{


			//if (e.ColumnIndex == 1)
			//{
			//	if ((e.Value != null))
			//	{
			//		string[] redSplit = textBoxRed.Text.Split(',');
			//		for (int i = 0; i < redSplit.Length; i++)
			//		{
			//			if (redSplit[i].ToString().Trim().Length == 0)
			//			{
			//				continue;
			//			}
			//			if (dataGridViewMeter.Cells[0].Rows[1].Contains(redSplit[i].ToString().Trim()))
			//			{
			//				e.CellStyle.BackColor = Color.Red;
			//				e.CellStyle.ForeColor = Color.White;
			//			}
			//		}
			//	}
			//}
		}

		private void button27_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < dataGridViewMain.Rows.Count; i++)
			{
				dataGridViewMain.Rows[i].Cells[2].Style.BackColor = Color.White;
			}
		}

		private void button16_Click_1(object sender, EventArgs e)
		{
			for (int i = 0; i < dataGridViewMeter.Rows.Count; i++)
			{
				for (int k = 2; k < Convert.ToInt32(textBoxIdCount.Text) + 2; k++)
				{
					dataGridViewMeter.Rows[i].Cells[k].Style.BackColor = Color.White;

				}
			}
		}

		private void button24_Click(object sender, EventArgs e)
		{
			{
				for (int i = 0; i < dataGridViewIR.Rows.Count; i++)
				{
					for (int k = 2; k < Convert.ToInt32(textBoxIdCount.Text) + 2; k++)
					{
						dataGridViewIR.Rows[i].Cells[k].Style.BackColor = Color.White;

					}
				}
			}
		}

		private void buttonMeterDefault_Click(object sender, EventArgs e)
		{
			button16_Click_1(sender, e);

			for (int i = 0; i < dataGridViewMeter.Rows.Count; i++)
			{
				for (int k = 2; k < Convert.ToInt32(textBoxIdCount.Text) + 2; k++)
				{
					if (dataGridViewMeter.Rows[i].Cells[k].Value.ToString() == "")
					{
						//dataGridViewMeter.Rows[i].Cells[k].Style.BackColor = Color.Orange;
						//continue;
					}
					if (dataGridViewMeter.Rows[i].Cells[k].Value.ToString() == dataGridViewMeter.Rows[i].Cells[dataGridViewMeter.Columns.Count - 1].Value.ToString())
					{
						dataGridViewMeter.Rows[i].Cells[k].Style.BackColor = Color.White;

					}
					else if (dataGridViewMeter.Rows[i].Cells[k].Value.ToString() == "")
					{
						dataGridViewMeter.Rows[i].Cells[k].Style.BackColor = Color.Orange;
					}
					else
					{
						dataGridViewMeter.Rows[i].Cells[k].Style.BackColor = Color.Aqua;

					}
				}
			}
		}


		private void button23_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < dataGridViewIR.Rows.Count; i++)
			{
				for (int k = 2; k < Convert.ToInt32(textBoxIdCount.Text) + 2; k++)
				{
					if (dataGridViewIR.Rows[i].Cells[k].Value.ToString() == "")
					{
						dataGridViewIR.Rows[i].Cells[k].Style.BackColor = Color.Orange;
						//continue;
					}
					if (dataGridViewIR.Rows[i].Cells[k].Value.ToString() == dataGridViewIR.Rows[i].Cells[dataGridViewIR.Columns.Count - 1].Value.ToString())
					{
						dataGridViewIR.Rows[i].Cells[k].Style.BackColor = Color.White;

					}
					else
					{
						dataGridViewIR.Rows[i].Cells[k].Style.BackColor = Color.Aqua;

					}
				}

			}
		}



		private void button21_Click(object sender, EventArgs e)
		{
			button16_Click_1(sender, e);
			for (int i = 0; i < dataGridViewMeter.Rows.Count; i++)
			{
				for (int k = 3; k < Convert.ToInt32(textBoxIdCount.Text) + 2; k++)
				{
					if (dataGridViewMeter.Rows[i].Cells[k].Value.ToString() == "")
					{
						//dataGridViewMeter.Rows[i].Cells[k].Style.BackColor = Color.Orange;
						//continue;
					}
					else if (dataGridViewMeter.Rows[i].Cells[k].Value.ToString() == dataGridViewMeter.Rows[i].Cells[k - 1].Value.ToString())
					{
					dataGridViewMeter.Rows[i].Cells[k].Style.BackColor = Color.White;
					}

				}
			}
					for (int i = 0; i < dataGridViewMeter.Rows.Count; i++)
			{
				for (int k = 2; k < Convert.ToInt32(textBoxIdCount.Text) + 2; k++)
				{
					if (dataGridViewMeter.Rows[i].Cells[k].Value.ToString() == "")
					{
						dataGridViewMeter.Rows[i].Cells[k].Style.BackColor = Color.Orange;
					}
					else
					{
						dataGridViewMeter.Rows[i].Cells[k].Style.BackColor = Color.Aqua;
					}
				}
			}





		}

		private void button22_Click(object sender, EventArgs e)
		{
			button24_Click(sender, e);
			//for (int i = 0; i < dataGridViewIR.Rows.Count; i++)
			//{
			//	for (int k = 3; k < Convert.ToInt32(textBoxIdCount.Text) + 2; k++)
			//	{
			//		if (dataGridViewIR.Rows[i].Cells[k].Value == null)
			//		{
			//			continue;
			//		}
			//		if (dataGridViewIR.Rows[i].Cells[k].Value.ToString() == dataGridViewIR.Rows[i].Cells[k - 1].Value.ToString())
			//		{
			//			dataGridViewIR.Rows[i].Cells[k].Style.BackColor = Color.White;
			//		}
			//		else
			//		{
			//			dataGridViewIR.Rows[i].Cells[k].Style.BackColor = Color.Aqua;
			//		}
			//	}
			//}

			for (int i = 0; i < dataGridViewIR.Rows.Count; i++)
			{
				for (int k = 3; k < Convert.ToInt32(textBoxIdCount.Text) + 2; k++)
				{
					if (dataGridViewIR.Rows[i].Cells[k].Value.ToString() == "")
					{
						//dataGridViewMeter.Rows[i].Cells[k].Style.BackColor = Color.Orange;
						//continue;
					}
					else if (dataGridViewIR.Rows[i].Cells[k].Value.ToString() == dataGridViewIR.Rows[i].Cells[k - 1].Value.ToString())
					{
						dataGridViewIR.Rows[i].Cells[k].Style.BackColor = Color.White;
					}

				}
			}
			for (int i = 0; i < dataGridViewIR.Rows.Count; i++)
			{
				for (int k = 2; k < Convert.ToInt32(textBoxIdCount.Text) + 2; k++)
				{
					if (dataGridViewIR.Rows[i].Cells[k].Value.ToString() == "")
					{
						dataGridViewIR.Rows[i].Cells[k].Style.BackColor = Color.Orange;
					}
					else
					{
						dataGridViewIR.Rows[i].Cells[k].Style.BackColor = Color.Aqua;
					}
				}
			}




		}

		private void dataGridViewMain_SelectionChanged(object sender, EventArgs e)
		{

			int index = dataGridViewMain.CurrentCell.RowIndex;
			if (index < 0)
			{
				return;
			}
			textBoxMainPos.Text = dataGridViewMain.Rows[index].Cells[0].Value.ToString();
			if (dataGridViewMain.Rows[index].Cells[2].Value == null)
			{
				textBoxMainValue.Text = "";
			}
			else
				textBoxMainValue.Text = dataGridViewMain.Rows[index].Cells[2].Value.ToString();
		}

		private void dataGridViewMeter_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				int index = dataGridViewMeter.CurrentCell.RowIndex;
				int ColumnIndex = dataGridViewMeter.CurrentCell.ColumnIndex;

				if (index < 0)
				{
					return;
				}
				if (dataGridViewMeter.Rows[index].Cells[0].Value == null)
				{
					return;
				}
				textBoxMeterPos.Text = dataGridViewMeter.Rows[index].Cells[0].Value.ToString();

				if ((dataGridViewMeter.Columns[ColumnIndex].HeaderText == "헤링본") || (dataGridViewMeter.Columns[ColumnIndex].HeaderText == "DESC") || (dataGridViewMeter.Columns[ColumnIndex].HeaderText == "POS"))
				{
					return;
				}
				else textBoxMeterId.Text = (dataGridViewMeter.Columns[ColumnIndex].HeaderText);

				if (dataGridViewMeter.Rows[index].Cells[ColumnIndex].Value == null)
				{
					textBoxMeterValue.Text = "";
				}
				else textBoxMeterValue.Text = dataGridViewMeter.Rows[index].Cells[ColumnIndex].Value.ToString();

				//if (index < 0)
				//{
				//	return;
				//}
				//textBoxMeterPos.Text = dataGridViewMeter.Rows[index].Cells[0].Value.ToString();
				//if (dataGridViewMeter.Rows[index].Cells[2].Value == null)
				//{
				//	textBoxMeterValue.Text = "";
				//}
				//else
				//	textBoxMeterValue.Text = dataGridViewMeter.Rows[index].Cells[2].Value.ToString();
			}
			catch (Exception ex)
			{


			}

		}

		private void dataGridViewIR_SelectionChanged(object sender, EventArgs e)
		{
			int index = dataGridViewIR.CurrentCell.RowIndex;
			int ColumnIndex = dataGridViewIR.CurrentCell.ColumnIndex;

			if (index < 0)
			{
				return;
			}
			if (dataGridViewIR.Rows[index].Cells[0].Value == null)
			{
				return;
			}
			textBoxIrPos.Text = dataGridViewIR.Rows[index].Cells[0].Value.ToString();

			if ((dataGridViewIR.Columns[ColumnIndex].HeaderText == "헤링본") || (dataGridViewIR.Columns[ColumnIndex].HeaderText == "DESC") || (dataGridViewIR.Columns[ColumnIndex].HeaderText == "POS"))
			{
				return;
			}
			else textBoxIrId.Text = (dataGridViewIR.Columns[ColumnIndex].HeaderText);

			if (dataGridViewIR.Rows[index].Cells[ColumnIndex].Value == null)
			{
				textBoxIrValue.Text = "";
			}
			else textBoxIrValue.Text = dataGridViewIR.Rows[index].Cells[ColumnIndex].Value.ToString();
		}

		private void checkBox5_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox5.Checked == true)
			{
				checkBox5.Text = "읽기모드";
				dataGridViewMain.ReadOnly = true;
				dataGridViewMeter.ReadOnly = true;
				dataGridViewIR.ReadOnly = true;
			}
			else
			{
				checkBox5.Text = "수정모드";
				dataGridViewMain.ReadOnly = false;
				dataGridViewMeter.ReadOnly = false;
				dataGridViewIR.ReadOnly = false;
				dataGridViewMain.EditMode = DataGridViewEditMode.EditOnEnter;
				dataGridViewMeter.EditMode = DataGridViewEditMode.EditOnEnter;
				dataGridViewIR.EditMode = DataGridViewEditMode.EditOnEnter;
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
