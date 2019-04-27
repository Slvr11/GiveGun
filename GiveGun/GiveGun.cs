using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;
using InfinityScript;
//using InfinityScript.ISWait;
using wait = InfinityScript.ISWait;
using System.IO;

namespace CustomWeapon
{
    public class CustomWeapon : BaseScript
    {
        public int search_entID = 0;
        public CustomWeapon()
        {

            //working guns
            Call("precacheitem", "at4_mp");
            Call("precacheitem", "airdrop_mega_marker_mp");
            Call("precacheitem", "throwingknife_rhand_mp");
            Call("precacheitem", "iw5_mk12spr_mp");
            Call("precacheitem", "lightstick_mp");
            Call("precacheitem", "killstreak_double_uav_mp");
            Call("precacheitem", "strike_marker_mp");
            Call("precacheitem", "killstreak_helicopter_minigun_mp");
            Call("precacheitem", "airdrop_juggernaut_def_mp");
            Call("precacheitem", "uav_strike_missile_mp");
            Call("precacheitem", "uav_strike_projectile_mp");
            Call("precacheitem", "iw5_xm25_mp");
            Call("precacheitem", "iw5_riotshield_mp");
            
            //turret-only
            Call("precacheitem", "manned_minigun_turret_mp");
            Call("precacheitem", "manned_gl_turret_mp");
            Call("precacheitem", "remote_uav_weapon_mp");
            Call("precacheitem", "aamissile_projectile_mp");

            //Call("setdvar", "scr_elevator_disabled", "0");
            Call(332, "ui_customModeName", "^2Test Mode FGTS");
            Call("setDvar", "ui_customModeName", "^2Test Mode FGTS");
            Call(332, "ui_selectedFeederMap", "^2Test Mode FGTS");
            Call("setDvar", "ui_selectedFeederMap", "^2Test Mode FGTS");
            Call(332, "ui_netGametypeName", "^2Test Mode FGTS");
            Call("setDvar", "ui_netGametypeName", "^2Test Mode FGTS");
            Call(332, "ui_recipeName", "^2Test Mode FGTS");
            Call("setDvar", "ui_recipeName", "^2Test Mode FGTS");
            Call(332, "ui_browserMod", "^2Test Mode FGTS");
            Call("setDvar", "ui_browserMod", "^2Test Mode FGTS");
            Call("setDvarIfUninitialized", "dev_showNotifyMessages", 0);
            //Entity[] elevator = { Call<Entity>("spawn", "script_model", new Vector3(0, 0, 0)), Call<Entity>("spawn", "script_brushmodel", new Vector3(0, 0, 0)) };

            Notified += new Action<string, Parameter[]>((message, parameters) =>
                    {
                        int dvar = Call<int>("getdvarint", "dev_showNotifyMessages");
                        if (dvar != 1) return;
                        if (parameters.Length > 0)
                            foreach (string p in parameters)
                                Log.Write(LogLevel.All, "Notified " + message + ":" + p);
                        else Log.Write(LogLevel.All, "Notified " + message);
                    });
            PlayerConnected += new Action<Entity>(entity =>
                {
                    entity.SpawnedPlayer += () => OnPlayerSpawned(entity);
                    /*
                    entity.Notified += new Action<string, Parameter[]>((message, parameters) =>
                    {
                        int dvar = Call<int>("getdvarint", "dev_showNotifyMessages");
                        if (dvar != 1) return;
                        if (parameters.Length > 0)
                            foreach (string p in parameters)
                                Log.Write(LogLevel.All, entity.Name + ";" + message + ":" + p);
                        else Log.Write(LogLevel.All, entity.Name + " Notified " + message);
                    });
                     */
                    //wait.Start(waittillTest(entity));
                });
        }
        public void OnPlayerSpawned(Entity thisentity)
        {
            //int isSSPlayer = player.Call<int>(33454);
            //Log.Write(LogLevel.All, "isSSPlayer:");
            //Log.Write(LogLevel.All, "{0}", isSSPlayer);
        }

