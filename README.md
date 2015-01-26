#### Example

```csharp
public class Program
{
    public static void Main()
    {
        var hubConnection = new HubConnection("http://localhost:55555/");
        var hub = hubConnection.CreateHubProxy<IChatHub>("ChatHub");
        var client = new ChatHubClient();
        hubConnection.Start().Wait();

        // Replaces multiple calls to hub.On("NewMessage", ...);
        hub.MapClientMethods<IChatHubClient>(client);

        // Replaces hub.Invoke<string>("GetConnectionId");
        var connectionId = hub.Invoke(h => h.GetConnectionId()).Result;
        Console.WriteLine("Connected with ID: {0}", connectionId);

        Console.Write("What's your name? ");
        var name = Console.ReadLine();
        // Replaces hub.Invoke("Login", name);
        hub.Invoke(h => h.Login(name));

        while (true)
        {
            var message = Console.ReadLine();
            // Replaces hub.Invoke("SendMessage", (int x) => OnProgress(x), message);
            hub.Invoke((h, p) => h.SendMessage(message, p), (int x) => OnProgress(x));
        }
    }

    private static void OnProgress(int progress)
    {
        Console.WriteLine("{0}%", progress);
    }
}

public interface IChatHub
{
    string GetConnectionId();
    void SendMessage(string message, IProgress<int> onProgress);
    void Login(string name);
}

public interface IChatHubClient
{
    void NewMessage(string message);
    void Greet(string name);
}

public class ChatHub : Hub<IChatHubClient>, IChatHub
{
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }

    public void SendMessage(string message, IProgress<int> onProgress)
    {
        for (var i = 1; i <= 10; i++)
        {
            Thread.Sleep(200);
            onProgress.Report(i * 10);
        }
        Clients.All.NewMessage(message);
    }

    public void Login(string name)
    {
        Clients.Caller.Greet(name);
    }
}

public class ChatHubClient : IChatHubClient
{
    public void Greet(string name)
    {
        Console.WriteLine("Hello, {0}!", name);
    }

    public void NewMessage(string message)
    {
        Console.WriteLine("MESSAGE RECEIVED: {0}", message);
    }
}
```
