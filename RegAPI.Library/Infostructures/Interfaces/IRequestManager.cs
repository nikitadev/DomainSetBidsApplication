using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace RegAPI.Library.Models.Interfaces
{
	[ContractClass(typeof(RequestManagerContract))]
    public interface IRequestManager
    {
        /// <summary>
        /// Возвращает текст
        /// </summary>
        /// <param name="uri">путь</param>
        /// <returns></returns>
        Task<string> Get(Uri uri);

        /// <summary>
        /// Возвращает объект
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">путь</param>
        /// <returns></returns>
        Task<T> GetObject<T>(Uri uri);

        /// <summary>
        /// Возвращает JToken
        /// </summary>
        /// <param name="uri">путь</param>
        /// <param name="isToken">токен</param>
        /// <param name="isGzip"></param>
        /// <returns></returns>
        Task<JToken> GetJToken(Uri uri);

        /// <summary>
        /// Отправляет post запрос
        /// </summary>
        /// <param name="uri">путь</param>
        /// <param name="data">данные запроса</param>
        /// <returns></returns>
        Task<string> Post<T>(Uri uri, T data) where T : class;
    }

    [ContractClassFor(typeof(IRequestManager))]
    internal abstract class RequestManagerContract : IRequestManager
    {
        public Task<string> Get(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null, "uri is null.");

            return Task.FromResult(default(string));
        }

        public Task<T> GetObject<T>(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null, "uri is null.");

            return Task.FromResult(default(T));
        }

        public Task<JToken> GetJToken(Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null, "uri is null.");

            return Task.Factory.StartNew(() => default(JToken));
        }

        public Task<string> Post<T>(Uri uri, T data) where T : class
        {
            Contract.Requires<ArgumentNullException>(uri != null, "uri is null.");
            Contract.Requires<ArgumentNullException>(data != null, "data is null.");

            return Task.FromResult(default(string));
        }
    }
}
