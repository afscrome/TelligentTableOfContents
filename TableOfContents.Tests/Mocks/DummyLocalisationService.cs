
namespace Telligent.Evolution.Extensions.TableOfContents.Tests.Mocks
{
	public class DummyLocalisationService : ILocalisationService
	{
		public string GetString(string resourceKey)
		{
			return string.Concat("[[", resourceKey, "]]");
		}
	}
}
