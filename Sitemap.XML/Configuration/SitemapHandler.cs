#region

using System;
using Sitecore;
using Sitecore.Caching;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;
using Sitemap.XML.Models;

#endregion

namespace Sitemap.XML.Configuration
{
	public class SitemapHandler : HttpRequestProcessor
	{
		#region Properties

		public string ExcludedPaths { get; set; }
		public string CacheTime { get; set; }

		#endregion

		public override void Process(HttpRequestArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			if (Context.Site == null || string.IsNullOrEmpty(Context.Site.RootPath.Trim())) return;
			if (Context.Page.FilePath.Length > 0) return;
			var sitemapHandler = string.IsNullOrWhiteSpace(Context.Site.Properties["sitemapHandler"])
				? "sitemap.xml"
				: Context.Site.Properties["sitemapHandler"];
			if (!args.Url.FilePath.Contains(sitemapHandler)) return;

			// Important to return qualified XML (text/xml) for sitemaps
			args.Context.Response.ClearHeaders();
			args.Context.Response.ClearContent();
			args.Context.Response.ContentType = "text/xml";

			// Checking the HTML cache first
			var site = Context.Site;
#if !DEBUG
            var cacheKey = "UltimateSitemapXML_" + site.Name;
            var cache = CacheManager.GetHtmlCache(site).GetHtml(cacheKey);
            if (!string.IsNullOrWhiteSpace(cache))
            {
                args.Context.Response.Write(cache);
                args.Context.Response.End();
                return;
            }
#endif

			var content = string.Empty;
			try
			{
				var config = new SitemapManagerConfiguration(site.Name);
				var sitemapManager = new SitemapManager(config);

				content = sitemapManager.BuildSiteMapForHandler();
				args.Context.Response.Write(content);
			}
			catch (Exception e)
			{
				Log.Error("Error Sitemap", e, this);
			}
			finally
			{
#if !DEBUG
                CacheManager.GetHtmlCache(site).SetHtml(cacheKey, content);
#endif
				args.Context.Response.Flush();
				args.Context.Response.End();
			}
		}
	}
}