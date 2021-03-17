
namespace CustomWinFormsControls {
	partial class ScrollBarH {
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
			this.panelLeft = new System.Windows.Forms.Panel();
			this.panelRight = new System.Windows.Forms.Panel();
			this.panelChannel = new System.Windows.Forms.Panel();
			this.panelMargin = new System.Windows.Forms.Panel();
			this.panelHandle = new System.Windows.Forms.Panel();
			this.panelChannel.SuspendLayout();
			this.panelMargin.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelLeft
			// 
			this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelLeft.Location = new System.Drawing.Point(0, 0);
			this.panelLeft.Margin = new System.Windows.Forms.Padding(0);
			this.panelLeft.Name = "panelLeft";
			this.panelLeft.Size = new System.Drawing.Size(15, 15);
			this.panelLeft.TabIndex = 0;
			this.panelLeft.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelLeft_Paint);
			this.panelLeft.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PanelLeft_MouseClick);
			this.panelLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelLeft_MouseDown);
			this.panelLeft.MouseEnter += new System.EventHandler(this.PanelLeft_MouseEnter);
			this.panelLeft.MouseLeave += new System.EventHandler(this.PanelLeft_MouseLeave);
			this.panelLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelLeft_MouseUp);
			// 
			// panelRight
			// 
			this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.panelRight.Location = new System.Drawing.Point(135, 0);
			this.panelRight.Margin = new System.Windows.Forms.Padding(0);
			this.panelRight.Name = "panelRight";
			this.panelRight.Size = new System.Drawing.Size(15, 15);
			this.panelRight.TabIndex = 1;
			this.panelRight.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelRight_Paint);
			this.panelRight.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PanelRight_MouseClick);
			this.panelRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelRight_MouseDown);
			this.panelRight.MouseEnter += new System.EventHandler(this.PanelRight_MouseEnter);
			this.panelRight.MouseLeave += new System.EventHandler(this.PanelRight_MouseLeave);
			this.panelRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelRight_MouseUp);
			// 
			// panelChannel
			// 
			this.panelChannel.Controls.Add(this.panelMargin);
			this.panelChannel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelChannel.Location = new System.Drawing.Point(15, 0);
			this.panelChannel.Margin = new System.Windows.Forms.Padding(0);
			this.panelChannel.Name = "panelChannel";
			this.panelChannel.Size = new System.Drawing.Size(120, 15);
			this.panelChannel.TabIndex = 1;
			// 
			// panelMargin
			// 
			this.panelMargin.Controls.Add(this.panelHandle);
			this.panelMargin.Location = new System.Drawing.Point(0, 0);
			this.panelMargin.Margin = new System.Windows.Forms.Padding(0);
			this.panelMargin.Name = "panelMargin";
			this.panelMargin.Size = new System.Drawing.Size(120, 15);
			this.panelMargin.TabIndex = 1;
			// 
			// panelHandle
			// 
			this.panelHandle.Location = new System.Drawing.Point(0, 0);
			this.panelHandle.Margin = new System.Windows.Forms.Padding(0);
			this.panelHandle.Name = "panelHandle";
			this.panelHandle.Size = new System.Drawing.Size(50, 15);
			this.panelHandle.TabIndex = 1;
			this.panelHandle.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelHandle_Paint);
			this.panelHandle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelHandle_MouseDown);
			this.panelHandle.MouseEnter += new System.EventHandler(this.PanelHandle_MouseEnter);
			this.panelHandle.MouseLeave += new System.EventHandler(this.PanelHandle_MouseLeave);
			this.panelHandle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelHandle_MouseMove);
			this.panelHandle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelHandle_MouseUp);
			// 
			// ScrollBarH
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panelChannel);
			this.Controls.Add(this.panelRight);
			this.Controls.Add(this.panelLeft);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ScrollBarH";
			this.Size = new System.Drawing.Size(150, 15);
			this.Resize += new System.EventHandler(this.ScrollBarH_Resize);
			this.panelChannel.ResumeLayout(false);
			this.panelMargin.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelLeft;
		private System.Windows.Forms.Panel panelRight;
		private System.Windows.Forms.Panel panelChannel;
		private System.Windows.Forms.Panel panelMargin;
		private System.Windows.Forms.Panel panelHandle;
	}
}
