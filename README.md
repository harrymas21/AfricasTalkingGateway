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
	//put from if you using custom alpahnumeric id otherwise leave blank
	string from = "Your custom sender id";
	AfricasTalkingGateway gateway = new AfricasTalkingGateway(username, apikey);
                try
                {
                    object results = gateway.sendMessage(recepients, message, from);
                }
                catch (AfricasTalkingGatewayException ex)
                {
                    MessageBox.Show(ex.Message, "Encountered an error while sending the message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
```

## 2017
