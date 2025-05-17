using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Serialization;

namespace wpfHUSH.Model
{
    public class Swipes
    {     
        public int SwiperId { get; set; }
        public int SwipedId { get; set; }
        public bool Action { get; set; }
        public bool IsNotificated { get; set; }
        public User Swiper { get; set; }
        public User Swiped { get; set; }

        public string Message
        {
            get
            {
                if (Swiper.Gender == false)
                    return "Ты понравилась!";
                else
                    return "Ты понравился!";
            }
        }

        public bool IsNew => !IsNotificated;

    }
}
