
using JetBrains.Annotations;
using Model.Deity;

namespace Meta.EventArgs
{
    public class DeityEventArgs : System.EventArgs
    {
        [CanBeNull]
        public readonly Deity Deity;

        public DeityEventArgs(Deity currentDeity)
        {
            Deity = currentDeity;
        }
    }
}