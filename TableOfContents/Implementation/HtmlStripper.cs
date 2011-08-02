namespace Telligent.Evolution.Extensions.TableOfContents
{
	public class HtmlStripper : IHtmlStripper
	{
		public string RemoveHtml(string html)
		{
			return Telligent.Evolution.Components.Formatter.RemoveHtml(html);
		}
	}
}
