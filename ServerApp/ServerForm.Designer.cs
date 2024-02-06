namespace DankordServerApp
{
	partial class ServerForm
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
			StartButton = new Button();
			portInputBox = new TextBox();
			portLabel = new Label();
			OutputTextWindow = new RichTextBox();
			SuspendLayout();
			// 
			// StartButton
			// 
			StartButton.Location = new Point(122, 6);
			StartButton.Margin = new Padding(2);
			StartButton.Name = "StartButton";
			StartButton.Size = new Size(80, 23);
			StartButton.TabIndex = 0;
			StartButton.Text = "Start";
			StartButton.UseVisualStyleBackColor = true;
			StartButton.Click += StartButton_Click;
			// 
			// portInputBox
			// 
			portInputBox.Location = new Point(50, 6);
			portInputBox.Margin = new Padding(2);
			portInputBox.Name = "portInputBox";
			portInputBox.Size = new Size(68, 23);
			portInputBox.TabIndex = 1;
			portInputBox.Text = "7777";
			portInputBox.TextAlign = HorizontalAlignment.Center;
			// 
			// portLabel
			// 
			portLabel.AutoSize = true;
			portLabel.Location = new Point(11, 9);
			portLabel.Margin = new Padding(2, 0, 2, 0);
			portLabel.Name = "portLabel";
			portLabel.Size = new Size(35, 15);
			portLabel.TabIndex = 2;
			portLabel.Text = "PORT";
			// 
			// OutputTextWindow
			// 
			OutputTextWindow.Location = new Point(11, 33);
			OutputTextWindow.Margin = new Padding(2);
			OutputTextWindow.Name = "OutputTextWindow";
			OutputTextWindow.Size = new Size(538, 343);
			OutputTextWindow.TabIndex = 3;
			OutputTextWindow.Text = "";
			// 
			// ServerForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(560, 387);
			Controls.Add(OutputTextWindow);
			Controls.Add(portLabel);
			Controls.Add(portInputBox);
			Controls.Add(StartButton);
			Margin = new Padding(2);
			Name = "ServerForm";
			Text = "Form1";
			Load += Form1_Load;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private Button StartButton;
		private TextBox portInputBox;
		private Label portLabel;
		private RichTextBox OutputTextWindow;
	}
}
