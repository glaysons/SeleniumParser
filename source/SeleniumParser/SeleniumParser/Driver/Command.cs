using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumParser.Models;
using System;
using System.Linq;
using System.Text;

namespace SeleniumParser.Driver
{
	public abstract class Command : ICommand
	{

		public Context Current { get; set; }

		public abstract void Perform(SeleniumSideModel testes, SeleniumTestModel teste, SeleniumCommandModel comando);

		protected IWebElement SearchElement(SeleniumCommandModel sender)
		{
			foreach (var item in sender.Targets)
			{
				if ((item.Length < 2) || !item[0].ContainsText("="))
					continue;

				var element = SearchWebElement(item);

				if (element != null)
					return element;
			}

			var message = CreateMessage(sender);
			throw new Exception(message.ToString());
		}

		private IWebElement SearchWebElement(string[] item)
		{
			var tipo = item[1];
			var valor = item[0].Split('=')[1];

			if (tipo.IsEquals("id"))
				return Current.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id(valor)));

			if (tipo.IsEquals("name"))
				return Current.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Name(valor)));

			if (tipo.IsEquals("css:finder"))
				return Current.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector(valor)));

			if (tipo.IsEquals("xpath:attributes"))
				return Current.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(valor)));

			if (tipo.IsEquals("xpath:idRelative"))
				return Current.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(valor)));

			if (tipo.IsEquals("xpath:position"))
				return Current.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(valor)));

			return null;
		}

		private StringBuilder CreateMessage(SeleniumCommandModel sender)
		{
			var message = new StringBuilder();

			message.Append("Could not find component: ");
			message.Append(sender.Targets.FirstOrDefault());

			if (!string.IsNullOrEmpty(sender.Comment))
			{
				message.AppendLine();
				message.Append(sender.Comment);
			}

			return message;
		}

		protected void Wait(double delay)
		{
			var now = DateTime.Now;
			var wait = new WebDriverWait(Current.Driver, TimeSpan.FromMilliseconds(delay));
			wait.PollingInterval = TimeSpan.FromMilliseconds(1000);
			wait.Until(wd => (DateTime.Now - now) - TimeSpan.FromMilliseconds(delay) > TimeSpan.Zero);
		}

	}
}
