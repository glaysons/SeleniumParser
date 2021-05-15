using System;

namespace SeleniumParser.Driver
{
	public static class CommandFactory
	{

		public static ICommand Criar(Context context, string command)
		{
			var commandType = "SeleniumParser.Commands." + command + "Command";
			if (!(typeof(CommandFactory).Assembly.CreateInstance(commandType, ignoreCase: true) is Command comando))
				throw new NotImplementedException(command);
			comando.Current = context;
			return comando;
		}

	}
}
