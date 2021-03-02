using System.Windows.Forms;

namespace CustomWinFormsControls {
	/// <summary>
	/// Modified button which has no focus rectangles when the form which contains this button loses focus while the button was focused.
	/// </summary>
	public class NoFocusCuesButton : Button {
		protected override bool ShowFocusCues => false;

		/// <summary>
		/// Creates a new instance of a <see cref="NoFocusCueButton"/>
		/// </summary>
		public NoFocusCueButton() { }

		public override void NotifyDefault(bool value) {
			base.NotifyDefault(false);
		}
	}
}