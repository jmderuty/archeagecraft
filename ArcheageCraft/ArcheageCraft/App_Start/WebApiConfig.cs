﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace ArcheageCraft
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuration et services Web API
            // Configurez Web API pour utiliser uniquement l’authentification par jeton de support.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Utilisez la casse mixte pour les données JSON.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Routes Web API
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "recipes",
                routeTemplate: "api/items/{id}/recipes",
                defaults: new { controller = "items", action="recipes",id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
               name: "prices",
               routeTemplate: "api/items/{id}/prices",
               defaults: new { controller = "items", action = "prices", id = RouteParameter.Optional }
           );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional, action="" }
            );
        }
    }
}
