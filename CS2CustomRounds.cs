using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;


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
    public List<CustomRound> CustomRounds { get; set; } = new List<CustomRound>()
    {

        new BombermanRound(),
        new SpeedRound(),
        new OneHPDecoyRound(),
        new DeagleHSOnlyRound(),
        new RandomSpawnRound(),
        new TeamReloadRound(),
        new TPOnHitRound(),
        new TPOnKillRound(),
        new DropWeaponOnMissRound(),
        new TPGunRound(),
        new TankyRound(),
        new HEOnlyRound(),
        new WallHackRound(),
        new BounceRound(),
        new TPlantAnywhereRound(),
        new LowGravityRound(),
        new RandomLoadoutRound(),
        new OneBulletSwitchRound(),
        new ReloadSwitchRound(),
        new InvisibleRound(),
        new IceFloorRound(),
    };


    public CustomRound SelectedCustomRound { get; set; } = new InvisibleRound();

    public override void Load(bool hotReload)
    {
        CreateCommands();
        Server.ExecuteCommand("mp_warmuptime 999999");
        
    }

    [GameEventHandler]
    public HookResult OnRoundPreStart(EventRoundPrestart @event, GameEventInfo info)
    {
        return HookResult.Continue;
    }
    [GameEventHandler]
    public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {

        ShowRoundStartMessage();
        ShowRoundDescription();
        SelectedCustomRound.RoundStart();
        return HookResult.Continue;

    }

    [GameEventHandler]
    public HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        SelectedCustomRound.RoundEnd();
        RandomSelectRound();
        SetParametersNextRound();
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

    [GameEventHandler]
    public HookResult OnBombExplode(EventBombExploded @event, GameEventInfo info)
    {
        //Server.ExecuteCommand("mp_ignore_round_win_conditions false");
        //foreach (var ent in Utilities.GetAllEntities())
        //{
        //    Server.PrintToChatAll(ent.DesignerName);
        //    Server.PrintToChatAll(ent.GetType().Name);
        //}
        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        SelectedCustomRound.PlayerDeath(@event);
        return HookResult.Handled;
    }

    [GameEventHandler]
    public HookResult OnPlayerHit(EventPlayerHurt @event, GameEventInfo info)
    {
        SelectedCustomRound.PlayerHurt(@event);


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
    private void RandomSelectRound()
    {
        if (WarmupEnded)
        {
            Random random = new Random();
            int index = random.Next(0, CustomRounds.Count);
            SelectedCustomRound = CustomRounds[index];
        }
    }
    private void ShowRoundStartMessage()
    {
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
            Server.ExecuteCommand("mp_freezetime 15");
            Server.ExecuteCommand("mp_buytime 20");
        }
        else
        {
            Server.ExecuteCommand("mp_freezetime 0");
            Server.ExecuteCommand("mp_buytime 0");
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
            if (player == null) return;
            string mapName = NativeAPI.GetMapName();
            var coords = ValidMapCoordinates.ValidMapCoordinatesDict[mapName].Select(innerList => new List<double>(innerList)).ToList();
            if (Index >= coords.Count)
            {
                Index = 0;
            }
            var coord = coords[Index];
            Vector vector = new Vector((float)coord[0], (float)coord[1], (float)coord[2] + 20);
            var pawn = player.PlayerPawn.Get();

            if (pawn != null)
            {
                pawn.Teleport(vector);
            }
            Index++;
        });

        AddCommand("css_start", "A test command",
        (player, info) =>
        {
            Server.ExecuteCommand("mp_warmup_end");
            WarmupEnded = true;

        });
    }


}
