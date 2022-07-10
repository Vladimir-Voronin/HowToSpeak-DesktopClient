using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowToSpeak_DesktopClient.Infrastructure.Server
{
    class HTSServer
    {
        private static HTSServer instance;
        private static object syncRoot = new Object();

        private const string base_url_additional = "/howtospeak/api";

        private string _domen;

        private readonly string _base_url;

        protected HTSServer(string domen)
        {
            _domen = domen;
            _base_url = domen + base_url_additional;
        }

        public static HTSServer getInstance(string name)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new HTSServer(name);
                }
            }
            return instance;
        }


    }
}
