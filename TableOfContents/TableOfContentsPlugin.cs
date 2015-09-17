using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Telligent.DynamicConfiguration.Components;
using Telligent.Evolution.Extensibility.Api.Version1;
using Telligent.Evolution.Extensibility.EmbeddableContent.Version1;
using Telligent.Evolution.Extensibility.UI.Version1;
using Telligent.Evolution.Extensibility.Version1;

namespace AlexCrome.Telligent.TableOfContents
{
    public class TableOfContentsPlugin : ISingletonPlugin, ITranslatablePlugin, IInstallablePlugin, IEmbeddableContentFragmentType
    {
        private static readonly Version _emptyVersion = new Version(0, 0, 0, 0);
        private static readonly Guid _fragmentId = new Guid("ef1cc625-6d0f-40c2-a7e3-3259a72b9ae6");
        private readonly ITableOfContentsService _tableOfContentsService;
        private readonly ITableOfContentsBuilder _tableOfContentsBuilder;
        private ITranslatablePluginController _translations;

        public TableOfContentsPlugin()
        {
            _tableOfContentsService = new TableOfContentsService(new HtmlStripper());
            _tableOfContentsBuilder = new TableOfContentsBuilder(this);
        }

        internal TableOfContentsPlugin(ITableOfContentsService tableOfContentsService, ITableOfContentsBuilder tableOfContentsBuilder)
        {
            _tableOfContentsService = tableOfContentsService;
            _tableOfContentsBuilder = tableOfContentsBuilder;
        }

        public string Description => "Toc";
        public string Name => "Table of Contents";
        public string Title => _translations.GetLanguageResourceValue("title");
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public string ContentFragmentName => "Table of Contents"; //Translatable
        public string ContentFragmentDescription => "TODO"; //Translatable
        public Guid EmbeddedContentFragmentTypeId => _fragmentId;
        public PropertyGroup[] EmbedConfiguration => new PropertyGroup[0];
        public string PreviewImageUrl => "about:blank";


        public void Initialize()
        {
            PublicApi.Html.Events.Render += Events_Render;
            PublicApi.Html.Events.BeforeCreate += Html_Modified;
            PublicApi.Html.Events.BeforeUpdate += Html_Modified;

            PublicApi.WikiPages.Events.BeforeCreate += Events_BeforeCreate;
#if DEBUG
            Install(_emptyVersion);
#endif
        }

        void Events_BeforeCreate(WikiPageBeforeCreateEventArgs e)
        {
            e.Body = _tableOfContentsService.EnsureHeadersHaveAnchors(e.Body);
        }

        private void Html_Modified(EditableHtmlEventArgsBase e)
        {
            foreach (var property in e.Properties.PropertyNames.Where(x => x.Equals("Body", StringComparison.OrdinalIgnoreCase)))
                e.Properties[property] = _tableOfContentsService.EnsureHeadersHaveAnchors(e.Properties[property]);
        }


        private void Events_Render(HtmlRenderEventArgs e)
        {
            if (!e.RenderedProperty.Equals("Body", StringComparison.OrdinalIgnoreCase))
                return;

            switch (e.RenderTarget)
            {
                //TODO: Should we display for email, any other targets?
                case "Web":
                    e.RenderedHtml = InsertTableOfContents(e.RenderedHtml);
                    break;
                default:
                    e.RenderedHtml = e.RenderedHtml.Replace("[toc]", "");
                    break;
            }
        }

        public void SetController(ITranslatablePluginController controller)
        {
            _translations = controller;
        }

        public Translation[] DefaultTranslations
        {
            get
            {
                var en = new Translation("en-us");
                en.Set("title", "Table of Contents");

                return new[] { en };
            }
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

            if (hierarchy == null || !hierarchy.Any())
                return html;

            var tableOfContents = _tableOfContentsBuilder.BuildTableOfContents(hierarchy);

            if (string.IsNullOrEmpty(tableOfContents))
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
        internal static int GetTableOfContentsPosition(ref string html)
        {
            var index = html.IndexOf("[toc]", StringComparison.OrdinalIgnoreCase);

            if (index >= 3 && index <= html.Length - 9
                && html.Substring(index - 3, 12).Equals("<p>[toc]</p>", StringComparison.OrdinalIgnoreCase))
            {
                html = html.Remove(index - 3, 12);
                index = index - 3;
            }
            else if (index >= 0)
                html = html.Remove(index, 5);

            return index;
        }


        public void Install(Version lastInstalledVersion)
        {
            if (lastInstalledVersion == _emptyVersion)
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AlexCrome.Telligent.TableOfContents.Resources.styles.css");
                foreach (var theme in Themes.List(ThemeTypes.Site))
                {
                    ThemeFiles.AddUpdateFactoryDefault(theme, "cssFiles", "TableofContents.css", stream, (int)stream.Length);
                }
            }
        }

        public void Uninstall()
        {
        }

        public string Render(IEmbeddableContentFragment embeddedFragment, string target)
        {
            return "<div style=\"background-color: red; height: 100px; width: 100px;\">TODO: TABLE OF CONTENTS</div>";
        }

        public bool CanEmbed(Guid contentTypeId, int userId) => true;

        public EmbeddableContentFragmentValidationState Validate(IEmbeddableContentFragment embeddedFragment)
            => new EmbeddableContentFragmentValidationState(true);

        public void AddUpdateContentFragments(Guid contentId, Guid contentTypeId, IEnumerable<IEmbeddableContentFragment> embeddedFragments)
        {
            var s = "todo";
        }
    }
}