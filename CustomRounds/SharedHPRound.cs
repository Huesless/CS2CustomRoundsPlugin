using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace CS2CustomRoundsPlugin
{
    public class SharedHPRound : CustomRound
    {
        public override string RoundStartMessage => "Hivemind";
        public override string RoundStartDescription => "Your HP pool is shared. When one player takes damage, everyone on the team also takes that damage. (Self damage with nades isn't shared)";
        public override void RoundEnd()
        {
            SetHealth(100);
        }

        public override void RoundStart()
        {
            SetHealth(500);
        }
        public override void PlayerHurt(EventPlayerHurt @event)
        {
            var player = @event.Userid;
            //Server.PrintToChatAll(@event.Attacker?.PlayerName ?? "");
            //Server.PrintToChatAll(player?.PlayerName ?? "");
            //Server.PrintToChatAll(@event.Weapon ?? "");
            //Server.PrintToChatAll(@event.EventName);
            //Server.PrintToChatAll(@event.Hitgroup.ToString() ?? "");
            if (player != null && @event.Attacker?.UserId != player.UserId)
            {
                foreach (var teamplayer in Utilities.GetPlayers())
                {
                    if (teamplayer == null || !teamplayer.IsValid || teamplayer.Team != player?.Team || teamplayer == player)
                        continue;
                    var pawn = teamplayer.PlayerPawn.Get();
                    if (pawn != null)
                    {
                        //Server.PrintToChatAll("Set health");
                        pawn!.AddEntityIOEvent("SetHealth", null, null, (pawn.Health - @event.DmgHealth).ToString());
                    }
                }
            }

        }
        private void SetHealth(int health)
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid)
                    continue;
                var pawn = player.PlayerPawn.Get();

                if (pawn != null)
                {
                    pawn.MaxHealth = health;
                    pawn.Health = health;
                }
            }
        } 
    }
}
