using Microsoft.AspNetCore.SignalR;
namespace SignalRWebpack.Hubs;
public class UserTracker
{
    private readonly Dictionary<string, string> _userConnections = new Dictionary<string, string>();

    public void AddConnection(string userId, string connectionId)
    {
        lock (_userConnections)
        {
            _userConnections[connectionId] = userId;
        }
    }

    public void RemoveConnection(string connectionId)
    {
        lock (_userConnections)
        {
            if (_userConnections.ContainsKey(connectionId))
            {
                _userConnections.Remove(connectionId);
            }
        }
    }

    public List<string> GetConnectedUsers()
    {
        lock (_userConnections)
        {
            return _userConnections.Values.ToList();
        }
    }
}