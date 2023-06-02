using Godot;
using System.Diagnostics;

using Nekos.Connection;


namespace Nekos.Autoload{
  public partial class AIAutoload: Node{
    private const string _ai_programPath = "";
    private const ushort _ai_programPort = 4050;

    private Process _aiProcess = new Process();
    private ConnectionServer _connServer = new ConnectionServer();


    public override void _Ready(){
      ProcessStartInfo _startInfo = new ProcessStartInfo(_ai_programPath);
      _startInfo.CreateNoWindow = true;

      _startInfo.ArgumentList.Add(string.Format("port={0}", _ai_programPort));

      _aiProcess.StartInfo = _startInfo;
      _aiProcess.Start();
    }
  }
}