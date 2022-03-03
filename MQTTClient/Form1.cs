using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


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
				dt.Rows.Add(DateTime.Now.ToString("HH:mm:ss"), myStr1 + Environment.NewLine, myStr2 + Environment.NewLine);
				dataGridViewMessage.DataSource = dt;
			}
		}

		private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs data)
		{
			byte[] a = System.Text.Encoding.UTF8.GetBytes(data.Topic);
			ShowMessage(System.Text.Encoding.UTF8.GetString(a), System.Text.Encoding.UTF8.GetString(data.Message), dataGridViewMessage);
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
	}
}
