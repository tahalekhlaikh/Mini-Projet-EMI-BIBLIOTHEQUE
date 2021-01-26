using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(gestionDeBiblio.Startup))]
namespace gestionDeBiblio
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }

       
    }
}
