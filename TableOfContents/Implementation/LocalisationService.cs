
namespace Telligent.Evolution.TableOfContents
{
	public class LocalisationService : ILocalisationService
	{
		private const string ResourceFile = "TableOfContents.xml";
		
		public string GetString(string resourceKey)
		{
			return CommunityServer.Components.ResourceManager.GetString(resourceKey, ResourceFile);
		}
	}
}
