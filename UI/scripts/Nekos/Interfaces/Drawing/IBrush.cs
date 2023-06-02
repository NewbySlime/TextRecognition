using Godot;

using Nekos.Drawing;


namespace Nekos.Interfaces.Drawing{
  public interface IBrush{
    public Vector2 BrushPosition{
      get;
      set;
    }

    public bool AsEraser{
      get;
      set;
    }

    public void Bind(IDrawable drawable);
    public void Brush(Vector2 pos);
    public void Brush();

    public void SetBrushProperties(BrushProperties properties);
  }
}