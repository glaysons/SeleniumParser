﻿using FluentAssertions;
using SeleniumParser.Driver;
using SeleniumParser.Models;

namespace SeleniumParser.Commands
{
	public class ClearCommand : Command
	{
		public override void Perform(SeleniumSideModel tests, SeleniumTestModel test, SeleniumCommandModel command)
		{
			var element = SearchElement(command);

			element
				.Should()
				.NotBeNull();

			element.Clear();
		}
	}
}
