using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CustomWinFormsControls {
	/// <summary>
	/// Button which represents the default close, minimize or maximize button of the default Windows 10 theme.
	/// </summary>
	[ToolboxItem(true)]
	public class WindowsDefaultTitleBarButton : NoFocusCuesButton {
		/// <summary>
		/// Represents the 3 possible types of the windows border buttons.
		/// </summary>
		public enum Type {
			Close,
			Maximize,
			Minimize
		}

		private Pen activeIconColorPen;
		private Brush activeIconColorBrush;
		private Brush activeColorBrush;

		/// <summary>
		/// The type which defines the buttons behaviour.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(Type.Close)]
		[Category("Appearance")]
		[Description("The type which defines the buttons behaviour.")]
		public Type ButtonType { get; set; } = Type.Close;

		/// <summary>
		/// The background color of the button when the mouse is inside the buttons bounds.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Appearance")]
		[Description("The background color of the button when the mouse is inside the buttons bounds.")]
		public Color HoverColor { get; set; } = Color.Transparent;

		/// <summary>
		/// The background color of the button when the button is clicked.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Appearance")]
		[Description("The background color of the button when the button is clicked.")]
		public Color ClickColor { get; set; } = Color.Transparent;

		/// <summary>
		/// The default color of the icon.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Appearance")]
		[Description("The default color of the icon.")]
		public Color IconColor { get; set; } = Color.Transparent;

		/// <summary>
		/// The color of the icon when the mouse is inside the buttons bounds.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Appearance")]
		[Description("The color of the icon when the mouse is inside the buttons bounds.")]
		public Color HoverIconColor { get; set; } = Color.Transparent;

		/// <summary>
		/// The color of the icon when the button is clicked.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Appearance")]
		[Description("The color of the icon when the button is clicked.")]
		public Color ClickIconColor { get; set; } = Color.Transparent;

		/// <summary>
		/// Property which returns the active background color of the button depending on if the button is clicked or hovered.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public virtual Color ActiveColor {
			get {
				if (this.Clicked)
					return this.ClickColor == null ? Color.Transparent : this.ClickColor;

				if (this.Hovered)
					return this.HoverColor == null ? Color.Transparent : this.HoverColor;

				return this.BackColor == null ? Color.Transparent : this.BackColor;
			}
		}

		/// <summary>
		/// Property which returns the active color of the buttons icon depending on if the button is clicked or hovered.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public virtual Color ActiveIconColor {
			get {
				if (this.Clicked)
					return this.ClickIconColor == null ? Color.Transparent : this.ClickIconColor;

				if (this.Hovered)
					return this.HoverIconColor == null ? Color.Transparent : this.HoverIconColor;

				return this.IconColor == null ? Color.Transparent : this.IconColor;
			}
		}

		/// <summary>
		/// Property which indicates if the mouse is currently inside the bounds of the button.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DefaultValue(false)]
		public bool Hovered { get; set; }

		/// <summary>
		/// Property which indicates if the left mouse button was pressed down inside the buttons bounds. Can be true before the click event is triggered.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DefaultValue(false)]
		public bool Clicked { get; set; }

		public WindowsDefaultTitleBarButton() { }

		protected override void OnMouseEnter(EventArgs e) {
			base.OnMouseEnter(e);
			this.Hovered = true;
		}

		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave(e);
			this.Hovered = false;
		}

		protected override void OnMouseDown(MouseEventArgs mevent) {
			base.OnMouseDown(mevent);
			this.Clicked = true;
		}

		protected override void OnMouseUp(MouseEventArgs mevent) {
			base.OnMouseUp(mevent);
			this.Clicked = false;
		}

		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
		}

		protected override void OnClick(EventArgs e) {
			if (this.FindForm() == null)
				return;

			if (this.ButtonType == Type.Close)
				this.FindForm().Close();
			else if (this.ButtonType == Type.Maximize)
				this.FindForm().WindowState = this.FindForm().WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
			else
				this.FindForm().WindowState = FormWindowState.Minimized;

			base.OnClick(e);
		}

		protected override void OnPaint(PaintEventArgs pevent) {
			this.activeColorBrush?.Dispose();
			this.activeColorBrush = new SolidBrush(this.ActiveColor);

			pevent.Graphics.FillRectangle(new SolidBrush(this.ActiveColor), pevent.ClipRectangle);
			pevent.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			this.activeIconColorBrush?.Dispose();
			this.activeIconColorPen?.Dispose();

			this.activeIconColorBrush = new SolidBrush(this.ActiveIconColor);
			this.activeIconColorPen = new Pen(this.activeIconColorBrush, 1.0f);

			if (this.ButtonType == Type.Close)
				this.drawCloseIcon(pevent, new Rectangle(0, 0, this.Width, this.Height));
			else if (this.ButtonType == Type.Maximize)
				this.drawMaximizeIcon(pevent, new Rectangle(0, 0, this.Width, this.Height));
			else
				this.drawMinimizeIcon(pevent, new Rectangle(0, 0, this.Width, this.Height));
		}

		protected virtual void drawCloseIcon(PaintEventArgs e, Rectangle drawRect) {
			e.Graphics.DrawLine(
				this.activeIconColorPen,
				drawRect.X + drawRect.Width / 2 - 5,
				drawRect.Y + drawRect.Height / 2 - 5,
				drawRect.X + drawRect.Width / 2 + 5,
				drawRect.Y + drawRect.Height / 2 + 5);

			e.Graphics.DrawLine(
				this.activeIconColorPen,
				drawRect.X + drawRect.Width / 2 - 5,
				drawRect.Y + drawRect.Height / 2 + 5,
				drawRect.X + drawRect.Width / 2 + 5,
				drawRect.Y + drawRect.Height / 2 - 5); ;
		}

		protected virtual void drawMaximizeIcon(PaintEventArgs e, Rectangle drawRect) {
			if (this.FindForm()?.WindowState == FormWindowState.Normal) {
				e.Graphics.DrawRectangle(
					this.activeIconColorPen,
					new Rectangle(
						drawRect.X + drawRect.Width / 2 - 5,
						drawRect.Y + drawRect.Height / 2 - 5,
						10, 10));
			}
			else if (this.FindForm()?.WindowState == FormWindowState.Maximized) {
				e.Graphics.DrawRectangle(
					this.activeIconColorPen,
					new Rectangle(
						drawRect.X + drawRect.Width / 2 - 3,
						drawRect.Y + drawRect.Height / 2 - 5,
						8, 8));

				Rectangle rect = new Rectangle(
					drawRect.X + drawRect.Width / 2 - 5,
					drawRect.Y + drawRect.Height / 2 - 3,
					8, 8);

				e.Graphics.FillRectangle(this.activeColorBrush, rect);
				e.Graphics.DrawRectangle(this.activeIconColorPen, rect);
			}
		}

		protected virtual void drawMinimizeIcon(PaintEventArgs e, Rectangle drawRect) {
			e.Graphics.DrawLine(
				this.activeIconColorPen,
				drawRect.X + drawRect.Width / 2 - 5,
				drawRect.Y + drawRect.Height / 2,
				drawRect.X + drawRect.Width / 2 + 5,
				drawRect.Y + drawRect.Height / 2);
		}
	}
}