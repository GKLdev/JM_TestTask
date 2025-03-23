using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControllerView_Public
{
    public interface IPlayerView
    {
        /// <summary>
        /// Set head angle. used for player controlled character.
        /// </summary>
        void SetHeadAngles(Vector2 _angles);
    }
}