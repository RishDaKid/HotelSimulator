using HotelSimulatie.Actors;
using HotelSimulatie.Facilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HotelSimulatie.Factory;
using HotelSimulatie.Graph;
using HotelSimulatie.Enums;
using HotelEvents;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Timers;
using HotelSimulatie.Model;

namespace HotelSimulatie.Actors
{
    public class Cleaner : Human
    {
        // If cleaner is available to clean
        public StatusCleaner StatusCleaner { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="humanParam"></param>
        public Cleaner(MovableParam humanParam) : base(humanParam)
        {
            // defining properties
            Path = new List<Node>();
            Image = Image.FromFile("../../Resources/cleaner.png");
            StatusCleaner = StatusCleaner.NOT_WORKING;
            StatusHuman = StatusHuman.WAITING;
            Location.SetLocationType(humanParam.StartLocation);
            Xpos = Location.LocationType.Position.X * Location.LocationType.oneDimensionSize.Width;
            Ypos = Location.LocationType.Position.Y * Location.LocationType.oneDimensionSize.Height;
        }

        /// <summary>
        /// set walking status of cleaner
        /// </summary>
        public override void SetWalkingStatus()
        {
            if (CurrentEvent.EventType == HotelEventType.EVACUATE)
            {
                StatusHuman = StatusHuman.EVACUATE;
            }
            else if (StatusHuman == StatusHuman.WAITING)
            {
                StatusHuman = StatusHuman.WALK;
            }
            else
            {
                StatusHuman = StatusHuman.WAITING;
            }
        }

        /// <summary>
        /// Check at the two methods for action
        /// </summary>
        /// <param name="drawUpdateTime"></param>
        public override void Update(double drawUpdateTime)
        {
            // Check if cleaner has to walk
            WalkCheck(drawUpdateTime);
            // Check if cleaner watns to interact
            InteractWithDestination(drawUpdateTime);
        }

        /// <summary>
        /// interacting with locationType
        /// </summary>
        /// <param name="drawUpdateTime"></param>
        public void InteractWithDestination(double drawUpdateTime)
        {
            // When cleaner has interacted(meaning : joined the facility)
            if (StatusHuman == StatusHuman.INTERACTING)
            {
                // If cleaner status is working (status is set when cleaner with shortest path is found)
                if (StatusCleaner == StatusCleaner.WORKING)
                {
                    // if this facility has to be cleaned
                    if (this.Location.LocationType.NoNeedToBeCleaned == false)
                    {
                        // with this speed this locationtype is going to be cleaned
                        if (this.Location.LocationType.CleanFacility(drawUpdateTime) == true)
                        {
                            // Is areatype has been cleaned, we are not working anymore
                            StatusCleaner = StatusCleaner.NOT_WORKING;
                            // So we leave the Areatype
                            this.Location.LocationType.LeaveFacility(this);
                            SetWalkingStatus();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Path creating and setting first location
        /// Lobby tells cleaner that there is a place to clean so based on that cleaner will get a destination to walk to and to clean
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public List<Node> CreateGraph(LocationType destination)
        {
            Destination = destination;
            // Cleaner creates his path andreturns it
            Graph.Graph graph = new Graph.Graph();
            Path = graph.CreateNodeGraph(Location.LocationType, Destination, PathFinding, CurrentEvent.EventType);

            // if visitor changes his mind, he's steps will be reset. Step 0 is his location where he stands on, we don't want him to walk to that place
            NextStep = 1;
            return Path;
        }
    }
}
