﻿using System.Web;
using System.Web.Mvc;
using FeriaVirtualWeb.Filter;

namespace FeriaVirtualWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CheckSession());
        }
    }
}
