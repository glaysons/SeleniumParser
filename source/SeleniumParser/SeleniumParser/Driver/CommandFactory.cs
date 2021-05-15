using System;

namespace SeleniumParser.Driver
{
	public static class CommandFactory
	{

		public static ICommand Criar(Context context, string command)
		{
			var tipo = "SeleniumParser.Driver.Commands." + command + "Command";
			if (!(typeof(CommandFactory).Assembly.CreateInstance(tipo, ignoreCase: true) is Command comando))
				throw new NotImplementedException(command);
			comando.Current = context;
			return comando;
		}

	}
}
