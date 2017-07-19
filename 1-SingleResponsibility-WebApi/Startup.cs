using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(_1_SingleResponsibility_WebApi.Startup))]

namespace _1_SingleResponsibility_WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
