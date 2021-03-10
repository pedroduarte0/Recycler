using System;
using System.Threading.Tasks;

namespace BusinessLogic.FrameworkAbstractions
{
    /// <summary>
    /// Abstraction for threading related methods.
    /// </summary>
    public interface IThreadWrapper
    {
        Task TaskFactoryStartNew(Action action);
        void ThreadSleep(int millisecondsTimeout);
    }
}
