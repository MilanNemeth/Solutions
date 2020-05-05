#define DEBUG
#undef DEBUG

namespace Cinemas
{
    abstract class CommonAttributes
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                name = value.ToUpper();
            }
        }
        public CommonAttributes(string Name)
        {
            #region debug message
#if DEBUG
            IO_Handler.LogItsCaller();
#endif
            #endregion
            this.Name = Name;
        }
    }
}
