using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CustomWinFormsControls {
	public partial class ScrollBarV : UserControl, IScrollBar {
		private float _scrollValue = 0.0f;
		private float _largeChange = 0.15f;
		private int _channelMargin = 3;
		private bool _showButtons = true;
		private Color _buttonColor;
		private Color _buttonHoverColor;
		private Color _buttonClickColor;
		private Color _handleColor;
		private Color _handleHoverColor;
		private Color _handleClickColor;
		private IScrollable _target;

		private bool hoverUp;
		private bool clickUp;

		private bool hoverDown;
		private bool clickDown;

		private bool hoverHandle;
		private bool clickHandle;
		private int yOffset;

		private int arrowHeight = 5;

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(0.0f)]
		[Category("Behaviour")]
		[Description("The scroll value of the scroll bar.")]
		public float ScrollValue {
			get => this._scrollValue;
			set {
				this._scrollValue = value;
				this.SetHandleByScrollValue();
				this.OnScrollValueChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(0.15f)]
		[Category("Behaviour")]
		[Description("The change which should be applied to the scroll value when clicking a button.")]
		public float LargeChange { get => this._largeChange; set => this._largeChange = value; }

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(3)]
		[Category("Appearance")]
		[Description("The spacing between the border of the channel and the border of the handle.")]
		public int ChannelMargin {
			get => this._channelMargin;
			set {
				if (this._channelMargin == value)
					return;

				this._channelMargin = value;
				this.RecalculateSizes();
				this.SetHandleByVisiblePercent();
				this.SetHandleByScrollValue();
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(true)]
		[Category("Appearance")]
		[Description("If true the scroll bar shows the two arrow buttons to enable scrolling which the IScrollBar.LargeChange")]
		public bool ShowButtons {
			get => this._showButtons;
			set {
				if (this._showButtons == value)
					return;

				this._showButtons = value;
				this.panelUp.Visible = this._showButtons;
				this.panelDown.Visible = this._showButtons;

				this.RecalculateSizes();
				this.SetHandleByVisiblePercent();
				this.SetHandleByScrollValue();
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Appearance")]
		[Description("The default color of the arrow buttons.")]
		public Color ButtonColor {
			get => this._buttonColor;
			set {
				this._buttonColor = value;
				this.panelUp.Invalidate();
				this.panelDown.Invalidate();
			}
		}


		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Appearance")]
		[Description("The color of the arrow buttons when the mouse hovers over it.")]
		public Color ButtonHoverColor {
			get => this._buttonHoverColor;
			set {
				this._buttonHoverColor = value;
				this.panelUp.Invalidate();
				this.panelDown.Invalidate();
			}
		}


		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Appearance")]
		[Description("The color of the arrow buttons on click.")]
		public Color ButtonClickColor {
			get => this._buttonClickColor;
			set {
				this._buttonClickColor = value;
				this.panelUp.Invalidate();
				this.panelDown.Invalidate();
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Appearance")]
		[Description("The default color of the handle.")]
		public Color HandleColor {
			get => this._handleColor;
			set {
				this._handleColor = value;
				this.panelHandle.Invalidate();
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Appearance")]
		[Description("The color of the handle when the mouse hovers over it.")]
		public Color HandleHoverColor {
			get => this._handleHoverColor;
			set {
				this._handleHoverColor = value;
				this.panelHandle.Invalidate();
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Appearance")]
		[Description("The color of the handle on click.")]
		public Color HandleClickColor {
			get => this._handleClickColor;
			set {
				this._handleClickColor = value;
				this.panelHandle.Invalidate();
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Behaviour")]
		[Description("The target of the scroll bar.")]
		public IScrollable Target {
			get => this._target;
			set {
				if (this._target == value)
					return;

				if (!(this._target is null)) {
					this._target.OnScrollValueVChanged -= this.Target_OnScrollValueVChanged;
					this._target.OnVisiblePercentVChanged -= this.Target_OnVisiblePercentVChanged;
				}

				this._target = value;

				if (!(this._target is null)) {
					this._target.OnScrollValueVChanged += this.Target_OnScrollValueVChanged;
					this._target.OnVisiblePercentVChanged += this.Target_OnVisiblePercentVChanged;
					this.SetHandleByVisiblePercent();
					this._scrollValue = this._target.ScrollValueV;
				}
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Behaviour")]
		[Description("Event Handler which is called when the scroll value was changed by moving the handle or setting the property.")]
		public event TypedEventHandler<IScrollBar, EventArgs> OnScrollValueChanged = null;

		public ScrollBarV() {
			this.InitializeComponent();
		}

		private void RecalculateSizes() {
			this.panelMargin.Location = new Point(this._channelMargin, this._showButtons ? 0 : this._channelMargin);

			int width = this.panelChannel.Width - 2 * this._channelMargin;
			this.panelMargin.Size = new Size(width < 0 ? 0 : width, this._showButtons ? this.panelChannel.Height : this.panelChannel.Height - 2 * this._channelMargin);

			this.panelHandle.Size = new Size(this.panelMargin.Width, this.panelHandle.Height);
			this.SetHandleByVisiblePercent();
			this.SetHandleByScrollValue();
		}

		private void SetScrollValueByHandle() {
			int maxMoveArea = this.panelMargin.Height - this.panelHandle.Height;

			if (maxMoveArea <= 0) {
				this._scrollValue = 1.0f;
				return;
			}

			this._scrollValue = this.panelHandle.Location.Y / (float)maxMoveArea;
		}

		private void SetHandleByScrollValue() {
			int maxMoveArea = this.panelMargin.Height - this.panelHandle.Height;
			this.panelHandle.Location = new Point(this.panelHandle.Location.X, (int)(maxMoveArea * this._scrollValue));
		}

		private void SetHandleByVisiblePercent() {
			if (this.Target is null)
				return;

			int height = (int)(this.panelMargin.Height * this.Target.VisiblePercentV);

			if (height < 20)
				height = 20;

			this.panelHandle.Size = new Size(this.panelHandle.Width, height);
		}

		private void ScrollBarV_Resize(object sender, EventArgs e) {
			this.RecalculateSizes();
		}

		#region Paint
		private void PanelDown_Paint(object sender, PaintEventArgs e) {
			Color color = this.clickDown ? this.ButtonClickColor : (this.hoverDown ? this.ButtonHoverColor : this.ButtonColor);

			e.Graphics.TranslateTransform(this.panelDown.Width / 2.0f, this.panelDown.Height / 2.0f);
			e.Graphics.RotateTransform(180);
			e.Graphics.TranslateTransform(-this.panelDown.Width / 2.0f, -this.panelDown.Height / 2.0f);

			this.DrawArrow(e.Graphics, this.panelUp, color);
		}

		private void PanelUp_Paint(object sender, PaintEventArgs e) {
			Color color = this.clickUp ? this.ButtonClickColor : (this.hoverUp ? this.ButtonHoverColor : this.ButtonColor);
			this.DrawArrow(e.Graphics, this.panelUp, color);
		}

		private void DrawArrow(Graphics g, Panel p, Color c) {
			Brush brush = new SolidBrush(c);
			var p1 = new PointF(p.Width / 2.0f, this.arrowHeight);
			var p2 = new PointF(this._channelMargin, p.Height - this.arrowHeight);
			var p3 = new PointF(p.Width - this._channelMargin, p2.Y);
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			g.FillPolygon(brush, new PointF[] { p1, p2, p3 });
			brush.Dispose();
		}

		private void PanelHandle_Paint(object sender, PaintEventArgs e) {
			Color color = this.clickHandle ? this.HandleClickColor : (this.hoverHandle ? this.HandleHoverColor : this.HandleColor);
			
			Brush brush = new SolidBrush(color);
			e.Graphics.FillRectangle(brush, e.ClipRectangle);
			brush.Dispose();
		}
		#endregion

		#region PanelUp
		private void PanelUp_MouseDown(object sender, MouseEventArgs e) {
			this.clickUp = true;
			this.panelUp.Invalidate();
		}

		private void PanelUp_MouseEnter(object sender, EventArgs e) {
			this.hoverUp = true;
			this.panelUp.Invalidate();
		}

		private void PanelUp_MouseLeave(object sender, EventArgs e) {
			this.hoverUp = false;
			this.panelUp.Invalidate();
		}

		private void PanelUp_MouseUp(object sender, MouseEventArgs e) {
			this.clickUp = false;
			this.panelUp.Invalidate();
		}

		private void PanelUp_MouseClick(object sender, MouseEventArgs e) {
			this._scrollValue -= this._largeChange;

			if (this._scrollValue < 0.0f)
				this._scrollValue = 0.0f;

			this.SetHandleByScrollValue();
			this.OnScrollValueChanged?.Invoke(this, EventArgs.Empty);
		}
		#endregion

		#region PanelDown
		private void PanelDown_MouseDown(object sender, MouseEventArgs e) {
			this.clickDown = true;
			this.panelDown.Invalidate();
		}

		private void PanelDown_MouseEnter(object sender, EventArgs e) {
			this.hoverDown = true;
			this.panelDown.Invalidate();
		}

		private void PanelDown_MouseLeave(object sender, EventArgs e) {
			this.hoverDown = false;
			this.panelDown.Invalidate();
		}

		private void PanelDown_MouseUp(object sender, MouseEventArgs e) {
			this.clickDown = false;
			this.panelDown.Invalidate();
		}

		private void PanelDown_MouseClick(object sender, MouseEventArgs e) {
			this._scrollValue += this._largeChange;

			if (this._scrollValue > 1.0f)
				this._scrollValue = 1.0f;

			this.SetHandleByScrollValue();
			this.OnScrollValueChanged?.Invoke(this, EventArgs.Empty);
		}
		#endregion

		#region Handle
		private void PanelHandle_MouseDown(object sender, MouseEventArgs e) {
			this.clickHandle = true;
			this.yOffset = e.Location.Y;
			this.panelHandle.Invalidate();
		}

		private void PanelHandle_MouseEnter(object sender, EventArgs e) {
			this.hoverHandle = true;
			this.panelHandle.Invalidate();
		}

		private void PanelHandle_MouseLeave(object sender, EventArgs e) {
			this.hoverHandle = false;
			this.panelHandle.Invalidate();
		}

		private void PanelHandle_MouseUp(object sender, MouseEventArgs e) {
			this.clickHandle = false;
			this.panelHandle.Invalidate();
		}

		private void PanelHandle_MouseMove(object sender, MouseEventArgs e) {
			if (!this.clickHandle)
				return;

			int newY = this.panelHandle.Location.Y + e.Location.Y - this.yOffset;

			if (newY < 0)
				this.panelHandle.Location = new Point(0, 0);
			else if (newY > this.panelMargin.Height - this.panelHandle.Height)
				this.panelHandle.Location = new Point(0, this.panelMargin.Height - this.panelHandle.Height);
			else
				this.panelHandle.Location = new Point(0, newY);

			this.SetScrollValueByHandle();
			this.OnScrollValueChanged?.Invoke(this, EventArgs.Empty);
		}
		#endregion

		private void Target_OnVisiblePercentVChanged(IScrollable sender, EventArgs e) {
			this.SetHandleByVisiblePercent();
			this.SetHandleByScrollValue();
		}

		private void Target_OnScrollValueVChanged(IScrollable sender, EventArgs e) {
			this._scrollValue = sender.ScrollValueV;
			this.SetHandleByScrollValue();
		}
	}
}