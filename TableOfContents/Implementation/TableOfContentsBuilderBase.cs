
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telligent.Evolution.TableOfContents
{
	public abstract class TableOfContentsBuilderBase : ITableOfContentsBuilder
	{
		public abstract void StartTableOfContents(StringBuilder builder);
		public abstract void EndTableOfContents(StringBuilder builder);
		public abstract void StartHierarchyList(StringBuilder builder);
		public abstract void EndHierarchyList(StringBuilder builder);
		public abstract void StartHierarchyItem(StringBuilder builder);
		public abstract void EndHierarchyItem(StringBuilder builder);

		public string BuildTableOfContents(ICollection<HierarchyItem<Heading>> headings)
		{
			if (headings == null || !headings.Any())
				return String.Empty;

			var tableOfContents = new StringBuilder();

			StartTableOfContents(tableOfContents);
			tableOfContents.Append(tableOfContents.ToString());
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
			builder.Append(heading.Item.Contents);
			builder.Append("</a>");
			BuildTableOfContentsLayer(builder, heading.Children);
			EndHierarchyItem(builder);
		}

	}
}
