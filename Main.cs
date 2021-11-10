/*
You need to add these DLLs to references:
0Harmony
ACTk-Runtime
Assembly-Csharp
Assembly-Csharp-firstpass
BepInEx.Core
BepInEx.IL2CPP
Il2Cppmscorlib
UnhollowerRuntimeLib
UnhollowerBaseLib
Unity.TextMeshPro
UnityEngine.CoreModule
UnityEngine.IMGUIModule
UnityEngine.InputLegacyModule
UnityEngine.InputModule
UnityEngine.PhysicsModule

There might be more but idk
*/
using BepInEx;
using BepInEx.IL2CPP;
using UnhollowerRuntimeLib;
using UnityEngine;
using CodeStage.AntiCheat.Detectors;
using HarmonyLib;
using System;

namespace MiniMod
{
	//Holds all variables
    public class vars
    {
        public static Vector3 playerVelocity = new Vector3();

        public static bool areChamsOn = false;
        public static bool isGodOn = false;
        public static bool isHelpMenuOn = false;
        public static bool isInfCrouchOn = false;
        public static bool isInfJumpOn = false;
        public static bool isNCon = false;
        public static bool isSpeedHackOn = false;
        public static bool isAKBon = false;
    }
	//Thanks so much do daniel_crime for this AKB code
    [HarmonyPatch(typeof(GameManager), "PunchPlayer")]

    [System.Obsolete]
    class punchPlayerManager
    {
        static bool Prefix(ulong puncher, ref ulong punched, ref Vector3 dir)
        {
            if (vars.isAKBon)
            {
                punched = 0;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(PlayerStatus), "DamagePlayer")]
    [System.Obsolete]
    class NoKnockback1
    {
        static bool Prefix(uint dmg, ref Vector3 damageDir, ulong damageDoerId, int itemId)
        {
            if (vars.isAKBon)
            {
                damageDir = Vector3.zero;
            }

            return true;
        }
    }
    public class Main : MonoBehaviour
    {
        public Main(System.IntPtr ptr) : base(ptr) { }
		//Thanks to daniel_crime
        public static void Chams(bool on)
        {
            if (on)
            {
                foreach (Component component in UnityEngine.Object.FindObjectsOfType<OnlinePlayerMovement>())
                {
                    foreach (SkinnedMeshRenderer componentsInChild in component.GetComponentsInChildren<SkinnedMeshRenderer>())
                    {
                        ((Renderer)componentsInChild).material.shader = Shader.Find("Hidden/Internal-Colored");
                        ((Renderer)componentsInChild).material.SetInt("_ZTest", 0);
                        ((Renderer)componentsInChild).material.color = Color.green;
                    }
                }
            }
            else
            {
                foreach (Component component in UnityEngine.Object.FindObjectsOfType<OnlinePlayerMovement>())
                {
                    foreach (SkinnedMeshRenderer componentsInChild in component.GetComponentsInChildren<SkinnedMeshRenderer>())
                    {

                        ((Renderer)componentsInChild).material.shader = Shader.Find("Standard");
                        ((Renderer)componentsInChild).material.SetInt("_ZTest", 4);
                        switch (componentsInChild.name)
                        {
                            case ("Hair0"):
                                ((Renderer)componentsInChild).material.color = new Color(0.1533268f, 0.0860733f, 0.1062634f, 1);
                                break;
                            case ("Shoes"):
                                ((Renderer)componentsInChild).material.color = new Color(0.9063317f, 0.9063317f, 0.9063317f, 1);
                                break;
                            default:
                                ((Renderer)componentsInChild).material.color = new Color(1, 1, 1, 1);
                                break;
                        }

                    }
                }
            }
        }
        public static void GodMode(bool on)
        {
            if (on)
            {
                PlayerStatus.Instance.maxHp = 12800000;
                PlayerStatus.Instance.currentHp = 12800000;
            }
            else
            {
                PlayerStatus.Instance.maxHp = 100;
                PlayerStatus.Instance.currentHp = 100;
            }
        }

        public static void InfiniteCrouch(bool on)
        {
            if (on)
            {
                PlayerMovement.Instance.readyToCrouch = true;
            }
            else
            {

            }
        }

