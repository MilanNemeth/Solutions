#define DEBUG
#undef DEBUG

using System;

namespace Cinemas
{
    class Movie : CommonAttributes
    {
        public TimeSpan MinutesOfLength { get; private set; }

        public Movie(string Name, byte MinutesOfLength=0) : base(Name)
        {
            #region debug message
#if DEBUG
            Program.LogThisCaller();
#endif
            #endregion
            this.MinutesOfLength = TimeSpan.FromMinutes(MinutesOfLength);
            if (!ObjectContainer.MDB.Contains(this))
            {
                ObjectContainer.MDB.Add(this);
            }
        }

        #region OVERRIDES
        public override string ToString()
        {
            #region debug message
#if DEBUG
            Program.LogThisCaller();
#endif
            #endregion
            return Name;
        }
        public override bool Equals(object obj)
        {
            #region debug message
#if DEBUG
            Program.LogThisCaller();
#endif
            #endregion
            return obj is Movie movie
                && String.Equals(Name, movie.Name);
        }
        #endregion
    }
}