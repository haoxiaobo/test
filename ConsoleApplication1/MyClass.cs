using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class MyClass
    {
        public string Name
        {
            get;
            set;
        }

        private DateTime _dtBirthday;
        public void SetDirthday(DateTime dt)
        {
            this._dtBirthday = dt;
        }

        private int _iChilds;
        public int Childs
        {
            get { return this._iChilds; }
            set { this._iChilds = value; }
        }

        public MyClass MakeCopy()
        {
            return (MyClass)this.MemberwiseClone();
        }
    }
}
