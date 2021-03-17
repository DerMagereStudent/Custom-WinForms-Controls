using System;

namespace CustomWinFormsControls {
	/// <summary>
	/// Interface which enables a driving class to be modified by a <see cref="ScrollBarH"/> and a <see cref="ScrollBarV"/>
	/// </summary>
	public interface IScrollable {
		/// <summary>
		/// The visible percent of the content of the scrollable control in horizontal direction.
		/// </summary>
		float VisiblePercentH { get; }

		/// <summary>
		/// The visible percent of the content of the scrollable control in vertical direction.
		/// </summary>
		float VisiblePercentV { get; }

		/// <summary>
		/// The percentage value of how much the content of the control is scrolled down in horizontal direction.
		/// </summary>
		float ScrollValueH { get; set; }

		/// <summary>
		/// The percentage value of how much the content of the control is scrolled down in vertical direction.
		/// </summary>
		float ScrollValueV { get; set; }

		/// <summary>
		/// The value the mouse wheel delta is multiplied with when scrolling.
		/// </summary>
		float ScrollSpeed { get; set; }

		/// <summary>
		/// Show option which specifies when the horizontal scrollbar shows up.
		/// </summary>
		ShowScrollBarOption ShowHorizontal { get; set; }

		/// <summary>
		/// Show option which specifies when the horizontal scrollbar shows up.
		/// </summary>
		ShowScrollBarOption ShowVertical { get; set; }

		/// <summary>
		/// The horizontal scroll bar, whose values ​​should be synchronized with those of the scrollable control.
		/// </summary>
		ScrollBarH ScrollBarH { get; set; }

		/// <summary>
		/// The vertical scroll bar, whose values ​​should be synchronized with those of the scrollable control.
		/// </summary>
		ScrollBarV ScrollBarV { get; set; }

		/// <summary>
		/// Event Handler which is called when the horizontal visible percent value was changed.
		/// </summary>
		event TypedEventHandler<IScrollable, EventArgs> OnVisiblePercentHChanged;

		/// <summary>
		/// Event Handler which is called when the vertical visible percent value was changed.
		/// </summary>
		event TypedEventHandler<IScrollable, EventArgs> OnVisiblePercentVChanged;

		/// <summary>
		/// Event Handler which is called when the horizontal scroll values were changed.
		/// </summary>
		event TypedEventHandler<IScrollable, EventArgs> OnScrollValueHChanged;

		/// <summary>
		/// Event Handler which is called when the vertical scroll values were changed.
		/// </summary>
		event TypedEventHandler<IScrollable, EventArgs> OnScrollValueVChanged;
	}
}