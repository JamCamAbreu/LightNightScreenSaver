﻿using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers.Containers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HPScreen.Admin;

namespace LightNightScreenSaver.Entities
{
    public class Firework : GravityObject
    {
        public bool IsDead
        {
            get
            {
                return Fuse <= 0;
            }
        }
        public int Fuse { get; set; }
        public Firework()
        {
            Mass = 4f;
            Gravity = 0.35f;
            AirResistance = 0.9f;
            XVelocity = 0;
            YVelocity = 0;
            XAcceleration = 0;
            YAcceleration = 0;
            Fuse = 100;
        }
        public override void Update()
        {
            base.Update();
            Fuse--;
        }
        public void Explode()
        {
            ParticleEmitters.Current.AddNewEffect(GenerateEffect(180));
        }

        public LifeParticleEffect GenerateEffect(int duration)
        {
            // Create a 1x1 white texture for particles
            Texture2D particleTexture = new Texture2D(Graphics.Current.Device, 1, 1);
            particleTexture.SetData(new[] { Color.White });
            var textureRegion = new Texture2DRegion(particleTexture);

            float radius = Ran.Current.Next(50, 200);
            float speedmin = Ran.Current.Next(40, 120);
            float speedmax = Ran.Current.Next(220, 420);

            // Create the particle effect
            var particleEffect = new ParticleEffect()
            {
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(textureRegion, 500, TimeSpan.FromSeconds(4),
                            Profile.Circle(radius, Profile.CircleRadiation.Out))
                    {
                        Parameters = new ParticleReleaseParameters
                        {
                            Speed = new Range<float>(speedmin, speedmax), // Speed range for particles
                            Quantity = Ran.Current.Next(80, 200), // Number of particles emitted
                            Scale = new Range<float>(1f, 4f), // Scale of particles
                            Rotation = new Range<float>(-MathHelper.Pi, MathHelper.Pi)
                        },
                        Modifiers =
                        {
                            new AgeModifier
                            {
                                Interpolators =
                                {
                                    new ColorInterpolator
                                    {
                                        StartValue = new HslColor(Ran.Current.Next(0f, 1f), Ran.Current.Next(0f, 1f), Ran.Current.Next(0f, 1f)), // Starting color
                                        EndValue = new HslColor(Ran.Current.Next(0f, 1f), Ran.Current.Next(0f, 1f), Ran.Current.Next(0f, 1f)), // Starting color
                                    },
                                    new OpacityInterpolator
                                    {
                                        StartValue = 1f, // Start fully opaque
                                        EndValue = 0f // Fade out to transparent
                                    }
                                }
                            },
                            new LinearGravityModifier
                            {
                                Direction = Vector2.UnitY, // Gravity pulling downwards
                                Strength = 100f // Strength of gravity
                            }
                        }
                    }
                }
            };

            // Set the initial position of the firework effect
            particleEffect.Position = new Vector2(Xpos, Ypos);

            return new LifeParticleEffect(particleEffect, duration);
        }
    }
}