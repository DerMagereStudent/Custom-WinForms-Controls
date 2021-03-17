using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWinFormsControls {
	/// <summary>
	/// Show option which specifies when s scrollbar should show up.
	/// </summary>
	public enum ShowScrollBarOption {
		/// <summary>
		/// The scroll bar should show up never.
		/// </summary>
		Never,

		/// <summary>
		/// The scrollbar should show up always, also if the visible percent value is 1.0f (100%)
		/// </summary>
		Always,

		/// <summary>
		/// The scrollbar should only show up if the visible percent value is less than 1.0f (100%)
		/// </summary>
		OnOverflow
	}
}