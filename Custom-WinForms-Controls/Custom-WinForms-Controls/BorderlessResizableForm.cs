using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CustomWinFormsControls {
	/// <summary>
	/// Form with a <see cref="FormBorderStyle.None"/> which can be resized and moved if drag control is set.
	/// </summary>
	public class BorderlessResizableForm : Form {
		private FormDragControl dragControl;
		private Dictionary<Control, BorderWndProcFilter> borderFilters;

		private int _resizeBorderThickness = 5;

		/// <summary>
		/// The control which should be used to move the window around.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Behaviour")]
		[Description("The control which should be used to move the window around.")]
		public Control DragTarget {
			get => this.dragControl?.Target;
			set {
				if (!(this.dragControl is null))
					this.dragControl.Target = value;
			}
		}

		/// <summary>
		/// The thickness of the resize border on all sides of the window. The border is invisible. Setting it will not result in a padding.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Behaviour")]
		[Description("The thickness of the resize border on all sides of the window. The border is invisible. Setting it will not result in a padding.")]
		public int ResizeBorderThickness {
			get => this._resizeBorderThickness;
			set {
				if (this._resizeBorderThickness == value)
					return;

				this._resizeBorderThickness = value;

				foreach (BorderWndProcFilter filter in this.borderFilters.Values)
					filter.BorderThinckness = value;
			}
		}

		/// <summary>
		/// Creates a new instance of the class <see cref="BorderlessResizableForm"/>
		/// </summary>
		public BorderlessResizableForm() {
			this.FormBorderStyle = FormBorderStyle.None;
			this.SetStyle(ControlStyles.ResizeRedraw, true);

			this.dragControl = new FormDragControl();
			this.borderFilters = new Dictionary<Control, BorderWndProcFilter>();
			this.ControlAdded += this.Control_ControlAdded;
			this.ControlRemoved += this.Control_ControlRemoved;
		}

		private void Control_ControlAdded(object sender, ControlEventArgs e) {
			this.ApplyFiltersRecursive(this, e.Control);
		}

		private void Control_ControlRemoved(object sender, ControlEventArgs e) {
			this.RemoveFiltersRecursive(e.Control);
		}

		private void ApplyFiltersRecursive(Control parent, Control control) {
			if (control is null)
				return;

			this.borderFilters[control] = new BorderWndProcFilter(parent, control, this.ResizeBorderThickness);

			// remove them in case they were already added.
			control.ControlAdded -= this.Control_ControlAdded;
			control.ControlRemoved -= this.Control_ControlRemoved;

			control.ControlAdded += this.Control_ControlAdded;
			control.ControlRemoved += this.Control_ControlRemoved;

			foreach (Control c in control.Controls)
				if (!(c is null))
					this.ApplyFiltersRecursive(parent, c);
		}

		private void RemoveFiltersRecursive(Control control) {
			if (control is null)
				return;

			this.borderFilters.Remove(control);
			control.ControlAdded -= this.Control_ControlAdded;
			control.ControlRemoved -= this.Control_ControlRemoved;

			foreach (Control c in control.Controls)
				if (!(c is null))
					this.RemoveFiltersRecursive(c);
		}

		protected override void WndProc(ref Message m) {
			if (m.Msg != BorderWndProcFilter.WM_NCHITTEST) {
				base.WndProc(ref m);
				return;
			}

			Point pos = this.PointToClient(new Point(m.LParam.ToInt32()));

			// if in top left corner
			if (pos.X <= this.ResizeBorderThickness && pos.Y <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(BorderWndProcFilter.HTTOPLEFT);
				return;
			}

			// if in top right corner
			if (pos.X >= this.ClientSize.Width - this.ResizeBorderThickness && pos.Y <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(BorderWndProcFilter.HTTOPRIGHT);
				return;
			}

			// if in bottom left corner
			if (pos.X <= this.ResizeBorderThickness && pos.Y >= this.ClientSize.Height - this.ResizeBorderThickness) {
				m.Result = new IntPtr(BorderWndProcFilter.HTBOTTOMLEFT);
				return;
			}

			// if in bottom right corner
			if (pos.X >= this.ClientSize.Width - this.ResizeBorderThickness && pos.Y >= this.ClientSize.Height - this.ResizeBorderThickness) {
				m.Result = new IntPtr(BorderWndProcFilter.HTBOTTOMRIGHT);
				return;
			}

			// if on the left
			if (pos.X <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(BorderWndProcFilter.HTLEFT);
				return;
			}

			// if on top
			if (pos.Y <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(BorderWndProcFilter.HTTOP);
				return;
			}

			// if on the right
			if (pos.X >= this.ClientSize.Width - this.ResizeBorderThickness) {
				m.Result = new IntPtr(BorderWndProcFilter.HTRIGHT);
				return;
			}

			// if on the bottom
			if (pos.Y >= this.ClientSize.Height - this.ResizeBorderThickness) {
				m.Result = new IntPtr(BorderWndProcFilter.HTBOTTOM);
				return;
			}

			base.WndProc(ref m);
		}
	}

	/// <summary>
	/// A filter for the WM_NCHITTEST message to ignore it when the mouse hovers the resize border.
	/// </summary>
	public class BorderWndProcFilter : NativeWindow {
		public const int WM_NCHITTEST = 0x0084;

		public const int HTCAPTION = 0x00000002;
		public const int HTTOP = 0x0000000C;
		public const int HTBOTTOM = 0x0000000F;
		public const int HTLEFT = 0x0000000A;
		public const int HTRIGHT = 0x0000000B;
		public const int HTTOPLEFT = 0x0000000D;
		public const int HTTOPRIGHT = 0x0000000E;
		public const int HTBOTTOMLEFT = 0x00000010;
		public const int HTBOTTOMRIGHT = 0x00000011;
		public const int HTTRANSPARENT = -1;

		public static List<Type> TypeBlacklist { get; set; } = new List<Type>();

		private Control parent;

		public int BorderThinckness { get; set; }
		public bool ResizeBorderLeft { get; set; }
		public bool ResizeBorderRight { get; set; }
		public bool ResizeBorderTop { get; set; }
		public bool ResizeBorderBottom { get; set; }

		/// <summary>
		/// Creates a new instance of the class <see cref="BorderWndProcFilter"/>. Sets all borders to true.
		/// </summary>
		/// <param name="parent">The resizable control which should have a non blocked resize border.</param>
		/// <param name="child">The child control the filter should be applied to for not blocking the resize border of the parent control.</param>
		/// <param name="borderThinckness">The resize border thickness of the parent.</param>
		public BorderWndProcFilter(Control parent, Control child, int borderThinckness) {
			this.parent = parent;

			try {
				if (!BorderWndProcFilter.TypeBlacklist.Contains(child.GetType()))
					this.AssignHandle(child.Handle);
			} catch (Exception) { }

			this.BorderThinckness = borderThinckness;

			this.ResizeBorderLeft = true;
			this.ResizeBorderRight = true;
			this.ResizeBorderTop = true;
			this.ResizeBorderBottom = true;
		}

		/// <summary>
		/// Creates a new instance of the class <see cref="BorderWndProcFilter"/>. Sets all borders to true.
		/// </summary>
		/// <param name="parent">The resizable control which should have a non blocked resize border.</param>
		/// <param name="child">The child control the filter should be applied to for not blocking the resize border of the parent control.</param>
		/// <param name="borderThinckness">The resize border thickness of the parent.</param>
		/// <param name="left">If true the parent has a left-side resize border which should not be blocked.</param>
		/// <param name="right">If true the parent has a right-side resize border which should not be blocked.</param>
		/// <param name="top">If true the parent has a top-side resize border which should not be blocked.</param>
		/// <param name="bottom">If true the parent has a bottom-side resize border which should not be blocked.</param>
		public BorderWndProcFilter(Control parent, Control child, int borderThinckness, bool left, bool right, bool top, bool bottom) {
			this.parent = parent;
			this.AssignHandle(child.Handle);
			this.BorderThinckness = borderThinckness;

			this.ResizeBorderLeft = left;
			this.ResizeBorderRight = right;
			this.ResizeBorderTop = top;
			this.ResizeBorderBottom = bottom;
		}

		protected override void WndProc(ref Message m) {
			if (m.Msg == WM_NCHITTEST) {
				var pos = new Point(m.LParam.ToInt32());
				if (pos.X <= this.parent.Left + this.BorderThinckness && this.ResizeBorderLeft ||
					pos.Y <= this.parent.Top + this.BorderThinckness && this.ResizeBorderTop ||
					pos.X >= this.parent.Right - this.BorderThinckness && this.ResizeBorderRight ||
					pos.Y >= this.parent.Bottom - this.BorderThinckness && this.ResizeBorderBottom) {
					m.Result = new IntPtr(HTTRANSPARENT);
					return;
				}
			}

			base.WndProc(ref m);
		}
	}
}