using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Debug_Module.Tools.ControlScreenshot {
    public class HighlightControl : Control {

        private int _topHeight    = 0;
        private int _rightWidth   = 0;
        private int _bottomHeight = 0;
        private int _leftWidth    = 0;

        public int TopHeight {
            get => _topHeight;
            set => SetProperty(ref _topHeight, value, true);
        }

        public int RightWidth {
            get => _topHeight;
            set => SetProperty(ref _rightWidth, value, true);
        }

        public int BottomHeight {
            get => _topHeight;
            set => SetProperty(ref _bottomHeight, value, true);
        }

        public int LeftWidth {
            get => _topHeight;
            set => SetProperty(ref _leftWidth, value, true);
        }

        public HighlightControl() {
            this.ZIndex = Int32.MaxValue;
        }

        /// <inheritdoc />
        protected override CaptureType CapturesInput() {
            return CaptureType.None;
        }

        private Rectangle _layoutTopBounds;
        private Rectangle _layoutRightBounds;
        private Rectangle _layoutBottomBounds;
        private Rectangle _layoutLeftBounds;

        /// <inheritdoc />
        public override void RecalculateLayout() {
            Size = Graphics.SpriteScreen.Size;

            _layoutTopBounds = new Rectangle(0, 0, Graphics.SpriteScreen.Width, _topHeight);
            _layoutRightBounds = new Rectangle(Graphics.SpriteScreen.Width - _rightWidth, _topHeight, _rightWidth, Graphics.SpriteScreen.Height - _topHeight - _bottomHeight);
            _layoutBottomBounds = new Rectangle(0, Graphics.SpriteScreen.Height - _bottomHeight, Graphics.SpriteScreen.Width, _bottomHeight);
            _layoutLeftBounds = new Rectangle(0, _topHeight, _leftWidth, Graphics.SpriteScreen.Height - _topHeight - _bottomHeight);
        }

        public void Show(Control control) {
            if (control == null) {
                this.Hide();
                return;
            }

            this.Show(control.AbsoluteBounds.WithPadding(control.Padding));
        }

        public void Show(Rectangle bounds) {
            Animation.Tweener.Tween(this, new {
                TopHeight    = bounds.Top,
                RightWidth   = _size.X - bounds.Right,
                BottomHeight = _size.Y - bounds.Bottom,
                LeftWidth    = bounds.Left
            }, 0.3f);

            base.Show();
        }

        /// <inheritdoc />
        protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds) {
            spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _layoutTopBounds,    Color.Black * 0.6f);
            spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _layoutRightBounds,  Color.Black * 0.6f);
            spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _layoutBottomBounds, Color.Black * 0.6f);
            spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _layoutLeftBounds,   Color.Black * 0.6f);
        }

    }
}
