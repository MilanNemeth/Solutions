#define DEBUG
#undef DEBUG

using System;
using System.Collections.Generic;

namespace Cinemas
{
    /// <summary>
    /// <see cref="Cinema"/> is a huge place within one or more <see cref="Auditorium"/>s, where <see cref="Projection"/>s take place about different <see cref="Movie"/>s.
    /// </summary>
    class Cinema : CommonAttributes
    {
        public Dictionary<byte, Auditorium> OwnAuditoriums { get; private set; }

        public Cinema(string Name, byte NumberOfAuditoriums) : base(Name)
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
#endif
            #endregion
            OwnAuditoriums = new Dictionary<byte, Auditorium>();
            InitAuditoriums(NumberOfAuditoriums);
            ObjectContainer.CDB.Add(this);
        }

        #region Auditorium Methods
        private void InitAuditoriums(byte NumberOfAuditoriums)
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
#endif
            #endregion
            byte rows;
            byte cols;
            byte id;
            for (int indexer = 0; indexer < NumberOfAuditoriums; indexer++)
            {
                id = (byte)(indexer + 1);
                do
                {
                    rows = IO_Handler.EnterByte($"Number of rows for Auditorium #No.{id}: "); 
                } while (rows<1);
                do
                {
                    cols = IO_Handler.EnterByte($"Number of columns for Auditorium #No.{id}: "); 
                } while (cols<1);
                OwnAuditoriums.Add(id,new Auditorium(id, this, rows, cols));
            }
        }

        public Auditorium FindAuditorium()
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
#endif
            #endregion
            byte audNo = IO_Handler.EnterByte("Enter the #No. of the Auditorium you are looking for: ");

            if (OwnAuditoriums.Count < 1)
            {
                throw new ArgumentOutOfRangeException($"There hasn't been any Auditoriums created yet in this Cinema: {Name}");
            }
            else if(audNo <= OwnAuditoriums.Count)
            {
                return TryToReturn(audNo);
            }
            else
            {
                throw new ArgumentOutOfRangeException($"You have entered a too large value.\n" +
                    $"Please, select within the range of [ 1 - {OwnAuditoriums.Count} ]");
            }
            
        }
        private Auditorium TryToReturn(byte audNo)
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
#endif
            #endregion
            try
            {
                return OwnAuditoriums[audNo];
            }
            catch (Exception e)
            {
                throw new Exception($"There\'s no such Auditorium in this Cinema: {Name}\n\n", e);
            }
        }
        #endregion

        #region OVERRIDES
        public override string ToString()
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
#endif
            #endregion
            return Name;
        }
        public override bool Equals(object obj)
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
#endif
            #endregion
            return obj is Cinema cinema
                 && (Name == cinema.Name);
        }
        #endregion
    }
}
