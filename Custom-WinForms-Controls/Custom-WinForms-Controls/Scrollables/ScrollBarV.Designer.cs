
namespace CustomWinFormsControls {
	partial class ScrollBarV {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.panelUp = new System.Windows.Forms.Panel();
			this.panelDown = new System.Windows.Forms.Panel();
			this.panelChannel = new System.Windows.Forms.Panel();
			this.panelMargin = new System.Windows.Forms.Panel();
			this.panelHandle = new System.Windows.Forms.Panel();
			this.panelChannel.SuspendLayout();
			this.panelMargin.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelUp
			// 
			this.panelUp.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelUp.Location = new System.Drawing.Point(0, 0);
			this.panelUp.Margin = new System.Windows.Forms.Padding(0);
			this.panelUp.Name = "panelUp";
			this.panelUp.Size = new System.Drawing.Size(15, 15);
			this.panelUp.TabIndex = 0;
			this.panelUp.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelUp_Paint);
			this.panelUp.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PanelUp_MouseClick);
			this.panelUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelUp_MouseDown);
			this.panelUp.MouseEnter += new System.EventHandler(this.PanelUp_MouseEnter);
			this.panelUp.MouseLeave += new System.EventHandler(this.PanelUp_MouseLeave);
			this.panelUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelUp_MouseUp);
			// 
			// panelDown
			// 
			this.panelDown.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelDown.Location = new System.Drawing.Point(0, 135);
			this.panelDown.Margin = new System.Windows.Forms.Padding(0);
			this.panelDown.Name = "panelDown";
			this.panelDown.Size = new System.Drawing.Size(15, 15);
			this.panelDown.TabIndex = 1;
			this.panelDown.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelDown_Paint);
			this.panelDown.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PanelDown_MouseClick);
			this.panelDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelDown_MouseDown);
			this.panelDown.MouseEnter += new System.EventHandler(this.PanelDown_MouseEnter);
			this.panelDown.MouseLeave += new System.EventHandler(this.PanelDown_MouseLeave);
			this.panelDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelDown_MouseUp);
			// 
			// panelChannel
			// 
			this.panelChannel.Controls.Add(this.panelMargin);
			this.panelChannel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelChannel.Location = new System.Drawing.Point(0, 15);
			this.panelChannel.Name = "panelChannel";
			this.panelChannel.Size = new System.Drawing.Size(15, 120);
			this.panelChannel.TabIndex = 2;
			// 
			// panelMargin
			// 
			this.panelMargin.Controls.Add(this.panelHandle);
			this.panelMargin.Location = new System.Drawing.Point(0, 0);
			this.panelMargin.Name = "panelMargin";
			this.panelMargin.Size = new System.Drawing.Size(15, 120);
			this.panelMargin.TabIndex = 3;
			// 
			// panelHandle
			// 
			this.panelHandle.Location = new System.Drawing.Point(0, 0);
			this.panelHandle.Name = "panelHandle";
			this.panelHandle.Size = new System.Drawing.Size(15, 50);
			this.panelHandle.TabIndex = 4;
			this.panelHandle.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelHandle_Paint);
			this.panelHandle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelHandle_MouseDown);
			this.panelHandle.MouseEnter += new System.EventHandler(this.PanelHandle_MouseEnter);
			this.panelHandle.MouseLeave += new System.EventHandler(this.PanelHandle_MouseLeave);
			this.panelHandle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelHandle_MouseMove);
			this.panelHandle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelHandle_MouseUp);
			// 
			// ScrollBarV
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panelChannel);
			this.Controls.Add(this.panelDown);
			this.Controls.Add(this.panelUp);
			this.Name = "ScrollBarV";
			this.Size = new System.Drawing.Size(15, 150);
			this.Resize += new System.EventHandler(this.ScrollBarV_Resize);
			this.panelChannel.ResumeLayout(false);
			this.panelMargin.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelUp;
		private System.Windows.Forms.Panel panelDown;
		private System.Windows.Forms.Panel panelChannel;
		private System.Windows.Forms.Panel panelMargin;
		private System.Windows.Forms.Panel panelHandle;
	}
}
