﻿// -----------------------------------------------------------------------
// <copyright file="Round.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Handlers
{
    using System.Linq;
    using Exiled.Events.Handlers.EventArgs;
    using MEC;

    /// <summary>
    /// Handles some round clean-up events and some others related to players.
    /// </summary>
    public class Round
    {
        /// <inheritdoc cref="Server.OnWaitingForPlayers"/>
        public void OnWaitingForPlayers(object sender, System.EventArgs ev)
        {
            API.Features.Map.Rooms.Clear();
            API.Features.Map.Doors.Clear();
            API.Features.Map.Lifts.Clear();
            API.Features.Map.TeslaGates.Clear();

            API.Features.Player.IdsCache.Clear();
            API.Features.Player.UserIdsCache.Clear();
        }

        /// <inheritdoc cref="Server.OnRoundStarted"/>
        public void OnRoundStarted(object sender, System.EventArgs ev)
        {
            foreach (API.Features.Player player in API.Features.Player.List.Values)
            {
                if (player.IsOverwatchEnabled)
                {
                    player.IsOverwatchEnabled = false;
                    Timing.CallDelayed(2f, () => player.IsOverwatchEnabled = true);
                }
            }
        }

        /// <inheritdoc cref="Player.OnLeft(LeftEventArgs)"/>
        public void OnPlayerLeft(object sender, LeftEventArgs ev)
        {
            if (API.Features.Player.IdsCache.ContainsKey(ev.Player.Id))
                API.Features.Player.IdsCache.Remove(ev.Player.Id);

            foreach (var entry in API.Features.Player.UserIdsCache.Where(userId => userId.Value == ev.Player).ToList())
                API.Features.Player.UserIdsCache.Remove(entry.Key);

            if (!API.Features.Player.List.ContainsKey(ev.Player.GameObject))
                API.Features.Player.List.Remove(ev.Player.GameObject);
        }

        /// <inheritdoc cref="Player.OnJoined(JoinedEventArgs)"/>
        public void OnPlayerJoined(object sender, JoinedEventArgs ev)
        {
            if (ev.Player == null || ev.Player.IsHost || string.IsNullOrEmpty(ev.Player.UserId))
                return;

            if (!API.Features.Player.List.ContainsKey(ev.Player.GameObject))
                API.Features.Player.List.Add(ev.Player.GameObject, ev.Player);
        }

        /// <inheritdoc cref="Player.OnChangingRole(ChangingRoleEventArgs)"/>
        public void OnChangingRole(object sender, ChangingRoleEventArgs ev)
        {
            if (ev.Player == null || ev.Player.IsHost || string.IsNullOrEmpty(ev.Player.UserId))
                return;

            if (ev.NewRole == RoleType.Spectator && Config.ShouldDropInventory)
                ev.Player.Inventory.ServerDropAll();
        }
    }
}