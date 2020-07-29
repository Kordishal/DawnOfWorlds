using System;

namespace Model.Geo.Support
{
    [Serializable]
    public class Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        /**
         * <summary>
         * A position is a neighbour of another position if they share an axis.
         *
         *<param name="other">The other position.</param>
         * <returns>true/false</returns>
         * </summary>
         */
        public bool IsNeighbour(Position other)
        {
            if (other.x == x)
            {
                return other.y == y + 1 || other.y == y - 1;
            }

            if (other.y == y)
            {
                return other.x == x + 1 || other.x == x - 1;
            }

            return false;
        }

        protected bool Equals(Position other)
        {
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x * 397) ^ y;
            }
        }

        public static bool operator ==(Position left, Position right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !Equals(left, right);
        }


    }
}