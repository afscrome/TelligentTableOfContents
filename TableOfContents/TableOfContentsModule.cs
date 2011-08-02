using System;
using System.Text;
using System.Xml;
using Telligent.Evolution.Components;
using Telligent.Evolution.Wikis.Components;
using Services = Telligent.Common.Services;

namespace Telligent.Evolution.Extensions.TableOfContents
{
	public class TableOfContentsModule : ICSModule
	{
		private readonly ITableOfContentsService _tableOfContentsService;
		private readonly ITableOfContentsBuilder _tableOfContentsBuilder;

		/// <summary>
		/// Creates a new TableOfContentsModule using default implementations of
		/// ITableOfContentsService and ITableOfContentsBuilder
		/// </summary>
		/// <remarks>Used when instantiating the CSModule as this requires 0 paramaters</remarks>
		public TableOfContentsModule() : this(Services.Get<ITableOfContentsService>(), Services.Get<ITableOfContentsBuilder>())
		{
		}

		/// <summary>
		/// Creates a new TableOfContentsModule using the specified implementations of
		/// ITableOfContentsService and ITableOfContentsBuilder
		/// </summary>
		/// <param name="tableOfContentsService">The ITableOfContentsService implementation to use</param>
		/// <param name="tableOfContentsBuilder">The ITableOfContentsBuilder  implementation to use</param>
		public TableOfContentsModule(ITableOfContentsService tableOfContentsService, ITableOfContentsBuilder tableOfContentsBuilder)
		{
			_tableOfContentsService = tableOfContentsService;
			_tableOfContentsBuilder = tableOfContentsBuilder;
		}

		public void Init(CSApplication csa, XmlNode node)
		{
			WikiEvents.BeforeAddPage += WikiEvents_BeforePageChange;
			WikiEvents.BeforeUpdatePage += WikiEvents_BeforePageChange;
			csa.PrePostUpdate += csa_PrePostUpdate;

			WikiEvents.RenderPage += WikiEvents_RenderViewableContent;
			WikiEvents.RenderPageRevision += WikiEvents_RenderViewableContent;
			csa.PreRenderPost += csa_PreRenderPost;
		}

		private void csa_PrePostUpdate(IContent content, CSPostEventArgs e)
		{
			if (e.State != ObjectState.Create && e.State != ObjectState.Update)
				return;

			EnsureHeadersHaveAnchors(content);
		}

		private void WikiEvents_BeforePageChange(Page page, EventArgs e)
		{
			EnsureHeadersHaveAnchors(page);
		}


		void WikiEvents_RenderViewableContent(IViewableContent content, RenderEventArgs e)
		{
			e.RenderedContent = InsertTableOfContents(e.RenderedContent);
		}

		void csa_PreRenderPost(IContent content, CSPostEventArgs e)
		{
			content.FormattedBody = InsertTableOfContents(content.FormattedBody);
		}


		/// <summary>
		/// Makes sure that all headers have anchor tags in them
		/// </summary>
		/// <param name="content"></param>
		private void EnsureHeadersHaveAnchors(IContent content)
		{
			content.Body = _tableOfContentsService.EnsureHeadersHaveAnchors(content.Body);
			content.FormattedBody = _tableOfContentsService.EnsureHeadersHaveAnchors(content.FormattedBody);
		}


		/// <summary>
		/// Takes an HTML string and inserts the Table of Contents into
		/// the Html
		/// </summary>
		/// <param name="html">The html to insert the Table of Contents into</param>
		/// <returns>An Html string containing the original html along with a table of contents</returns>
		internal string InsertTableOfContents(string html)
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

		/// <summary>
		/// Calculates the index at which the Table of Contents should be inserted into a string.
		/// </summary>
		/// <param name="html">The html to insert the Table of Contents into</param>
		/// <returns>The index where the table of contents should be inserted</returns>
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
