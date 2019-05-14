/* *********************************************************************** *
 * File   : SitemapHandler.cs                             Part of Sitecore *
 * Version: 1.0.0                                         www.sitecore.net *
 *                                                                         *
 *                                                                         *
 * Purpose: Contains logic which fires when event submitted                *
 *                                                                         *
 * Copyright (C) 1999-2009 by Sitecore A/S. All rights reserved.           *
 *                                                                         *
 * This work is the property of:                                           *
 *                                                                         *
 *        Sitecore A/S                                                     *
 *        Meldahlsgade 5, 4.                                               *
 *        1613 Copenhagen V.                                               *
 *        Denmark                                                          *
 *                                                                         *
 * This is a Sitecore published work under Sitecore's                      *
 * shared source license.                                                  *
 *                                                                         *
 * *********************************************************************** */

using System;
using Sitecore.Diagnostics;

namespace Sitemap.XML.Models
{
    public class SitemapHandler
    {
        public void RefreshSitemap(object sender, EventArgs args)
        {
	        try
	        {
		        var sites = SitemapManagerConfiguration.GetSiteNames();
		        foreach (var site in sites)
		        {
			        var config = new SitemapManagerConfiguration(site);
			        var sitemapManager = new SitemapManager(config);
			        sitemapManager.SubmitSitemapToSearchenginesByHttp();

			        if (!config.GenerateRobotsFile) continue;
			        sitemapManager.RegisterSitemapToRobotsFile();
		        }
	        }
	        catch (Exception e)
	        {
		        Log.Error("Error Sitemap",e,this);
	        }
        }
    }
}
