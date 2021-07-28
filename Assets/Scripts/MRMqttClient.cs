using System.Text;
using M2MqttUnity;

public class MRMqttClient : M2MqttUnityClient
{
    private const string COMMANDS_TOPIC = "commands"; 
    
    public void SendStoreIn(int shelfId)
    {
        var command = Encoding.UTF8.GetBytes($"si{shelfId}");
        client.Publish(COMMANDS_TOPIC, command);
    }
    
    public void SendStoreOut(int shelfId)
    {
        var command = Encoding.UTF8.GetBytes($"so{shelfId}");
        client.Publish(COMMANDS_TOPIC, command);
    }

    private void OnDestroy()
    {
        Disconnect();
    }
}