using HPScreen.Admin;
using HPScreen.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LightNightScreenSaver.Entities
{
    public class GravityObject : Sprite
    {
        public float Mass { get; set; }
        public float AirResistance { get; set; }
        public float XVelocity { get; protected set; }
        public float YVelocity { get; protected set; }
        public float XAcceleration { get; protected set; }
        public float YAcceleration { get; protected set; }
        public float Gravity { get; protected set; }
        public GravityObject()
        {
            Mass = 5f;
            Gravity = 0.4f;
            AirResistance = 0.9f;
            XVelocity = 0;
            YVelocity = 0;
            XAcceleration = 0;
            YAcceleration = 0;
        }
        public override void Update()
        {
            ApplyGravity();
            ApplyAirResistance();

            // Apply forces as changes in acceleration
            XVelocity += XAcceleration;
            YVelocity += YAcceleration;

            // Update positions based on velocity
            Xpos += XVelocity;
            Ypos += YVelocity;

            // Reset accelerations after applying them
            XAcceleration = 0;
            YAcceleration = 0;
        }
        public void ApplyForce(float forceX, float forceY)
        {
            XAcceleration += forceX / Mass;
            YAcceleration += forceY / Mass;
        }

        #region Internal
        protected void ApplyGravity()
        {
            YAcceleration += Gravity;
        }
        protected void ApplyAirResistance()
        {
            XAcceleration *= AirResistance;
            if (XAcceleration < 0.05 && XAcceleration > -0.05)
            {
                XAcceleration = 0;
            }
        }
        #endregion
    }
}
