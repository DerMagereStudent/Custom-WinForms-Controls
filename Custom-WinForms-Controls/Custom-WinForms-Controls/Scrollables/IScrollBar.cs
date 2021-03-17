using System;
using System.ComponentModel;
using System.Drawing;

namespace CustomWinFormsControls {
	/// <summary>
	/// Interface that defines all base properties of a scroll bar.
	/// </summary>
	public interface IScrollBar {
		/// <summary>
		/// The scroll value of the scroll bar.
		/// </summary>
		float ScrollValue { get; set; }

		/// <summary>
		/// The change which should be applied to the scroll value when clicking a button.
		/// </summary>
		float LargeChange { get; set; }

		/// <summary>
		/// The spacing between the border of the channel and the border of the handle.
		/// </summary>
		int ChannelMargin { get; set; }

		/// <summary>
		/// If true the scroll bar shows the two arrow buttons to enable scrolling which the <see cref="IScrollBar.LargeChange"/>
		/// </summary>
		bool ShowButtons { get; set; }

		/// <summary>
		/// The default color of the arrow buttons.
		/// </summary>
		Color ButtonColor { get; set; }

		/// <summary>
		/// The color of the arrow buttons when the mouse hovers over it.
		/// </summary>
		Color ButtonHoverColor { get; set; }

		/// <summary>
		/// The color of the arrow buttons on click.
		/// </summary>
		Color ButtonClickColor { get; set; }

		/// <summary>
		/// The default color of the handle.
		/// </summary>
		Color HandleColor { get; set; }

		/// <summary>
		/// The color of the handle when the mouse hovers over it.
		/// </summary>
		Color HandleHoverColor { get; set; }

		/// <summary>
		/// The color of the handle on click.
		/// </summary>
		Color HandleClickColor { get; set; }

		/// <summary>
		/// The target of the scroll bar.
		/// </summary>	
		IScrollable Target { get; set; }

		/// <summary>
		/// Event Handler which is called when the scroll value was changed by moving the handle or setting the property.
		/// </summary>
		event TypedEventHandler<IScrollBar, EventArgs> OnScrollValueChanged;
	}
}