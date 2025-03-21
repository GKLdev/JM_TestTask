using Modules.AITemplate_Public;
using Modules.CharacterController;
using Modules.CharacterController_Public;
using Modules.DamageManager_Public;
using Modules.ReferenceDb_Public;
using System.Collections;
using UnityEngine;

namespace Modules.CharacterFacade_Public
{
    [CreateAssetMenu(fileName = "DbEntryCharacter", menuName = "Configs/Character/DbEntryCharacter")] 
    public class DbEntryCharacter : DbEntryBase
    {
        [Header("Configs")]
        public ConfigCharacterController    Config;
        public ConfigDamageable             Damageable;
        public AITemplateBase               AITemplate;

        [Header("Prefabs")]
        public LogicBase                CharacterFacade;
        public LogicBase                CharacterVisual;
    }
}