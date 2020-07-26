using System;

namespace Model.Deity
{
    [Serializable]
    public class Deity
    {
        public int identifier;
        public string name;
        public int currentPowerPoints;

        public Deity() {}
        public Deity(int identifier, string name = "", int currentPowerPoints = 0)
        {
            this.identifier = identifier;
            this.name = name;
            this.currentPowerPoints = currentPowerPoints;
        }

        protected bool Equals(Deity other)
        {
            return identifier == other.identifier;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Deity) obj);
        }

        public override int GetHashCode()
        {
            return identifier;
        }

        public static bool operator ==(Deity left, Deity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Deity left, Deity right)
        {
            return !Equals(left, right);
        }
    }
}