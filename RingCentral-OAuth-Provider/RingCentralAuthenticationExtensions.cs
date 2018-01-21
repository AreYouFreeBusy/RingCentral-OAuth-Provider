//  Copyright 2017 Stefan Negritoiu (FreeBusy). See LICENSE file for more information.

using System;

namespace Owin.Security.Providers.RingCentral
{
    public static class RingCentralAuthenticationExtensions
    {
        public static IAppBuilder UseRingCentralAuthentication(this IAppBuilder app, RingCentralAuthenticationOptions options)
        {
            if (app == null)
                throw new ArgumentNullException("app");
            if (options == null)
                throw new ArgumentNullException("options");

            app.Use(typeof(RingCentralAuthenticationMiddleware), app, options);

            return app;
        }

        public static IAppBuilder UseRingCentralAuthentication(this IAppBuilder app, string clientId, string clientSecret)
        {
            return app.UseRingCentralAuthentication(new RingCentralAuthenticationOptions
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            });
        }
    }
}