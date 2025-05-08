using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace CS2CustomRoundsPlugin
{
    public class SchizoRound : CustomRound
    {
        private BasePlugin Plugin;
        public override string RoundStartMessage => "Schizophrenia mode";
        public override string RoundStartDescription => "You're seeing and hearing things. It's all in your head. (MIGHT BE LOUD)";
        private bool RoundEnded { get; set; } = false;
        public SchizoRound(BasePlugin plugin)
        {
            this.Plugin = plugin;
        }
        public override void RoundEnd()
        {
            RoundEnded = true;
            RemoveEntites();
        }

        public override void RoundStart()
        {
            
            RoundEnded = false;

        }

        public override void FreezeTimeEnd(EventRoundFreezeEnd @event)
        {
            SpawnEntities();
            ScheduleNextRandomEvent();
            MoveEntitiesEvent();
        }
        private CounterStrikeSharp.API.Modules.Timers.Timer? _randomTimer;
        private CounterStrikeSharp.API.Modules.Timers.Timer? _randomEntityTimer;

        private void ScheduleNextRandomEvent()
        {
            if (!RoundEnded)
            {
                float delay = Random.Shared.NextSingle() * 1.5f + 0.5f;

                _randomTimer = Plugin.AddTimer(delay, () =>
                {
                    RunRandomEvent();
                    ScheduleNextRandomEvent();
                });

            }

        }
        private void MoveEntitiesEvent()
        {
            if (!RoundEnded)
            {
                float delay = Random.Shared.NextSingle() * 1.5f + 0.5f;

                _randomEntityTimer = Plugin.AddTimer(delay, () =>
                {
                    MoveEntitiesEvent();
                    MoveEntities();
                });

            }

        }
        private int SchizoChance { get; set; } = 75;
        private void RunRandomEvent()
        {
            try
            {
                foreach (var player in Utilities.GetPlayers())
                {
                    Random random = new Random();

                    if (player == null || !player.IsValid || SchizoChance < random.Next(0, 100))
                        continue;
                    int eventIndex = random.Next(0, 10);
                    switch (eventIndex)
                    {
                        case 0:
                            var pawn = player.PlayerPawn.Get();
                            pawn!.AddEntityIOEvent("SetHealth", null, null, (pawn.Health - 1).ToString());
                            //pawn!.AddEntityIOEvent("SetHealth", null, null, (pawn.Health + 1).ToString());
                            break;
                        case 1:
                        case 2:
                            player.ExecuteClientCommand($"play {Sounds[random.Next(0, Sounds.Count)]}");
                            break;

                        default:
                            RecipientFilter filter = new RecipientFilter([player]);
                            foreach(var entity in SoundEmitterEntities)
                            {
                                if(random.Next(0, 100) < 50)
                                {
                                    continue;
                                }
                                int soundIndex = random.Next(0, SoundEvent.Count);
                                entity.EmitSound(SoundEvent[soundIndex], filter, 100f, 0);
                            }
                            
                            break;
                    }
                }
            }
            catch
            {

            }
            

        }
        private void MoveEntities()
        {
            if (!RoundEnded)
            {
                Random random = new Random();
                var entity = SoundEmitterEntities[random.Next(0, SoundEmitterEntities.Count)];
                var vector = entity.AbsOrigin;
                entity.Teleport(new Vector(vector!.X + random.Next(-100, 100), vector.Y + random.Next(-100, 100), vector.Z + random.Next(-10, 10)));
            }

        }
        private void RemoveEntites()
        {
            foreach (var item in SoundEmitterEntities)
            {
                if (item.IsValid)
                {
                    item.Remove();
                }
            }
            SoundEmitterEntities.Clear();
        }
        private List<string> Models = new List<string>()
        {
            "characters/models/ctm_fbi/ctm_fbi.vmdl",
            "characters/models/ctm_sas/ctm_sas.vmdl",
            "models/chicken/chicken.vmdl",
            "characters/models/tm_phoenix/tm_phoenix.vmdl",
            "characters/models/tm_professional/tm_professional_varf.vmdl",
            "characters/models/ctm_st6/ctm_st6_variante.vmdl",
            "characters/models/ctm_gendarmerie/ctm_gendarmerie_varianta.vmdl",
            //"characters/models/ctm_swat/ctm_swat_variante.vmdl",
            "characters/models/tm_leet/tm_leet_varianta.vmdl",
            "characters/models/tm_balkan/tm_balkan_variantf.vmdl",
            "models/hostage/hostage.vmdl",
            "models/hostage/hostage_carry.vmdl"
        };
        private List<string> Sounds = new List<string>()
        {
            //"weapons/flashbang/flashbang_explode2",
            //"weapons/flashbang/flashbang_explode2_distant",
            ////"weapons/flashbang/flashbang_explode1",
            //"weapons/flashbang/flashbang_explode1_distant",
            //"weapons/c4/c4_initiate",
            //"weapons/c4/c4_disarmstart",
            //"weapons/hegrenade/hegrenade_distant_detonate_01",
            //"weapons/hegrenade/hegrenade_distant_detonate_02",
            //"weapons/knife/knife_stab",
            //"player/headshot_armor_e1",
            //"player/headshot_armor_flesh",
            //"player/death1",
            //"player/death3",
            "player/burn_damage1",
            "player/burn_damage2",
            "player/land",
            "player/land2",
            "player/playerping",
            "player/halloween/shake_01",
            "player/footsteps/dirt_01",
            "player/footsteps/dirt_02",
            "player/footsteps/dirt_03",
            "player/footsteps/wood_01",
            "player/footsteps/wood_02",
            "player/footsteps/wood_03",
            "player/footsteps/grass_01",
            "player/footsteps/grass_02",
            "player/footsteps/grass_03",
            "player/footsteps/water_wade_01",
            "player/footsteps/water_wade_02",
            "player/footsteps/water_wade_03",
            "player/footsteps/metal_auto_01",
            "player/footsteps/metal_auto_02",
            "player/footsteps/metal_solid_47",
            "player/footsteps/metal_solid_50",

        };
        private List<string> SoundEvent = new List<string>()
        {
            "Flashbang.Explode",
            "Flashbang.ExplodeDistant",
            "HEGrenade.Bounce",
            "BaseGrenade.Explode",
            "BaseGrenade.ExplodeDistant",
            "Weapon_AK47.Single",
            "Weapon_M4A4.Single",
            "Weapon_AWP.Single",
            "c4.initiate",
            "c4.disarmstart",
            "c4.plant",
            "Player.DamageHeadShot.Onlooker",
            "Player.BurnDamageKevlar",
            "Default.BulletImpact",
            "BaseSmokeEffect.Sound",
            "Flashbang.PullPin_Grenade",
            "Flashbang.Bounce",

            "Base.Footstep",
            "Base.Land",
            "Land_MetalVent.StepLeft",
            "T_Concrete.StepLeft",
            "T_Dirt.StepLeft",
            "Land_MetalVehicle.StepLeft",
            "Land_WaterVol.StepLeft",
            "CT_Ladder.StepLeft",
            "Base.Footstep",
            "Base.Land",
            "Land_MetalVent.StepLeft",
            "T_Concrete.StepLeft",
            "T_Dirt.StepLeft",
            "Land_MetalVehicle.StepLeft",
            "Land_WaterVol.StepLeft",
            "CT_Ladder.StepLeft",
            "Base.Footstep",
            "Base.Land",
            "Land_MetalVent.StepLeft",
            "T_Concrete.StepLeft",
            "T_Dirt.StepLeft",
            "Land_MetalVehicle.StepLeft",
            "Land_WaterVol.StepLeft",
            "CT_Ladder.StepLeft",
            "Base.Footstep",
            "Base.Land",
            "Land_MetalVent.StepLeft",
            "T_Concrete.StepLeft",
            "T_Dirt.StepLeft",
            "Land_MetalVehicle.StepLeft",
            "Land_WaterVol.StepLeft",
            "CT_Ladder.StepLeft",
            "Base.Footstep",
            "Base.Land",



        };
        private List<CBaseEntity> SoundEmitterEntities { get; set; } = new List<CBaseEntity>();
        private void SpawnEntities()
        {
            Random random = new Random();
            foreach (var coord in ValidMapCoordinates.Coords)
            {
                var prop = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");
                if (prop != null)
                {
                    SoundEmitterEntities.Add(prop);
                    prop.Teleport(new Vector((float)coord[0], (float)coord[1], (float)coord[2]));
                    prop.DispatchSpawn();
                    
                    prop.SetModel(Models[random.Next(0, Models.Count)]);
                }

            }
        }
    }
}
