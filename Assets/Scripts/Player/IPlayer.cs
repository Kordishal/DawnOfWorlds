using Model.Deity;

namespace Player
{
    public interface IPlayer
    {
        string Name { get; set; }
        Deity ActiveDeity { get; set; }
        void RefreshPowerPoints();
    }
}