        public static void InfJump(bool on)
        {
            if (on)
            {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        PlayerMovement.FECONEHJOMF.PushPlayer(new Vector3(0.0f, 10.0f, 0.0f));
                    }
            }
        }

        public static void NoClip(bool on)
        {
            if (on)
            {
                foreach (Collider collider in UnityEngine.Object.FindObjectsOfType<Collider>())
                    collider.enabled = false;
            }
            else
            {
                foreach (Collider collider in UnityEngine.Object.FindObjectsOfType<Collider>())
                    collider.enabled = true;
            }
        }

        public static void SpeedHack(bool on)
        {
            if (on)
            {
                PlayerMovement.Instance.maxRunSpeed = 50f;
                PlayerMovement.Instance.maxWalkSpeed = 25f;
            }
            else
            {
                PlayerMovement.Instance.maxRunSpeed = 13f;
                PlayerMovement.Instance.maxWalkSpeed = 6.5f;
            }
        }

        public void Awake()
        {
            var harmony = new Harmony("xyz.fl1pnatic.bepminihack");
            harmony.PatchAll();
            //Turn off the anticheat
            InjectionDetector.StopDetection();
            ObscuredCheatingDetector.Dispose();
            SpeedHackDetector.StopDetection();
            WallHackDetector.StopDetection();
            TimeCheatingDetector.StopDetection();
        }
        public void Update()
        {
            if (!Chatbox.Instance.typing)
            {
                //Show the commands menu
                if (Input.GetKeyDown(KeyCode.I))
                {
                    vars.isHelpMenuOn = !vars.isHelpMenuOn;
                }

                //Chams
                if (Input.GetKeyDown(KeyCode.U))
                {
                    vars.areChamsOn = !vars.areChamsOn;
                    Chatbox.Instance.AppendMessage(1UL, "Chams: " + vars.areChamsOn + ".", "MiniHack");
                }
                Chams(vars.areChamsOn);

                //Glass Chams
                var il2CppArrayBaseG = UnityEngine.Object.FindObjectsOfType<GlassBreak>();
                foreach (var item in il2CppArrayBaseG)
                {


                    foreach (var a in item.GetComponentsInChildren<MeshRenderer>())
                    {
                        a.material.shader = Shader.Find("Hidden/Internal-Colored");
                        a.material.SetInt("_ZTest", 0);
                        a.material.color = Color.red;
                    }
                }

                //Godmode
                if (Input.GetKeyDown(KeyCode.G))
                {
                    vars.isGodOn = !vars.isGodOn;
                    Chatbox.Instance.AppendMessage(1UL, "Godmode: " + vars.isGodOn + ".", "MiniHack");
                }
                GodMode(vars.isGodOn);

                //Inf Crouch
                if (Input.GetKeyDown(KeyCode.K))
                {
                    vars.isInfCrouchOn = !vars.isInfCrouchOn;
                    Chatbox.Instance.AppendMessage(1UL, "Infinite crouch: " + vars.isInfCrouchOn + ".", "MiniHack");
                }
                InfiniteCrouch(vars.isInfCrouchOn);

                //Inf Jump
                if (Input.GetKeyDown(KeyCode.J))
                {
                    vars.isInfJumpOn = !vars.isInfJumpOn;
                    Chatbox.Instance.AppendMessage(1UL, "Infinite Jump: " + vars.isInfJumpOn + ".", "MiniHack");
                }
                InfJump(vars.isInfJumpOn);

                //Noclip
                if (Input.GetKeyDown(KeyCode.N))
                {
                    vars.isNCon = !vars.isNCon;
                    Chatbox.Instance.AppendMessage(1UL, "Noclip: " + vars.isNCon + ".", "MiniHack");
                }
                NoClip(vars.isNCon);

                //Speedhack
                if (Input.GetKeyDown(KeyCode.L))
                {
                    vars.isSpeedHackOn = !vars.isSpeedHackOn;
                    Chatbox.Instance.AppendMessage(1UL, "Speed: " + vars.isSpeedHackOn + ".", "MiniHack");
                }
                SpeedHack(vars.isSpeedHackOn);

                //Anti-knockback
                if (Input.GetKeyDown(KeyCode.O))
                {
                    vars.isAKBon = !vars.isAKBon;
                    Chatbox.Instance.AppendMessage(1UL, "AKB: " + vars.isAKBon + ".", "MiniHack");
                }
				
				//Fly
                if (Input.GetKeyDown(KeyCode.F))
                {
                    PlayerMovement.Instance.underWater = true;
                    PlayerMovement.Instance.swimSpeed = 5000f;
                    Chatbox.Instance.AppendMessage(1UL, "Flight activated.", "MiniHack");
                }
                if (Input.GetKeyUp(KeyCode.F))
                {
                    PlayerMovement.Instance.underWater = false;
                    Chatbox.Instance.AppendMessage(1UL, "Flight deactivated.", "MiniHack");
                }

                //Tp to 0,0,0
                if (Input.GetKeyDown(KeyCode.T))
                {
                    PlayerStatus.Instance.gameObject.transform.position = new Vector3(0f, 0f, 0f);
                    Chatbox.Instance.AppendMessage(1UL, "Teleported to 0,0,0.", "MiniHack");
                }

                //Heal
                if (Input.GetKeyDown(KeyCode.H))
                {
                    PlayerStatus.Instance.currentHp = PlayerStatus.Instance.maxHp;
                    Chatbox.Instance.AppendMessage(1UL, "You were healed!", "MiniHack");
                }

                //TP to cursor
                if (Input.GetMouseButtonDown(2))
                {
                    RaycastHit raycastHit;
                    Physics.Raycast(new Ray(PlayerMovement.Instance.transform.position, Camera.main.transform.forward), out raycastHit);
                    if (raycastHit.transform.name == "ProximityChat (1)")
                    {
                        UnityEngine.Object.Destroy(GameObject.Find("ProximityChat (1)"));
                        RaycastHit raycast2;
                        Physics.Raycast(new Ray(PlayerMovement.Instance.transform.position, Camera.main.transform.forward), out raycast2, 12f);
                        PlayerMovement.Instance.transform.position = raycast2.point + new Vector3(0f, 2f, 0f);
                        return;
                    }
                    PlayerMovement.Instance.transform.position = raycastHit.point + new Vector3(0f, 2f, 0f);
                    Chatbox.Instance.AppendMessage(1UL, "You were teleported.", "MiniHack");
                }

                //Force starting the game
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    Chatbox.Instance.AppendMessage(1UL, "Force starting the game.", "MiniHack");
                    GameLoop.Instance.StartGames();
                    UnityEngine.Object.FindObjectOfType<LobbyReadyInteract>().TryInteract();
                }
            }
        }

