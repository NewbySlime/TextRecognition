using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;


namespace Nekos.Connection{
  public class ConnectionServer{
    private Socket _serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
    private Socket _clientConn = null;
    private Task _listenerThread = null;

    public event EventHandler<(ReadOnlyCollection<byte>, int)> OnReceiveData;
    public event EventHandler<Exception> OnError;


    public void StartListen(ushort ConnectionPort){
      if(_listenerThread == null){
        _listenerThread = Task.Run(async () => {
          IPEndPoint endPoint = new IPEndPoint(new IPAddress(new byte[]{172, 0, 0, 1}), ConnectionPort);

          _serverSocket.Bind(endPoint);
          _serverSocket.Listen();

          byte[] buffer = new byte[1024];

          try{
            _clientConn = await _serverSocket.AcceptAsync();

            while(true){
              int recv = await _serverSocket.ReceiveAsync(buffer, SocketFlags.None);
              OnReceiveData?.Invoke(this, (Array.AsReadOnly<byte>(buffer), recv));
            }
          }
          catch(Exception e){
            if(e is not ObjectDisposedException)
              OnError?.Invoke(this, e);
          }

          _clientConn = null;
          _listenerThread = null;
        });
      }
    }

    public Task StopListen(){
      Task _listener = _listenerThread;
      _serverSocket.Close();

      return _listener;
    }

    public async Task<int> SendData(byte[] data){
      int _res = 0;

      try{
        if(_clientConn != null)
          await _clientConn.SendAsync(data, SocketFlags.None);
      }
      catch(Exception){
        _res = -1;
      }

      return _res;
    }
  }
}