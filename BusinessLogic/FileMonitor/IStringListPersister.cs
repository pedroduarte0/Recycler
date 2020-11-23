using System.Collections.Generic;

namespace BusinessLogic.FileMonitor
{
    public interface IStringListPersister
    {
        void Persist(IList<string> list);
    }
}
