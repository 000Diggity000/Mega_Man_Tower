global using BTD_Mod_Helper;
global using MelonLoader;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppSystem.IO;
using UnityEngine;
using BTD_Mod_Helper.Api.Enums;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using Il2Cpp;
using BTD_Mod_Helper.Api;

[assembly: MelonInfo(typeof(Mega_Man.Main), "Mega Man", "1.0.1", "000diggity000")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace Mega_Man
{
    public class Main : BloonsTD6Mod
    {
        public class MegaMan : ModTower
        {
            public override string Portrait => "portrait";
            public override string Icon => "portrait";

            public override TowerSet TowerSet => TowerSet.Magic;
            public override string BaseTower => TowerType.DartMonkey;
            public override int Cost => 900;

            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 6;
            public override int BottomPathUpgrades => 0;
            public override string Description => "Shoots bloons with his arm.";

            public override string DisplayName => "Mega Man";

            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.range += 15;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range += 15;
                attackModel.weapons[0].rate = 0.87f;


                var projectile = attackModel.weapons[0].projectile;
                projectile.pierce += 5;
                projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.Purple;
                projectile.ApplyDisplay<BulletDisplay>();
                projectile.scale = 0.6f;
                towerModel.ApplyDisplay<MegaMan000>();
                towerModel.displayScale = 0.1f;
            }

        }
        public class Faster_Shooting : ModUpgrade<MegaMan>
        {
            public override string Icon => "FastFiringUpgradeIcon";

            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override int Cost => 650;

            public override string DisplayName => "Faster Shooting";

            public override string Description => "Shoots faster.";

            public override void ApplyUpgrade(TowerModel tower)
            {
                tower.GetWeapon().rate -= 0.1f;
            }
        }
        public class Exploding_Lasers : ModUpgrade<MegaMan>
        {
            public override string Icon => "bombt";

            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override int Cost => 1200;

            public override string DisplayName => "Exploding Lasers";

            public override string Description => "Lasers explode.";

            public override void ApplyUpgrade(TowerModel tower)
            {
                var bomb = Game.instance.model.GetTower("BombShooter",2, 0 ,0).GetWeapon().projectile.Duplicate();
                bomb.ApplyDisplay<BombDisplay>();
                bomb.pierce = 6f;
                bomb.scale = 0.7f;
                tower.GetWeapon().projectile = bomb;
            }
        }
        public class Visor : ModUpgrade<MegaMan>
        {
            public override string Icon => "visor";

            public override int Path => MIDDLE;
            public override int Tier => 3;
            public override int Cost => 1300;

            public override string DisplayName => "Visor";

            public override string Description => "Gains a visor to see C.A.M.O. bloons and gains more range.";

            public override void ApplyUpgrade(TowerModel tower)
            {
                tower.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                tower.range += 20f;
                tower.GetAttackModel().range += 20;
                tower.GetAttackModel().attackThroughWalls = true;
                tower.ApplyDisplay<MegaMan030>();
                //var lasers = Game.instance.model.GetTower("SuperMonkey", 1, 0, 0).GetBehavior<AttackModel>().weapons[0].Duplicate();
                //tower.GetAttackModel().AddBehavior(lasers);
            }
        }
        public class Rockets : ModUpgrade<MegaMan>
        {
            public override string Icon => "Rockets";

            public override int Path => MIDDLE;
            public override int Tier => 4;
            public override int Cost => 20000;

            public override string DisplayName => "Rockets";

            public override string Description => "Bombs become homing.";

            public override void ApplyUpgrade(TowerModel tower)
            {
                tower.GetWeapon().projectile.AddBehavior<AdoraTrackTargetModel>(new AdoraTrackTargetModel("AdoraTrackTargetModel_", 9.0f, 70, 360, 20, 99999f, 5, 30));
                tower.GetWeapon().emission = new ArcEmissionModel("ArcEmissionModel_", 4, 0, 160, null, false, false);
                tower.GetWeapon().projectile.maxPierce = 99f;
                tower.GetWeapon().projectile.pierce = 99f;
                tower.GetWeapon().projectile.ApplyDisplay<RocketDisplay>();
                tower.GetWeapon().projectile.scale = 0.8f;
            }
        }
        //
        public class Overdrive : ModUpgrade<MegaMan>
        {
            public override string Icon => "overdrive";
            public override string Portrait => "redportrait";

            public override int Path => MIDDLE;
            public override int Tier => 5;
            public override int Cost => 50000;

            public override string DisplayName => "Overdrive";

            public override string Description => "Every stat increases.";

            public override void ApplyUpgrade(TowerModel tower)
            {
                tower.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 30f;
                tower.GetWeapon().rate -= 0.3f;
                tower.range += 10f;
                tower.GetAttackModel().range += 10f;
                tower.GetAttackModel().weapons[0].projectile.ApplyDisplay<BlueDisplay>();
                tower.GetAttackModel().weapons[0].projectile.scale = 1f;
                tower.ApplyDisplay<MegaMan050>();
            }
        }
        public static Shader? GetOutlineShader()
        {
            var superMonkey = GetVanillaAsset("Assets/Monkeys/DartMonkey/Graphics/SuperMonkey.prefab")?.Cast<GameObject>();
            if (superMonkey == null) return null;
            superMonkey.AddComponent<UnityDisplayNode>();
            var litOutlineShader = superMonkey.GetComponent<UnityDisplayNode>().GetMeshRenderer().material.shader;
            return litOutlineShader;
        }
        public static UnityEngine.Object? GetVanillaAsset(string name)
        {
            foreach (var assetBundle in AssetBundle.GetAllLoadedAssetBundles().ToArray())
            {
                if (assetBundle.Contains(name))
                {
                    return assetBundle.LoadAsset(name);
                }
            }
            return null;
        }
        public class MegaMan000 : ModCustomDisplay
        {
            public override string AssetBundleName => "megaman"; // loads from "assets.bundle"
            public override string PrefabName => "MegaMan"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (SkinnedMeshRenderer s in node.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    s.material.shader = GetOutlineShader();
                    s.SetOutlineColor(new Color(99f / 255, 128f / 255, 255f / 255));
                }
            }
        }
        public class MegaMan030 : ModCustomDisplay
        {
            public override string AssetBundleName => "megaman"; // loads from "assets.bundle"
            public override string PrefabName => "MegaMan-030"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (SkinnedMeshRenderer s in node.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    s.material.shader = GetOutlineShader();
                    s.SetOutlineColor(new Color(99f / 255, 128f / 255, 255f / 255));
                }
            }

        }
        public class MegaMan050 : ModCustomDisplay
        {
            public override string AssetBundleName => "megaman"; // loads from "assets.bundle"
            public override string PrefabName => "MegaMan-050"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (SkinnedMeshRenderer s in node.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    s.material.shader = GetOutlineShader();
                    s.SetOutlineColor(new Color(242f / 255, 7f / 255, 7f / 255));
                }
            }

        }
        public class BulletDisplay : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "bullet");
            }
        }
        public class BombDisplay : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "bombt");
            }
        }
        public class RocketDisplay : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "rocket");
            }
        }
        public class BlueDisplay : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "blue");
            }
        }
        public override void OnWeaponFire(Weapon weapon)
        {
            if (weapon.attack.tower.towerModel.name.Contains("Mega Man-MegaMan"))
            {
                 weapon.attack.tower.Node.graphic.GetComponent<Animator>().SetTrigger("Shoot");
            }
        }
        public override void OnTowerSelected(Tower tower)
        {
            
            if (tower.towerModel.name.Contains("Mega Man-MegaMan"))
            {
                foreach (SkinnedMeshRenderer s in tower.Node.graphic.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    s.material.shader = GetOutlineShader();

                    {
                        s.SetOutlineColor(new Color(255f / 255, 255f / 255, 255f / 255));
                    }

                }
            }
        }
        public override void OnTowerDeselected(Tower tower)
        {
            if (tower.towerModel.name.Contains("Mega Man-MegaMan-050"))
            {
                foreach (SkinnedMeshRenderer s in tower.Node.graphic.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    s.material.shader = GetOutlineShader();
                    s.SetOutlineColor(new Color(242f / 255, 7f / 255, 7f / 255));
                }
            }
            else if(tower.towerModel.name.Contains("Mega Man-MegaMan"))
            {
                foreach (SkinnedMeshRenderer s in tower.Node.graphic.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    s.material.shader = GetOutlineShader();
                    s.SetOutlineColor(new Color(99f / 255, 128f / 255, 255f / 255));
                }
            }
        }
        

    }
}