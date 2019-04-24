using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{

    public string IPAdrr; //In Unity eingeben
    public int connectionId;
    public byte error;
    public int hostId;
    public int reliableCh;
    public int unreliableCh;
    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }

    public void Init()
    {
        NetworkTransport.Init();

        GlobalConfig gc = new GlobalConfig();
        gc.MaxPacketSize = 500;
        NetworkTransport.Init(gc);

        ConnectionConfig config = new ConnectionConfig();
        reliableCh = config.AddChannel(QosType.Reliable);
        unreliableCh = config.AddChannel(QosType.Unreliable);

        HostTopology topology = new HostTopology(config, 10);

        hostId = NetworkTransport.AddHost(topology, 8888);

        connectionId = NetworkTransport.Connect(hostId, IPAdrr, 8888, 0, out error);

        if ((NetworkError)error == NetworkError.Ok)
        {
            Debug.Log("cool (Verbindung erfolgreich)");
        }
        else
        {
            Debug.Log("not cool (keine Verbindung gefunden)");
        }
    }

    public void Send(byte[] send, byte reliable0fast1)
    {
        int bufferSize = send.Length;

        if (reliable0fast1 == 0)
        {
            NetworkTransport.Send(hostId, connectionId, reliableCh, send, bufferSize, out error);
        }
        else if (reliable0fast1 == 1)
        {
            NetworkTransport.Send(hostId, connectionId, unreliableCh, send, bufferSize, out error);
        }
        else
        {
            Debug.Log("Nur 1 oder 0 beim Senden benutzen");
        }



        if ((NetworkError)error != NetworkError.Ok)
        {
            Debug.Log("not cool (Fehler beim Senden)");
        }
    }

    void Update()
    {

        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.Nothing: break;
            case NetworkEventType.ConnectEvent: break;
            case NetworkEventType.DataEvent: break;
            case NetworkEventType.DisconnectEvent: break;

            case NetworkEventType.BroadcastEvent:

                break;
        }
    }

    public void Disconect()
    {
        NetworkTransport.Disconnect(hostId, connectionId, out error);

        if ((NetworkError)error == NetworkError.Ok)
        {
            Debug.Log("cool (erfolgreich abgebaut)");
        }
        else
        {
            Debug.Log("not cool (Fehler beim Verbindung trennen)");
        }
    }
}
