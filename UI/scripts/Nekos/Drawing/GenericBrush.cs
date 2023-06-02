using Godot;

using Nekos.Interfaces.Drawing;

namespace Nekos.Drawing{
  public partial class GenericBrush: Marker2D, IBrush{
    private readonly double _pt_maxDeltaRange = Mathf.Sqrt(Mathf.Pow(1, 2) + Mathf.Pow(1, 2)) - 1;


    private Vector2 _currentPosition;
    private IDrawable _currentCanvas;

    private double _radius = 2;
    private bool _asEraser = false;




    public Vector2 BrushPosition{
      get{
        return _currentPosition;
      }

      set{
        _currentPosition = value;
      }
    }

    public bool AsEraser{
      get{
        return _asEraser;
      }

      set{
        _asEraser = value;
      }
    }



    public void Bind(IDrawable drawable){
      _currentCanvas = drawable;
    }

    public void Brush(Vector2 _o_pos){
      var _pair_res = _currentCanvas.GetResolution();
      _o_pos = _o_pos / _currentCanvas.CurrentGlobalSize * new Vector2(_pair_res.Item1, _pair_res.Item2);

      for(int i_x = 0; i_x <= _radius*2; i_x++){
        for(int i_y = 0; i_y <= _radius*2; i_y++){
          int _x = Mathf.RoundToInt(_o_pos.X + (-_radius + i_x));
          int _y = Mathf.RoundToInt(_o_pos.Y + (-_radius + i_y));
          Vector2 _pt_pos = new Vector2(_x, _y);

          double _range = _o_pos.DistanceTo(_pt_pos);

          Vector2 _dir = _o_pos.DirectionTo(_pt_pos);
          double _angle = Mathf.Atan2(_dir.Y, _dir.X);
          double _pt_range = Mathf.Abs(Mathf.Sin(_angle*2)) * _pt_maxDeltaRange + 1;

          double _var1 = _range - (_radius + _pt_range);
          if(_var1 < 0){
            double _mult = Mathf.Clamp(Mathf.Abs(_var1)/(_pt_range*2), 0, 1);

            if(_asEraser)
              _mult = Mathf.Abs(_mult-1);

            _currentCanvas.ModifyPixel((_x, _y), (ref Color _col) => {
              double _gray;

              if(_asEraser)
                _gray = Mathf.Min(_mult, _col.R);
              else
                _gray = Mathf.Max(_mult, _col.R);
                
              _col.R = (float)_gray;
              _col.G = (float)_gray;
              _col.B = (float)_gray;
              _col.A = (float)1.0;
            });
          }
        }
      }
    }

    public void Brush(){
      Brush(_currentPosition);
    }


    public void SetBrushProperties(BrushProperties properties){

    }
  }
}