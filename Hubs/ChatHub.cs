using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using NuGet.Versioning;
using SignalRWebpack.ViewModel;
using System.Linq;
using System.Security.Claims;

namespace SignalRWebpack.Hubs;
public static class UserHandler
{
    public static HashSet<UserInfo> UserConnects = new HashSet<UserInfo>();
}
[Authorize]
public class ChatHub : Hub
{
    private readonly UserTracker _userTracker;

    public string? Email { get; private set; }

    public ChatHub(UserTracker userTracker)
    {
        _userTracker = userTracker;
    }

    public HashSet<UserInfo> GetConnectedUsers()
    {
        return UserHandler.UserConnects;
    }
    public override Task OnConnectedAsync()
    {
        var newConnect = new UserInfo {
            ConnectionId = Context.ConnectionId,
            Email = Context.User.Identity.Name
        };
        var count = 0;
        foreach(var user in UserHandler.UserConnects)
        {
            if(user.Email == newConnect.Email)
            {
                count++;
            }
        }
        if(count == 0)
        {
            UserHandler.UserConnects.Add(newConnect);
            Clients.All.SendAsync("NewUserLoggedIn");
        }
        return base.OnConnectedAsync();
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        //UserHandler.UserConnects.Remove(Context.ConnectionId);
        foreach (var user in UserHandler.UserConnects)
        {
            if (user.Email == Context.User.Identity.Name)
            {
                UserHandler.UserConnects.Remove(user);
            }
        }
        Clients.All.SendAsync("NewUserLoggedIn");
        return base.OnDisconnectedAsync(exception);
    }
    public async Task SendMessage(string username, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", username, message);
    }
}