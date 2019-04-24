using System;
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
    //angekommende Daten; über Funktion abholen
    public Boolean hasMissedData;
    private byte[] incData = new byte[0];
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

    public void Send(byte[] send, Boolean reliable)
    {
        int bufferSize = send.Length;

        if (reliable)
        {
            NetworkTransport.Send(hostId, connectionId, reliableCh, send, bufferSize, out error);
        }
        else
        {
            NetworkTransport.Send(hostId, connectionId, unreliableCh, send, bufferSize, out error);
        }

        if ((NetworkError)error != NetworkError.Ok)
        {
            Debug.Log("not cool (Fehler beim Senden)");
        }
    }


    /*
     * @return true falls Daten nicht rechtzeitig abgeholt wurden
     */
    public Boolean getRecData(out byte[] recData)
    {
        recData = incData;
        incData = new byte[0];
        Boolean returnB = false;
        if (hasMissedData)
        {
            returnB = true;
            hasMissedData = false;
        }
        return returnB;
    }

    void Update()
    {

        int recHostId;
        int otherConnectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out otherConnectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.Nothing: break;
            case NetworkEventType.ConnectEvent:
                if (otherConnectionId == connectionId)
                { 
                    Debug.Log("cool (Verbinden erfolgreich)");
                }
                else
                {
                    Debug.Log("not cool (Verbinden nicht erfolgreich)");
                }
            break;
            case NetworkEventType.DataEvent:
                if (incData.Length > 0)
                {
                    hasMissedData = true;
                }
                incData = recBuffer;
                if((NetworkError) error == NetworkError.MessageToLong)
                {
                    Debug.Log("not cool (erhaltene Nachricht zu lang)");
                }
                break;
            case NetworkEventType.DisconnectEvent:
                if (otherConnectionId == connectionId)
                {
                    Debug.Log("cool (Verbindung erfolgreich aufgelöst)");
                }
                else
                {
                    Debug.Log("not cool (Verbindung nicht erfolgreich aufgelöst)");
                }
                break;
            case NetworkEventType.BroadcastEvent:
                Debug.Log("BroadcastEvent (wurde ignoriert)");
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
