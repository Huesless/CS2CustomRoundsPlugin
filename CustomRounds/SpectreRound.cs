﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class SpectreRound : CustomRound
    {
        public override string RoundStartMessage => "That one Shroud's game";
        public override string RoundStartDescription => $"Decoys spawn a clone. Press E to switch positions with the clone. {cooldown} second cooldown. Doesn't switch when crouching (important for defuses).";
        public override void RoundEnd()
        {
            foreach(var pair in PlayerClonePair)
            {
                pair.Value.Remove();
                PlayerClonePair.Remove(pair.Key);
            }
        }

        public override void RoundStart()
        {
            GiveDecoy();
            SetupCooldownManager();
        }
        private Dictionary<int, CDynamicProp> PlayerClonePair = new Dictionary<int, CDynamicProp>();
        private void GiveDecoy()
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid || player.UserId == null)
                    continue;
                player.GiveNamedItem(CsItem.Decoy);
            }
        }
        public override void DecoyStarted(EventDecoyStarted @event)
        {
            var player = @event.Userid;
            if(player != null && player.UserId != null)
            {
                bool isInitialized = PlayerClonePair.ContainsKey(player.UserId.Value);
                if(!isInitialized)
                {
                    var prop = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");
                    if (prop != null)
                    {
                        PlayerClonePair[player.UserId.Value] = prop;
                        prop.DispatchSpawn();
                        if (player.Team == CsTeam.Terrorist)
                        {
                            prop.SetModel("characters/models/tm_phoenix/tm_phoenix.vmdl");
                        }
                        else if (player.Team == CsTeam.CounterTerrorist)
                        {
                            prop.SetModel("characters/models/ctm_sas/ctm_sas.vmdl");
                        }
                    }
                }

                var clone = PlayerClonePair[player.UserId.Value];
                if (clone != null)
                {
                    clone.Teleport(new Vector(@event.X, @event.Y, @event.Z));
                }
            }

        }

        //public override void PlayerPing(EventPlayerPing @event)
        //{
        //    var player = @event.Userid;
        //    if (player != null && player.UserId != null)
        //    {
        //        var clone = PlayerClonePair[player.UserId.Value];
        //        if (clone != null)
        //        {
        //            var pawn = player.PlayerPawn.Get();
        //            if (pawn != null)
        //            {
        //                Vector playerPos = new Vector(pawn.AbsOrigin!.X, pawn.AbsOrigin.Y, pawn.AbsOrigin.Z);
        //                pawn.Teleport(clone.AbsOrigin);
        //                clone.Teleport(playerPos);

        //            }
        //        }
        //    }
        //}
        private void Switch(CCSPlayerController player)
        {
            if (player != null && player.UserId != null)
            {
                bool isInitialized = PlayerClonePair.ContainsKey(player.UserId.Value);
                if (isInitialized)
                {
                    var clone = PlayerClonePair[player.UserId.Value];
                    if (clone != null)
                    {
                        var pawn = player.PlayerPawn.Get();
                        if (pawn != null)
                        {
                            Vector playerPos = new Vector(pawn.AbsOrigin!.X, pawn.AbsOrigin.Y, pawn.AbsOrigin.Z);
                            pawn.Teleport(clone.AbsOrigin);
                            clone.Teleport(playerPos);

                        }
                    }
                }

            }
        }
        private void SetupCooldownManager()
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player != null && player.UserId != null)
                {
                    if (!CooldownManager.ContainsKey(player.UserId!.Value))
                    {
                        CooldownManager[player.UserId.Value] = 0;
                    }
                }

            }
        }
        private double cooldown = 1;
        private Dictionary<int, double> CooldownManager = new Dictionary<int, double>();
        public override void OnTick()
        {

            foreach (var player in Utilities.GetPlayers())
            {
                if (player != null
                && player.IsValid
                && !player.IsBot
                && player.PawnIsAlive
                && player.UserId != null)
                {
                    var buttons = player.Buttons;
                    var pawn = player.PlayerPawn.Value!;
                    if ((buttons & PlayerButtons.Use) != 0 && (buttons & PlayerButtons.Duck) == 0 && (Server.TickedTime - CooldownManager[player.UserId.Value]) > cooldown)
                    {
                        Switch(player);
                        CooldownManager[player.UserId.Value] = Server.TickedTime;
                    }
                }
            }
        }
    }
}
