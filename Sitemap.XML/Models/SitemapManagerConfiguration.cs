/* *********************************************************************** *
 * File   : SitemapManagerConfiguration.cs                Part of Sitecore *
 * Version: 1.0.0                                         www.sitecore.net *
 *                                                                         *
 *                                                                         *
 * Purpose: Class for getting config information from db and conf file     *
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

using System.Runtime.InteropServices;
using System.Web;
using System.Xml;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Xml;
using System.Collections.Specialized;
using Sitecore.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Sitecore;
using Sitecore.Sites;

namespace Sitemap.XML.Models
{
    public class SitemapManagerConfiguration
    {
        #region Fields

        #endregion

        #region Constructor 

        public SitemapManagerConfiguration(string siteName)
        {
            Assert.IsNotNullOrEmpty(siteName, "siteName");
            SiteName = siteName;
        }

		#endregion

		#region Properties

		public static string SitesMultilingual
		{
			get
			{
				return GetValueByName("sitesMultilingual");
			}
		}

		public static string XmlnsTpl
        {
            get
            {
                return GetValueByName("xmlnsTpl");
            }
        }

	    public static string XmlnsXhtmlTpl
		{
		    get
		    {
			    return GetValueByName("xmlnsXhtmlTpl");
		    }
	    }

		public static string WorkingDatabase
        {
            get
            {
                return GetValueByName("database");
            }
        }

        public string SitemapConfigurationItemPath
        {
            get
            {
                var site = Factory.GetSite(SiteName); // GetSite(SiteName);
                var sitemapPath = site.Properties["sitemapPath"];
                if (string.IsNullOrWhiteSpace(sitemapPath))
                {
                    return GetValueByName("sitemapConfigurationItemPath") + SiteName;
                }
                else
                {
                    return sitemapPath;
                }
            }
        }

        public bool GenerateRobotsFile
        {
            get
            {
                string doGenerate = GetValueByName("generateRobotsFile");
                return !string.IsNullOrEmpty(doGenerate) && (doGenerate.ToLower() == "true" || doGenerate == "1");
            }
        }

        public string EnabledTemplates => GetValueByNameFromDatabase(Constants.WebsiteDefinition.EnabledTemplatesFieldName);
	    public string ExcludedItems => GetValueByNameFromDatabase(Constants.WebsiteDefinition.ExcludedItemsFieldName);

        public bool CleanupBucketPath => GetValueByNameFromDatabase(Constants.WebsiteDefinition.CleanupBucketPath) == "1";

        public string ServerUrl
        {
            get
            {
                var url =  GetValueByNameFromDatabase(Constants.WebsiteDefinition.ServerUrlFieldName);
                return string.IsNullOrWhiteSpace(url) ? HttpContext.Current.Request.Url.Scheme+"://" 
                    +Context.Site.Properties["hostname"] : url.Trim('/');
            }
        }

        public static bool IsProductionEnvironment
        {
            get
            {
                var production = GetValueByName("productionEnvironment");
                return !string.IsNullOrEmpty(production) && (production.ToLower() == "true" || production == "1");
            }
        }

        public string SiteName { get; } = string.Empty;

        public string FileName => GetValueByNameFromDatabase(Constants.WebsiteDefinition.FileNameFieldName);

        #endregion properties

        #region Private Methods

        private static string GetValueByName(string name)
        {
            var result = string.Empty;

            foreach (XmlNode node in Factory.GetConfigNodes("sitemapVariables/sitemapVariable"))
            {
                if (XmlUtil.GetAttribute("name", node) != name) continue;
                result = XmlUtil.GetAttribute("value", node);
                break;
            }

            return result;
        }

        private string GetValueByNameFromDatabase(string name)
        {
            string result = string.Empty;

            Database db = Factory.GetDatabase(WorkingDatabase);
            if (db != null)
            {
                Item configItem = db.Items[SitemapConfigurationItemPath];
                if (configItem != null)
                {
                    result = configItem[name];
                }
            }

            return result;
        }

        #endregion

        #region Public Methods

        public static IEnumerable<string> GetSiteNames()
        {
            var sitemapXmlSystemRootId = Constants.SitemapModuleSettingsRootItemId;
            var configRoot = Factory.GetDatabase(WorkingDatabase).GetItem(sitemapXmlSystemRootId);
            if (configRoot == null) return null;

            var configs = configRoot.Children.Where(i=>i.TemplateName!="Folder");
            if (!configs.Any()) return null;

            var siteNames = configs.Select(c => c.Name);
            return siteNames;
        }

        public static string GetServerUrl(string siteName)
        {
            string result = string.Empty;

            foreach (XmlNode node in Factory.GetConfigNodes("sitemapVariables/sites/site"))
            {

                if (XmlUtil.GetAttribute("name", node) == siteName)
                {
                    result = XmlUtil.GetAttribute("serverUrl", node);
                    break;
                }
            }

            return result;
        }

        #endregion
    }
}
