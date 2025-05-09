﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Admin;
using System.Reflection;
using System;


namespace CS2CustomRoundsPlugin;

public class ConfigGen : BasePluginConfig
{
    [JsonPropertyName("Enabled")] public bool Enabled { get; set; } = true;

}
public class CS2CustomRounds : BasePlugin
{
    public override string ModuleName => "Custom Rounds Plugin";

    public override string ModuleVersion => "0.0.1";

    public bool WarmupEnded { get; set; } = false;
    private static bool LoggingCoordinates = false;
    private static bool TestingCoordinates = false;
    public List<CustomRound> CustomRounds { get; set; } = new List<CustomRound>();
    public List<CustomRound>? CustomRoundsPool { get; set; }


    public CustomRound SelectedCustomRound { get; set; } = new NormalRound();

    public void SetCustomRoundsList()
    {
        CustomRounds = new List<CustomRound>()
        {
            //Don't work perfectly yet
            //new BombermanRound(),
            //new InvertedControlsRound(),
            

            //Boring/not that interesting
            //new TankyRound(),
            //new BounceRound(),
            //new IceFloorRound(),

            new SpeedRound(),
            new DeagleHSOnlyRound(),
            new OneHPDecoyRound(),
            new RandomSpawnRound(),
            new TeamReloadRound(),
            new TPOnHitRound(),
            new TPOnKillRound(),
            new DropWeaponOnMissRound(),
            new TPGunRound(),
            new HEOnlyRound(),
            new WallHackRound(),
            new TPlantAnywhereRound(),
            new LowGravityRound(),
            new RandomLoadoutRound(),
            new OneBulletSwitchRound(),
            new ReloadSwitchRound(),
            new InvisibleRound(),
            new BigPlayersRound(),
            new SmallPlayerRound(),
            new SchizoRound(this),
            new SharedHPRound(),
            new TpOnFlashedRound(),
            new CommanderRound(),
            new KnockBackRound(),
            new SpectreRound(),
            new DashRound(),
            new ChickenRound(),
            new EnderpearlRound(),
            new SwitcherooRound(),
            new RocketJumpRound(),
            new LongerFlashesRound(),


        };
    }

    public override void Load(bool hotReload)
    {
        RegisterListener<Listeners.OnServerPrecacheResources>((manifest) =>
        {
            manifest.AddResource("models/hostage/hostage.vmdl");
            manifest.AddResource("models/hostage/hostage_carry.vmdl");
            manifest.AddResource("models/props_vehicles/bus01_2.vmdl");
            manifest.AddResource("models/props/cs_office/coffee_mug.vmdl");
            manifest.AddResource("models/props/cs_office/computer.vmdl");
            manifest.AddResource("models/props/cs_office/plant01.vmdl");
            manifest.AddResource("models/props/cs_office/snowman_face.vmdl");
            manifest.AddResource("models/props/de_inferno/goldfish.vmdl");
            manifest.AddResource("models/props/de_inferno/hay_bail_stack.vmdl");
            manifest.AddResource("models/props/de_vertigo/pallet_01.vmdl");
            manifest.AddResource("models/props/de_vertigo/concretebags.vmdl");
        });
        RegisterListener<Listeners.OnTick>(() =>
        {
            SelectedCustomRound.OnTick();
        });
        CreateCommands();

    }

    [GameEventHandler]
    public HookResult OnWarmupEnd(EventWarmupEnd @event, GameEventInfo info)
    {
        Start();
        return HookResult.Continue;
    }
    [GameEventHandler]
    public HookResult OnRoundPreStart(EventRoundPrestart @event, GameEventInfo info)
    {
        RandomSelectRound();
        SetParametersNextRound();
        return HookResult.Continue;
    }
    [GameEventHandler]
    public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {

        ShowRoundStartMessage();
        ShowRoundDescription();
        SetParametersStartRound();
        SelectedCustomRound.RoundStart();
        return HookResult.Continue;

    }

    [GameEventHandler]
    public HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        SelectedCustomRound.RoundEnd();
        CustomRoundsPool?.Remove(SelectedCustomRound);
        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnBombPlant(EventBombPlanted @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player != null)
        {
            SelectedCustomRound.BombPlant(player);
        }

        //var csEntities = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("cs_").ToArray();

