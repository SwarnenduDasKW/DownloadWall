using System;
using System.Collections.Generic;
using System.Net;
using HtmlAgilityPack;
using System.Configuration;


namespace DownloaderSD
{
    public class Process
    {
        Logger log = new Logger();

        private string WebsitePrefix = string.Empty;
        private string DownloadDir = string.Empty;
        private string Resolution = string.Empty;

        public Process()
        {
            WebsitePrefix = ConfigurationManager.AppSettings["WebsitePrefix"];
            DownloadDir   = ConfigurationManager.AppSettings["DownloadDir"];

            string res = ConfigurationManager.AppSettings["Resolution"];
            switch (res)
            {
                case "HD":
                    Resolution = "1366x768.jpg";
                    break;
                case "FULLHD":
                    Resolution = "1920x1080.jpg";
                    break;
                case "UHD":
                    Resolution = "3840x2160.jpg";
                    break;
                case "4K":
                    Resolution = "7680x4320.jpg";
                    break;
                case "DUALHD":
                    Resolution = "3480x1080.jpg";
                    break;
                default:
                    Resolution = "1920x1080.jpg";
                    break;
            }
        }

        public int Start(string masterURL)
        {
            log.BigBanner("Walppaperwide","Auto Downloaded");

            string fullLink = string.Empty;
            int finalPage = 0;
            string pagelink = "";
            string fileName = string.Empty;
            string urlToProcess = string.Empty;

            int temp = 0, finalSlash = 0;
            int count = 0;
            int pageCount = 0;

            List<string> pagination = new List<string>();
            List<string> links = new List<string>();
            List<WallpaperCollection> wallCollection = new List<WallpaperCollection>();

            do
            {
                pageCount++;

                if (pageCount > 1)
                {
                    
                    urlToProcess = string.Format("{0}{1}", masterURL, pageCount.ToString());
                    wallCollection.Clear();
                    log.WriteToFile("Start", string.Format("Processing Page {0} of {1}", pageCount.ToString(), finalPage.ToString()));
                    log.WriteToFile("Start", string.Format("Master URL: {0}", urlToProcess));
                }
                else
                {
                    urlToProcess = masterURL;
                    log.WriteToFile("Start", string.Format("Processing Page {0}", pageCount.ToString()));
                    log.WriteToFile("Start", string.Format("Master URL: {0}", urlToProcess));
                }

                wallCollection = ScrapAllLinks(urlToProcess);

                foreach (WallpaperCollection col in wallCollection)
                {
                    if (col.Category.EndsWith("-wallpapers.html"))
                    {
                        fullLink = string.Format("{0}{1}", WebsitePrefix, col.Category);
                        if (links.IndexOf(fullLink) < 0)
                        {
                            links.Add(fullLink);
                        }
                    }

                    //get the total count of the pages per category
                    if (pageCount == 1 && col.Category.Contains("page"))
                    {
                        //if (pagination.IndexOf(col.Category) < 0)
                        //{

                            pagelink = col.Category;
                            finalSlash = pagelink.LastIndexOf('/');
                            int.TryParse(pagelink.Substring(finalSlash + 1), out temp);
                            if (temp > finalPage)
                            {
                                finalPage = temp;
                            }

                        //    pagination.Add(col.Category);
                        //}
                    }
                }

                // After the list is populated clear the original
                wallCollection.Clear();
                links.Reverse();
                foreach (string wall in links)
                {
                    count++;
                    
                    wallCollection = ScrapAllLinks(wall, Resolution);

                    foreach (WallpaperCollection col in wallCollection)
                    {
                        fullLink = string.Format("{0}{1}", WebsitePrefix, col.Category);
                        fileName = fullLink.Substring(fullLink.LastIndexOf('/') + 1);
                        DownloadFile(fullLink, fileName);
                    }

                    wallCollection.Clear();

                    //considering 10 wallpapers per page
                    if (count == 10) break;
                }

                //clear
                links.Clear();
            } while (pageCount < finalPage);

          return 0;
        }

        public List<WallpaperCollection> ScrapAllLinks(string url)
        {
            int counter = 0;
            string hrefLink = string.Empty;

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();

            doc = web.Load(url);

            List<WallpaperCollection> wc = new List<WallpaperCollection>();

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                HtmlAttribute att = link.Attributes["href"];

                counter++;
                hrefLink = att.Value;

                wc.Add(new WallpaperCollection() { Index = counter, Category = hrefLink, IsSelected = 0 });
            }

            return wc;
        }

        public List<WallpaperCollection> ScrapAllLinks(string url, string filter)
        {
            int counter = 0;
            string hrefLink = string.Empty;

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();

            doc = web.Load(url);

            List<WallpaperCollection> wc = new List<WallpaperCollection>();

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                HtmlAttribute att = link.Attributes["href"];

                counter++;
                hrefLink = att.Value;

                //refine the url
                if (att.Value.Contains("a") && att.Value.Contains(filter))
                {
                    wc.Add(new WallpaperCollection() { Index = counter, Category = hrefLink, IsSelected = 0 });
                }
            }

            return wc;
        }

        public void DownloadFile(string url, string fileName)
        {
            try
            {
                string FileNameWithPath = string.Empty;
                FileNameWithPath = string.Format("{0}{1}", DownloadDir, fileName);
                WebClient wc = new WebClient();
                Uri uri = new Uri(url);
                log.WriteToFile("DownloadFile", string.Format("Downloading {0} ...",url));
                wc.DownloadFile(uri, FileNameWithPath);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Wallpaper " + url + ": " + fileName + " downloaded successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error downloading - " + e.Message.ToString());
            }
        }

    }
}
