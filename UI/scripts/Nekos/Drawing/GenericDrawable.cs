using Godot;
using System;

using Nekos.Interfaces.Drawing;


namespace Nekos.Drawing{
	public partial class GenericDrawable: ColorRect, IDrawable{
		private Image _arrayData = new Image();
		private ShaderMaterial shader;

		private (int, int) _resolution = (0, 0);
		private bool _doUpdate = false;

		private Mutex _pixelData_mutex = new Mutex();


		public Vector2 CurrentGlobalPosition{
			get{
				return GlobalPosition;
			}
		}

		public Vector2 CurrentGlobalSize{
			get{
				return Size;
			}
		}



		private void _mutexWrapper(Mutex m, Action callback){
			m.Lock();
			callback.Invoke();
			m.Unlock();
		}

		private void _updateDraw(){
			if(_doUpdate){
				_doUpdate = false;

				_mutexWrapper(_pixelData_mutex, () => {
					ImageTexture img = ImageTexture.CreateFromImage(_arrayData);

					shader.SetShaderParameter("_canvas_data", img);
				});
			}
		}



		public override void _Ready(){
			if(Material is not ShaderMaterial)
				throw new MissingMemberException("Canvas shader is not found or not assigned.");

			shader = Material as ShaderMaterial;

			_doUpdate = true;
			_updateDraw();
		}



		public void SetResolution((int, int) res){
			_mutexWrapper(_pixelData_mutex, () => {
				_resolution = (res.Item1, res.Item2);

				_arrayData = Image.Create(res.Item1, res.Item2, false, Image.Format.Rgb8);

				_doUpdate = true;
				_updateDraw();
			});
		}


		public (int, int) GetResolution(){
			return _resolution;
		}

		public void DoUpdate(){
			_updateDraw();
		}

		// position should be based on this canvas position
		public void ModifyPixel((int, int) position, IDrawable.GetPixelCallback callback){
			if(position.Item1 < 0 || position.Item1 >= _resolution.Item1 || position.Item2 < 0 || position.Item2 >= _resolution.Item2)
				return;
				
			_mutexWrapper(_pixelData_mutex, () => {
				Color _col = _arrayData.GetPixel(position.Item1, position.Item2);
				callback.Invoke(ref _col);
				_arrayData.SetPixel(position.Item1, position.Item2, _col);

				_doUpdate = true;
			});
		}


		public override void _Draw(){
			base._Draw();

			_updateDraw();
		}
	}
} 
