using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2CustomRoundsPlugin
{
    public class ChickenRound : CustomRound
    {
        public override string RoundStartMessage => "Why did we cross the road?";
        public override string RoundStartDescription => "Every player's model is set to a chicken.";

        public override void RoundEnd()
        {
            return;
        }

        public override void RoundStart()
        {
            SetPlayerModels();
        }
        //private List<string> Models = new List<string>()
        //{
        //    "models/chicken/chicken.vmdl",
        //    "models/props/cs_office/coffee_mug.vmdl",
        //    "models/props/cs_office/computer.vmdl",
        //    "models/props/cs_office/plant01.vmdl",
        //    "models/props/cs_office/snowman_face.vmdl",
        //    "models/props/de_inferno/goldfish.vmdl",
        //    "models/props/de_inferno/hay_bail_stack.vmdl",
        //    "models/props/de_vertigo/pallet_01.vmdl",
        //    "models/props/de_vertigo/concretebags.vmdl"
        //};
        private void SetPlayerModels()
        {
            foreach(var player in Utilities.GetPlayers())
            {
                if(player != null && player.IsValid)
                {
                    var pawn = player.PlayerPawn.Get();
                    if(pawn != null)
                    {
                        pawn.SetModel("models/chicken/chicken.vmdl");
                    } 
                }
            }
        }
    }
}
