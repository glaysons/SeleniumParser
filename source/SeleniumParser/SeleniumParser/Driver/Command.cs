using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SeleniumParser.Driver
{
	public abstract class Command : ICommand
	{

		public Context Current { get; set; }

		public abstract void Perform(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command);

		public void PerformInternal(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command)
		{
			LoadWindowHandles(command);
			Perform(tests, test, command);
			if (command.OpensWindow)
			{
				command.Variables[command.WindowHandleName] = WaitForWindow(command);
			}
        }


        protected IWebElement SearchElement(SeleniumCommandModel sender)
		{
			IWebElement element = null;

			if (TryGetElement(sender, out element))
				return element;

			var message = CreateMessage(sender);
			throw new Exception(message.ToString());
		}

		protected bool TryGetElement(SeleniumCommandModel sender, out IWebElement el)
		{
			IWebElement element = null;

            var elementFound = Current.Wait.Until(d =>
            {
                foreach (var target in sender.Targets)
                {
                    if (TryGetTargetElement(target, out element) && (element != null))
                        return true;
                }

                if (!string.IsNullOrEmpty(sender.Target))
                {
                    var target = CreateTarget(sender.Target);
                    if ((target != null) && TryGetTargetElement(target, out element) && (element != null))
                        return true;
                }
                return false;
            });

			el = element;
			return elementFound;
        }

        private bool TryGetTargetElement(string[] target, out IWebElement element)
		{
			if ((target.Length < 2) || !target[0].ContainsText("="))
			{
				element = null;
				return false;
			}

			element = SearchWebElement(target);
			return true;
		}

		private IWebElement SearchWebElement(string[] target)
		{
			var targetType = target[1];
			var targetValue = target[0];
            targetValue = targetValue.Replace($"{targetValue.Split('=')[0]}=","");

			switch (targetType)
			{
				case "id":
                    return Current.Driver.FindElement(By.Id(targetValue));
				case "name":
                    return Current.Driver.FindElement(By.Name(targetValue));
				case "css:finder":
                    return Current.Driver.FindElement(By.CssSelector(targetValue));
				case "xpath:attributes":
                    return Current.Driver.FindElement(By.XPath(targetValue));
				case "xpath:idRelative":
                    return Current.Driver.FindElement(By.XPath(targetValue));
				case "xpath:position":
                    return Current.Driver.FindElement(By.XPath(targetValue));
                case "linkText":
                    return Current.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.LinkText(targetValue)));
				default:
                    return null;
            }
        }

		private string[] CreateTarget(string target)
		{
			string targetType = null;

			if (target.StartsWithText("css="))
				targetType = "css:finder";

			else if (target.StartsWithText("id="))
				targetType = "id";

			else if (target.StartsWithText("name="))
				targetType = "name";

			else if(target.StartsWithText("linkText="))
                targetType = "linkText";

            if (targetType == null)
				return null;

			return new[] { target, targetType };
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

        protected void WaitElement(double delay, SeleniumCommandModel sender)
		{
            var now = DateTime.Now;
            var wait = new WebDriverWait(Current.Driver, TimeSpan.FromMilliseconds(delay));
            wait.PollingInterval = TimeSpan.FromMilliseconds(1000);
			wait.Until(wd => TryGetElement(sender, out var element));

        }

        protected void WaitElementVisible(double delay, SeleniumCommandModel sender)
        {
            var now = DateTime.Now;
            var wait = new WebDriverWait(Current.Driver, TimeSpan.FromMilliseconds(delay));
            wait.PollingInterval = TimeSpan.FromMilliseconds(1000);
            wait.Until(wd => TryGetElement(sender, out var element) && element.Displayed);
        }

        protected void LoadWindowHandles(SeleniumCommandModel command)
		{
            command.Variables["WindowHandles"] = Current.Driver.WindowHandles;

        }
        protected string WaitForWindow(SeleniumCommandModel command)
        {
            try
            {
                Thread.Sleep(command.WindowTimeout);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            var whNow = ((IReadOnlyCollection<object>)Current.Driver.WindowHandles).ToList();
            var whThen = ((IReadOnlyCollection<object>)command.Variables["WindowHandles"]).ToList();
            if (whNow.Count > whThen.Count)
            {
                return whNow.Except(whThen).First().ToString();
            }
            else
            {
                return whNow.First().ToString();
            }
        }


        protected T GetCustomEvent<T>() where T : Delegate
		{
			if (Current.Events.TryGetValue(typeof(T), out Delegate customEvent))
				return customEvent as T;
			return null;
		}

	}
}
