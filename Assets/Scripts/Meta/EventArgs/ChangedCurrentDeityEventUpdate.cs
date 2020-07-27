
using JetBrains.Annotations;
using Model.Deity;

namespace Meta.EventArgs
{
    public class ChangedCurrentDeityEventUpdate : System.EventArgs
    {
        [CanBeNull]
        public readonly Deity ChangedCurrentDeity;

        public ChangedCurrentDeityEventUpdate(Deity currentDeity)
        {
            ChangedCurrentDeity = currentDeity;
        }
    }
}