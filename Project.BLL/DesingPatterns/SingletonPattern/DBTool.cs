using Project.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DesingPatterns.SingletonPattern
{
    public class DBTool
    {
         DBTool()
        {

        }

        static MyContext _dBInstance;

        public static MyContext DBInstance
        {
            get {
                if (_dBInstance==null)
                {
                    _dBInstance = new MyContext();
                }
                return _dBInstance; }
        }
    }
}
