using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomWinFormsControls {
	public class ScrollablePanel : ResizablePanel, IScrollable {
		private float _visiblePercentH;
		private float _visiblePercentV;

		private float _scrollValueH = 0.0f;
		private float _scrollValueV = 0.0f;

		private float _scrollSpeed = 0.5f;

		private ShowScrollBarOption _showHorizontal = ShowScrollBarOption.OnOverflow;
		private ShowScrollBarOption _showVertical = ShowScrollBarOption.OnOverflow;

		private ScrollBarH _scrollBarH;
		private ScrollBarV _scrollBarV;

		private Panel panelInner;

		protected override ControlCollection CreateControlsInstance() => new ScrollablePanelControlCollection(this, this.panelInner);

		public float VisiblePercentH => this._visiblePercentH;
		public float VisiblePercentV => this._visiblePercentV;

		public float ScrollValueH {
			get => this._scrollValueH;
			set {
				this._scrollValueH = value;

				if (this._scrollValueH < 0.0f)
					this._scrollValueH = 0.0f;

				if (this._scrollValueH > 1.0f)
					this._scrollValueH = 1.0f;

				this.SetPanelInnerByScrollValues();
				this.OnScrollValueHChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public float ScrollValueV {
			get => this._scrollValueV;
			set {
				this._scrollValueV = value;

				if (this._scrollValueV < 0.0f)
					this._scrollValueV = 0.0f;

				if (this._scrollValueV > 1.0f)
					this._scrollValueV = 1.0f;

				this.SetPanelInnerByScrollValues();
				this.OnScrollValueVChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public float ScrollSpeed { get => this._scrollSpeed; set => this._scrollSpeed = value; }

		public ShowScrollBarOption ShowHorizontal { get => this._showHorizontal; set => this._showHorizontal = value; }
		public ShowScrollBarOption ShowVertical { get => this._showVertical; set => this._showVertical = value; }

		public ScrollBarH ScrollBarH {
			get => this._scrollBarH;
			set {
				if (this._scrollBarH == value)
					return;

				if (!(this._scrollBarH is null))
					this._scrollBarH.OnScrollValueChanged -= this.ScrollBarH_OnScrollValueChanged;

				this._scrollBarH = value;

				if (!(this._scrollBarH is null))
					this._scrollBarH.OnScrollValueChanged += this.ScrollBarH_OnScrollValueChanged;
			}
		}

		public ScrollBarV ScrollBarV {
			get => this._scrollBarV;
			set {
				if (this._scrollBarV == value)
					return;

				if (!(this._scrollBarV is null))
					this._scrollBarV.OnScrollValueChanged -= this.ScrollBarV_OnScrollValueChanged;

				this._scrollBarV = value;

				if (!(this._scrollBarV is null))
					this._scrollBarV.OnScrollValueChanged += this.ScrollBarV_OnScrollValueChanged;
			}
		}

		public event TypedEventHandler<IScrollable, EventArgs> OnVisiblePercentHChanged;
		public event TypedEventHandler<IScrollable, EventArgs> OnVisiblePercentVChanged;
		public event TypedEventHandler<IScrollable, EventArgs> OnScrollValueHChanged;
		public event TypedEventHandler<IScrollable, EventArgs> OnScrollValueVChanged;

		public ScrollablePanel() {
			this.panelInner = new Panel();
			this.panelInner.Location = new Point(0, 0);
			this.panelInner.Size = this.Size;
			this.Controls.Add(this.panelInner);

			this.ControlAdded += this.ScrollablePanel_ControlAdded;
			this.ControlRemoved += this.ScrollablePanel_ControlRemoved;
			this.Resize += this.ScrollablePanel_Resize;
			this.MouseWheel += this.ScrollablePanel_MouseWheel;
		}

		protected override void OnSizeChanged(EventArgs e) {
			this.RecalculateSize();
			this.SetPanelInnerByScrollValues();
		}

		private void RecalculateSize() {
			int width = this.Width;
			int height = this.Height;

			foreach (Control c in this.panelInner.Controls) {
				if (c.Dock == DockStyle.Fill)
					continue;

				if (c.Dock != DockStyle.Top && c.Dock != DockStyle.Bottom) {
					int w = c.Location.X + c.Width;

					if (w > width)
						width = w;
				}

				if (c.Dock != DockStyle.Left && c.Dock != DockStyle.Right) {
					int h = c.Location.Y + c.Height;

					if (h > height)
						height = h;
				}
			}

			if (this.panelInner.Size.Width != width || this.panelInner.Size.Height != height) {
				this.panelInner.Size = new Size(width, height);
				this.SetVisiblePercentByPanelInner();
			}
		}

		private void SetVisiblePercentByPanelInner() {
			this._visiblePercentH = this.panelInner.Width <= 0 ? 1.0f : this.Width / (float)this.panelInner.Width;
			this._visiblePercentV = this.panelInner.Height <= 0 ? 1.0f : this.Height / (float)this.panelInner.Height;

			if (!(this._scrollBarH is null)) {
				bool value = false;

				switch (this._showHorizontal) {
					case ShowScrollBarOption.Never:
						value = false;
						break;
					case ShowScrollBarOption.Always:
						value = true;
						break;
					case ShowScrollBarOption.OnOverflow:
						value = this._visiblePercentH < 1.0f;
						break;
				}

				this._scrollBarH.Visible = value;
				this._scrollBarH.FindForm()?.Refresh();
				System.Diagnostics.Trace.WriteLine("ScrollbarH set to: " + value);
			}

			if (!(this._scrollBarV is null)) {
				bool value = false;

				switch (this._showVertical) {
					case ShowScrollBarOption.Never:
						value = false;
						break;
					case ShowScrollBarOption.Always:
						value = true;
						break;
					case ShowScrollBarOption.OnOverflow:
						value = this._visiblePercentV < 1.0f;
						break;
				}

				this._scrollBarV.Visible = value;
				this._scrollBarV.FindForm()?.Refresh();
				System.Diagnostics.Trace.WriteLine("ScrollbarV set to: " + this._scrollBarV.Visible);
			}

			this.RecalculateSize();

			System.Diagnostics.Trace.WriteLine(this._visiblePercentH);

			this.OnVisiblePercentHChanged?.Invoke(this, EventArgs.Empty);
			this.OnVisiblePercentVChanged?.Invoke(this, EventArgs.Empty);
		}

		private void SetScrollValuesByPanelInner() {
			int maxMoveAreaH = this.panelInner.Width - this.Width;
			int maxMoveAreaV = this.panelInner.Height - this.Height;

			this._scrollValueH = maxMoveAreaH == 0 ? 0.0f : Math.Abs(this.panelInner.Location.X) / (float)maxMoveAreaH;
			this._scrollValueV = maxMoveAreaV == 0 ? 0.0f : Math.Abs(this.panelInner.Location.Y) / (float)maxMoveAreaV;
		}

		private void SetPanelInnerByScrollValues() {
			int maxMoveAreaH = this.panelInner.Width - this.Width;
			int maxMoveAreaV = this.panelInner.Height - this.Height;

			this.panelInner.Location = new Point(
				maxMoveAreaH <= 0 ? 0 : -(int)(maxMoveAreaH * this._scrollValueH),
				maxMoveAreaV <= 0 ? 0 : -(int)(maxMoveAreaV * this._scrollValueV)
			);
		}

		private void ScrollablePanel_MouseWheel(object sender, MouseEventArgs e) {
			if (this._visiblePercentV >= 1.0f)
				return;

			int totalWheelDelta = (int)(e.Delta * this._scrollSpeed);
			int newY = this.panelInner.Location.Y + totalWheelDelta;

			if (newY > 0)
				this.panelInner.Location = new Point(this.panelInner.Location.X, 0);
			else if (newY + this.panelInner.Height < this.Height)
				this.panelInner.Location = new Point(this.panelInner.Location.X, this.Height - this.panelInner.Height);
			else
				this.panelInner.Location = new Point(this.panelInner.Location.X, newY);

			this.SetScrollValuesByPanelInner();
			this.OnScrollValueHChanged?.Invoke(this, EventArgs.Empty);
			this.OnScrollValueVChanged?.Invoke(this, EventArgs.Empty);
		}

		private void ScrollBarH_OnScrollValueChanged(IScrollBar sender, EventArgs e) {
			if (sender is null)
				return;

			this._scrollValueH = sender.ScrollValue;
			this.SetPanelInnerByScrollValues();
		}

		private void ScrollBarV_OnScrollValueChanged(IScrollBar sender, EventArgs e) {
			if (sender is null)
				return;
			
			this._scrollValueV = sender.ScrollValue;
			this.SetPanelInnerByScrollValues();
		}

		private void ScrollablePanel_Resize(object sender, EventArgs e) {
			this.RecalculateSize();
			this.SetPanelInnerByScrollValues();
		}

		private void ScrollablePanel_ControlAdded(object sender, ControlEventArgs e) {
			this.RecalculateSize();
			this.SetPanelInnerByScrollValues();
		}

		private void ScrollablePanel_ControlRemoved(object sender, ControlEventArgs e) {
			this.RecalculateSize();
			this.SetPanelInnerByScrollValues();
		}
	}

	public class ScrollablePanelControlCollection : Control.ControlCollection {
		private Panel container;

		public ScrollablePanelControlCollection(Control owner, Panel container) : base(owner) {
			this.container = container;
		}

		public override void Add(Control value) {
			if (this.container == value)
				base.Add(value);
			else
				this.container.Controls.Add(value);
		}

		public override void Remove(Control value) {
			if (this.container == value)
				base.Remove(value);
			else
				this.container.Controls.Remove(value);
		}

		public override void Clear() {
			this.container?.Invoke(new Action(() => {
				for (int i = this.container.Controls.Count - 1; i >= 0; i--) {
					Control c = this.container.Controls[i];
					this.container.Controls.Remove(c);
					c.Dispose();
				}

				this.container.Controls.Clear();
			}));
		}

		public override IEnumerator GetEnumerator() {
			return this.container?.Controls.GetEnumerator();
		}
	}
}