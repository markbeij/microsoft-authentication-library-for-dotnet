﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Net;
using System.Net.Http;
using Microsoft.Identity.Client.Http;

namespace Microsoft.Identity.Client.Platforms.net45.Http
{
    internal class NetDesktopHttpClientFactory : IMsalHttpClientFactory
    {
        private static HttpClient s_httpClient;

        public HttpClient GetHttpClient()
        {
            EnsureInitialized();
            return s_httpClient;
        }

        private static void EnsureInitialized()
        {
            if (s_httpClient == null)
            {
                s_httpClient = new HttpClient(new DnsSensitiveClientHandler());

                HttpClientConfig.ConfigureRequestHeadersAndSize(s_httpClient);
                ConfigureServicePointManager();
            }
        }

        private static void ConfigureServicePointManager()
        {
            // Default is 2 minutes, see https://msdn.microsoft.com/en-us/library/system.net.servicepointmanager.dnsrefreshtimeout(v=vs.110).aspx
            ServicePointManager.DnsRefreshTimeout = (int)HttpClientConfig.ConnectionLifeTime.TotalMilliseconds;

            // Increases the concurrent outbound connections
            ServicePointManager.DefaultConnectionLimit = HttpClientConfig.MaxConnections;
        }
    }
}
