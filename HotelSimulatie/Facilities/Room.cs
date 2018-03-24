using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using HotelSimulatie.Actors;

namespace HotelSimulatie.Facilities
{
    public class Room : LocationType
    {
        // How high the rank is of this room
        public string Classification { get; set; }
        // If this room is empty or not, we decide with this boo
        public bool Empty { get;  set; } = true;

        public Room(Facility specs) : base(specs)
        {
            Classification = specs.Classification;
            InitializeImage();
        }

        /// <summary>
        /// Updating this object based on the gameloop
        /// </summary>
        /// <param name="drawUpdateTime"></param>
        public override void Update(double drawUpdateTime)
        {

        }

        /// <summary>
        /// Giving this room a specific image based on the classification
        /// </summary>
        public void InitializeImage()
        {
            int wantedroomClassification = 0; // what room a visitor wants
            for (int i = 0; i < Classification.Length; i++)
            {
                if (Char.IsDigit(Classification[i]))
                {
                    wantedroomClassification += (int)char.GetNumericValue(Classification[i]);
                    break;
                }
            }
            
            // We pick the right image based on the room classification
            if (wantedroomClassification == 1)
                Image = Image.FromFile("../../Resources/room_1star.png");

            if (wantedroomClassification == 2)
                Image = Image.FromFile("../../Resources/room_2star.png");

            if (wantedroomClassification == 3)
                Image = Image.FromFile("../../Resources/room_3star.png");

            if (wantedroomClassification == 4)
                Image = Image.FromFile("../../Resources/room_4star.png");

            if (wantedroomClassification == 5)
                Image = Image.FromFile("../../Resources/room_5star.png");
        }

        /// <summary>
        /// visitor interacting with room when he goes out and when he goes in
        /// </summary>
        /// <param name="human"></param>
        public override void Interact(Human human)
        {
            if (human is Visitor)
            {
                if ((human as Visitor).Room == this)
                {
                    // if visitor his room is this room, let him in
                    if (CurrentHumans.Count != 0)
                    {
                        foreach (var item in CurrentHumans)
                        {
                            if (item == human)
                            {
                                LeaveFacility(human);
                                break;
                            }
                        }
                    }
                    else
                    {
                        JoinFacility(human);
                    }
                }
            }
            else if(human is Cleaner)
            {
                JoinFacility(human);
            }
        }

    }
}