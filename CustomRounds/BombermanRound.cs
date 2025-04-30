using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;

namespace CS2CustomRoundsPlugin

{
    public class BombermanRound : CustomRound
    {
        public override string RoundStartMessage => "BOMBA";
        public override string RoundStartDescription => "Everyone has unlimited c4's... good luck";
        public override bool BuyAllowed => false;
        public override void RoundStart()
        {
            Server.ExecuteCommand("mp_anyone_can_pickup_c4 true");
            Server.ExecuteCommand("mp_c4_cannot_be_defused true");
            Server.ExecuteCommand("mp_c4timer 20");
            Server.ExecuteCommand("mp_plant_c4_anywhere true");
            Server.ExecuteCommand("mp_ignore_round_win_conditions true");

        }
        public override void RoundEnd()
        {
            Server.ExecuteCommand("mp_anyone_can_pickup_c4 false");
            Server.ExecuteCommand("mp_c4_cannot_be_defused false");
            Server.ExecuteCommand("mp_c4timer 40");
            Server.ExecuteCommand("mp_plant_c4_anywhere false");
            Server.ExecuteCommand("mp_ignore_round_win_conditions false");

            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid)
                    continue;
            }
        }

        public override void FreezeTimeEnd(EventRoundFreezeEnd @event)
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid)
                    continue;
                player.RemoveWeapons();
                player.GiveNamedItem(CsItem.C4);
            }
        }
        public override void BombPlant(CCSPlayerController player)
        {
            if (player != null && player.IsValid)
            {
                player.GiveNamedItem(CsItem.C4);
            }
        }


        public override void PlayerDeath(EventPlayerDeath @event)
        {
            var player = @event.Userid;
            if (player != null && @event.Weapon == "planted_c4")
            {
                Server.PrintToChatAll(@event.Weapon);
                //Server.ExecuteCommand("mp_ignore_round_win_conditions false");

                Server.NextWorldUpdate(() =>
                {
                    player.Respawn();
                    player.CommitSuicide(true, true);
                }
                );


            }
        }

    }
}
