using System;
using System.Text;
using System.Xml;
using CommunityServer.Components;
using CommunityServer.Wikis.Components;

namespace Telligent.Evolution.TableOfContents
{
	public class TableOfContentsModule : ICSModule
	{
		private readonly ITableOfContentsService _tableOfContentsService;
		private readonly ITableOfContentsBuilder _tableOfContentsBuilder;
		public TableOfContentsModule()
		{
			_tableOfContentsService = Telligent.Common.Services.Get<ITableOfContentsService>();
			_tableOfContentsBuilder = Telligent.Common.Services.Get<ITableOfContentsBuilder>();
		}

		public TableOfContentsModule(ITableOfContentsService tableOfContentsService, ITableOfContentsBuilder tableOfContentsBuilder)
		{
			_tableOfContentsService = tableOfContentsService;
			_tableOfContentsBuilder = tableOfContentsBuilder;
		}

		public void Init(CSApplication csa, XmlNode node)
		{
			WikiEvents.BeforeAddPage += WikiEvents_BeforePageChange;
			WikiEvents.BeforeUpdatePage += WikiEvents_BeforePageChange;

			WikiEvents.RenderPage += WikiEvents_RenderViewableContent;
			WikiEvents.RenderPageRevision += WikiEvents_RenderViewableContent;
		}

		void WikiEvents_BeforePageChange(Page page, EventArgs e)
		{
			// Whenever saving articles to the DB, make sure all headings have anchors
			page.Body = _tableOfContentsService.EnsureHeadersHaveAnchors(page.Body);
		}

		void WikiEvents_RenderViewableContent(IViewableContent content, RenderEventArgs e)
		{
			e.RenderedContent = InsertTableOfContents(e.RenderedContent);
		}


		public string InsertTableOfContents(string html)
		{
			var position = GetTableOfContentsPosition(ref html);

			if (position == -1)
				return html;

			var hierarchy = _tableOfContentsService.GetHeadingHierarchy(html);

			var tableOfContents = _tableOfContentsBuilder.BuildTableOfContents(hierarchy);

			if (String.IsNullOrEmpty(tableOfContents))
				return html;

			var newHtml = new StringBuilder();
			newHtml.Append(html.Substring(0, position));
			newHtml.Append(tableOfContents);
			newHtml.Append(html.Substring(position));

			return newHtml.ToString();
		}

		public virtual int GetTableOfContentsPosition(ref string html)
		{
			var index = html.IndexOf("[toc]", StringComparison.InvariantCultureIgnoreCase);

			if (index >= 3 && index <= html.Length - 9
				&& String.Compare(html.Substring(index - 3, 12), "<p>[toc]</p>", true) == 0)
			{
				html = html.Remove(index - 3, 12);
				index = index - 3;
			}
			else if (index >= 0)
				html = html.Remove(index, 5);

			return index;
		}


	}
}
