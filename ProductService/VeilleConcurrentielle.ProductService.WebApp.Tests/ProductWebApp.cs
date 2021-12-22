extern alias mywebapp;

using mywebapp::VeilleConcurrentielle.ProductService.WebApp.Data;
using VeilleConcurrentielle.Infrastructure.TestLib;

namespace VeilleConcurrentielle.ProductService.WebApp.Tests
{
    public class ProductWebApp : WebAppBase<mywebapp.Program, ProductDbContext>
    {
    }
}
