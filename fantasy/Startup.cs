using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;

namespace fantasy
{
    internal static partial class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}