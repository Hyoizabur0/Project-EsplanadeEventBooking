using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsplanadeEventBooking.Client.Static
{
    public static class Endpoints
    {
        private static readonly string Prefix = "api";

        public static readonly string BookeventsEndpoint = $"{Prefix}/bookevent";
        public static readonly string EusersEndpoint = $"{Prefix}/euser";
        public static readonly string CreatorsEndpoint = $"{Prefix}/creator";
        public static readonly string TicketsEndpoint = $"{Prefix}/tickets";
        public static readonly string LocationsEndpoint = $"{Prefix}/location";
    }
}
