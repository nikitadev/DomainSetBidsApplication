//*********************************************************
//
// Copyright (c) 2015 nikitadev. All rights reserved.
//
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using RegAPI.Library.Models.Interfaces;

namespace RegAPI.Library.Models.Implements
{
    public class RequestManager : IRequestManager
    {
        public async Task<string> Get(Uri uri)
        {
            string url = uri.ToString();
            string result;
            using (var httpClient = new HttpClient())
            {
                result = await httpClient.GetStringAsync(url);
            }

            return result;
        }

        public async Task<T> GetObject<T>(Uri uri)
        {
            string json = await Get(uri);

            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<JToken> GetJToken(Uri uri)
        {
            string json = await Get(uri);

            return JContainer.Parse(json);
        }

        public async Task<string> Post<T>(Uri uri, T data) where T : class
        {
            string url = uri.ToString();
            string result;
            using (var httpClient = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(data);

                var stringContent = new StringContent(json);
                var message = await httpClient.PostAsync(url, stringContent);

                message.EnsureSuccessStatusCode();
                result = await message.Content.ReadAsStringAsync();
            }

            return result;
        }
    }
}
