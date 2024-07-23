using OpenQA.Selenium;
using SeleniumParser.Delegates;
using SeleniumParser.Driver;
using SeleniumParser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SeleniumParser
{
	public class Parser
	{

		public TypeCommandDelegate OnTypeCommand { get; set; }

		public SendKeysCommandDelegate OnSendKeysCommand { get; set; }

		public ClickCommandDelegate OnClickCommand { get; set; }

		public DoubleClickCommandDelegate OnDoubleClickCommand { get; set; }

		public void ParseTests(string sideFile, IWebDriver driver)
		{
			var tests = ConvertSideFileToModel(sideFile);
			foreach (var test in tests.Tests)
			{
				var context = CreateContext(driver);
				foreach (var command in test.Commands)
					PerformCommand(tests, context, test, command);
			}
		}

		private Context CreateContext(IWebDriver driver)
		{
			var context = new Context(driver);
			AddContextEvent(context, OnTypeCommand);
			AddContextEvent(context, OnSendKeysCommand);
			AddContextEvent(context, OnClickCommand);
			AddContextEvent(context, OnDoubleClickCommand);
			return context;
		}

		private void AddContextEvent<T>(Context context, T onCommand) where T : Delegate
		{
			if (onCommand != null)
				context.Events.Add(typeof(T), onCommand);
		}

		private void ApplyVariables(SeleniumCommandModel command)
		{
            command.Value = ReplaceVariables(command.Value, command.Variables);
            command.Target = ReplaceVariables(command.Target, command.Variables);
			if(command.Targets != null)
			{
                List<string[]> targets = new List<string[]>();
                foreach (var t in command.Targets)
                {
                    List<string> ts = new List<string>();
                    for (var i = 0; i < t.Length; i++)
                    {
						ts.Add(ReplaceVariables(t[i], command.Variables));
                    }
					targets.Add(ts.ToArray());
                }
                command.Targets = targets;
            }
        }
        private string ReplaceVariables(string text, IDictionary<string,object> variables)
		{
			if (string.IsNullOrEmpty(text))
				return text;

            var regEx = "\\${[\\w;.;-;_]*}";
			foreach(Match match in Regex.Matches(text, regEx))
			{
				var key = match.Value.Replace("${", "").Replace("}", "");
				if (variables.ContainsKey(key))
				{
					text = text.Replace(match.Value, variables[key].ToString());
                }
			}
			return text;
        }

		private void PerformCommand(SeleniumSideModel tests, Context context, SeleniumTestModel test, SeleniumCommandModel command)
		{
			command.Variables = context.Variables;
			ApplyVariables(command);
            var current = CommandFactory.Create(context, command.Command);
			if (!(current is INextCommand))
				current.PerformInternal(tests, test, command);
			if (context.LastCommand is INextCommand)
				context.LastCommand.PerformInternal(tests, test, command);
			context.LastCommand = current;
            context.Variables = command.Variables;

        }

		public void ParseOneTestByBrowserInstance(string sideFile, Func<IWebDriver> driverConstructor)
		{
			var tests = ConvertSideFileToModel(sideFile);
			foreach (var test in tests.Tests)
			{
				using (var driver = driverConstructor())
				{
					var context = CreateContext(driver);
					foreach (var command in test.Commands)
						PerformCommand(tests, context, test, command);
				}
			}
		}

		private SeleniumSideModel ConvertSideFileToModel(string sideFile)
		{
			using (var arquivo = new StreamReader(sideFile))
				return Newtonsoft.Json.JsonConvert.DeserializeObject<SeleniumSideModel>(arquivo.ReadToEnd());
		}

		public void ParseAllTestsOnSameBrowserInstance(string sideFile, Func<IWebDriver> driverConstructor, Dictionary<string, object> inputParameters = null)
		{
			var tests = ConvertSideFileToModel(sideFile);
			using (var driver = driverConstructor())
			{
				var context = CreateContext(driver);
				context.Variables = inputParameters?? new Dictionary<string, object>();

                foreach (var test in tests.Tests)
				{
					foreach (var command in test.Commands)
						if(!command.Command.StartsWith("//"))
							PerformCommand(tests, context, test, command);
				}
			}
		}



	}
}
