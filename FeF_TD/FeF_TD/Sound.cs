using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 *  Author Kighten
 */

namespace FeF_TD
{
    public class Sound
    {

#region variables

        private string _path;

#endregion



#region properties
   
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }

#endregion



#region methodes

        public Sound()
        {

        }

        public void Play()
        {

        }

#endregion

    }
}
