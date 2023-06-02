using Godot;

using Nekos.Drawing;
using Nekos.Interfaces.Drawing;


namespace Nekos.Autoload{
  public partial class BrushData: Node{
    public IBrush GetBrush(){
      return new GenericBrush();
    }
  }
}