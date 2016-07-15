using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Echovoice.UniversalAnalytics
{
    /// <summary>
    /// Metering data temporary queue.
    /// </summary>
    public class MeteringDataQueue
    {
        #region Fields

        private ConcurrentQueue<StringBuilder> rawData = new ConcurrentQueue<StringBuilder>();

        #endregion Fields

        #region Methods

        /// <summary>
        /// Asynchronously gets the metric data item count.
        /// </summary>
        /// <returns>Number of <see cref="StringBuilder" /> items in the queue.</returns>
        public Task<int> CountAsync()
        {
            return CountDataAsync();
        }

        /// <summary>
        /// Asynchronously dequeue the metric data.
        /// </summary>
        /// <returns><see cref="StringBuilder" />.</returns>
        public Task<StringBuilder> DequeueAsync()
        {
            return DequeueDataAsync();
        }

        /// <summary>
        /// Asynchronously enqueue metric to temporary buffer.
        /// </summary>
        /// <param name="e">The <see cref="StringBuilder" /> collection.</param>
        /// <returns></returns>
        public virtual Task EnqueueAsync(IEnumerable<StringBuilder> e)
        {
            return EnqueueDataAsync(e);
        }

        /// <summary>
        /// Asynchronously enqueue metric to temporary buffer.
        /// </summary>
        /// <param name="e">The <see cref="StringBuilder" />.</param>
        /// <returns></returns>
        public Task EnqueueAsync(StringBuilder e)
        {
            return EnqueueAsync(new List<StringBuilder>() { e });
        }

        /// <summary>
        /// Asynchronously peek the metric data.
        /// </summary>
        /// <returns><see cref="StringBuilder" />.</returns>
        public Task<StringBuilder> PeekAsync()
        {
            return PeekDataAsync();
        }

        /// <summary>
        /// Asynchronously gets the metric data item count.
        /// </summary>
        /// <returns>Number of <see cref="StringBuilder" /> items in the queue.</returns>
        protected virtual Task<int> CountDataAsync()
        {
            return Task.FromResult(rawData.Count);
        }

        /// <summary>
        /// Asynchronously dequeue the metric data.
        /// </summary>
        /// <returns><see cref="StringBuilder" />.</returns>
        protected virtual Task<StringBuilder> DequeueDataAsync()
        {
            StringBuilder result;
            if (rawData.TryDequeue(out result))
            {
                return Task.FromResult(result);
            }
            return Task.FromResult(default(StringBuilder));
        }

        /// <summary>
        /// Asynchronously enqueues the metric data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected virtual Task EnqueueDataAsync(IEnumerable<StringBuilder> data)
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
        /// <returns><see cref="StringBuilder" />.</returns>
        protected virtual Task<StringBuilder> PeekDataAsync()
        {
            StringBuilder result;
            if (rawData.TryPeek(out result))
            {
                return Task.FromResult(result);
            }
            return Task.FromResult(default(StringBuilder));
        }

        #endregion Methods
    }
}