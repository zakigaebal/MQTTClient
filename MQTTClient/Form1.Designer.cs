namespace MQTTClient
{
	partial class Form1
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.dataGridViewMessage = new System.Windows.Forms.DataGridView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonClear = new System.Windows.Forms.Button();
			this.listBoxSub = new System.Windows.Forms.ListBox();
			this.textBoxHost = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button14 = new System.Windows.Forms.Button();
			this.label15 = new System.Windows.Forms.Label();
			this.buttonUnscribe = new System.Windows.Forms.Button();
			this.label16 = new System.Windows.Forms.Label();
			this.button12 = new System.Windows.Forms.Button();
			this.label17 = new System.Windows.Forms.Label();
			this.button11 = new System.Windows.Forms.Button();
			this.label18 = new System.Windows.Forms.Label();
			this.button10 = new System.Windows.Forms.Button();
			this.label19 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.label21 = new System.Windows.Forms.Label();
			this.checkBoxRetain = new System.Windows.Forms.CheckBox();
			this.label22 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBoxQos = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.buttonLocal = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonMqttServer = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.buttonPublish4 = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.buttonPublish3 = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.buttonPublish2 = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.buttonPublish = new System.Windows.Forms.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.buttonSubscribe = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.buttonDisconnect = new System.Windows.Forms.Button();
			this.label13 = new System.Windows.Forms.Label();
			this.buttonConnect = new System.Windows.Forms.Button();
			this.textBox13 = new System.Windows.Forms.TextBox();
			this.textBoxPort = new System.Windows.Forms.TextBox();
			this.textBox14 = new System.Windows.Forms.TextBox();
			this.textBoxM4 = new System.Windows.Forms.TextBox();
			this.textBox15 = new System.Windows.Forms.TextBox();
			this.textBoxM3 = new System.Windows.Forms.TextBox();
			this.textBox16 = new System.Windows.Forms.TextBox();
			this.textBoxM2 = new System.Windows.Forms.TextBox();
			this.textBox17 = new System.Windows.Forms.TextBox();
			this.textBoxMessage = new System.Windows.Forms.TextBox();
			this.textBox18 = new System.Windows.Forms.TextBox();
			this.textBoxPT4 = new System.Windows.Forms.TextBox();
			this.textBox19 = new System.Windows.Forms.TextBox();
			this.textBoxPT3 = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.textBoxPT2 = new System.Windows.Forms.TextBox();
			this.textBoxSubTopic = new System.Windows.Forms.TextBox();
			this.textBoxPubTopic = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.checkBoxTopicpub = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewMessage)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1281, 628);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.dataGridViewMessage);
			this.tabPage1.Controls.Add(this.panel1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(1273, 602);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Mqtt";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// dataGridViewMessage
			// 
			this.dataGridViewMessage.AllowUserToAddRows = false;
			this.dataGridViewMessage.AllowUserToDeleteRows = false;
			this.dataGridViewMessage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridViewMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridViewMessage.Location = new System.Drawing.Point(3, 409);
			this.dataGridViewMessage.Name = "dataGridViewMessage";
			this.dataGridViewMessage.ReadOnly = true;
			this.dataGridViewMessage.RowTemplate.Height = 23;
			this.dataGridViewMessage.Size = new System.Drawing.Size(1267, 190);
			this.dataGridViewMessage.TabIndex = 9;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.checkBoxTopicpub);
			this.panel1.Controls.Add(this.buttonClear);
			this.panel1.Controls.Add(this.listBoxSub);
			this.panel1.Controls.Add(this.textBoxHost);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.button14);
			this.panel1.Controls.Add(this.label15);
			this.panel1.Controls.Add(this.buttonUnscribe);
			this.panel1.Controls.Add(this.label16);
			this.panel1.Controls.Add(this.button12);
			this.panel1.Controls.Add(this.label17);
			this.panel1.Controls.Add(this.button11);
			this.panel1.Controls.Add(this.label18);
			this.panel1.Controls.Add(this.button10);
			this.panel1.Controls.Add(this.label19);
			this.panel1.Controls.Add(this.label20);
			this.panel1.Controls.Add(this.checkBox2);
			this.panel1.Controls.Add(this.label21);
			this.panel1.Controls.Add(this.checkBoxRetain);
			this.panel1.Controls.Add(this.label22);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.comboBoxQos);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.buttonLocal);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.buttonMqttServer);
			this.panel1.Controls.Add(this.label7);
			this.panel1.Controls.Add(this.buttonPublish4);
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.buttonPublish3);
			this.panel1.Controls.Add(this.label9);
			this.panel1.Controls.Add(this.buttonPublish2);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.buttonPublish);
			this.panel1.Controls.Add(this.label11);
			this.panel1.Controls.Add(this.buttonSubscribe);
			this.panel1.Controls.Add(this.label10);
			this.panel1.Controls.Add(this.buttonDisconnect);
			this.panel1.Controls.Add(this.label13);
			this.panel1.Controls.Add(this.buttonConnect);
			this.panel1.Controls.Add(this.textBox13);
			this.panel1.Controls.Add(this.textBoxPort);
			this.panel1.Controls.Add(this.textBox14);
			this.panel1.Controls.Add(this.textBoxM4);
			this.panel1.Controls.Add(this.textBox15);
			this.panel1.Controls.Add(this.textBoxM3);
			this.panel1.Controls.Add(this.textBox16);
			this.panel1.Controls.Add(this.textBoxM2);
			this.panel1.Controls.Add(this.textBox17);
			this.panel1.Controls.Add(this.textBoxMessage);
			this.panel1.Controls.Add(this.textBox18);
			this.panel1.Controls.Add(this.textBoxPT4);
			this.panel1.Controls.Add(this.textBox19);
			this.panel1.Controls.Add(this.textBoxPT3);
			this.panel1.Controls.Add(this.label12);
			this.panel1.Controls.Add(this.textBoxPT2);
			this.panel1.Controls.Add(this.textBoxSubTopic);
			this.panel1.Controls.Add(this.textBoxPubTopic);
			this.panel1.Controls.Add(this.label14);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1267, 406);
			this.panel1.TabIndex = 10;
			// 
			// buttonClear
			// 
			this.buttonClear.Location = new System.Drawing.Point(1173, 359);
			this.buttonClear.Name = "buttonClear";
			this.buttonClear.Size = new System.Drawing.Size(89, 41);
			this.buttonClear.TabIndex = 10;
			this.buttonClear.Text = "Clear";
			this.buttonClear.UseVisualStyleBackColor = true;
			this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
			// 
			// listBoxSub
			// 
			this.listBoxSub.FormattingEnabled = true;
			this.listBoxSub.ItemHeight = 12;
			this.listBoxSub.Location = new System.Drawing.Point(559, 46);
			this.listBoxSub.Name = "listBoxSub";
			this.listBoxSub.Size = new System.Drawing.Size(263, 88);
			this.listBoxSub.TabIndex = 9;
			// 
			// textBoxHost
			// 
			this.textBoxHost.Location = new System.Drawing.Point(99, 17);
			this.textBoxHost.Name = "textBoxHost";
			this.textBoxHost.Size = new System.Drawing.Size(267, 21);
			this.textBoxHost.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(62, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "Host:";
			// 
			// button14
			// 
			this.button14.Location = new System.Drawing.Point(564, 224);
			this.button14.Name = "button14";
			this.button14.Size = new System.Drawing.Size(258, 23);
			this.button14.TabIndex = 8;
			this.button14.Text = "Log Folder";
			this.button14.UseVisualStyleBackColor = true;
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(562, 26);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(117, 12);
			this.label15.TabIndex = 0;
			this.label15.Text = "Subscriptions topic:";
			// 
			// buttonUnscribe
			// 
			this.buttonUnscribe.Enabled = false;
			this.buttonUnscribe.Location = new System.Drawing.Point(564, 198);
			this.buttonUnscribe.Name = "buttonUnscribe";
			this.buttonUnscribe.Size = new System.Drawing.Size(258, 23);
			this.buttonUnscribe.TabIndex = 8;
			this.buttonUnscribe.Text = "Unsubscribe";
			this.buttonUnscribe.UseVisualStyleBackColor = true;
			this.buttonUnscribe.Click += new System.EventHandler(this.buttonUnscribe_Click);
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(926, 144);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(27, 12);
			this.label16.TabIndex = 0;
			this.label16.Text = "Red";
			// 
			// button12
			// 
			this.button12.Location = new System.Drawing.Point(747, 161);
			this.button12.Name = "button12";
			this.button12.Size = new System.Drawing.Size(75, 23);
			this.button12.TabIndex = 8;
			this.button12.Text = "Save";
			this.button12.UseVisualStyleBackColor = true;
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(926, 174);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(39, 12);
			this.label17.TabIndex = 0;
			this.label17.Text = "Green";
			// 
			// button11
			// 
			this.button11.Location = new System.Drawing.Point(654, 160);
			this.button11.Name = "button11";
			this.button11.Size = new System.Drawing.Size(75, 23);
			this.button11.TabIndex = 8;
			this.button11.Text = "Subscribe";
			this.button11.UseVisualStyleBackColor = true;
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(927, 199);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(43, 12);
			this.label18.TabIndex = 0;
			this.label18.Text = "Yellow";
			// 
			// button10
			// 
			this.button10.Location = new System.Drawing.Point(564, 161);
			this.button10.Name = "button10";
			this.button10.Size = new System.Drawing.Size(75, 23);
			this.button10.TabIndex = 8;
			this.button10.Text = "Open";
			this.button10.UseVisualStyleBackColor = true;
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(926, 223);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(32, 12);
			this.label19.TabIndex = 0;
			this.label19.Text = "Gray";
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(926, 246);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(34, 12);
			this.label20.TabIndex = 0;
			this.label20.Text = "Navy";
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new System.Drawing.Point(691, 13);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(15, 14);
			this.checkBox2.TabIndex = 5;
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Location = new System.Drawing.Point(927, 273);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(41, 12);
			this.label21.TabIndex = 0;
			this.label21.Text = "Purple";
			// 
			// checkBoxRetain
			// 
			this.checkBoxRetain.AutoSize = true;
			this.checkBoxRetain.Location = new System.Drawing.Point(351, 69);
			this.checkBoxRetain.Name = "checkBoxRetain";
			this.checkBoxRetain.Size = new System.Drawing.Size(15, 14);
			this.checkBoxRetain.TabIndex = 5;
			this.checkBoxRetain.UseVisualStyleBackColor = true;
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Location = new System.Drawing.Point(927, 301);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(33, 12);
			this.label22.TabIndex = 0;
			this.label22.Text = "Lime";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(301, 69);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(44, 12);
			this.label5.TabIndex = 4;
			this.label5.Text = "Retain:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(62, 69);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "Port:";
			// 
			// comboBoxQos
			// 
			this.comboBoxQos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxQos.FormattingEnabled = true;
			this.comboBoxQos.Items.AddRange(new object[] {
            "0 At most once",
            "1 At least once",
            "2 Exactly once"});
			this.comboBoxQos.Location = new System.Drawing.Point(220, 66);
			this.comboBoxQos.Name = "comboBoxQos";
			this.comboBoxQos.Size = new System.Drawing.Size(75, 20);
			this.comboBoxQos.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(174, 69);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(33, 12);
			this.label4.TabIndex = 0;
			this.label4.Text = "QoS:";
			// 
			// buttonLocal
			// 
			this.buttonLocal.Location = new System.Drawing.Point(510, 15);
			this.buttonLocal.Name = "buttonLocal";
			this.buttonLocal.Size = new System.Drawing.Size(28, 23);
			this.buttonLocal.TabIndex = 2;
			this.buttonLocal.Text = "L";
			this.buttonLocal.UseVisualStyleBackColor = true;
			this.buttonLocal.Click += new System.EventHandler(this.buttonLocal_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(9, 105);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(84, 12);
			this.label3.TabIndex = 0;
			this.label3.Text = "Topic for sub:";
			// 
			// buttonMqttServer
			// 
			this.buttonMqttServer.Location = new System.Drawing.Point(475, 15);
			this.buttonMqttServer.Name = "buttonMqttServer";
			this.buttonMqttServer.Size = new System.Drawing.Size(29, 23);
			this.buttonMqttServer.TabIndex = 2;
			this.buttonMqttServer.Text = "K";
			this.buttonMqttServer.UseVisualStyleBackColor = true;
			this.buttonMqttServer.Click += new System.EventHandler(this.buttonMqttServer_Click);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(9, 172);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(84, 12);
			this.label7.TabIndex = 0;
			this.label7.Text = "Topic for pub:";
			// 
			// buttonPublish4
			// 
			this.buttonPublish4.Enabled = false;
			this.buttonPublish4.Location = new System.Drawing.Point(372, 364);
			this.buttonPublish4.Name = "buttonPublish4";
			this.buttonPublish4.Size = new System.Drawing.Size(166, 23);
			this.buttonPublish4.TabIndex = 2;
			this.buttonPublish4.Text = "Publish";
			this.buttonPublish4.UseVisualStyleBackColor = true;
			this.buttonPublish4.Click += new System.EventHandler(this.buttonPublish4_Click);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(31, 198);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(62, 12);
			this.label8.TabIndex = 0;
			this.label8.Text = "Message:";
			// 
			// buttonPublish3
			// 
			this.buttonPublish3.Enabled = false;
			this.buttonPublish3.Location = new System.Drawing.Point(372, 308);
			this.buttonPublish3.Name = "buttonPublish3";
			this.buttonPublish3.Size = new System.Drawing.Size(166, 23);
			this.buttonPublish3.TabIndex = 2;
			this.buttonPublish3.Text = "Publish";
			this.buttonPublish3.UseVisualStyleBackColor = true;
			this.buttonPublish3.Click += new System.EventHandler(this.buttonPublish3_Click);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(9, 228);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(84, 12);
			this.label9.TabIndex = 0;
			this.label9.Text = "Topic for pub:";
			// 
			// buttonPublish2
			// 
			this.buttonPublish2.Enabled = false;
			this.buttonPublish2.Location = new System.Drawing.Point(372, 252);
			this.buttonPublish2.Name = "buttonPublish2";
			this.buttonPublish2.Size = new System.Drawing.Size(166, 23);
			this.buttonPublish2.TabIndex = 2;
			this.buttonPublish2.Text = "Publish";
			this.buttonPublish2.UseVisualStyleBackColor = true;
			this.buttonPublish2.Click += new System.EventHandler(this.buttonPublish2_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(712, 15);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(137, 12);
			this.label6.TabIndex = 0;
			this.label6.Text = "토픽단위 로그 파일 생성";
			// 
			// buttonPublish
			// 
			this.buttonPublish.Enabled = false;
			this.buttonPublish.Location = new System.Drawing.Point(372, 196);
			this.buttonPublish.Name = "buttonPublish";
			this.buttonPublish.Size = new System.Drawing.Size(166, 23);
			this.buttonPublish.TabIndex = 2;
			this.buttonPublish.Text = "Publish";
			this.buttonPublish.UseVisualStyleBackColor = true;
			this.buttonPublish.Click += new System.EventHandler(this.buttonPublish_Click);
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(9, 284);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(84, 12);
			this.label11.TabIndex = 0;
			this.label11.Text = "Topic for pub:";
			// 
			// buttonSubscribe
			// 
			this.buttonSubscribe.Enabled = false;
			this.buttonSubscribe.Location = new System.Drawing.Point(372, 102);
			this.buttonSubscribe.Name = "buttonSubscribe";
			this.buttonSubscribe.Size = new System.Drawing.Size(166, 23);
			this.buttonSubscribe.TabIndex = 2;
			this.buttonSubscribe.Text = "Subscribe";
			this.buttonSubscribe.UseVisualStyleBackColor = true;
			this.buttonSubscribe.Click += new System.EventHandler(this.buttonSubscribe_Click);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(31, 254);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(62, 12);
			this.label10.TabIndex = 0;
			this.label10.Text = "Message:";
			// 
			// buttonDisconnect
			// 
			this.buttonDisconnect.Enabled = false;
			this.buttonDisconnect.Location = new System.Drawing.Point(372, 66);
			this.buttonDisconnect.Name = "buttonDisconnect";
			this.buttonDisconnect.Size = new System.Drawing.Size(166, 23);
			this.buttonDisconnect.TabIndex = 2;
			this.buttonDisconnect.Text = "Disconnect";
			this.buttonDisconnect.UseVisualStyleBackColor = true;
			this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(9, 340);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(84, 12);
			this.label13.TabIndex = 0;
			this.label13.Text = "Topic for pub:";
			// 
			// buttonConnect
			// 
			this.buttonConnect.Location = new System.Drawing.Point(381, 15);
			this.buttonConnect.Name = "buttonConnect";
			this.buttonConnect.Size = new System.Drawing.Size(75, 23);
			this.buttonConnect.TabIndex = 2;
			this.buttonConnect.Text = "Connect";
			this.buttonConnect.UseVisualStyleBackColor = true;
			this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
			// 
			// textBox13
			// 
			this.textBox13.Location = new System.Drawing.Point(978, 139);
			this.textBox13.Name = "textBox13";
			this.textBox13.Size = new System.Drawing.Size(179, 21);
			this.textBox13.TabIndex = 1;
			// 
			// textBoxPort
			// 
			this.textBoxPort.Location = new System.Drawing.Point(99, 68);
			this.textBoxPort.Name = "textBoxPort";
			this.textBoxPort.Size = new System.Drawing.Size(56, 21);
			this.textBoxPort.TabIndex = 1;
			// 
			// textBox14
			// 
			this.textBox14.Location = new System.Drawing.Point(978, 165);
			this.textBox14.Name = "textBox14";
			this.textBox14.Size = new System.Drawing.Size(179, 21);
			this.textBox14.TabIndex = 1;
			// 
			// textBoxM4
			// 
			this.textBoxM4.Location = new System.Drawing.Point(99, 366);
			this.textBoxM4.Name = "textBoxM4";
			this.textBoxM4.Size = new System.Drawing.Size(260, 21);
			this.textBoxM4.TabIndex = 1;
			// 
			// textBox15
			// 
			this.textBox15.Location = new System.Drawing.Point(978, 189);
			this.textBox15.Name = "textBox15";
			this.textBox15.Size = new System.Drawing.Size(179, 21);
			this.textBox15.TabIndex = 1;
			// 
			// textBoxM3
			// 
			this.textBoxM3.Location = new System.Drawing.Point(99, 310);
			this.textBoxM3.Name = "textBoxM3";
			this.textBoxM3.Size = new System.Drawing.Size(260, 21);
			this.textBoxM3.TabIndex = 1;
			// 
			// textBox16
			// 
			this.textBox16.Location = new System.Drawing.Point(978, 216);
			this.textBox16.Name = "textBox16";
			this.textBox16.Size = new System.Drawing.Size(179, 21);
			this.textBox16.TabIndex = 1;
			// 
			// textBoxM2
			// 
			this.textBoxM2.Location = new System.Drawing.Point(99, 254);
			this.textBoxM2.Name = "textBoxM2";
			this.textBoxM2.Size = new System.Drawing.Size(260, 21);
			this.textBoxM2.TabIndex = 1;
			// 
			// textBox17
			// 
			this.textBox17.Location = new System.Drawing.Point(978, 243);
			this.textBox17.Name = "textBox17";
			this.textBox17.Size = new System.Drawing.Size(179, 21);
			this.textBox17.TabIndex = 1;
			// 
			// textBoxMessage
			// 
			this.textBoxMessage.Location = new System.Drawing.Point(99, 198);
			this.textBoxMessage.Name = "textBoxMessage";
			this.textBoxMessage.Size = new System.Drawing.Size(260, 21);
			this.textBoxMessage.TabIndex = 1;
			// 
			// textBox18
			// 
			this.textBox18.Location = new System.Drawing.Point(978, 270);
			this.textBox18.Name = "textBox18";
			this.textBox18.Size = new System.Drawing.Size(179, 21);
			this.textBox18.TabIndex = 1;
			// 
			// textBoxPT4
			// 
			this.textBoxPT4.Location = new System.Drawing.Point(99, 337);
			this.textBoxPT4.Name = "textBoxPT4";
			this.textBoxPT4.Size = new System.Drawing.Size(439, 21);
			this.textBoxPT4.TabIndex = 1;
			// 
			// textBox19
			// 
			this.textBox19.Location = new System.Drawing.Point(978, 298);
			this.textBox19.Name = "textBox19";
			this.textBox19.Size = new System.Drawing.Size(179, 21);
			this.textBox19.TabIndex = 1;
			// 
			// textBoxPT3
			// 
			this.textBoxPT3.Location = new System.Drawing.Point(99, 281);
			this.textBoxPT3.Name = "textBoxPT3";
			this.textBoxPT3.Size = new System.Drawing.Size(439, 21);
			this.textBoxPT3.TabIndex = 1;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(31, 310);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(62, 12);
			this.label12.TabIndex = 0;
			this.label12.Text = "Message:";
			// 
			// textBoxPT2
			// 
			this.textBoxPT2.Location = new System.Drawing.Point(99, 225);
			this.textBoxPT2.Name = "textBoxPT2";
			this.textBoxPT2.Size = new System.Drawing.Size(439, 21);
			this.textBoxPT2.TabIndex = 1;
			// 
			// textBoxSubTopic
			// 
			this.textBoxSubTopic.Location = new System.Drawing.Point(99, 102);
			this.textBoxSubTopic.Name = "textBoxSubTopic";
			this.textBoxSubTopic.Size = new System.Drawing.Size(260, 21);
			this.textBoxSubTopic.TabIndex = 1;
			// 
			// textBoxPubTopic
			// 
			this.textBoxPubTopic.Location = new System.Drawing.Point(121, 169);
			this.textBoxPubTopic.Name = "textBoxPubTopic";
			this.textBoxPubTopic.Size = new System.Drawing.Size(417, 21);
			this.textBoxPubTopic.TabIndex = 1;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(31, 366);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(62, 12);
			this.label14.TabIndex = 0;
			this.label14.Text = "Message:";
			// 
			// checkBoxTopicpub
			// 
			this.checkBoxTopicpub.AutoSize = true;
			this.checkBoxTopicpub.Location = new System.Drawing.Point(100, 172);
			this.checkBoxTopicpub.Name = "checkBoxTopicpub";
			this.checkBoxTopicpub.Size = new System.Drawing.Size(15, 14);
			this.checkBoxTopicpub.TabIndex = 11;
			this.checkBoxTopicpub.UseVisualStyleBackColor = true;
			this.checkBoxTopicpub.CheckedChanged += new System.EventHandler(this.checkBoxTopicpub_CheckedChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1281, 628);
			this.Controls.Add(this.tabControl1);
			this.Name = "Form1";
			this.Text = "MQTT Client";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewMessage)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Button buttonLocal;
		private System.Windows.Forms.Button buttonMqttServer;
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.TextBox textBoxHost;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DataGridView dataGridViewMessage;
		private System.Windows.Forms.Button button14;
		private System.Windows.Forms.Button buttonUnscribe;
		private System.Windows.Forms.Button button12;
		private System.Windows.Forms.Button button11;
		private System.Windows.Forms.Button button10;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBoxRetain;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox comboBoxQos;
		private System.Windows.Forms.Button buttonPublish4;
		private System.Windows.Forms.Button buttonPublish3;
		private System.Windows.Forms.Button buttonPublish2;
		private System.Windows.Forms.Button buttonPublish;
		private System.Windows.Forms.Button buttonSubscribe;
		private System.Windows.Forms.Button buttonDisconnect;
		private System.Windows.Forms.TextBox textBoxPort;
		private System.Windows.Forms.TextBox textBoxM4;
		private System.Windows.Forms.TextBox textBoxM3;
		private System.Windows.Forms.TextBox textBoxM2;
		private System.Windows.Forms.TextBox textBoxMessage;
		private System.Windows.Forms.TextBox textBoxPT4;
		private System.Windows.Forms.TextBox textBoxPT3;
		private System.Windows.Forms.TextBox textBoxPT2;
		private System.Windows.Forms.TextBox textBoxPubTopic;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox textBoxSubTopic;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox textBox19;
		private System.Windows.Forms.TextBox textBox18;
		private System.Windows.Forms.TextBox textBox17;
		private System.Windows.Forms.TextBox textBox16;
		private System.Windows.Forms.TextBox textBox15;
		private System.Windows.Forms.TextBox textBox14;
		private System.Windows.Forms.TextBox textBox13;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonClear;
		private System.Windows.Forms.ListBox listBoxSub;
		private System.Windows.Forms.CheckBox checkBoxTopicpub;
	}
}

