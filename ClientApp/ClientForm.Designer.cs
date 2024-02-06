namespace DankordClientApp
{
	partial class ClientForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			label3 = new Label();
			label4 = new Label();
			ClientPort = new TextBox();
			ClientIP = new TextBox();
			ConnectClient = new Button();
			MessText = new TextBox();
			Send = new Button();
			BackgroundReceiver = new System.ComponentModel.BackgroundWorker();
			BackgroundSender = new System.ComponentModel.BackgroundWorker();
			nameBox = new TextBox();
			label7 = new Label();
			Change = new Button();
			Messages = new RichTextBox();
			Local = new Button();
			vitekButton = new Button();
			SuspendLayout();
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(359, 44);
			label3.Margin = new Padding(2, 0, 2, 0);
			label3.Name = "label3";
			label3.Size = new Size(29, 15);
			label3.TabIndex = 8;
			label3.Text = "Port";
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(11, 45);
			label4.Margin = new Padding(2, 0, 2, 0);
			label4.Name = "label4";
			label4.Size = new Size(62, 15);
			label4.TabIndex = 7;
			label4.Text = "IP Address";
			// 
			// ClientPort
			// 
			ClientPort.Location = new Point(359, 61);
			ClientPort.Margin = new Padding(2);
			ClientPort.Name = "ClientPort";
			ClientPort.Size = new Size(69, 23);
			ClientPort.TabIndex = 6;
			// 
			// ClientIP
			// 
			ClientIP.Location = new Point(194, 61);
			ClientIP.Margin = new Padding(2);
			ClientIP.Name = "ClientIP";
			ClientIP.Size = new Size(161, 23);
			ClientIP.TabIndex = 5;
			// 
			// ConnectClient
			// 
			ConnectClient.Location = new Point(432, 61);
			ConnectClient.Margin = new Padding(2);
			ConnectClient.Name = "ConnectClient";
			ConnectClient.Size = new Size(75, 23);
			ConnectClient.TabIndex = 11;
			ConnectClient.Text = "Connect";
			ConnectClient.UseVisualStyleBackColor = true;
			ConnectClient.Click += ConnectClient_Click;
			// 
			// MessText
			// 
			MessText.AllowDrop = true;
			MessText.Location = new Point(11, 387);
			MessText.Margin = new Padding(2);
			MessText.Name = "MessText";
			MessText.Size = new Size(457, 23);
			MessText.TabIndex = 14;
			// 
			// Send
			// 
			Send.Location = new Point(472, 387);
			Send.Margin = new Padding(2);
			Send.Name = "Send";
			Send.Size = new Size(75, 23);
			Send.TabIndex = 15;
			Send.Text = "Send";
			Send.UseVisualStyleBackColor = true;
			Send.Click += SendButton_Click;
			// 
			// BackgroundReceiver
			// 
			BackgroundReceiver.DoWork += Receiver_DoWork;
			// 
			// nameBox
			// 
			nameBox.Location = new Point(57, 12);
			nameBox.Margin = new Padding(2);
			nameBox.Name = "nameBox";
			nameBox.Size = new Size(108, 23);
			nameBox.TabIndex = 17;
			nameBox.Text = "danek";
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Location = new Point(11, 15);
			label7.Margin = new Padding(2, 0, 2, 0);
			label7.Name = "label7";
			label7.Size = new Size(42, 15);
			label7.TabIndex = 18;
			label7.Text = "Name:";
			// 
			// Change
			// 
			Change.Location = new Point(169, 12);
			Change.Margin = new Padding(2);
			Change.Name = "Change";
			Change.Size = new Size(75, 23);
			Change.TabIndex = 19;
			Change.Text = "Change";
			Change.UseVisualStyleBackColor = true;
			Change.Click += ChangeButton_Click;
			// 
			// Messages
			// 
			Messages.Location = new Point(11, 89);
			Messages.Margin = new Padding(2);
			Messages.Name = "Messages";
			Messages.ReadOnly = true;
			Messages.Size = new Size(536, 294);
			Messages.TabIndex = 20;
			Messages.Text = "";
			// 
			// Local
			// 
			Local.Location = new Point(11, 61);
			Local.Margin = new Padding(2);
			Local.Name = "Local";
			Local.Size = new Size(75, 23);
			Local.TabIndex = 21;
			Local.Text = "Local\r\n";
			Local.UseVisualStyleBackColor = true;
			Local.Click += Local_Click;
			// 
			// vitekButton
			// 
			vitekButton.Location = new Point(90, 61);
			vitekButton.Margin = new Padding(2);
			vitekButton.Name = "vitekButton";
			vitekButton.Size = new Size(75, 23);
			vitekButton.TabIndex = 22;
			vitekButton.Text = "Vitek";
			vitekButton.UseVisualStyleBackColor = true;
			vitekButton.Click += vitekButton_Click;
			// 
			// ClientForm
			// 
			AcceptButton = Send;
			AutoScaleDimensions = new SizeF(96F, 96F);
			AutoScaleMode = AutoScaleMode.Dpi;
			ClientSize = new Size(558, 421);
			Controls.Add(vitekButton);
			Controls.Add(Local);
			Controls.Add(Messages);
			Controls.Add(Change);
			Controls.Add(label7);
			Controls.Add(nameBox);
			Controls.Add(Send);
			Controls.Add(MessText);
			Controls.Add(ConnectClient);
			Controls.Add(label3);
			Controls.Add(label4);
			Controls.Add(ClientPort);
			Controls.Add(ClientIP);
			Margin = new Padding(2);
			Name = "DankordWindow";
			Text = "Form1";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private Label label3;
		private Label label4;
		private TextBox ClientPort;
		private TextBox ClientIP;
		private Button ConnectClient;
		private TextBox MessText;
		private Button Send;
		private System.ComponentModel.BackgroundWorker BackgroundReceiver;
		private System.ComponentModel.BackgroundWorker BackgroundSender;
		private TextBox nameBox;
		private Label label7;
		private Button Change;
		private RichTextBox Messages;
		private Button Local;
		private Button vitekButton;
	}
}
