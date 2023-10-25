using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvoidGame.Play.Test.UI
{
    public class PlayingPanel : PanelBase
    {
        public override GameState TargetState => GameState.Playing;
    }
}
