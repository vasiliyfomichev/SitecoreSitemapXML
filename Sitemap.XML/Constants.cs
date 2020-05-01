#region

using Sitecore.Configuration;

#endregion

namespace Sitemap.XML
{
    public struct Constants
    {
        /// <summary>
        /// The default values support the Sitemap XML Page Settings template fields.
        /// These values can be overriden using the Sitemap.XML.Fields.* setting values in 
        /// the SitemapXML.config configuration file.
        /// </summary>
        public struct SeoSettings
        {
            public static string Title = Settings.GetSetting("Sitemap.XML.Fields.Title", "Sitemap Title");
            public static string Priority = Settings.GetSetting("Sitemap.XML.Fields.Priority", "Priority");
            public static string ChangeFrequency = Settings.GetSetting("Sitemap.XML.Fields.ChangeFrequency", "Change Frequency");
        }

        public struct SharedContent
        {
            public static string ParentItemFieldName = "Parent Item";
            public static string ContentLocationFieldName = "Content Location";
        }

        public struct WebsiteDefinition
        {
            public static string SearchEnginesFieldName = "Search Engines";
            public static string EnabledTemplatesFieldName = "Enabled Templates";
            public static string ExcludedItemsFieldName = "Excluded Items";
            public static string FileNameFieldName = "File Name";
            public static string ServerUrlFieldName = "Server Url";
            public static string CleanupBucketPath = "Cleanup Bucket Path";
	        public static string SitemapNameForRobots = "Sitemap name for Robots";
        }

		public static string SitemapParserUser = @"extranet\Anonymous";
        public static string SitemapModuleSettingsRootItemId = "{6003D67E-0000-4A4D-BFB1-11408B9ADCFD}";
        public static string RobotsFileName = "robots.txt";
        public static string SitemapSubmissionUriFieldName = "Sitemap Submission Uri";
    }
}