        //foreach (var entity in csEntities.Where(x => x.DesignerName == "cs_gamerules"))
        //{
        //    // It's safe to cast to `CCSGameRules` here as we know the entity is a cs_gamerules entity.
        //    var gameRules = entity.As<CCSGameRules>();
        //    Server.PrintToChatAll(gameRules.RoundEndWinnerTeam.ToString());
        //    Server.PrintToChatAll("A");
        //    gameRules.TCantBuy = true;
        //    gameRules.RoundEndWinnerTeam = 2;
        //    gameRules.BombPlanted = false;
        //    Server.PrintToChatAll(gameRules.RoundWinReason.ToString());
        //    gameRules.RoundWinReason = 0;
        //}
        return HookResult.Continue;
    }

    [GameEventHandler(HookMode.Pre)]
    public HookResult OnBombExplode(EventBombBeginplant @event, GameEventInfo info)
    {
        //Server.ExecuteCommand("mp_ignore_round_win_conditions false");
        //foreach (var ent in Utilities.GetAllEntities())
        //{
        //    Server.PrintToChatAll(ent.DesignerName);
        //    Server.PrintToChatAll(ent.GetType().Name);
        //}
        return HookResult.Continue;
    }

    [GameEventHandler(HookMode.Pre)]
    public HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        SelectedCustomRound.PlayerDeath(@event);
        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerHit(EventPlayerHurt @event, GameEventInfo info)
    {
        SelectedCustomRound.PlayerHurt(@event);
        //var csEntities = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("cs_gamerules").ToArray();
        //foreach (var entity in csEntities)
        //{
        //    Server.PrintToChatAll("Rules terminate");
        //    var gameRules = entity.As<CCSGameRules>();
        //    gameRules.TerminateRound(1, RoundEndReason.BombDefused);
        //}

        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerReload(EventWeaponReload @event, GameEventInfo info)
    {
        SelectedCustomRound.PlayerReload(@event);

        return HookResult.Continue;

    }

    [GameEventHandler]
    public HookResult OnPlayerShoot(EventWeaponFire @event, GameEventInfo info)
    {

        if (LoggingCoordinates)
        {
            LogCoordinate(@event.Userid);
        }
        if (TestingCoordinates)
        {
            TpNext(@event.Userid);
        }
        SelectedCustomRound.WeaponFire(@event);

        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        
        SelectedCustomRound.PlayerSpawn(@event);
        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnBulletimpact(EventBulletImpact @event, GameEventInfo info)
    {

        SelectedCustomRound.BulletImpact(@event);
        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnWeaponFireEmpty(EventWeaponFireOnEmpty @event, GameEventInfo info)
    {
        SelectedCustomRound.WeaponFireEmpty(@event);
        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnFreezeTimeEnd(EventRoundFreezeEnd @event, GameEventInfo info)
    {
        SelectedCustomRound.FreezeTimeEnd(@event);
        return HookResult.Continue;
    }
    [GameEventHandler]
    public HookResult OnPlayerBlind(EventPlayerBlind @event, GameEventInfo info)
    {
        SelectedCustomRound.PlayerBlind(@event);
        return HookResult.Continue;
    }
    [GameEventHandler]
    public HookResult DecoyStarted(EventDecoyStarted @event, GameEventInfo info)
    {
        
        SelectedCustomRound.DecoyStarted(@event);
        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult InspectWeapon(EventInspectWeapon @event, GameEventInfo info)
    {
        //doesn't work
        Server.PrintToChatAll("inspect");
        SelectedCustomRound.InspectWeapon(@event);
        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult PlayerPing(EventPlayerPing @event, GameEventInfo info)
    {
        SelectedCustomRound.PlayerPing(@event);

        //var player = @event.Userid;
        //player!.ExecuteClientCommand("cl_teamid_overhead_mode -1");
        //player!.ExecuteClientCommandFromServer("cl_teamid_overhead_mode -1");
        //var prop = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");
        //if (prop != null)
        //{
        //    prop.Teleport(new Vector(@event.X, @event.Y, @event.Z));
        //    prop.DispatchSpawn();
        //    //prop.ExplodeEffectTickBegin = 1;
        //    //prop.IsLive = true;
        //    //prop.DetonateTime = 1;
        //    //prop.DmgRadius = 800;
        //    //prop.Damage = 100;
        //    prop.SetModel("models/props_vehicles/bus01_2.vmdl");
        //    prop.MoveCollide = MoveCollide_t.MOVECOLLIDE_DEFAULT;
        //    prop.Collision.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_DEFAULT;
        //    prop.Collision.CollisionAttribute.CollisionGroup = (byte)CollisionGroup.COLLISION_GROUP_DEFAULT;
        //    prop.MoveType = MoveType_t.MOVETYPE_VPHYSICS;
        //    prop.Collision.SolidType = SolidType_t.SOLID_VPHYSICS;
        //    prop.DispatchSpawn();

        //}
        return HookResult.Continue;
    }
    [GameEventHandler]
    public HookResult HegrenadeDetonate(EventHegrenadeDetonate @event, GameEventInfo info)
    {
        SelectedCustomRound.HegrenadeDetonate(@event);
        return HookResult.Continue;
    }

    private void RandomSelectRound()
    {
        if (WarmupEnded)
        {

            if(CustomRoundsPool == null || CustomRoundsPool.Count == 0)
            {
                CustomRoundsPool = CustomRounds.ToArray().ToList();
            }
            Random random = new Random();
            int index = random.Next(0, CustomRoundsPool.Count);
            SelectedCustomRound = CustomRoundsPool[index];
        }
    }
    private void ShowRoundStartMessage()
    {
        Server.PrintToConsole(SelectedCustomRound.RoundStartMessage);
        foreach (var player in Utilities.GetPlayers())
        {
            if (player != null)
            {
                player.PrintToCenter(SelectedCustomRound.RoundStartMessage);
            }

        }

    }

    private void ShowRoundDescription()
    {
        Server.PrintToChatAll(SelectedCustomRound.RoundStartDescription);
    }

    private void SetParametersNextRound()
    {
        if (SelectedCustomRound.BuyAllowed)
        {
            Server.ExecuteCommand("mp_buytime 20");
            Server.ExecuteCommand("mp_death_drop_gun 1");
        }
        else
        {
            Server.ExecuteCommand("mp_buytime 0");
            foreach (var player in Utilities.GetPlayers())
            {
                player.GiveNamedItem(CsItem.KevlarHelmet);
            }
            Server.ExecuteCommand("mp_death_drop_gun 0");
        }
    }

    private void SetParametersStartRound()
    {
        if(!SelectedCustomRound.BuyAllowed)
        {
            foreach (var player in Utilities.GetPlayers())
            { 
                if (player != null)
                {
                    player.GiveNamedItem(CsItem.KevlarHelmet);
                }
            }
        }
    }
    private int Index { get; set; } = 0;
    private void CreateCommands()
    {
        AddCommand("css_respawn", "A test command",
        (player, info) =>
        {
            if (player == null) return;
            //player.CommitSuicide(true, true);
            player.Respawn();

        });

        AddCommand("css_suicide", "A test command",
        (player, info) =>
        {
            if (player == null) return;
            player.CommitSuicide(true, true);

        });

        AddCommand("css_tp", "A test command",
        (player, info) =>
        {
            TpNext(player);

        });

        AddCommand("css_start", "A test command",
        (player, info) =>
        {
            Start();

        });

        AddCommand("css_printcoordinates", "A test command",
        (player, info) =>
        {
            string result = "[" + string.Join(",", CoordinateLog.Select(innerList => "[" + string.Join(",", innerList) + "]")) + "]";
            Server.PrintToConsole(result);
            Server.PrintToChatAll(result);

        });
    }
    public List<List<double>> CoordinateLog { get; set; } = new List<List<double>>();
    public void LogCoordinate(CCSPlayerController? player)
    {
        if (player == null) return;
        var pawn = player.PlayerPawn.Get();
        if (pawn != null)
        {
            Vector? vector = pawn.AbsOrigin;
            if(vector != null)
            {
                CoordinateLog.Add(new List<double>() { vector.X, vector.Y, vector.Z});
            }
        }
    }
    public void TpNext(CCSPlayerController? player)
    {
        if (player == null) return;
        string mapName = NativeAPI.GetMapName();
        var coords = ValidMapCoordinates.ValidMapCoordinatesDict[mapName].Select(innerList => new List<double>(innerList)).ToList();
        if (Index >= coords.Count)
        {
            Index = 0;
        }
        var coord = coords[Index];
        Vector vector = new Vector((float)coord[0], (float)coord[1], (float)coord[2]);
        var pawn = player.PlayerPawn.Get();

        if (pawn != null)
        {
            pawn.Teleport(vector);
        }
        Index++;
    }
    public void Start()
    {
        SetCustomRoundsList();
        Server.ExecuteCommand("mp_maxmoney 16000");
        Server.ExecuteCommand("mp_startmoney 16000");
        Server.ExecuteCommand("mp_afterroundmoney 16000");
        Server.ExecuteCommand("mp_freezetime 15");
        Server.ExecuteCommand("mp_warmup_end");
        Server.ExecuteCommand("bot_quota 10");
        Server.ExecuteCommand("bot_quota_mode normal");
        Server.ExecuteCommand("mp_winlimit 13");
        WarmupEnded = true;

    }
}
