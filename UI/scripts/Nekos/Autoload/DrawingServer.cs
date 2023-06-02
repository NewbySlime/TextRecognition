using Godot;
using Godot.Collections;

using Nekos.Drawing;
using Nekos.Interfaces.Drawing;


namespace Nekos.Autoload{
	public partial class DrawingServer: Node{
		private const double _updateInterval = 0.1;

		public enum DrawableTypeEnum{
			Generic
		}


		[Export]
		private PackedScene _scene_genericDrawable;


		private Dictionary<int, GenericDrawable> _currentDrawable = new();
		private Timer _updateTimer = new Timer();
		
		private void _onUpdateInterval(){
			foreach(var pair in _currentDrawable)
				pair.Value.DoUpdate();
		}


		public DrawingServer(){
			_updateTimer.Timeout += _onUpdateInterval;
		}

		~DrawingServer(){
			_updateTimer.Timeout -= _onUpdateInterval;
		}


		public override void _Ready(){
      AddChild(_updateTimer);

			_updateTimer.OneShot = false;
			_updateTimer.Start(_updateInterval);
		}


		public IDrawable CreateDrawable(Node2D Parent, DrawableTypeEnum type){
			switch(type){
				case DrawableTypeEnum.Generic:{
					GenericDrawable _canvas = _scene_genericDrawable.Instantiate<GenericDrawable>();
					Parent.AddChild(_canvas);

					_currentDrawable[_canvas.GetHashCode()] = _canvas;

					return _canvas;
				}
			}

			return null;
		}
	}
}
