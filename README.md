# AFRICASTALKING GATEWAY FOR .NET 3.5
	-This is a simple API class I rewrote with reference from AfricasTalking Class
	-to downgrade from .NET 4.5.2 to .NET 3.5.
	-I wrote it in order to be able to use their services in lower .NET versions.

## It makes use of:
	-C#
	
## Reference used while downgrading:
	-Africa's Talking Bulky SMS API
	
## To use the class install this references to your project:
	-System.Web.Extensions
	-System.Web
	-System.Net.Http
	-System.Net.Http.Formatting
	-Newtonsoft from Nuget
	
## Parameters required to call the class

```c#
	string username = "The one you registered with";
	string apiKey = "Your api key";
	string recipients = "+254phonenumber,+254....";
	string message = "Message Here!!";
	//put from if you using custom alpahnumeric id
	string from = "Your custom sender id";
	AfricasTalkingGateway gateway = new AfricasTalkingGateway(username, apiKey);
	try
		{
			dynamic results = gateway.sendMessage(recipients, message);
			//dynamic results = gateway.sendMessage(recipients, message) if you have sender-id
			}
	catch (AfricasTalkingGatewayException ex)
		{
			Console.WriteLine("Encountered an error: " + ex.Message);
			}
```

## 2017