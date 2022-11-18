using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DevAttic.ConfigCrypter.Helpers
{
    //https://social.msdn.microsoft.com/Forums/en-US/163ef755-ff7b-4ea5-b226-bbe8ef5f4796/is-there-a-pattern-for-calling-an-async-method-synchronously?forum=async

    /// <summary>
    /// customerList = AsyncHelpers.RunSync.<List<Customer>>(() => GetCustomers());
    /// </summary>
    public static class AsyncHelpers
    {
        /// <summary>
        /// Execute's an async Task.<T> method which has a void return value synchronously
        /// </summary>
        /// <param name="task">Task<T> method to execute</param>
        public static void RunSync(Func<Task> task)
        {
            var oldContext = SynchronizationContext.Current;
            using var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
            synch.Post(async _ =>
            {
                try
                {
                    await task().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            },
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates
            null
            );
            synch.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oldContext);
        }


        /// Execute's an async Task.<T> method which has a T return type synchronously.
        /// </summary>
        /// <typeparam name="T">Return Type.</typeparam>
        /// <param name="task">Task method to execute.</param>
        /// <returns>T.</returns>
        public static T RunSync<T>(Func<Task<T>> task)
        {
            var oldContext = SynchronizationContext.Current;
            using var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            T ret = default;

#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
            synch.Post(async _ =>
            {
                try
                {
                    ret = await task().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            },
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates
            null
            );

            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(oldContext);
#pragma warning disable CS8603 // Possible null reference return.
            return ret;
#pragma warning restore CS8603 // Possible null reference return.
        }

        private class ExclusiveSynchronizationContext : SynchronizationContext, IDisposable
        {
            private bool done;
            public Exception InnerException { get; set; }

            private readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);
            private readonly Queue<Tuple<SendOrPostCallback, object>> items =
                new Queue<Tuple<SendOrPostCallback, object>>();

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("We cannot send to our same thread");
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                lock (items)
                {
                    items.Enqueue(Tuple.Create(d, state));
                }
                workItemsWaiting.Set();
            }

            public void EndMessageLoop()
            {
                Post(_ => done = true, null);
            }

            public void BeginMessageLoop()
            {
                while (!done)
                {
                    Tuple<SendOrPostCallback, object> task = null;
                    lock (items)
                    {
                        if (items.Count > 0)
                        {
                            task = items.Dequeue();
                        }
                    }
                    if (task != null)
                    {
                        task.Item1(task.Item2);
                        if (InnerException != null) // the method threw an exeption
                        {
                            throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
                        }
                    }
                    else
                    {
                        workItemsWaiting.WaitOne();
                    }
                }
            }

            public override SynchronizationContext CreateCopy()
            {
                return this;
            }

            #region Dispose
            // Dispose() calls Dispose(true)
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            // NOTE: Leave out the finalizer altogether if this class doesn't 
            // own unmanaged resources itself, but leave the other methods
            // exactly as they are. 
            ~ExclusiveSynchronizationContext()
            {
                // Finalizer calls Dispose(false)
                Dispose(false);
            }

            // The bulk of the clean-up code is implemented in Dispose(bool)
            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    // free managed resources
                    workItemsWaiting.Dispose();
                }
                // free native resources here if there are any
            }

            #endregion
        }
    }
}
