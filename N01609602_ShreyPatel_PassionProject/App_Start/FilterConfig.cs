﻿using System.Web;
using System.Web.Mvc;

namespace N01609602_ShreyPatel_PassionProject
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
