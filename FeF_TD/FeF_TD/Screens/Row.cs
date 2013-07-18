using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeF_TD
{
    public class Row
    {
        private Bouton bouton;

        internal Bouton Bouton
        {
            get { return bouton; }
            set { bouton = value; }
        }
        private String[] headerStrings;

        public String[] HeaderStrings
        {
            get { return headerStrings; }
            set { headerStrings = value; }
        }

        public Row(Bouton b, String[] strings)
        {
            this.bouton = b;
            this.headerStrings = strings;
        }

    }
}
