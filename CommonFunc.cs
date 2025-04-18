﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public static class CommonFunc
    {
        public static bool PlayerHasBomb(CCSPlayerController player)
        {
            foreach (var weapon in player.PlayerPawn.Value.WeaponServices?.MyWeapons)
            {
                var weaponEntity = weapon.Value;
                if (weaponEntity == null)
                    continue;

                var weaponName = weaponEntity.DesignerName;
                if (!string.IsNullOrEmpty(weaponName) && weaponName.Contains("weapon_c4"))
                {
                    return true;
                }
            }
            return false;
        }

        public static void RemoveWeapons(CCSPlayerController player)
        {
            var bombcarrier = CommonFunc.PlayerHasBomb(player);
            player.RemoveWeapons();
            if (bombcarrier)
            {
                player.GiveNamedItem(CsItem.C4);
            }
        }
    }
}
