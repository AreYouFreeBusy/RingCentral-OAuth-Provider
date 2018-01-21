//  Copyright 2017 Stefan Negritoiu (FreeBusy). See LICENSE file for more information.

using System;
using System.Globalization;
using System.Security.Claims;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json.Linq;

namespace Owin.Security.Providers.RingCentral
{
    /// <summary>
    /// Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.
    /// </summary>
    public class RingCentralAuthenticatedContext : BaseContext
    {
        /// <summary>
        /// Initializes a <see cref="RingCentralAuthenticatedContext"/>
        /// </summary>
        /// <param name="context">The OWIN environment</param>
        /// <param name="user">The JSON-serialized user</param>
        /// <param name="accessToken">RingCentral access token</param>
        public RingCentralAuthenticatedContext(
            IOwinContext context, 
            string accessToken, string accessTokenExpires, 
            string refreshToken, string refreshTokenExpires,
            string scope,
            string userId, JObject userJson) 
            : base(context)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;

            int expiresValue;
            if (Int32.TryParse(accessTokenExpires, NumberStyles.Integer, CultureInfo.InvariantCulture, out expiresValue)) 
            {
                ExpiresIn = TimeSpan.FromSeconds(expiresValue);
            }

            if (Int32.TryParse(refreshTokenExpires, NumberStyles.Integer, CultureInfo.InvariantCulture, out expiresValue)) 
            {
                RefreshTokenExpiresIn = TimeSpan.FromSeconds(expiresValue);
            }

            string[] scopeSeparators = new string[1] { " " };
            Scope = scope.Split(scopeSeparators, StringSplitOptions.RemoveEmptyEntries);

            UserId = userId;
            if (userJson != null) 
            {
                // per http://ringcentral-api-docs.readthedocs.io/en/latest/account_extension/#retrieving-extension-data
                UserId = userJson["id"]?.Value<string>();
                Name = userJson["name"]?.Value<string>();
                Email = userJson["contact"]["email"]?.Value<string>();
                GivenName = userJson["contact"]["firstName"]?.Value<string>();
                Surname = userJson["contact"]["lastName"]?.Value<string>();
            }
        }

        /// <summary>
        /// Gets the RingCentral OAuth access token
        /// </summary>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Gets the scope for this RingCentral OAuth access token
        /// </summary>
        public string[] Scope { get; private set; }

        /// <summary>
        /// Gets the RingCentral access token expiration time
        /// </summary>
        public TimeSpan? ExpiresIn { get; private set; }

        /// <summary>
        /// Gets the RingCentral OAuth refresh token
        /// </summary>
        public string RefreshToken { get; private set; }

        /// <summary>
        /// Gets the RingCentral refresh token expiration time
        /// </summary>
        public TimeSpan? RefreshTokenExpiresIn { get; private set; }

        /// <summary>
        /// Gets the RingCentral user ID
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// Gets the email address
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Gets the user's first name
        /// </summary>
        public string GivenName { get; private set; }
        
        /// <summary>
        /// Gets the user's last name
        /// </summary>
        public string Surname { get; private set; }

        /// <summary>
        /// Gets the user's display name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the <see cref="ClaimsIdentity"/> representing the user
        /// </summary>
        public ClaimsIdentity Identity { get; set; }

        /// <summary>
        /// Gets or sets a property bag for common authentication properties
        /// </summary>
        public AuthenticationProperties Properties { get; set; }

        private static string TryGetValue(JObject user, string propertyName)
        {
            JToken value;
            return user.TryGetValue(propertyName, out value) ? value.ToString() : null;
        }
    }
}
