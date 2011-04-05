
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telligent.Evolution.TableOfContents
{
	public abstract class TableOfContentsBuilderBase : ITableOfContentsBuilder
	{
		protected abstract void StartTableOfContents(StringBuilder builder);
		protected abstract void EndTableOfContents(StringBuilder builder);
		protected abstract void StartHierarchyList(StringBuilder builder);
		protected abstract void EndHierarchyList(StringBuilder builder);
		protected abstract void StartHierarchyItem(StringBuilder builder);
		protected abstract void EndHierarchyItem(StringBuilder builder);

		public string BuildTableOfContents(ICollection<HierarchyItem<Heading>> headings)
		{
			if (headings == null || !headings.Any())
				return String.Empty;

			var tableOfContents = new StringBuilder();

			StartTableOfContents(tableOfContents);
			BuildTableOfContentsLayer(tableOfContents, headings);
			EndTableOfContents(tableOfContents);

			return tableOfContents.ToString();
		}

		public virtual void BuildTableOfContentsLayer(StringBuilder builder, ICollection<HierarchyItem<Heading>> hierarchyItems)
		{
			if (hierarchyItems == null || !hierarchyItems.Any())
				return;

			StartHierarchyList(builder);

			foreach (var hierarchyItem in hierarchyItems)
				BuildTableOfContentsItem(builder, hierarchyItem);

			EndHierarchyList(builder);
		}

		public virtual void BuildTableOfContentsItem(StringBuilder builder, HierarchyItem<Heading> heading)
		{
			StartHierarchyItem(builder);
			builder.Append("<a href=\"#");
			builder.Append(heading.Item.AnchorName);
			builder.Append("\">");
			builder.Append(heading.Item.Title);
			builder.Append("</a>");
			BuildTableOfContentsLayer(builder, heading.Children);
			EndHierarchyItem(builder);
		}

	}
}
