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
    public class RobotHandler : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
			Assert.ArgumentNotNull(args, "args");
            if (Context.Site == null || string.IsNullOrEmpty(Context.Site.RootPath.Trim())) return;
            if (Context.Page.FilePath.Length > 0) return;
	        var site = Context.Site;
	        if (!args.Url.FilePath.Contains(Constants.RobotsFileName)) return;

            args.Context.Response.ClearHeaders();
            args.Context.Response.ClearContent();
            args.Context.Response.ContentType = "text/plain";
			
            var content = string.Empty;
            try
            {
                var config = new SitemapManagerConfiguration(site.Name);
                var sitemapManager = new SitemapManager(config);

                content = sitemapManager.GetRobotSite();
                args.Context.Response.Write(content);
            }
            catch (Exception e)
            {
	            Log.Error("Error Robots", e, this);
            }
			finally
            {

                args.Context.Response.Flush();
                args.Context.Response.End();
            }
        }
    }
}