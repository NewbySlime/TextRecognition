using Godot;
using System;


namespace Nekos.Interfaces.Drawing{
  public interface IDrawable: IDisposable{
    public Vector2 CurrentGlobalPosition{
      get;
    }

    public Vector2 CurrentGlobalSize{
      get;
    }


    public delegate void GetPixelCallback(ref Color color);

    public void SetResolution((int, int) resolution);
    public (int, int) GetResolution();

    public void DoUpdate();

    public void ModifyPixel((int, int) position, GetPixelCallback callback);
  }
}