        public IEnumerator waittillTest(Entity player)
        {
            //Log.Write(LogLevel.All, "Waiting for begin_firing...");
            //yield return wait.WaitTill("begin_firing");
            Log.Write(LogLevel.All, "Waiting for jump...");
            yield return player.WaitTill("jumped");
            Log.Write(LogLevel.All, "Player jumped");
            Log.Write(LogLevel.All, "Waiting for global begin_firing...");
            yield return wait.WaitTill("begin_firing");
            Log.Write(LogLevel.All, "Global notify begin_firing");
        }

        public override void OnSay(Entity player, string name, string message)
        {

            if (message.StartsWith("giveall "))
            {
                foreach (Entity p in Players)
                {
                    string gun = message.Split(' ')[1];
                    p.GiveWeapon(gun);
                    p.AfterDelay(200, (p2) =>
                        {
                            p2.TakeWeapon(player.CurrentWeapon);
                            p2.SwitchToWeaponImmediate(gun);
                            p2.Call("givemaxammo", gun);
                        });
                }
                return;
            }
            if (message.StartsWith("test2 "))
            {
                Entity trigger = Call<Entity>("spawn", "trigger_radius", player.Origin, 0, 64, 64);
                trigger.SetField("angles", new Vector3(0, 0, 0));
                trigger.Call("solid");
                trigger.Call("setcontents", 1);
                trigger.Call("solid");
                Entity triggerV = Call<Entity>("spawn", "script_model", player.Origin);
                triggerV.Call("setmodel", "test_sphere_silver");
                return;
                Entity fags = Call<Entity>(2);
                Log.Write(LogLevel.All, fags.ToString());
                //Entity test = Call<Entity>("spawn", "script_model", new Vector3(0, 0, 0));
                //test.Call("setmodel", "test_sphere_silver");
                //test.Call(33281);
                //HudElem test = Call<HudElem>(455, ");
                //Entity test = Call<Entity>(32765, "Test");
                //Log.Write(LogLevel.All, "{0}", test);
            }
            if (message == "dumpHud")
            {
                HudElem[] ents = new HudElem[1024];
                for (int i = 65536; i < 66560; i++)
                {
                    ents[i - 65536] = HudElem.GetHudElem(i);
                }
                StreamWriter debugLog = new StreamWriter("scripts\\hudDebugLog.txt", false);
                //int worldNum = Call<int>("worldentnumber");
                debugLog.WriteLine("HudElem data RT with {0} clients connected", Players.Count);
                foreach (HudElem ent in ents)
                {
                    //foreach (Entity e in entBank[key])
                    {
                        string font = "";
                        float alpha = 0f;
                        string label = "";
                        int sort = -1;
                        int X = -1;
                        int Y = -1;
                        bool Archived = false;
                        font = ent.GetField<string>("font");
                        alpha = ent.GetField<float>("alpha");
                        label = ent.GetField<string>("label");
                        sort = ent.GetField<int>("sort");
                        X = ent.GetField<int>("x");
                        Y = ent.GetField<int>("y");
                        Archived = ent.GetField<int>("archived") != 0;

                        debugLog.WriteLine("Hud {0}; font = {1}; alpha = {2}; label = {3}; sort = {4}; X = {5}; Y = {6}; Archived = {7}", ent.Entity.EntRef, font, alpha, label, sort, X, Y, Archived);
                    }
                }
                debugLog.Flush();
                debugLog.Close();
                debugLog.Dispose();
            }
            /*
            if (message.StartsWith("getField "))
            {
                object ret;
                int id = int.Parse(message.Split(' ')[1]);
                ret = player.GetField(id);
                try
                {
                    Log.Write(LogLevel.All, "Field {0} = " + ret.ToString(), id);
                }
                catch
                {
                    Log.Write(LogLevel.All, "Field {0} is null", id);
                }
            }
            if (message.StartsWith("setField "))
            {
                int id = int.Parse(message.Split(' ')[1]);
                int intVal;
                if (int.TryParse(message.Split(' ')[2], out intVal))
                {
                    int val = int.Parse(message.Split(' ')[2]);
                    player.SetField(id, val);
                }
                else player.SetField(id, message.Split(' ')[2]);
            }
            if (message.StartsWith("getPers "))
            {
                Entity pers = (Entity)player.GetField(24597);
                object ret;
                int id = int.Parse(message.Split(' ')[1]);
                ret = pers.GetField(id);
                try
                {
                    Log.Write(LogLevel.All, "Field {0} = " + ret.ToString(), id);
                }
                catch
                {
                    Log.Write(LogLevel.All, "Field {0} is null", id);
                }
            }
             */
            if (message.StartsWith("setPlayerData"))
            {
                int val = int.Parse(message.Split(' ')[2]);
                player.Call(33306, message.Split(' ')[1], val);
            }
            if (message.StartsWith("setClientMatchData"))
            {
                if (message.Split(' ').Length < 3)
                {
                    Utilities.SayTo(player, "^2Invalid params. Usage: setClientMatchData <name> <value>");
                    return;
                }
                if (message.Split(' ').Length == 3)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[2], out value))
                    {
                        int val = int.Parse(message.Split(' ')[2]);
                        Call("setclientmatchdata", message.Split(' ')[1], val);
                    }
                    else Call("setclientmatchdata", message.Split(' ')[1], message.Split(' ')[2]);
                }
                else if (message.Split(' ').Length == 4)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[3], out value))
                    {
                        int val = int.Parse(message.Split(' ')[3]);
                        Call("setclientmatchdata", message.Split(' ')[1], message.Split(' ')[2], val);
                    }
                    else Call("setclientmatchdata", message.Split(' ')[1], message.Split(' ')[2], message.Split(' ')[3]);
                }
                Call("sendclientmatchdata");
                Log.Write(LogLevel.All, "Sent client data successfully");
            }
            wait.Start(OnSpeak(player, name, message));
        }
        public Vector3 PhysicsTraceFromEye(Entity e)
        {
            var headPosition = e.Call<Vector3>(32921); // props to kenny
            Vector3 playerangles = e.Call<Vector3>(33532);
            var forwardAngles = Call<Vector3>(252, playerangles);
            var traceEndPosition = headPosition + forwardAngles * 50000; // far to the forward towards crosshair
            Vector3 trace = Call<Vector3>(118, headPosition, traceEndPosition);
            return trace;
        }
        public IEnumerator OnSpeak(Entity player, string name, string message)
        {
            if (message.StartsWith("turret"))
            {
                //
                //Entity veh = Call<Entity>("spawnvehicle", "defaultvehicle", "defaultvehicle_mp", "defaultvehicle_mp", player.Origin + new Vector3(0, 200, 5), player.GetField<Vector3>("angles"));
                //veh.Call("makevehiclesolidcapsule", 64f, 0f, 32f);
                //veh.Call(33323, "allies");
                //veh.Call(33322, "The Dickweed");
                //yield return player.WaitTill("jumped");
                ////player.Call("freezecontrols", true);
                //player.Call("drivevehicleandcontrolturret", veh);
                //player.Call("playerlinkto", veh, "tag_player");
                ////player.Call("cameralinkto", veh, "tag_origin");
                //yield break;
                //Vector3 pathStart = player.Origin + new Vector3(0, 0, 125);
                //Vector3 end = player.Origin + new Vector3(0, 1500, 750);
                //Vector3 forward = Call<Vector3>(252, end - pathStart);
                ////Entity veh = Call<Entity>("spawnhelicopter", player, pathStart, forward, "littlebird_mp", "vehicle_little_bird_armed");
                //if (veh == null) yield break;
                //veh.Health = 3000;
                ////veh.SetField("missiles", 1);
                //veh.Call("enablelinkto");
                //veh.Call(33323, "allies");
                //veh.Call(33322, "The Dickweed");
                //veh.Call(33381, .1f);
                //veh.Call(33337, "littlebird_20mm_mp");//SetVehWeapon
                //player.Call("iprintlnbold", "Jump to enter Heli");
                //yield return player.WaitTill("jumped");
                //player.Call("freezecontrols", true);
                //Entity cam = Call<Entity>("spawn", "script_model", player.Call<Vector3>("geteye"));
                //cam.SetField("angles", player.GetField<Vector3>("angles"));
                //cam.Call("setmodel", "tag_origin");
                //player.Call("cameralinkto", cam, "tag_origin");
                //Vector3 camPos = veh.Call<Vector3>("gettagorigin", "tag_origin") + new Vector3(0, 0, 10);
                //cam.Call("moveto", camPos, 3, .5f, .5f);
                //cam.Call("rotateto", veh.GetField<Vector3>("angles"), 3, .5f, .5f);
                //yield return ISWait.Wait(3);
                //player.Origin = camPos;
                //player.Call("setstance", "crouch");
                //player.Call("playerlinkto", veh, "tag_driver");
                ////cam.Call("linkto", veh, "tag_driver", new Vector3(0, 0, 10), new Vector3(0, 30, 0));
                //cam.Call("delete");
                //player.Call("cameralinkto", veh, "tag_origin");
                //player.Call("controlslinkto", veh);
                ////player.Call("freezecontrols", false);
                ////Entity att = Call<Entity>(372, Players[1], 5000, 5000);
                //veh.OnNotify("damage", (heli, dmg, attacker, point, dir, mod, modelName, partName, tagName, iDFlags, weapon) =>
                //    {
                //        heli.Call(33362, attacker, attacker, dmg, iDFlags, mod, weapon, point, dir, tagName, 0, modelName, partName);
                //    });
                ///*
                //veh.OnInterval(5000, (v) =>
                //    {
                //        if (veh.GetField<int>("missiles") < 5)
                //            veh.SetField("missiles", veh.GetField<int>("missiles") + 1);
                //        return true;
                //    });
                // */
                //player.OnInterval(50, (p) =>
                //    {
                //        bool isShooting = p.Call<int>(33534) != 0;
                //        //bool isAltShooting = p.Call<int>(33535) != 0;
                //        if (isShooting)
                //        {
                //            veh.Call(33338);
                //            return true;
                //        }
                //            /*
                //        else if (isAltShooting)
                //        {
                //            if (veh.GetField<int>("missiles") > 0)
                //            {
                //                Call("magicbullet", "harrier_missile_mp", veh.Call<Vector3>("gettagorigin", "tag_flash"), Players[1].Origin, player);
                //                veh.SetField("missiles", veh.GetField<int>("missiles") - 1);
                //            }
                //            return true;
                //        }
                //             */
                //        else if (p.IsAlive) return true;
                //        else
                //        {
                //            player.Call("controlsunlink");
                //            player.Call("cameraunlink");
                //            veh.Call("delete");
                //            return false;
                //        }
                //    });
                ////veh.Call(33337, "cobra_20mm_mp");//SetVehWeapon
                ////veh.Call(33338, "tag_flash", player, new Vector3(0, 0, 0), .05f);//FireWeapon
                ////veh.Call(32714, 300, 75);//Veh_SetSpeed
                ////veh.Call(32742, end, 1);//setVehGoalPos
                Call("obituary", player, Players[1], message.Split(' ')[1], "MOD_RIFLE_BULLET");
                yield break;
                Entity turret = Call<Entity>("spawnturret", "misc_turret", player.Origin, message.Split(' ')[1]);
                turret.Call("setmodel", "sentry_minigun");
                turret.Health = 100;
                turret.Call("setCanDamage", true);
                //turret.Call(33054);
                //turret.Call(33083, 80);
                //turret.Call(33084, 80);
                //turret.Call(33086, 50);
                //turret.Call(32941);
                turret.Call(33053);
                turret.Call(33088, -89.0f);
                turret.Call(33122, true);
                turret.Call(32864, "sentry");
                //turret.SetField("owner", player);
                //turret.SetField("team", "allies");
                turret.Call(33051, "allies");
                turret.Call("settargetentity", Players[1]);
                //turret.Call(33006, player);
                //turret.Call("makeusable");
                //yield return player.WaitTill("jumped");
                //turret.Call("delete");
                player.OnNotify("jumped", (p)
                     => turret.Call("shootturret"));
                /*
                Entity trig = Call<Entity>("spawn", "trigger_radius", new Vector3(0, 0, 0), 0, 65, 30);
                trig.Origin = player.Origin;
                trig.SetField("angles", new Vector3(0, 90, 0));
                Entity vis = Call<Entity>("spawn", "script_model", trig.Origin);
                vis.Call("setmodel", "test_sphere_silver");
                trig.Call("makeusable");
                trig.Call(33123);
                trig.Call(32781, "allies");
                player.Call(32782, trig);
                trig.Call("setcursorhint", "HINT_NOICON");
                trig.Call("sethintstring", "Test");
                //Log.Write(LogLevel.All, "{0};{1};{2}", player.Origin.X, player.Origin.Y, player.Origin.Z);
                 */
            }
            if (message == "giveNuke")
                nuke.NukeFuncs.giveNuke(player);
            if (message.StartsWith("menu"))
                player.Call("openmenu", message.Split(' ')[1]);
            if (message.StartsWith("popupmenu"))
                player.Call("openpopupmenu", message.Split(' ')[1]);
            if (message.StartsWith("notify"))
            {
                if (message.Split(' ').Length == 2)
                    player.Notify(message.Split(' ')[1]);
                if (message.Split(' ').Length == 3)
                    player.Notify(message.Split(' ')[1], message.Split(' ')[2]);
                if (message.Split(' ').Length == 4)
                    player.Notify(message.Split(' ')[1], message.Split(' ')[2], message.Split(' ')[3]);
            }
            if (message.StartsWith("globalNotify"))
            {
                if (message.Split(' ').Length == 2)
                    Notify(message.Split(' ')[1]);
                if (message.Split(' ').Length == 3)
                    Notify(message.Split(' ')[1], message.Split(' ')[2]);
                if (message.Split(' ').Length == 4)
                    Notify(message.Split(' ')[1], message.Split(' ')[2], message.Split(' ')[3]);
            }
            if (message.StartsWith("self"))
            {
                int par;
                if (int.TryParse(message.Split(' ')[2], out par))
                    player.SetField(message.Split(' ')[1], par);
                else player.SetField(message.Split(' ')[1], message.Split(' ')[2]);
            }
            if (message.StartsWith("set "))
            {
                int ret;
                if (int.TryParse(message.Split(' ')[2], out ret))
                    player.Call(33473, message.Split(' ')[1], ret);
                else player.SetClientDvar(message.Split(' ')[1], message.Split(' ')[2]);
            }
            if (message.StartsWith("give "))
            {
                player.GiveWeapon(message.Split(' ')[1]);
                yield return wait.WaitForFrame();
                player.TakeWeapon(player.CurrentWeapon);
                player.SwitchToWeaponImmediate(message.Split(' ')[1]);
                player.Call("givemaxammo", message.Split(' ')[1]);
            }
            if (message.StartsWith("add"))
            {
                player.Call(33487, message.Split(' ')[1]);
                yield return wait.WaitForFrame();
                player.SwitchToWeaponImmediate(message.Split(' ')[1]);
            }
            if (message.StartsWith("alt"))
            {
                yield return wait.WaitForFrame();
                player.SwitchToWeapon(message.Split(' ')[1]);
            }
            if (message.StartsWith("perk"))
            {
                foreach (Entity person in Players)
                {
                    person.SetPerk(message.Split(' ')[1], false, true);
                    //player.SetPerk(message.Split(' ')[1], false, true);
                }
            }
            if (message.StartsWith("codeperk"))
            {
                foreach (Entity person in Players)
                {
                    person.SetPerk(message.Split(' ')[1], true, true);
                    //player.SetPerk(message.Split(' ')[1], false, true);
                }
            }
            if (message.StartsWith("unsetperk"))
            {
                foreach (Entity person in Players)
                {
                    person.Call("unsetperk", message.Split(' ')[1], true);
                    person.Call("unsetperk", message.Split(' ')[1], true);
                }
            }
            if (message.StartsWith("s_getClientMatchData"))
            {
                if (message.Split(' ').Length < 2)
                {
                    Utilities.SayTo(player, "^2Invalid params. Usage: s_getClientMatchData <name>");
                    yield return 0;
                }
                string ret = "";
                if (message.Split(' ').Length == 2)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[1], out value))
                    {
                        int val = int.Parse(message.Split(' ')[1]);
                        ret = Call<string>("getclientmatchdata", val);
                    }
                    else ret = Call<string>("getclientmatchdata", message.Split(' ')[1]);
                }
                if (message.Split(' ').Length == 3)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[2], out value))
                    {
                        int val = int.Parse(message.Split(' ')[2]);
                        ret = Call<string>("getclientmatchdata", message.Split(' ')[1], val);
                    }
                    else ret = Call<string>("getclientmatchdata", message.Split(' ')[1], message.Split(' ')[2]);
                }
                else if (message.Split(' ').Length == 4)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[3], out value))
                    {
                        int val = int.Parse(message.Split(' ')[3]);
                        ret = Call<string>("getclientmatchdata", message.Split(' ')[1], message.Split(' ')[2], val);
                    }
                    else ret = Call<string>("getclientmatchdata", message.Split(' ')[1], message.Split(' ')[2], message.Split(' ')[3]);
                }
                Log.Write(LogLevel.All, ret);
            }
            if (message.StartsWith("i_getClientMatchData"))
            {
                if (message.Split(' ').Length < 2)
                {
                    Utilities.SayTo(player, "^2Invalid params. Usage: i_getClientMatchData <name>");
                    yield return 0;
                }
                int ret = 0;
                if (message.Split(' ').Length == 2)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[1], out value))
                    {
                        int val = int.Parse(message.Split(' ')[1]);
                        ret = Call<int>("getclientmatchdata", val);
                    }
                    else ret = Call<int>("getclientmatchdata", message.Split(' ')[1]);
                }
                if (message.Split(' ').Length == 3)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[2], out value))
                    {
                        int val = int.Parse(message.Split(' ')[2]);
                        ret = Call<int>("getclientmatchdata", message.Split(' ')[1], val);
                    }
                    else ret = Call<int>("getclientmatchdata", message.Split(' ')[1], message.Split(' ')[2]);
                }
                else if (message.Split(' ').Length == 4)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[3], out value))
                    {
                        int val = int.Parse(message.Split(' ')[3]);
                        ret = Call<int>("getclientmatchdata", message.Split(' ')[1], message.Split(' ')[2], val);
                    }
                    else ret = Call<int>("getclientmatchdata", message.Split(' ')[1], message.Split(' ')[2], message.Split(' ')[3]);
                }
                Log.Write(LogLevel.All, "{0}", ret);
            }
            if (message.StartsWith("setMatchData"))
            {
                if (message.Split(' ').Length < 3)
                {
                    Utilities.SayTo(player, "^2Invalid params. Usage: setMatchData <name> <value>");
                    yield return 0;
                }
                if (message.Split(' ').Length == 3)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[2], out value))
                    {
                        int val = int.Parse(message.Split(' ')[2]);
                        Call("setmatchdata", message.Split(' ')[1], val);
                    }
                    else Call("setmatchdata", message.Split(' ')[1], message.Split(' ')[2]);
                }
                else if (message.Split(' ').Length == 4)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[3], out value))
                    {
                        int val = int.Parse(message.Split(' ')[3]);
                        Call("setmatchdata", message.Split(' ')[1], message.Split(' ')[2], val);
                    }
                    else Call("setmatchdata", message.Split(' ')[1], message.Split(' ')[2], message.Split(' ')[3]);
                }
                Call("sendmatchdata");
                Call("setmatchdataid");
            }
            if (message.StartsWith("s_getMatchData"))
            {
                if (message.Split(' ').Length < 2)
                {
                    Utilities.SayTo(player, "^2Invalid params. Usage: s_getMatchData <name>");
                    yield return 0;
                }
                string ret = "";
                if (message.Split(' ').Length == 2)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[1], out value))
                    {
                        int val = int.Parse(message.Split(' ')[1]);
                        ret = Call<string>("getmatchdata", val);
                    }
                    else ret = Call<string>("getmatchdata", message.Split(' ')[1]);
                }
                if (message.Split(' ').Length == 3)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[2], out value))
                    {
                        int val = int.Parse(message.Split(' ')[2]);
                        ret = Call<string>("getmatchdata", message.Split(' ')[1], val);
                    }
                    else ret = Call<string>("getmatchdata", message.Split(' ')[1], message.Split(' ')[2]);
                }
                else if (message.Split(' ').Length == 4)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[3], out value))
                    {
                        int val = int.Parse(message.Split(' ')[3]);
                        ret = Call<string>("getmatchdata", message.Split(' ')[1], message.Split(' ')[2], val);
                    }
                    else ret = Call<string>("getmatchdata", message.Split(' ')[1], message.Split(' ')[2], message.Split(' ')[3]);
                }
                Log.Write(LogLevel.All, ret);
            }
            if (message.StartsWith("i_getMatchData"))
            {
                if (message.Split(' ').Length < 2)
                {
                    Utilities.SayTo(player, "^2Invalid params. Usage: i_getMatchData <name>");
                    yield return 0;
                }
                int ret = 0;
                if (message.Split(' ').Length == 2)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[1], out value))
                    {
                        int val = int.Parse(message.Split(' ')[1]);
                        ret = Call<int>("getmatchdata", val);
                    }
                    else ret = Call<int>("getmatchdata", message.Split(' ')[1]);
                }
                if (message.Split(' ').Length == 3)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[2], out value))
                    {
                        int val = int.Parse(message.Split(' ')[2]);
                        ret = Call<int>("getmatchdata", message.Split(' ')[1], val);
                    }
                    else ret = Call<int>("getmatchdata", message.Split(' ')[1], message.Split(' ')[2]);
                }
                else if (message.Split(' ').Length == 4)
                {
                    int value;
                    if (int.TryParse(message.Split(' ')[3], out value))
                    {
                        int val = int.Parse(message.Split(' ')[3]);
                        ret = Call<int>("getmatchdata", message.Split(' ')[1], message.Split(' ')[2], val);
                    }
                    else ret = Call<int>("getmatchdata", message.Split(' ')[1], message.Split(' ')[2], message.Split(' ')[3]);
                }
                Log.Write(LogLevel.All, "{0}", ret);
            }
            if (message.StartsWith("setClass"))
            {
                if (message.Split(' ').Length == 5)
                    player.Call(33306, "customClasses", int.Parse(message.Split(' ')[1]), message.Split(' ')[2], int.Parse(message.Split(' ')[3]), message.Split(' ')[4]);
                else if (message.Split(' ').Length == 4)
                    player.Call(33306, "customClasses", int.Parse(message.Split(' ')[1]), message.Split(' ')[2], message.Split(' ')[3]);
            }
            if (message.StartsWith("drop"))
            {
                Entity item = player.Call<Entity>(33314, message.Split(' ')[1]);
            }
            if (message.StartsWith("sound"))
                player.Call("playlocalsound", message.Split(' ')[1]);
            yield return 0;
        }
        public void initEntSearch(Entity player)
        {
            //for (int i = 0; i < 2048; i++)
            //{
            if (search_entID > 2048) return;
            Entity e = Entity.GetEntity(search_entID);
            if (e.GetField<string>("targetname") == "glass")
            {
                Log.Write(LogLevel.All, "Found glass on ent {0}", search_entID);
                player.Call("iprintlnbold", "Found glass: " + search_entID);
                //eye.Origin = e.Origin;
                //eye.Call("moveto", e.Origin, 2, .5f, .5f);
                //AfterDelay(5000, () =>
                //{
                search_entID++;
                initEntSearch(player);
                //});
                //e.Call("setmodel", "viewmodel_m16");
            }
            else
            {
                search_entID++;
                initEntSearch(player);
            }
            //}
        }
    }
}
