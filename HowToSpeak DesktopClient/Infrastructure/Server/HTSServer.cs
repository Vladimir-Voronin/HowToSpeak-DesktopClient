﻿using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Security;

namespace HowToSpeak_DesktopClient.Infrastructure.Server
{
    static class HttpWebRequestExtension
    {
        public static bool TryAddCookie(this HttpWebRequest webRequest, CookieCollection cookieCollection)
        {
            if (webRequest == null)
            {
                return false;
            }
            if (cookieCollection == null)
            {
                return false;
            }

            if (webRequest.CookieContainer == null)
            {
                webRequest.CookieContainer = new CookieContainer();
            }
            foreach (var item in cookieCollection)
            {
                webRequest.CookieContainer.Add((Cookie)item);
            }
            return true;
        }
    }

    class ServerResponse
    {
        public bool status = true;
        public HttpStatusCode status_code;
        public CookieCollection cookies;
        public WebHeaderCollection headers;
        public string status_desctiption;
        public string json_string;

        public bool IsOk()
        {
            return status;
        }

        public string getJsonByKey(string key)
        {
            if (json_string == null)
                return "";
            var d = JsonConvert.DeserializeObject<Dictionary<string, string>>(json_string);
            if (d.ContainsKey(key))
                return d[key];
            return "";
        }

    }

    class HTSServer
    {
        private static HTSServer instance;
        private static object syncRoot = new Object();

        private const string _url_additional = "/howtospeak/api";

        private string _domen;
        private CookieCollection _last_cookies;

        private readonly string _base_url;

        protected HTSServer(string domen)
        {
            _domen = domen;
            _base_url = domen + _url_additional;
        }

        public static HTSServer getInstance(string domen = null)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new HTSServer(domen);
                }
            }
            return instance;
        }

        public ServerResponse login(string username, string password)
        {
            string login_path = "/login";
            string full_path = _base_url + login_path;

            var json_d = new Dictionary<string, string>();
            json_d["username"] = username;
            json_d["password"] = password;
            var json = JsonConvert.SerializeObject(json_d);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(full_path);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.TryAddCookie(_last_cookies);

            var response = writeJson(httpWebRequest, json);
            if (!response.IsOk())
                return response;

            response = returnResponse(httpWebRequest);
            if (!response.IsOk())
                return response;

            _last_cookies = response.cookies;
            return response;
        }

        public ServerResponse register(string username, string password)
        {
            string register_path = "/register";
            string full_path = _base_url + register_path;

            var json_d = new Dictionary<string, string>();
            json_d["username"] = username;
            json_d["password"] = password;
            var json = JsonConvert.SerializeObject(json_d);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(full_path);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            var response = writeJson(httpWebRequest, json);
            if (!response.IsOk())
                return response;

            response = returnResponse(httpWebRequest);
            if (!response.IsOk())
                return response;

            _last_cookies = response.cookies;
            return response;
        }

        private ServerResponse writeJson(HttpWebRequest webRequest, string json)
        {
            HttpWebResponse httpResponse = null;
            ServerResponse serverResponse = new ServerResponse();
            try
            {
                using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }
            }
            catch (WebException e)
            {
                httpResponse = (HttpWebResponse)e.Response;
                serverResponse.status = false;

                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    serverResponse.status_code = httpResponse.StatusCode;
                    serverResponse.status_desctiption = httpResponse.StatusDescription;

                    using (Stream data = e.Response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        serverResponse.json_string = text;
                    }
                }
                else
                {
                    serverResponse.json_string = "{\'error\': \'Problems with connection to the server\'}";
                }
                return serverResponse;
            }
            return serverResponse;
        }


        private ServerResponse returnResponse(HttpWebRequest webRequest)
        {
            HttpWebResponse httpResponse = null;
            ServerResponse serverResponse = new ServerResponse();

            try
            {
                httpResponse = (HttpWebResponse)webRequest.GetResponse();
            }
            catch (WebException e)
            {
                httpResponse = (HttpWebResponse)e.Response;
                serverResponse.status = false;
                serverResponse.cookies = httpResponse.Cookies;
                serverResponse.headers = httpResponse.Headers;

                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    serverResponse.status_code = httpResponse.StatusCode;
                    serverResponse.status_desctiption = httpResponse.StatusDescription;

                    using (Stream data = e.Response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        serverResponse.json_string = text;
                    }
                }
                return serverResponse;
            }


            serverResponse.cookies = httpResponse.Cookies;
            serverResponse.headers = httpResponse.Headers;

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                serverResponse.json_string = streamReader.ReadToEnd();
            }

            return serverResponse;
        }
    }
}

