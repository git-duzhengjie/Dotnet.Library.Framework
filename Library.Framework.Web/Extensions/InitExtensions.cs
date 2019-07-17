using Library.Framework.Core.Aspnet;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Http.Internal;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace Library.Framework.Web
{
    public static class InitExtensions
    {
        private static string _baseDir=> AppDomain.CurrentDomain.BaseDirectory;
        public static void LoadPlugin(this IServiceCollection services) {
            var pluginDir = Path.Combine(_baseDir, "Plugins");
            foreach (string dll in Directory.GetFiles(pluginDir, "*.dll")) {
                try
                {
                    var assemb = AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
                    services.AddMvc().AddApplicationPart(assemb);
                }
                catch (Exception ex) {
                    //Console.WriteLine(ex.Message);
                }
            }
        }

        public static void LoadLib() {
            var libDir = Path.Combine(_baseDir, "Lib");
            foreach (string dll in Directory.GetFiles(libDir, "*.dll"))
            {
                try
                {
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
                }
                catch (Exception ex)
                {

                }
            }
        }

        //protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        //{
        //    if (!Request.Headers.ContainsKey(Options.UserIdHeaderName) || !Request.Headers.ContainsKey(Options.UserNameHeaderName))
        //    {
        //        return Task.FromResult(AuthenticateResult.NoResult());
        //    }
        //    var userId = Request.Headers[Options.UserIdHeaderName].ToString();
        //    var userName = Request.Headers[Options.UserNameHeaderName].ToString();
        //    var userRoles = new string[0];
        //    if (Request.Headers.ContainsKey(Options.UserRolesHeaderName))
        //    {
        //        userRoles = Request.Headers[Options.UserRolesHeaderName].ToString()
        //            .Split(new[] { Options.Delimiter }, StringSplitOptions.RemoveEmptyEntries);
        //    }
        //    var claims = new List<Claim>()
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, userId),
        //        new Claim(ClaimTypes.Name, userName),
        //    };

        //    if (userRoles.Length > 0)
        //    {
        //        claims.AddRange(userRoles.Select(r => new Claim(ClaimTypes.Role, r)));
        //    }
        //    if (Options.AdditionalHeaderToClaims.Count > 0)
        //    {
        //        foreach (var headerToClaim in Options.AdditionalHeaderToClaims)
        //        {
        //            if (Request.Headers.ContainsKey(headerToClaim.Key))
        //            {
        //                foreach (var val in Request.Headers[headerToClaim.Key].ToString().Split(new[] { Options.Delimiter }, StringSplitOptions.RemoveEmptyEntries))
        //                {
        //                    claims.Add(new Claim(headerToClaim.Value, val));
        //                }
        //            }
        //        }
        //    }
        //    // claims identity 's authentication type can not be null https://stackoverflow.com/questions/45261732/user-identity-isauthenticated-always-false-in-net-core-custom-authentication
        //    var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
        //    var ticket = new AuthenticationTicket(
        //        principal,
        //        Scheme.Name
        //    );
        //    return Task.FromResult(AuthenticateResult.Success(ticket));
        //}

        public static void LoadDoc(this SwaggerGenOptions options) {
            var dir = Path.Combine(_baseDir, "Doc");
            var din = new DirectoryInfo(dir);
            foreach (var d in din.GetFiles("*.xml", SearchOption.AllDirectories)) {
                options.IncludeXmlComments(d.FullName);
            }
        }
    }
}
