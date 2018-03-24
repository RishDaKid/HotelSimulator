using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Actors;
using System.Drawing;
using System.Timers;
using System.IO;
using Newtonsoft.Json;
using HotelSimulatie.Enums;

namespace HotelSimulatie.Facilities
{
    class Cinema : LocationType
    {
        // status of movie
        public bool MovieStatus { get; private set; } = false;

        public Cinema(Facility specs) : base(specs)
        {
            Image = Image.FromFile("../../Resources/cinema.png");
        }

        /// <summary>
        /// Check if movie has been started
        /// </summary>
        /// <param name="drawUpdateTime"></param>
        public override void Update(double drawUpdateTime)
        {
            CheckMovieStatus(drawUpdateTime);
        }

        /// <summary>
        /// Check if movie has been started
        /// </summary>
        /// <param name="drawUpdateTime"></param>
        public void CheckMovieStatus(double drawUpdateTime)
        {
            // if true
            if(MovieStatus == true)
            {
                // then count down till movie is over
                OneHTE -= (int)drawUpdateTime / HTE.CinemaTimeUnit;

                if (OneHTE < 1)
                {
                    // let the visitor leave the cinema
                    LeaveCinema();
                    // reset the value again
                    OneHTE = 1000;
                    // set movestatus on falsse because it's over
                    MovieStatus = false;
                }
            }
        }

        /// <summary>
        /// interact in a particular way with humans
        /// </summary>
        /// <param name="human"></param>
        public override void Interact(Human human)
        {
            // when it's a visitor
            if (human is Visitor)
            {
                // when movie is not started
                if (MovieStatus == false)
                {
                    // let visitor in
                    JoinFacility(human);
                }
                else
                {
                    // tell visitor that we are done helping him
                    //Console.WriteLine("Good luck going back mate ;)");
                    (human as Visitor).Communicate(InteractStatus.DONE_BEING_HELPED);
                }
            }
            else
            {
                // let the cleaner join the facility
                JoinFacility(human);
            }
        }

        /// <summary>
        /// Start the movie
        /// </summary>
        public void StartMovie()
        {
            MovieStatus = true;
        }

        /// <summary>
        /// Deleting all visitors and tell them they can leave
        /// </summary>
        public void LeaveCinema()
        {
            for (int i = 0; i < CurrentHumans.Count; i++)
            {
                (CurrentHumans[i] as Visitor).Communicate(InteractStatus.DONE_BEING_HELPED);
            }
            CurrentHumans.Clear();
        }
    }
}
