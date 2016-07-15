using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Echovoice.UniversalAnalytics
{
    /// <summary>
    /// Metering data temporary queue.
    /// </summary>
    public class MeteringDataQueue
    {
        #region Fields

        private ConcurrentQueue<HttpWebRequest> rawData = new ConcurrentQueue<HttpWebRequest>();

        #endregion Fields

        #region Methods

        /// <summary>
        /// Asynchronously gets the metric data item count.
        /// </summary>
        /// <returns>Number of <see cref="ICoreMeteringData" /> items in the queue.</returns>
        public Task<int> CountAsync()
        {
            return CountDataAsync();
        }

        /// <summary>
        /// Asynchronously dequeue the metric data.
        /// </summary>
        /// <returns><see cref="ICoreMeteringData" />.</returns>
        public Task<HttpWebRequest> DequeueAsync()
        {
            return DequeueDataAsync();
        }

        /// <summary>
        /// Asynchronously enqueue metric to temporary buffer.
        /// </summary>
        /// <param name="e">The <see cref="ICoreMeteringData" /> collection.</param>
        /// <returns></returns>
        public virtual Task EnqueueAsync(IEnumerable<HttpWebRequest> e)
        {
            return EnqueueDataAsync(e);
        }

        /// <summary>
        /// Asynchronously enqueue metric to temporary buffer.
        /// </summary>
        /// <param name="e">The <see cref="ICoreMeteringData" />.</param>
        /// <returns></returns>
        public Task EnqueueAsync(HttpWebRequest e)
        {
            return EnqueueAsync(new List<HttpWebRequest>() { e });
        }

        /// <summary>
        /// Asynchronously peek the metric data.
        /// </summary>
        /// <returns><see cref="ICoreMeteringData" />.</returns>
        public Task<HttpWebRequest> PeekAsync()
        {
            return PeekDataAsync();
        }

        /// <summary>
        /// Asynchronously gets the metric data item count.
        /// </summary>
        /// <returns>Number of <see cref="ICoreMeteringData" /> items in the queue.</returns>
        protected virtual Task<int> CountDataAsync()
        {
            return Task.FromResult(rawData.Count);
        }

        /// <summary>
        /// Asynchronously dequeue the metric data.
        /// </summary>
        /// <returns><see cref="ICoreMeteringData" />.</returns>
        protected virtual Task<HttpWebRequest> DequeueDataAsync()
        {
            HttpWebRequest result;
            if (rawData.TryDequeue(out result))
            {
                return Task.FromResult(result);
            }
            return Task.FromResult(default(HttpWebRequest));
        }

        /// <summary>
        /// Asynchronously enqueues the metric data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected virtual Task EnqueueDataAsync(IEnumerable<HttpWebRequest> data)
        {
            foreach (var item in data)
            {
                rawData.Enqueue(item);
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// Asynchronously peek the metric data.
        /// </summary>
        /// <returns><see cref="ICoreMeteringData" />.</returns>
        protected virtual Task<HttpWebRequest> PeekDataAsync()
        {
            HttpWebRequest result;
            if (rawData.TryPeek(out result))
            {
                return Task.FromResult(result);
            }
            return Task.FromResult(default(HttpWebRequest));
        }

        #endregion Methods
    }
}