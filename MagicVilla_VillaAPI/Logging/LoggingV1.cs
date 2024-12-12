namespace MagicVilla_VillaAPI.Logging
{
	public class LoggingV1:ILogging
	{
		public void Log(string message, string type)
		{
			if (type == "error")
			{
				Console.BackgroundColor = ConsoleColor.Red;
				Console.WriteLine("ERORR - " + message);
				Console.ForegroundColor = ConsoleColor.Black;
			}
			else
			{
				if (type == "warning")
				{ 
					Console.BackgroundColor = ConsoleColor.Red;
				Console.WriteLine("ERORR - " + message);
				Console.ForegroundColor = ConsoleColor.Black;
				}
				else
				{
					Console.WriteLine(message);
				}
			}
			
		}
	}
}