        public void OnGUI()
        {
            GUI.Label(new Rect(30, 35, 200, 50), "MiniMod v2.0.4-Git");
            GUI.Label(new Rect(30, 55, 200, 35), "https://github.com/Fl1pNatic/minimod/");
            GUI.Label(new Rect(30, 75, 200, 35), "Press I for commands.");

            if (vars.isHelpMenuOn)
            {

                GUI.Label(new Rect(30, 115, 200, 100), "Godmode: G");
                GUI.Label(new Rect(30, 135, 200, 100), "Heal: H");
                GUI.Label(new Rect(30, 155, 200, 100), "No cooldown Crouching: K");
                GUI.Label(new Rect(30, 175, 200, 100), "Fly: F");
                GUI.Label(new Rect(30, 195, 200, 100), "Force start: Backspace");
                GUI.Label(new Rect(30, 215, 200, 100), "TP to mouse cursor: Middle Mouse Button");
                GUI.Label(new Rect(30, 250, 200, 100), "Teleport to 0,0,0: T");
                GUI.Label(new Rect(30, 270, 200, 100), "Chams: U");
                GUI.Label(new Rect(30, 290, 200, 100), "Infinite Jump: J");
                GUI.Label(new Rect(30, 310, 200, 100), "Noclip: N");
                GUI.Label(new Rect(30, 330, 200, 100), "Speed: L");
                GUI.Label(new Rect(30, 350, 200, 100), "Anti-Knockback: O");
            }
            else
            {
            }

        }
    }
}