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

using System;
using System.Diagnostics.Contracts;
using RegAPI.Library.Models.Interfaces;

namespace RegAPI.Library.Models.Infostructures
{
    /// <summary>
    /// 
    /// </summary>
    [ContractClass(typeof(BaseApiProviderContract))]
    public abstract class BaseApiProvider
    {
        protected readonly IRequestManager _requestManager;

        public BaseApiProvider(IRequestManager requestManager)
		{
			_requestManager = requestManager;
		}

        protected BaseApiProvider()
        {
        }

        /// <summary>
        /// Создаёт правильный url по параметрам
        /// </summary>
        /// <param name="path"></param>
        /// <param name="settings"></param>
        /// <returns>{scheme}://{api}/{type}?{settings}</returns>
        protected virtual Uri CreateUri(string path, IQuerySettings querySettings)
        {
            if (querySettings != null)
                return CreateUri(path, querySettings.ToString());

            return CreateUri(path);
        }

        protected virtual Uri CreateUri(string path = null, string query = null)
        {
            var uriBuilder = GetBaseUri();

            if (!String.IsNullOrEmpty(path))
                uriBuilder.Path += path;

            if (!String.IsNullOrEmpty(query))
                uriBuilder.Query = query;

            return uriBuilder.Uri;
        }

        protected abstract UriBuilder GetBaseUri();
    }

    [ContractClassFor(typeof(BaseApiProvider))]
    internal abstract class BaseApiProviderContract : BaseApiProvider
    {
        protected override UriBuilder GetBaseUri()
        {
            Contract.Ensures(Contract.Result<UriBuilder>() != null);

            return default(UriBuilder);
        }

        [ContractInvariantMethod]
        private void Validate()
        {
            Contract.Invariant(_requestManager != null);
        }
    }
}
