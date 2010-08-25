
namespace Telligent.Evolution.TableOfContents.Tests.Mocks
{
	public class DummyHtmlStripper : IHtmlStripper
	{
		public string RemoveHtml(string html)
		{
			return html;
		}
	}
}
