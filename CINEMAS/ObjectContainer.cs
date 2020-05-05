using System;
using System.Linq;
using System.Collections.Generic;

namespace Cinemas
{
    static class ObjectContainer
    {
        public static List<Movie> MDB = new List<Movie>();
        public static List<Cinema> CDB = new List<Cinema>();
        public static List<Projection> PDB = new List<Projection>();

        public static Projection FindProjectionMostViewed()
        {
            try
            {
                Projection MostViewed = PDB.OrderByDescending(i => i.ReservedSeatsCount).FirstOrDefault();
                return MostViewed;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
