using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Reflection;
using System.Drawing;

namespace MQTTClient
{
	public partial class Form1 : Form
	{
		static MqttClient clientUser;
		DataTable dt = new DataTable();
		private delegate void ShowCallBack(string myStr1, string myStr2, DataGridView dgv);

		public Form1()
		{
			InitializeComponent();
			dataGridViewMessage.DoubleBuffered(true);
			dataGridViewMessage.SuspendLayout();
			//폼 닫기 이벤트 선언

			this.FormClosed += Form_Closing;
		}

		private void Form1_Load(object sender, EventArgs e)
		{

			mqttRecord();
			textBoxPort.Text = "1883";
			comboBoxQos.SelectedIndex = 1;
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
			dataGridViewMessage.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridViewMessage.Columns[dataGridViewMessage.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewMessage.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			dataGridViewMessage.Columns[1].Width = 200;



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
				//dgv.Rows.Add(myStr + Environment.NewLine);
				dt.Rows.Add(DateTime.Now.ToString("HH:mm:ss:fff"), myStr1 + Environment.NewLine, myStr2 + Environment.NewLine);
				dataGridViewMessage.DataSource = dt;
				dataGridViewMessage.CurrentCell = dataGridViewMessage.Rows[0].Cells[0];
			
			
			}
		}



			private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs data)
		{
			byte[] a = System.Text.Encoding.UTF8.GetBytes(data.Topic);
			ShowMessage(data.Topic, System.Text.Encoding.UTF8.GetString(data.Message), dataGridViewMessage);
	
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
				if(textBoxHost.Text.Length == 0)
			{
				return;
			}
			else if(!Int32.TryParse(textBoxPort.Text, out port))
			{
				return ;
			}
			else
			{
				try
				{
					clientUser = new MqttClient(textBoxHost.Text);
					clientUser.Connect(Guid.NewGuid().ToString());
					clientUser.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(client_MqttMsgPublishReceived);
				}
				catch
				{
					
				}
				if(clientUser != null && clientUser.IsConnected)
				{
					buttonSubscribe.Enabled = true;
					buttonPublish.Enabled = true;
					buttonPublish2.Enabled = true;
					buttonPublish3.Enabled = true;
					buttonPublish4.Enabled = true;
					
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
				
					listBoxSub.Items.Add(textBoxSubTopic.Text);
					
				}
			catch
			{
				return;
			}
			}
		}

		private void buttonPublish_Click(object sender, EventArgs e)
		{
			try
			{
				///게시///
				clientUser.Publish(textBoxPubTopic.Text, Encoding.UTF8.GetBytes(textBoxMessage.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
			}
			catch
			{
				return;
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
			if (listBoxSub.SelectedItem == null)
			{
				return;
			}
			else
			{
	
				clientUser.Unsubscribe(new string[] { listBoxSub.SelectedItem.ToString() });
				listBoxSub.Items.Remove(listBoxSub.SelectedItem);
			}
		}

		private void buttonClear_Click(object sender, EventArgs e)
		{
			listBoxSub.Items.Clear();
			((DataTable)dataGridViewMessage.DataSource).Rows.Clear();
		}

		private void checkBoxTopicpub_CheckedChanged(object sender, EventArgs e)
		{
			if(checkBoxTopicpub.Checked == true) 
			{ 
			textBoxPT2.Enabled = false;
			textBoxPT3.Enabled = false;
			textBoxPT4.Enabled = false;
			}
			else if(checkBoxTopicpub.Checked == false)
			{
				textBoxPT2.Enabled = true;
				textBoxPT3.Enabled = true;
				textBoxPT4.Enabled = true;
			}
		}

		private void buttonPublish2_Click(object sender, EventArgs e)
		{
			if(checkBoxTopicpub.Checked == true)
			{
				clientUser.Publish(textBoxPubTopic.Text, Encoding.UTF8.GetBytes(textBoxM2.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
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
				clientUser.Publish(textBoxPubTopic.Text, Encoding.UTF8.GetBytes(textBoxM3.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
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
					clientUser.Publish(textBoxPubTopic.Text, Encoding.UTF8.GetBytes(textBoxM4.Text), (byte)comboBoxQos.SelectedIndex, checkBoxRetain.Checked);
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
							if(textBoxRed.Text == "")
							{
								return;
							}
							else
							  if(text.Contains(",") == false)
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
						}
						if (text.Contains(textBoxPurple.Text))
						{
							if (textBoxPurple.Text == "")
							{
								return;
							}
							else
								e.CellStyle.BackColor = Color.Purple;
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

		

		}


		private void setttingSave()
		{
			Properties.Settings.Default.host = textBoxHost.Text;
			Properties.Settings.Default.port = textBoxPort.Text;
			Properties.Settings.Default.qos = comboBoxQos.Text;
			Properties.Settings.Default.topicSub = textBoxSubTopic.Text;
			Properties.Settings.Default.topicPub1 = textBoxPubTopic.Text;
			Properties.Settings.Default.topicPm1 = textBoxMessage.Text;

			Properties.Settings.Default.Save();
		}


		private void buttonSave_Click(object sender, EventArgs e)
		{
			//This line of code creates a text file for the data export.
			System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\dawoon101\lesson\MQTTClient\MQTTClient\bin\Debug\Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
			try
			{
				string sLine = "";

				//This for loop loops through each row in the table
				for (int r = 0; r <= dataGridViewMessage.Rows.Count - 1; r++)
				{
					//This for loop loops through each column, and the row number
					//is passed from the for loop above.
					for (int c = 0; c <= dataGridViewMessage.Columns.Count - 1; c++)
					{
						sLine = sLine + dataGridViewMessage.Rows[r].Cells[c].Value;
						if (c != dataGridViewMessage.Columns.Count - 1)
						{
							//A comma is added as a text delimiter in order
							//to separate each field in the text file.
							//You can choose another character as a delimiter.
							sLine = sLine + ",";
						}
					}
					//The exported text is written to the text file, one line at a time.
					file.WriteLine(sLine);
					sLine = "";
				}

				file.Close();
				System.Windows.Forms.MessageBox.Show("Export Complete.", "Program Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (System.Exception err)
			{
				System.Windows.Forms.MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				file.Close();
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

		private void button3_Click(object sender, EventArgs e)
		{
			setttingSave();
		}

		private void label7_Click(object sender, EventArgs e)
		{

		}

		private void label8_Click(object sender, EventArgs e)
		{

		}

		private void textBoxMessage_TextChanged(object sender, EventArgs e)
		{

		}

		private void textBoxPubTopic_TextChanged(object sender, EventArgs e)
		{

		}

		private void label16_Click(object sender, EventArgs e)
		{

		}

		private void textBoxRed_TextChanged(object sender, EventArgs e)
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

