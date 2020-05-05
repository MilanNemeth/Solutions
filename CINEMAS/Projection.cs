#define DEBUG
#undef DEBUG

using System;

namespace Cinemas
{
    /// <summary>
    /// Projections are events in different <see cref="Auditorium"/>s, about different <see cref="Movie"/>s.
    /// </summary>
    class Projection
    {
        public string Name { get; private set; }
        private enum Seat
        {
            Available = 0x0,
            UnAvailable = 0x1
        }
        private enum TableElement
        {
            row,
            column
        }
        public Auditorium OwnerAuditorium { get; private set; }
        public Movie OwnMovie { get; private set; }
        private Seat[,] Seats { get; set; }
        public short ReservedSeatsCount { get; private set; } = 0;

        public Projection(Auditorium OwnerAuditorium, Movie OwnMovie)
        {
            #region debug message
#if DEBUG
            Program.LogThisCaller();
#endif
            #endregion
            this.OwnerAuditorium = OwnerAuditorium;
            this.OwnMovie = OwnMovie;
            Name = OwnMovie.Name;
            Seats = new Seat[OwnerAuditorium.Rows,OwnerAuditorium.Columns];
            InitSeats();
            if (!ObjectContainer.PDB.Contains(this))
            {
                ObjectContainer.PDB.Add(this);

            }
        }

        #region Seating Methods
        private void InitSeats()
        {
            #region debug message
#if DEBUG
            Program.LogThisCaller();
#endif
            #endregion
            byte rows = OwnerAuditorium.Rows;
            byte cols = OwnerAuditorium.Columns;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Seats[i, j] = Seat.Available;
                }
            }
        }
        public void PrintOwnSeatsByAvailability()
        {
            #region debug message
#if DEBUG
            Program.LogThisCaller();
#endif
            #endregion
            char Av = '■';
            char UnAv = '■';
            //□■ ○● ☺☻
            byte rows = OwnerAuditorium.Rows;
            byte cols = OwnerAuditorium.Columns;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    switch (Seats[i,j])
                    {
                        case Seat.Available:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write($"{Av} ");
                            goto ResetColor;
                        case Seat.UnAvailable:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write($"{UnAv} ");
                            goto ResetColor;
                        default:
                            break;
                        ResetColor:
                            Console.ResetColor();
                            break;
                    }
                }
                Console.WriteLine();
            }
            Console.ReadKey(false);
        }
        public void ReserveSeat()
        {
            #region debug message
            #if DEBUG
            Program.LogThisCaller();
            #endif
            #endregion
            byte row = CheckAndReturnPosition(TableElement.row);
            byte col = CheckAndReturnPosition(TableElement.column);
            if (GetSeatAvailability(row, col) == Seat.Available)
            {
                FlipSeatAvailabilty(row, col);
                ReservedSeatsCount++;
            }
            else
            {
                IO_Handler.ErrorMessage("Already taken!");
            }
        }
        public void FreeSeat()
        {
            #region debug message
            #if DEBUG
            Program.LogThisCaller();
            #endif
            #endregion
            byte row = CheckAndReturnPosition(TableElement.row);
            byte col = CheckAndReturnPosition(TableElement.column);
            if (GetSeatAvailability(row, col) == Seat.UnAvailable)
            {
                FlipSeatAvailabilty(row, col);
                ReservedSeatsCount--;
            }
            else
            {
                IO_Handler.ErrorMessage("Still free!");
            }
        }
        private Seat GetSeatAvailability(byte row, byte col)
        {
            #region debug message
#if DEBUG
            Program.LogThisCaller();
#endif
            #endregion
            return Seats[row, col];
        }
        private void FlipSeatAvailabilty(byte row, byte col)
        {
            #region debug message
#if DEBUG
            Program.LogThisCaller();
#endif
            #endregion
            Seats[row, col] = Seats[row, col] ^ Seat.UnAvailable;
        }
        private byte CheckAndReturnPosition(TableElement element)
        {
            byte input;
            if (element==TableElement.row)
            {
                do
                {
                    input = IO_Handler.EnterByte("Please, enter the number of rows: ");
                    if (input < 1 || input > OwnerAuditorium.Rows)
                    {
                        IO_Handler.ErrorMessage($"Invalid input. Pick from range [1-{OwnerAuditorium.Rows}]");
                    }
                } while (input < 1 || input > OwnerAuditorium.Rows);
            }
            else if(element == TableElement.column)
            {
                do
                {
                    input = IO_Handler.EnterByte("Please, enter the number of columns: ");
                    if (input < 1 || input > OwnerAuditorium.Columns)
                    {
                        IO_Handler.ErrorMessage($"Invalid input. Pick from range [1-{OwnerAuditorium.Columns}]");
                    }
                } while (input < 1 || input > OwnerAuditorium.Columns);
            }
            else
            {
                throw new InvalidOperationException("There's no such TableElement! Where the hell did you get that from???");
            }
            return (byte)(input-1);
        }
        #endregion

        #region OVERRIDES
        public override string ToString()
        {
            #region debug message
            #if DEBUG
            Program.LogThisCaller();
            #endif
            #endregion
            return $"Projection of:\"{Name}\"";
        }
        public override bool Equals(object obj)
        {
            #region debug message
            #if DEBUG
            Program.LogThisCaller();
            #endif
            #endregion
            return obj is Projection projection
                && String.Equals(this.Name, projection.Name)
                && this.OwnerAuditorium.Equals(projection.OwnerAuditorium);
        }
        #endregion
    }
}
