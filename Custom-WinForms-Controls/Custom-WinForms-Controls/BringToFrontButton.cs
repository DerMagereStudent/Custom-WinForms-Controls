using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace CustomWinFormsControls {
	/// <summary>
	/// Button which brings a given target control to front when the button was clicked.
	/// </summary>
	public class BringToFrontButton : NoFocusCuesButton {
		/// <summary>
		/// The target control which should be brought to front.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Behaviour")]
		[Description("The target this button should bring to front.")]
		public Control Target { get; set; }

		/// <summary>
		/// Creates a new instance of a <see cref="BringToFrontButton"/>
		/// </summary>
		public BringToFrontButton() { }

		protected override void OnClick(EventArgs e) {
			this.Target?.BringToFront();
			base.OnClick(e);
		}
	}
}