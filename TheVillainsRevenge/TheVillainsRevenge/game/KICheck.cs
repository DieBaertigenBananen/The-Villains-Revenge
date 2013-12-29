using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheVillainsRevenge
{
    class KICheck
    {
        //Liste der IDs der KIPoints
        //Werden vom Spieler gespeichert sobald er an sie vorbeigeht
        public int time;
        public int id;
        public KICheck(int ntime, int nid)
        {
            time = ntime;
            id = nid;
        }

    }
}
