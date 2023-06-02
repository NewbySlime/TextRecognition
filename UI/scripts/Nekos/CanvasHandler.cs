using Godot;
using System;

using Nekos.Drawing;
using Nekos.Autoload;
using Nekos.Interfaces.Drawing;


namespace Nekos{
	public partial class CanvasHandler: Control{
		private BrushData _brushData;
		private DrawingServer _drawServer;

		private IBrush brush;
		private IDrawable drawable;

		private Node2D _drawableParent;

		private bool _mouse_isEntered = false;
		private bool _mouse_clicked = false;


		private void _onMouseUpdate(){
			if(_mouse_clicked)
				brush.Brush();
		}

		
		public override void _Ready(){
			_brushData = GetNode("/root/BrushData") as BrushData;
			_drawServer = GetNode("/root/DrawingServer") as DrawingServer;

			_drawableParent = GetNode("%DrawingCanvas") as Node2D;
			
			drawable = _drawServer.CreateDrawable(_drawableParent, DrawingServer.DrawableTypeEnum.Generic);
			drawable.SetResolution((28, 28));

			brush = _brushData.GetBrush();
			brush.Bind(drawable);


			Connect("mouse_entered", Callable.From(() => {
				_mouse_isEntered = true;
			}));

			Connect("mouse_exited", Callable.From(() => {
				_mouse_isEntered = false;
			}));
		}

		public override void _Input(InputEvent @event){
			if(@event is InputEventMouseButton){
				InputEventMouseButton _mouseEvent = @event as InputEventMouseButton;
				switch(_mouseEvent.ButtonIndex){
					case MouseButton.Left:{
						if(_mouse_isEntered && _mouseEvent.Pressed)
							_mouse_clicked = true;
						else
							_mouse_clicked = false;

						_onMouseUpdate();

						break;
					}
				}
			}

			if(@event is InputEventMouseMotion){
				InputEventMouseMotion _mouseEvent = @event as InputEventMouseMotion;

				brush.BrushPosition = GetGlobalMousePosition() - GlobalPosition - drawable.CurrentGlobalPosition;
				_onMouseUpdate();
			}

			if(@event is InputEventKey){
				InputEventKey _keyEvent = @event as InputEventKey;

				switch(_keyEvent.KeyLabel){
					case Key.E:{
						if(_keyEvent.Pressed)
							brush.AsEraser = !brush.AsEraser;

						break;
					}
				}
			}
		}
	}
}
