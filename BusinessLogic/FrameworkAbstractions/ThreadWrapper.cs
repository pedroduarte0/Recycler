using System;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.FrameworkAbstractions
{
    ///<inheritdoc/>
    public class ThreadWrapper : IThreadWrapper
    {
        public Task TaskFactoryStartNew(Action action)
        {
            return Task.Factory.StartNew(action);
        }

        public void ThreadSleep(int millisecondsTimeout)
        {
            Thread.Sleep(millisecondsTimeout);
        }
    }
}
