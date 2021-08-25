using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectSolution1.Models;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;

namespace ProjectSolution1.Controllers
{
    public class DynamicBlogCardController : Controller
    {

        // GET: DynamicBlogCard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getBlogCard(FormCollection form)
        {
            string query = form["searchInput"];
            List<BlogCard> BC = new List<BlogCard>();
            var item = Sitecore.Context.Database.GetItem("{5713C618-0FA5-44F0-8B14-8A963031E2A6}");
            Sitecore.Data.Fields.MultilistField BlogList = item.Fields["BlogList"];
            if (BlogList != null)
            {
                foreach (Sitecore.Data.Items.Item I in BlogList.GetItems())
                {
                    var ID = I.ID.ToString();
                    var CreatedDate = DateTime.ParseExact(I.Fields["PostedDate"].Value, "yyyyMMdd'T'HHmmss'Z'", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                    var Title = I.Fields["Title"].Value;
                    var SmallDescription = I.Fields["ShortDiscription"].Value;
                    var Image = I.Fields["ArticalsSmallImage"].Value;

                    Sitecore.Data.Fields.ImageField imageField = I.Fields["ArticalsSmallImage"];
                    Sitecore.Data.Items.MediaItem image = new Sitecore.Data.Items.MediaItem(imageField.MediaItem);
                    string imageSrc = Sitecore.StringUtil.EnsurePrefix('/',
                    Sitecore.Resources.Media.MediaManager.GetMediaUrl(image));
                    //var Buttontext = item.Fields["CardButtonText"].Value;

                    //string imgTag = String.Format(@"<img src=""{0}"" alt=""{1}"" />", src, image.Alt);

                    BC.Add(new BlogCard(ID, Title, CreatedDate, SmallDescription, imageSrc, image.Alt));
                }
            }



            if (query != null)
            {
                List<BlogCard> results = null;

                results = BC.FindAll(Findtitle);

                bool Findtitle(BlogCard AMp)
                {
                    if (AMp.BlogTitle.ToLower().Contains(query.ToLower()) || AMp.ShortDesc.ToLower().Contains(query.ToLower()))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return View("~/Views/ProjectViews/Dynamics.cshtml", results);
            }
            return View("~/Views/ProjectViews/Dynamics.cshtml", BC);

        }

    }
}