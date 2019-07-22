using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Pathing.Behaviors;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using ContextMenuStrip = Blish_HUD.Controls.ContextMenuStrip;
using Control = Blish_HUD.Controls.Control;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Debug_Module {

    [Export(typeof(Module))]
    public class DebugModule : Module {

        /* TODO:
         * - Make each tool an actual Tool class instance so that they can be loaded, enabled/disabled, etc. in a less spaghetti way.
         * - Make control selector tween from last position instead of jutting out like it currently is.
         */

        internal static DebugModule ModuleInstance;

        // Service Managers
        internal SettingsManager    SettingsManager    => this.ModuleParameters.SettingsManager;
        internal ContentsManager    ContentsManager    => this.ModuleParameters.ContentsManager;
        internal DirectoriesManager DirectoriesManager => this.ModuleParameters.DirectoriesManager;
        internal Gw2ApiManager      Gw2ApiManager      => this.ModuleParameters.Gw2ApiManager;

        // Settings
        private SettingEntry<bool> _showToolbox;

        // Controls (be sure to dispose of these in Unload()
        private CornerIcon       _debugIcon;
        private ContextMenuStrip _debugContextMenuStrip;

        private RuntimeToolbox _toolboxForm;

        private Tools.ControlScreenshot.HighlightControl _controlHighlighter;

        private bool _screenshotPickerActive    = false;
        private bool _magentaPickerActive       = false;
        private bool _runtimeViewerPickerActive = false;

        [ImportingConstructor]
        public DebugModule([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters) {
            ModuleInstance = this;
        }

        /// <inheritdoc />
        protected override void DefineSettings(SettingCollection settings) {
            _showToolbox = settings.DefineSetting("ShowToolbox", false);
        }

        protected override void Initialize() {
            
        }

        protected override async Task LoadAsync() {
            _debugIcon = new CornerIcon() {
                Icon             = ContentsManager.GetTexture(@"textures\156736.png"),
                BasicTooltipText = "Debug"
            };

            _debugContextMenuStrip = new ContextMenuStrip();

            var screenshotAction = _debugContextMenuStrip.AddMenuItem("Take Screenshot of Control");
            var magentaAction    = _debugContextMenuStrip.AddMenuItem("Mark Control Bounds");
            var forceGcAction    = _debugContextMenuStrip.AddMenuItem("Force GC");
            var showToolbox      = _debugContextMenuStrip.AddMenuItem("Show Toolbox");

            forceGcAction.Enabled = false;

            showToolbox.CanCheck = true;

            _controlHighlighter = new Tools.ControlScreenshot.HighlightControl() {Visible = false};
            _controlHighlighter.Parent = GameService.Graphics.SpriteScreen;

            screenshotAction.Click += delegate { _screenshotPickerActive    = true; };
            magentaAction.Click    += delegate { _magentaPickerActive = true; };

            forceGcAction.Click += delegate {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            };

            showToolbox.CheckedChanged += delegate {
                if (showToolbox.Checked) {
                    if (_toolboxForm == null || _toolboxForm.IsDisposed) {
                        _toolboxForm = new RuntimeToolbox();
                        _toolboxForm.FormClosed += delegate { _showToolbox.Value = false; };
                    }
                    _toolboxForm.Show();
                } else {
                    _toolboxForm.Hide();
                }

                _showToolbox.Value = showToolbox.Checked;
            };

            // Thread safety
            GameService.Overlay.QueueMainThreadUpdate((gameTime) => {
                _showToolbox.SettingChanged += delegate { showToolbox.Checked = _showToolbox.Value; };
                showToolbox.Checked = _showToolbox.Value;
            });

            _debugIcon.Click += delegate {
                _debugContextMenuStrip.Show(_debugIcon);
            };

            Control.ActiveControlChanged               += ControlOnActiveControlChanged;
            GameService.Input.LeftMouseButtonReleased  += InputOnLeftMouseButtonReleased;
            GameService.Input.RightMouseButtonReleased += InputOnRightMouseButtonReleased;
        }

        public void ActivateRuntimeViewerPicker() {
            _runtimeViewerPickerActive = true;
        }

        private void InputOnRightMouseButtonReleased(object sender, Blish_HUD.Input.MouseEventArgs e) {
            _screenshotPickerActive    = false;
            _magentaPickerActive       = false;
            _runtimeViewerPickerActive = false;
            _controlHighlighter.Hide();
        }

        private void InputOnLeftMouseButtonReleased(object sender, Blish_HUD.Input.MouseEventArgs e) {
            if (_screenshotPickerActive && Control.ActiveControl != null) {
                GameService.Graphics.QueueMainThreadRender((graphicsDevice) => {
                    var activeControl = Control.ActiveControl;
                    var absoluteBounds = activeControl.AbsoluteBounds;

                    var bounds = absoluteBounds.WithPadding(activeControl.Padding);

                    var screenshotTarget = new RenderTarget2D(graphicsDevice,
                                                              GameService.Graphics.SpriteScreen.Width,
                                                              GameService.Graphics.SpriteScreen.Height,
                                                              false,
                                                              graphicsDevice.PresentationParameters.BackBufferFormat,
                                                              DepthFormat.Depth24);

                    graphicsDevice.SetRenderTarget(screenshotTarget);
                    graphicsDevice.Clear(Color.Transparent);

                    using (var screenshotSpriteBatch = new SpriteBatch(graphicsDevice)) {
                        //GameService.Graphics.SpriteScreen.Draw(screenshotSpriteBatch, new Rectangle(Point.Zero, GameService.Graphics.SpriteScreen.Size), new Rectangle(Point.Zero, GameService.Graphics.SpriteScreen.Size));
                        activeControl.Draw(screenshotSpriteBatch, new Rectangle(Point.Zero, activeControl.Size), new Rectangle(Point.Zero, GameService.Graphics.SpriteScreen.Size));    
                    }

                    using (var pngStream = new MemoryStream()) {
                        var croppedTarget = screenshotTarget.GetRegion(bounds);

                        croppedTarget.SaveAsPng(pngStream, screenshotTarget.Width, screenshotTarget.Height);
                        pngStream.Seek(0, SeekOrigin.Begin);

                        var sysImg = System.Drawing.Image.FromStream(pngStream);

                        ClipboardUtils.SetClipboardImage(new Bitmap(sysImg), null, null);
                    }

                    ScreenNotification.ShowNotification("Screenshot copied to clipboard!");

                    graphicsDevice.SetRenderTarget(null);
                });

                _screenshotPickerActive = false;
            } else if (_magentaPickerActive && Control.ActiveControl != null) {
                Control.ActiveControl.BackgroundColor = Color.Magenta;

                _magentaPickerActive = false;
            } else if (_runtimeViewerPickerActive && Control.ActiveControl != null) {
                _toolboxForm?.SetActiveControl(Control.ActiveControl);

                _runtimeViewerPickerActive = false;
            }

            _controlHighlighter.Hide();
        }

        private void ControlOnActiveControlChanged(object sender, ControlActivatedEventArgs e) {
            if (_screenshotPickerActive || _magentaPickerActive || _runtimeViewerPickerActive)
                _controlHighlighter.Show(Control.ActiveControl);
        }

        protected override void OnModuleLoaded(EventArgs e) {
            base.OnModuleLoaded(e);
        }

        protected override void Update(GameTime gameTime) {
        
        }

        protected override void Unload() {
            ModuleInstance = null;

            _debugIcon.Dispose();
            _debugContextMenuStrip.Dispose();

            _toolboxForm.Hide();
            _toolboxForm.Dispose();
        }

    }

}
