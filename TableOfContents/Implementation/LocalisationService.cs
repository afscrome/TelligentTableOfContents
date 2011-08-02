
namespace Telligent.Evolution.Extensions.TableOfContents
{
	public class LocalisationService : ILocalisationService
	{
		private const string ResourceFile = "TableOfContents.xml";
		
		public string GetString(string resourceKey)
		{
			return Telligent.Evolution.Components.ResourceManager.GetString(resourceKey, ResourceFile);
		}
	}
}
