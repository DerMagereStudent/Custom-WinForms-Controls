using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CustomWinFormsControls {
	/// <summary>
	/// Component which allows to define the area of a WinForm whcih should be used to move the form.
	/// </summary>
	[ToolboxItem(true)]
	public class FormDragControl : Component {
		private Control _target;

		private bool drag = false;
		private Point MouseDownPosition;

		/// <summary>
		/// The target of the control which should be the draggable area.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Behaviour")]
		[Description("The target of the control which should be the draggable area.")]
		public Control Target {
			get => this._target;
			set {
				if (this._target == value)
					return;

				if (this._target != null) {
					this._target.MouseDown -= this.TargetMouseDown;
					this._target.MouseMove -= this.TargetMouseMove;
					this._target.MouseUp -= this.TargetMouseUp;
				}

				this._target = value;

				if (this._target != null) {
					this._target.MouseDown += this.TargetMouseDown;
					this._target.MouseMove += this.TargetMouseMove;
					this._target.MouseUp += this.TargetMouseUp;
				}
			}
		}

		/// <summary>
		/// Creates a new instance of the class <see cref="FormDragControl"/>
		/// </summary>
		public FormDragControl() { }

		protected virtual void TargetMouseDown(object sender, MouseEventArgs e) {
			this.drag = true;
			this.MouseDownPosition = new Point(e.X, e.Y);
		}

		protected virtual void TargetMouseMove(object sender, MouseEventArgs e) {
			if (!this.drag)
				return;

			Form form = this.Target?.FindForm();
			form?.SetDesktopLocation(Control.MousePosition.X - this.MouseDownPosition.X, Control.MousePosition.Y - this.MouseDownPosition.Y);
		}

		protected virtual void TargetMouseUp(object sender, MouseEventArgs e) {
			this.drag = false;
		}
	}
}