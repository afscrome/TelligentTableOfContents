using System;
using System.Text.RegularExpressions;

namespace AlexCrome.Telligent.TableOfContents.Tests.Mocks
{
	public class DummyHtmlStripper : IHtmlStripper
	{
		private static readonly Regex HtmlTagRegex = new Regex("<[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public string RemoveHtml(string html)
            => HtmlTagRegex.Replace(html, string.Empty);
	}
}
