#define DEBUG
#undef DEBUG

using System;
using System.Collections.Generic;

namespace Cinemas
{
    /// <summary>
    /// A hall of a cinema theatre. The <see cref="Cinema"/> class aggregates the <see cref="Auditorium"/> class.
    /// </summary>
    class Auditorium
    {
        public byte Id { get; private set; }
        public byte Rows { get; private set; }
        public byte Columns { get; private set; }
        public Dictionary<string, Projection> OwnProjections { get; private set; }
        public Cinema OwnerCinema;

        public Auditorium(byte Id, Cinema Owner, byte Rows, byte Columns)
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
#endif
            #endregion
            this.Id = Id;
            this.OwnerCinema = Owner;
            this.Rows = Rows;
            this.Columns = Columns;
            OwnProjections = new Dictionary<string, Projection>();
        }

        #region Projection Methods
        public void AddNewProjection()
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
#endif
            #endregion
            if (OwnProjections.Count < 5)
            {
                string movieName = IO_Handler.EnterString($"{OwnerCinema}/{this}:\n" +
                    $"Enter the name of the movie beeing projected: ").ToUpper();
                if (OwnProjections.ContainsKey(movieName))
                {
                    throw new OperationCanceledException("Operation canceled: This movie has already beeing projected here!");
                }
                byte movieLength = IO_Handler.EnterByte("Enter the length of this movie in minutes: ");
                Movie currentMovie = new Movie(movieName, movieLength);
                TestAndCreate(currentMovie);
            }
            else
            {
                IO_Handler.ErrorMessage("Projection Limit Reached");
            }
        }
        private void TestAndCreate(Movie movie)
        {
                try
                {
                    OwnProjections.Add(movie.Name, new Projection(this, movie));
                    IO_Handler.SuccessMessage($"New projection \"{movie.Name}\" has been added");
                }
                catch (Exception e)
                {
                    throw e;
                }
        }
        public Projection FindProjectionByName()
        {
            #region debug message
#if DEBUG
                    IO_Handler.LogItsCaller();
#endif
            #endregion
            if (OwnProjections.Count > 0)
            {
                return ReturnProjectionByName();
            }
            else
            {
                throw new InvalidOperationException("Related to this Auditorium, there hasn't been any Projections created yet.");
            }
        }      //Not implemented in PresentationLayer, so become obsolete:
        private Projection ReturnProjectionByName()
        {
            #region debug message
#if DEBUG
                    IO_Handler.LogItsCaller();
#endif
            #endregion
            string projectionName = "";
            while (!OwnProjections.ContainsKey(projectionName))
            {
                projectionName = IO_Handler.EnterString("Name of the movie you are looking for: ").ToUpper();
                if (!OwnProjections.ContainsKey(projectionName))
                {
                    IO_Handler.ErrorMessage($"There is no such Projection in this Auditorium No#{Id} with the given name!");
                    Console.WriteLine("Please, pick one from the following:");
                    IO_Handler.PrintCollection(OwnProjections.Keys);
                }
            }
            Projection Result = OwnProjections[projectionName];
            IO_Handler.SuccessMessage("Projection found!");
            return Result;
        }   //Not implemented in PresentationLayer, so become obsolete:
        #endregion

        #region OVERRIDES
        public override string ToString()
        {
            #region debug message
            #if DEBUG
            IO_Handler.LogItsCaller();
            #endif
            #endregion
            return $"Auditorium No.{Id}.";
        }
        public override bool Equals(object obj)
        {
            #region debug message
            #if DEBUG
            IO_Handler.LogItsCaller();
            #endif
            #endregion
            return obj is Auditorium auditorium
                && (Id == auditorium.Id);
        }
        #endregion
    }
}
