using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CounterStrikeSharp.API.Core;

namespace CS2CustomRoundsPlugin
{
    public class RandomLoadoutRound : CustomRound
    {
        public override bool BuyAllowed => false;
        public override string RoundStartMessage => "Gamba loadout";
        public override string RoundStartDescription => "Start with a random loadout.";
        public override void RoundStart()
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid)
                    continue;
                Random rand = new Random();
                bool bombCarrier = CommonFunc.PlayerHasBomb(player);
                player.RemoveWeapons();
                player.GiveNamedItem(CsItem.KevlarHelmet);
                player.GiveNamedItem(WeaponsSet.PrimaryWeapons.ElementAt(rand.Next(WeaponsSet.PrimaryWeapons.Count)));
                player.GiveNamedItem(WeaponsSet.SecondaryWeapons.ElementAt(rand.Next(WeaponsSet.SecondaryWeapons.Count)));
                player.GiveNamedItem(WeaponsSet.Knife.ElementAt(rand.Next(WeaponsSet.Knife.Count)));
                player.GiveNamedItem(WeaponsSet.Grenades.ElementAt(rand.Next(WeaponsSet.Grenades.Count)));
                if (bombCarrier)
                {
                    player.GiveNamedItem(CsItem.C4);
                }



            }
        }
        public override void RoundEnd()
        {
            return;
        }

        
    }
}
