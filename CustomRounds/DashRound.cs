using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class DashRound : CustomRound
    {
        public override string RoundStartMessage => "Valorant";
        public override string RoundStartDescription => $"Press E to dash in the direction of movement. {cooldown} seconds cooldown. Doesn't dash when crouching (important for defuses).";
        public bool RoundStarted { get; set; } = false;
        public override void RoundEnd()
        {
            RoundStarted = false;
        }

        public override void RoundStart()
        {
            SetupCooldownManager();
        }
        public override void FreezeTimeEnd(EventRoundFreezeEnd @event)
        {
            RoundStarted = true;
        }

        private void SetupCooldownManager()
        {
            foreach(var player in Utilities.GetPlayers())
            {
                if(player != null && player.UserId != null)
                {
                    if (!CooldownManager.ContainsKey(player.UserId!.Value))
                    {
                        CooldownManager[player.UserId.Value] = 0;
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
        private double cooldown = 3;
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
                            if ((buttons & PlayerButtons.Moveright) != 0)
                            {
                                DashRight(player);
                            }
                            else if ((buttons & PlayerButtons.Moveleft) != 0)
                            {
                                DashLeft(player);
                            }
                            else if ((buttons & PlayerButtons.Back) != 0)
                            {
                                DashBack(player);
                            }
                            else
                            {
                                Dash(player);
                            }

                            CooldownManager[player.UserId.Value] = Server.TickedTime;
                        }
                        //else if ((Server.TickedTime - MessageCooldownManager[player.UserId.Value]) > messageCooldown)
                        //{
                        //    player.PrintToCenter($"Ability on cooldown: {(cooldown - (Server.TickedTime - CooldownManager[player.UserId.Value])).ToString("F")}");
                        //    MessageCooldownManager[player.UserId.Value] = Server.TickedTime;
                        //}

                    }
                }
            }
        }

        private void Dash(CCSPlayerController player)
        {
            if (player != null)
            {
                var pawn = player!.PlayerPawn.Get();
                //var buttons = player.Buttons;
                var directionVec = new Vector();
                NativeAPI.AngleVectors(pawn!.EyeAngles.Handle, directionVec.Handle, IntPtr.Zero,
                    IntPtr.Zero);

                // Always shoot us up a little bit if were on the ground and not aiming up.
                if (directionVec.Z < 0.275)
                {
                    directionVec.Z = 0.275f;
                }

                directionVec *= 1000;

                pawn!.AbsVelocity.X = directionVec.X;
                pawn.AbsVelocity.Y = directionVec.Y;
                pawn.AbsVelocity.Z = directionVec.Z;
            }
        }

        private void DashBack(CCSPlayerController player)
        {
            if (player != null)
            {
                var pawn = player!.PlayerPawn.Get();
                //var buttons = player.Buttons;
                var directionVec = new Vector();
                NativeAPI.AngleVectors(pawn!.EyeAngles.Handle, directionVec.Handle, IntPtr.Zero,
                    IntPtr.Zero);

                // Always shoot us up a little bit if were on the ground and not aiming up.
                if (directionVec.Z < 0.275)
                {
                    directionVec.Z = 0.275f;
                }

                directionVec *= 1000;

                pawn!.AbsVelocity.X = -directionVec.X;
                pawn.AbsVelocity.Y = -directionVec.Y;
                pawn.AbsVelocity.Z = directionVec.Z;
            }
        }
        private void DashRight(CCSPlayerController player)
        {
            if (player != null)
            {
                var pawn = player!.PlayerPawn.Get();
                //var buttons = player.Buttons;
                var directionVec = new Vector();
                NativeAPI.AngleVectors(GetRightQAngle(pawn!.EyeAngles).Handle, directionVec.Handle, IntPtr.Zero,
                    IntPtr.Zero);

                // Always shoot us up a little bit if were on the ground and not aiming up.
                if (directionVec.Z < 0.275)
                {
                    directionVec.Z = 0.275f;
                }

                directionVec *= 1000;

                pawn!.AbsVelocity.X = - directionVec.X;
                pawn.AbsVelocity.Y = - directionVec.Y;
                pawn.AbsVelocity.Z = directionVec.Z;
            }
        }
        private void DashLeft(CCSPlayerController player)
        {
            if (player != null)
            {
                var pawn = player!.PlayerPawn.Get();
                //var buttons = player.Buttons;
                var directionVec = new Vector();
                NativeAPI.AngleVectors(GetLeftQAngle(pawn!.EyeAngles).Handle, directionVec.Handle, IntPtr.Zero,
                    IntPtr.Zero);

                // Always shoot us up a little bit if were on the ground and not aiming up.
                if (directionVec.Z < 0.275)
                {
                    directionVec.Z = 0.275f;
                }

                directionVec *= 1000;

                pawn!.AbsVelocity.X = - directionVec.X;
                pawn.AbsVelocity.Y = - directionVec.Y;
                pawn.AbsVelocity.Z = directionVec.Z;
            }
        }

        public QAngle GetLeftQAngle(QAngle eyeAngles)
        {
            // Subtract 90 degrees from yaw to turn left
            float newYaw = eyeAngles.Y - 90f;

            // Wrap around if < 0 or > 360
            if (newYaw < -180f) newYaw += 360f;
            if (newYaw > 180f) newYaw -= 360f;

            return new QAngle(eyeAngles.X, newYaw, 0f);
        }

        public QAngle GetRightQAngle(QAngle eyeAngles)
        {
            float newYaw = eyeAngles.Y + 90f;

            // Normalize yaw to [-180, 180] or [0, 360] as needed
            if (newYaw > 180f) newYaw -= 360f;
            if (newYaw < -180f) newYaw += 360f;

            return new QAngle(eyeAngles.X, newYaw, 0f);
        }
    }
}
