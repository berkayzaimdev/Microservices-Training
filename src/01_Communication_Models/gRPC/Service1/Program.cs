using Grpc.Net.Client;
using Service2;

var channel = GrpcChannel.ForAddress("https://localhost:7172");

var greetClient = new Greeter.GreeterClient(channel);

var response = greetClient.SayHello(new HelloRequest { Name = Console.ReadLine() });

await Task.Run(async () =>
{
	while(await response.ResponseStream.MoveNext(new CancellationToken()))
	{
		Console.WriteLine($"Gelen mesaj: {response.ResponseStream.Current.Message}");
	}
});