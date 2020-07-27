using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHealth.ApiTest.Auth
{
    public static class TestAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder builder, Action<TestAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(
                authenticationScheme: TestAuthenticationDefaults.AuthenticationScheme,
                displayName: TestAuthenticationDefaults.AuthenticationScheme,
                configureOptions: configureOptions
            );
        }
    }
}
