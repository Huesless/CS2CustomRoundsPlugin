using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class SwitcherooRound : CustomRound
    {
        public override string RoundStartMessage => "Switcheroo";
        public override string RoundStartDescription => $"When you press E, switch positions with one player on the enemy team. {cooldown} seconds cooldown. Crouching disables the ability.";

        public bool RoundStarted { get; set; } = false;
        public override void RoundEnd()
        {
            RoundStarted = false;
            TPawns.Clear();
            CTPawns.Clear();
        }

        public override void RoundStart()
        {
            SetupTeams();
            
        }
        public override void FreezeTimeEnd(EventRoundFreezeEnd @event)
        {
            SetupCooldownManager();
            RoundStarted = true;
        }
        public override void PlayerDeath(EventPlayerDeath @event)
        {
            var player = @event.Userid;
            if (player != null) 
            {
                var pawn = player.PlayerPawn.Get();
                if (pawn != null)
                {
                    if (player.Team == CsTeam.Terrorist)
                    {
                        TPawns.Remove(pawn);
                    }
                    else if (player.Team == CsTeam.CounterTerrorist)
                    {
                        CTPawns.Add(pawn);
                    }
                }
            }
        }
        private List<CCSPlayerPawn> TPawns = new List<CCSPlayerPawn>();
        private List<CCSPlayerPawn> CTPawns = new List<CCSPlayerPawn>();
        private void SetupCooldownManager()
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player != null && player.UserId != null)
                {
                    if (!CooldownManager.ContainsKey(player.UserId!.Value))
                    {
                        CooldownManager[player.UserId.Value] = Server.TickedTime;
                    }
                    if (!MessageCooldownManager.ContainsKey(player.UserId!.Value))
                    {
                        MessageCooldownManager[player.UserId.Value] = 0;
                    }
                }

            }
        }
        private double messageCooldown = 1;
        private Dictionary<int, double> MessageCooldownManager = new Dictionary<int, double>();

        private double cooldown = 15;
        private Dictionary<int, double> CooldownManager = new Dictionary<int, double>();
        public override void OnTick()
        {

            foreach (var player in Utilities.GetPlayers())
            {
                if (player != null
                && player.IsValid
                && !player.IsBot
                && player.PawnIsAlive
                && player.UserId != null
                && RoundStarted)
                {
                    var buttons = player.Buttons;
                    var pawn = player.PlayerPawn.Value!;
                    if ((buttons & PlayerButtons.Use) != 0 && (buttons & PlayerButtons.Duck) == 0)
                    {
                        if((Server.TickedTime - CooldownManager[player.UserId.Value]) > cooldown)
                        {
                            Switch(player);
                            CooldownManager[player.UserId.Value] = Server.TickedTime;
                        }
                        else if((Server.TickedTime - MessageCooldownManager[player.UserId.Value]) > messageCooldown)
                        {
                            player.PrintToCenter($"Ability on cooldown: {(cooldown - (Server.TickedTime - CooldownManager[player.UserId.Value])).ToString("F")}");
                            MessageCooldownManager[player.UserId.Value] = Server.TickedTime;
                        }
                    }
                }
            }
        }

        private void SetupTeams()
        {
            foreach(var player in Utilities.GetPlayers())
            {
                var pawn = player.PlayerPawn.Get();
                if(pawn != null)
                {
                    if(player.Team == CsTeam.Terrorist)
                    {
                        TPawns.Add(pawn);
                    }
                    else if(player.Team == CsTeam.CounterTerrorist)
                    {
                        CTPawns.Add(pawn);
                    }
                }
            }
        }

        private void Switch(CCSPlayerController player)
        {
            var pawn = player.PlayerPawn.Get();
            if (pawn != null)
            {
                Random random = new Random();
                List<CCSPlayerPawn> otherTeam = new List<CCSPlayerPawn>();
                if (player.Team == CsTeam.Terrorist)
                {
                    otherTeam = CTPawns;
                }
                else if (player.Team == CsTeam.CounterTerrorist)
                {
                    otherTeam = TPawns;
                }

                if (otherTeam.Count > 0)
                {
                    Vector playerPos = new Vector(pawn.AbsOrigin!.X, pawn.AbsOrigin.Y, pawn.AbsOrigin.Z);
                    var enemy = otherTeam[random.Next(0, otherTeam.Count)];
                    pawn.Teleport(enemy.AbsOrigin);
                    enemy.Teleport(playerPos);
                }
                
            }
        }

    }
}
