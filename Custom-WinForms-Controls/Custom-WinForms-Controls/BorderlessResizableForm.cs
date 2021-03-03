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
		/// <summary>
		/// A filter for the WM_NCHITTEST message to ignore it when the mouse hovers the resize border.
		/// </summary>
		public class BorderWndProcFilter : NativeWindow {
			private Form form;
			
			public int BorderThinckness;

			public BorderWndProcFilter(Form form, Control child, int borderThinckness) {
				this.form = form;
				this.AssignHandle(child.Handle);
				this.BorderThinckness = borderThinckness;
			}

			protected override void WndProc(ref Message m) {
				if (m.Msg == WM_NCHITTEST) {
					var pos = new Point(m.LParam.ToInt32());
					if (pos.X < this.form.Left + this.BorderThinckness ||
						pos.Y < this.form.Top + this.BorderThinckness ||
						pos.X > this.form.Right - this.BorderThinckness ||
						pos.Y > this.form.Bottom - this.BorderThinckness) {
						m.Result = new IntPtr(HTTRANSPARENT);
						return;
					}
				}
				base.WndProc(ref m);
			}
		}

		public const int WM_NCHITTEST = 0x0084;

		public const int HTCAPTION		= 0x00000002;
		public const int HTTOP			= 0x0000000C;
		public const int HTBOTTOM		= 0x0000000F;
		public const int HTLEFT			= 0x0000000A;
		public const int HTRIGHT		= 0x0000000B;
		public const int HTTOPLEFT		= 0x0000000D;
		public const int HTTOPRIGHT		= 0x0000000E;
		public const int HTBOTTOMLEFT	= 0x00000010;
		public const int HTBOTTOMRIGHT	= 0x00000011;
		public const int HTTRANSPARENT	= -1;

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

		private void ApplyFiltersRecursive(Form form, Control control) {
			if (control is null)
				return;

			this.borderFilters[control] = new BorderWndProcFilter(form, control, this.ResizeBorderThickness);

			// remove them in case they were already added.
			control.ControlAdded -= this.Control_ControlAdded;
			control.ControlRemoved -= this.Control_ControlRemoved;

			control.ControlAdded += this.Control_ControlAdded;
			control.ControlRemoved += this.Control_ControlRemoved;

			foreach (Control c in control.Controls)
				if (!(c is null))
					this.ApplyFiltersRecursive(form, c);
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
			if (m.Msg != WM_NCHITTEST) {
				base.WndProc(ref m);
				return;
			}

			Point globalPos = new Point(m.LParam.ToInt32());
			Point pos = this.PointToClient(globalPos);

			// if in top left corner
			if (pos.X <= this.ResizeBorderThickness && pos.Y <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(HTTOPLEFT);
				return;
			}

			// if in top right corner
			if (pos.X >= this.ClientSize.Width - this.ResizeBorderThickness && pos.Y <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(HTTOPRIGHT);
				return;
			}

			// if in bottom left corner
			if (pos.X <= this.ResizeBorderThickness && pos.Y >= this.ClientSize.Height - this.ResizeBorderThickness) {
				m.Result = new IntPtr(HTBOTTOMLEFT);
				return;
			}

			// if in bottom right corner
			if (pos.X >= this.ClientSize.Width - this.ResizeBorderThickness && pos.Y >= this.ClientSize.Height - this.ResizeBorderThickness) {
				m.Result = new IntPtr(HTBOTTOMRIGHT);
				return;
			}

			// if on the left
			if (pos.X <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(HTLEFT);
				return;
			}

			// if on top
			if (pos.Y <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(HTTOP);
				return;
			}

			// if on the right
			if (pos.X >= this.ClientSize.Width - this.ResizeBorderThickness) {
				m.Result = new IntPtr(HTRIGHT);
				return;
			}

			// if on the bottom
			if (pos.Y >= this.ClientSize.Height - this.ResizeBorderThickness) {
				m.Result = new IntPtr(HTBOTTOM);
				return;
			}

			base.WndProc(ref m);
		}
	}
}