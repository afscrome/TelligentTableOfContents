============================
      ABOUT THE ADDON
============================

The Table of Contents addon allows you to automatically generate a table of contents
for a wiki page, blog post, forum post or media file.


============================
      USING THE ADDON
============================

When authoring a long article insert the text "[toc]" (without the quotes)
towards the top of your article.  This will be replaced by an automatically
generated table of contents when users view your content.

To tell the addon how to generate the table of contents, you should use the
rich text editor to format the headings in your article to use the "Heading 2",
"Heading 3" etc. formats.


============================
        INSTALLATION
============================

1. Go to the "Site Theme" page in your community's Control Panel
		*	If on Telligent Community 5.6 / Telligent Enterprise 2.6 or higher - 
				Go to the Files tab, and upload the TableOfContents.css file as an
				additional CSS File in your site theme
		*	If on Telligent Community 5.5 / Telligent Enterprise 2.5 or lower -
				Go to the "Custom Styles (Advanced)" tab and copy the contents of
				the TableOfContents.css file into the CSS Overrides box

2. Copy the contents of the /web/ folder to your Community


============================
  BUILDING THE SOURCE CODE
============================

1.	Copy the binaries from the /bin/ directory of your Telligent Evolution platform
	community into the /source/TelligentEvolutionBinaries/ directory

2.	Open up Visual Studio and build the solution in the /source/ directory.