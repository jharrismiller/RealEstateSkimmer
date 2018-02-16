using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Common
{
    public static class RandomExtensions
    {

        public static void SetUserAgent(this HttpRequestHeaders httpRequestHeaders)
        {
            httpRequestHeaders.Remove("User-Agent");
            httpRequestHeaders.Add("User-Agent", UserAgents.Agents[0]);
        }

        public static void SetUserAgent(this HttpRequestHeaders httpRequestHeaders, int index)
        {
            httpRequestHeaders.Remove("User-Agent");
            httpRequestHeaders.Add("User-Agent", UserAgents.Agents[index]);
        }

        public static void SetUserAgentAndRecycleIndex(this HttpRequestHeaders httpRequestHeaders, int index)
        {
            httpRequestHeaders.Remove("User-Agent");
            if (index > UserAgents.Agents.GetUpperBound(0)) index = 0;
            httpRequestHeaders.SetUserAgent(index);
        }

    }
}
