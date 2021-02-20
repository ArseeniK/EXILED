// -----------------------------------------------------------------------
// <copyright file="StaticItemSpawn.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.CustomItems.API
{
    /// <summary>
    /// Handles static spawn locations.
    /// </summary>
    public class StaticItemSpawn
    {
        /// <summary>
        /// Gets or sets this spawn location's <see cref="Vector"/>.
        /// </summary>
        public Vector Position { get; set; }

        /// <summary>
        /// Gets or sets this spawn location's spawn chance.
        /// </summary>
        public float Chance { get; set; }

        /// <summary>
        /// Gets or sets this spawn location's name.
        /// </summary>
        public string Name { get; set; }
    }
}