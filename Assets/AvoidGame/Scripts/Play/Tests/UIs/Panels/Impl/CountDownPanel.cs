using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvoidGame.Play.Test.UI
{
    public class CountDownPanel : PanelBase
    {
        public override GameState TargetState => GameState.CountDown;
    }
}
