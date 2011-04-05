namespace Telligent.Evolution.TableOfContents
{
	public class HtmlStripper : IHtmlStripper
	{
		public string RemoveHtml(string html)
		{
			return CommunityServer.Components.Formatter.RemoveHtml(html);
		}
	}
}
