#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Singelton
{
    public class NSingelton<T> where T : NSingelton<T>
    {
        #region Singelton

        /// <summary>
        /// Static Readonly property.
        /// </summary>
        private static readonly Lazy<T> Lazy = new Lazy<T>(() => 
        {
            T ret = default(T);
            lock (typeof(T))
            {
                ret = Activator.CreateInstance(typeof(T), true) as T;
            }
            return ret;
        });
        /// <summary>
        /// Gets singleton instance.
        /// </summary>
        public static T Instance => Lazy.Value;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected NSingelton() : base() 
        {
            Console.WriteLine("Ctor:Base class");
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~NSingelton() { }

        #endregion
    }
}
