using System.Collections.Generic;

namespace Telligent.Evolution.Extensions.TableOfContents
{
	public class HierarchyItem<T>
	{
		public HierarchyItem(T item)
		{
			Item = item;
			Children = new List<HierarchyItem<T>>();
		}

		public T Item { get; private set; }
		public ICollection<HierarchyItem<T>> Children { get; private set; }
	}

}
