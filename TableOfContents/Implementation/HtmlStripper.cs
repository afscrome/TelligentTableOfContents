using Telligent.Evolution.Extensibility.Api.Version1;
namespace Telligent.Evolution.Extensions.TableOfContents
{
	public class HtmlStripper : IHtmlStripper
	{
		public string RemoveHtml(string html)
		{
			return PublicApi.Language.RemoveHtml(html);
		}
	}
}
