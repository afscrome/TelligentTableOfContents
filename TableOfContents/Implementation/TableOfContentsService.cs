using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Telligent.Evolution.TableOfContents
{
	public class TableOfContentsService : ITableOfContentsService
	{
		private static readonly Regex HeaderTagRegex = new Regex("<h([1-6]).*?>(.+?)<\\/h[1-6]>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
		private static readonly Regex AnchorRegex = new Regex("<a.*?name=\"([a-z]([a-z]|[1-9]|-|_|\\.|:)*?)\".*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

		private readonly IHtmlStripper _htmlStripper;

		public TableOfContentsService(IHtmlStripper htmlStripper)
		{
			_htmlStripper = htmlStripper;
		}

		public string EnsureHeadersHaveAnchors(string html)
		{
			var resultingHtml = new StringBuilder();
			var lastHeadingCloseIndex = 0;
			foreach (Match match in HeaderTagRegex.Matches(html))
			{
				//Insert all HTML since the last match
				resultingHtml.Append(html.Substring(lastHeadingCloseIndex, match.Index - lastHeadingCloseIndex));

				var headingHtml = match.Value;
				// If we don't have an anchor, add one
				if (!AnchorRegex.IsMatch(headingHtml))
					headingHtml = InsertAnchor(headingHtml);

				resultingHtml.Append(headingHtml);
				lastHeadingCloseIndex = match.Index + match.Length;
			}

			return resultingHtml.ToString();
		}

		public IEnumerable<Heading> GetHeadings(string html)
		{
			foreach (Match match in HeaderTagRegex.Matches(html))
			{
				var contents = match.Groups[2].Value;
				var anchor = AnchorRegex.Match(contents);
				if (anchor.Success)
					yield return new Heading
										{
											HeadingType = (HeadingType)byte.Parse(match.Groups[1].Value),
											Contents = _htmlStripper.RemoveHtml(contents),
											AnchorName = anchor.Groups[1].Value
										};
			}
		}

		public ICollection<HierarchyItem<Heading>> GetHeadingHierarchy(string html)
		{
			var topLevelHeadings = new List<HierarchyItem<Heading>>();
			var headingsStack = new Stack<HierarchyItem<Heading>>();

			foreach (var heading in GetHeadings(html))
			{
				var hiearchyItem = new HierarchyItem<Heading>(heading);

				var addedItem = false;
				while (headingsStack.Count > 0)
				{
					// Keep looking up the heading hierarchy until we find the 
					// first item that is a higher heading type than the current
					// heading
					var parentHeading = headingsStack.Peek();
					if (parentHeading.Item.HeadingType < heading.HeadingType)
					{
						headingsStack.Push(hiearchyItem);
						parentHeading.Children.Add(hiearchyItem);
						addedItem = true;
						break;
					}
					headingsStack.Pop();
				}

				if (!addedItem)
				{
					headingsStack.Push(hiearchyItem);
					topLevelHeadings.Add(hiearchyItem);
				}
			}

			return topLevelHeadings;
		}

		internal string InsertAnchor(string heading)
		{
			var match = HeaderTagRegex.Match(heading);
			if (!match.Success)
				throw new ArgumentOutOfRangeException("heading", heading, "Heading is not valid heading tag");

			// If we can't generate an anchor name, don't insert the anchor
			var headingContents = match.Groups[2].Value;
			var anchorName = MakeAnchorName(headingContents);
			if (String.IsNullOrEmpty(anchorName))
				return heading;

			var anchorPosition = match.Groups[2].Index;
			var anchor = string.Format("<a name=\"{0}\"></a>", anchorName);
			return heading.Insert(anchorPosition , anchor);
		}

		internal string MakeAnchorName(string heading)
		{
			var plainText = _htmlStripper.RemoveHtml(heading);
			var anchorName = new StringBuilder();
			// Anchor name must begin with with a letter, 

			var enumerator = plainText.GetEnumerator();
			enumerator.Reset();
			// First character must be a letter
			while (enumerator.MoveNext())
			{
				if (!char.IsLetter(enumerator.Current))
					continue;

				anchorName.Append(enumerator.Current);
				break;
			}

			//Subsequent charaters may additionally include numbers, -, _, : and .
			while (enumerator.MoveNext())
			{
				if (char.IsLetterOrDigit(enumerator.Current))
				{
					anchorName.Append(enumerator.Current);
					continue;
				}

				var lastCharacter = anchorName[anchorName.Length - 1];
				if (lastCharacter == '_' || lastCharacter == '-')
					continue;

				if (enumerator.Current == '-')
					anchorName.Append('-');
				else if (char.IsWhiteSpace(enumerator.Current) || char.IsPunctuation(enumerator.Current))
						anchorName.Append('_');
			}

			if (anchorName.Length == 0)
				return String.Empty;

			while (!char.IsLetterOrDigit(anchorName[anchorName.Length - 1]))
			{
				anchorName.Remove(anchorName.Length - 1, 1);
			}

			return anchorName.ToString();
		}

	}
}

