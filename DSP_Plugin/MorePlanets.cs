using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace DSP_Plugin {
    [BepInPlugin("touhma.dsp.plugins.galactic-scale-planets", "Galactic Scale - Planets Plug-In", "1.0.0.0")]
    public class MorePlanets : BaseUnityPlugin {
        private static ConfigEntry<int> _configStarsMax;
        private static ConfigEntry<int> _configStarsMin;

        internal void Awake() { }

        [HarmonyPatch(typeof(StarGen))]
        private class PatchOnStarGen {
            [HarmonyPrefix]
            [HarmonyPatch("CreateStarPlanets")]
            public static bool CreateStarPlanets(GalaxyData galaxy, StarData star, GameDesc gameDesc) {
                double[] pgasRef = Traverse.Create(typeof(PlanetGen)).Field(
                    "pGas").GetValue<double[]>();

                System.Random random1 = new System.Random(star.seed);
                random1.Next();
                random1.Next();
                random1.Next();
                System.Random random2 = new System.Random(random1.Next());
                double num1 = random2.NextDouble();
                double num2 = random2.NextDouble();
                double num3 = random2.NextDouble();
                double num4 = random2.NextDouble();
                double num5 = random2.NextDouble();
                double num6 = random2.NextDouble() * 0.2 + 0.9;
                double num7 = random2.NextDouble() * 0.2 + 0.9;
                if (star.type == EStarType.BlackHole) {
                    star.planetCount = 1;
                    star.planets = new PlanetData[star.planetCount];
                    int info_seed = random2.Next();
                    int gen_seed = random2.Next();
                    star.planets[0] =
                        PlanetGen.CreatePlanet(galaxy, star, gameDesc, 0, 0, 3, 1, false, info_seed, gen_seed);
                }
                else if (star.type == EStarType.NeutronStar) {
                    star.planetCount = 1;
                    star.planets = new PlanetData[star.planetCount];
                    int info_seed = random2.Next();
                    int gen_seed = random2.Next();
                    star.planets[0] =
                        PlanetGen.CreatePlanet(galaxy, star, gameDesc, 0, 0, 3, 1, false, info_seed, gen_seed);
                }
                else if (star.type == EStarType.WhiteDwarf) {
                    if (num1 < 0.699999988079071) {
                        star.planetCount = 1;
                        star.planets = new PlanetData[star.planetCount];
                        int info_seed = random2.Next();
                        int gen_seed = random2.Next();
                        star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 0, 0, 3, 1, false, info_seed,
                            gen_seed);
                    }
                    else {
                        star.planetCount = 2;
                        star.planets = new PlanetData[star.planetCount];
                        if (num2 < 0.300000011920929) {
                            int info_seed1 = random2.Next();
                            int gen_seed1 = random2.Next();
                            star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 0, 0, 3, 1, false,
                                info_seed1, gen_seed1);
                            int info_seed2 = random2.Next();
                            int gen_seed2 = random2.Next();
                            star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 1, 0, 4, 2, false,
                                info_seed2, gen_seed2);
                        }
                        else {
                            int info_seed1 = random2.Next();
                            int gen_seed1 = random2.Next();
                            star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 0, 0, 4, 1, true,
                                info_seed1, gen_seed1);
                            int info_seed2 = random2.Next();
                            int gen_seed2 = random2.Next();
                            star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 1, 1, 1, 1, false,
                                info_seed2, gen_seed2);
                        }
                    }
                }
                else if (star.type == EStarType.GiantStar) {
                    if (num1 < 0.300000011920929) {
                        star.planetCount = 1;
                        star.planets = new PlanetData[star.planetCount];
                        int info_seed = random2.Next();
                        int gen_seed = random2.Next();
                        star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 0, 0, num3 <= 0.5 ? 2 : 3, 1,
                            false, info_seed, gen_seed);
                    }
                    else if (num1 < 0.800000011920929) {
                        star.planetCount = 2;
                        star.planets = new PlanetData[star.planetCount];
                        if (num2 < 0.25) {
                            int info_seed1 = random2.Next();
                            int gen_seed1 = random2.Next();
                            star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 0, 0, num3 <= 0.5 ? 2 : 3,
                                1, false, info_seed1, gen_seed1);
                            int info_seed2 = random2.Next();
                            int gen_seed2 = random2.Next();
                            star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 1, 0, num3 <= 0.5 ? 3 : 4,
                                2, false, info_seed2, gen_seed2);
                        }
                        else {
                            int info_seed1 = random2.Next();
                            int gen_seed1 = random2.Next();
                            star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 0, 0, 3, 1, true,
                                info_seed1, gen_seed1);
                            int info_seed2 = random2.Next();
                            int gen_seed2 = random2.Next();
                            star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 1, 1, 1, 1, false,
                                info_seed2, gen_seed2);
                        }
                    }
                    else {
                        star.planetCount = 3;
                        star.planets = new PlanetData[star.planetCount];
                        if (num2 < 0.150000005960464) {
                            int info_seed1 = random2.Next();
                            int gen_seed1 = random2.Next();
                            star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 0, 0, num3 <= 0.5 ? 2 : 3,
                                1, false, info_seed1, gen_seed1);
                            int info_seed2 = random2.Next();
                            int gen_seed2 = random2.Next();
                            star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 1, 0, num3 <= 0.5 ? 3 : 4,
                                2, false, info_seed2, gen_seed2);
                            int info_seed3 = random2.Next();
                            int gen_seed3 = random2.Next();
                            star.planets[2] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 2, 0, num3 <= 0.5 ? 4 : 5,
                                3, false, info_seed3, gen_seed3);
                        }
                        else if (num2 < 0.75) {
                            int info_seed1 = random2.Next();
                            int gen_seed1 = random2.Next();
                            star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 0, 0, num3 <= 0.5 ? 2 : 3,
                                1, false, info_seed1, gen_seed1);
                            int info_seed2 = random2.Next();
                            int gen_seed2 = random2.Next();
                            star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 1, 0, 4, 2, true,
                                info_seed2, gen_seed2);
                            int info_seed3 = random2.Next();
                            int gen_seed3 = random2.Next();
                            star.planets[2] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 2, 2, 1, 1, false,
                                info_seed3, gen_seed3);
                        }
                        else {
                            int info_seed1 = random2.Next();
                            int gen_seed1 = random2.Next();
                            star.planets[0] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 0, 0, num3 <= 0.5 ? 3 : 4,
                                1, true, info_seed1, gen_seed1);
                            int info_seed2 = random2.Next();
                            int gen_seed2 = random2.Next();
                            star.planets[1] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 1, 1, 1, 1, false,
                                info_seed2, gen_seed2);
                            int info_seed3 = random2.Next();
                            int gen_seed3 = random2.Next();
                            star.planets[2] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, 2, 1, 2, 2, false,
                                info_seed3, gen_seed3);
                        }
                    }
                }
                else {
                    Array.Clear((Array) pgasRef, 0, pgasRef.Length);
                    if (star.index == 0) {
                        star.planetCount = 4;
                        pgasRef[0] = 0.0;
                        pgasRef[1] = 0.0;
                        pgasRef[2] = 0.0;
                    }
                    else if (star.spectr == ESpectrType.M) {
                        star.planetCount = num1 >= 0.1 ? (num1 >= 0.3 ? (num1 >= 0.8 ? 4 : 3) : 2) : 1;
                        if (star.planetCount <= 3) {
                            pgasRef[0] = 0.2;
                            pgasRef[1] = 0.2;
                        }
                        else {
                            pgasRef[0] = 0.0;
                            pgasRef[1] = 0.2;
                            pgasRef[2] = 0.3;
                        }
                    }
                    else if (star.spectr == ESpectrType.K) {
                        star.planetCount =
                            num1 >= 0.1 ? (num1 >= 0.2 ? (num1 >= 0.7 ? (num1 >= 0.95 ? 5 : 4) : 3) : 2) : 1;
                        if (star.planetCount <= 3) {
                            pgasRef[0] = 0.18;
                            pgasRef[1] = 0.18;
                        }
                        else {
                            pgasRef[0] = 0.0;
                            pgasRef[1] = 0.18;
                            pgasRef[2] = 0.28;
                            pgasRef[3] = 0.28;
                        }
                    }
                    else if (star.spectr == ESpectrType.G) {
                        star.planetCount = num1 >= 0.4 ? (num1 >= 0.9 ? 5 : 4) : 3;
                        if (star.planetCount <= 3) {
                            pgasRef[0] = 0.18;
                            pgasRef[1] = 0.18;
                        }
                        else {
                            pgasRef[0] = 0.0;
                            pgasRef[1] = 0.2;
                            pgasRef[2] = 0.3;
                            pgasRef[3] = 0.3;
                        }
                    }
                    else if (star.spectr == ESpectrType.F) {
                        star.planetCount = num1 >= 0.35 ? (num1 >= 0.8 ? 5 : 4) : 3;
                        if (star.planetCount <= 3) {
                            pgasRef[0] = 0.2;
                            pgasRef[1] = 0.2;
                        }
                        else {
                            pgasRef[0] = 0.0;
                            pgasRef[1] = 0.22;
                            pgasRef[2] = 0.31;
                            pgasRef[3] = 0.31;
                        }
                    }
                    else if (star.spectr == ESpectrType.A) {
                        star.planetCount = num1 >= 0.3 ? (num1 >= 0.75 ? 5 : 4) : 3;
                        if (star.planetCount <= 3) {
                            pgasRef[0] = 0.2;
                            pgasRef[1] = 0.2;
                        }
                        else {
                            pgasRef[0] = 0.1;
                            pgasRef[1] = 0.28;
                            pgasRef[2] = 0.3;
                            pgasRef[3] = 0.35;
                        }
                    }
                    else if (star.spectr == ESpectrType.B) {
                        star.planetCount = num1 >= 0.3 ? (num1 >= 0.75 ? 6 : 5) : 4;
                        if (star.planetCount <= 3) {
                            pgasRef[0] = 0.2;
                            pgasRef[1] = 0.2;
                        }
                        else {
                            pgasRef[0] = 0.1;
                            pgasRef[1] = 0.22;
                            pgasRef[2] = 0.28;
                            pgasRef[3] = 0.35;
                            pgasRef[4] = 0.35;
                        }
                    }
                    else if (star.spectr == ESpectrType.O) {
                        star.planetCount = num1 >= 0.5 ? 6 : 5;
                        pgasRef[0] = 0.1;
                        pgasRef[1] = 0.2;
                        pgasRef[2] = 0.25;
                        pgasRef[3] = 0.3;
                        pgasRef[4] = 0.32;
                        pgasRef[5] = 0.35;
                    }
                    else {
                        star.planetCount = 12;
                    }
                        

                    star.planets = new PlanetData[star.planetCount];
                    int num8 = 0;
                    int num9 = 0;
                    int orbitAround = 0;
                    int num10 = 1;
                    for (int index = 0; index < star.planetCount; ++index) {
                        int info_seed = random2.Next();
                        int gen_seed = random2.Next();
                        double num11 = random2.NextDouble();
                        double num12 = random2.NextDouble();
                        bool gasGiant = false;
                        if (orbitAround == 0) {
                            ++num8;
                            if (index < star.planetCount - 1 && num11 < pgasRef[index]) {
                                gasGiant = true;
                                if (num10 < 3)
                                    num10 = 3;
                            }

                            for (; star.index != 0 || num10 != 3; ++num10) {
                                int num13 = star.planetCount - index;
                                int num14 = 9 - num10;
                                if (num14 > num13) {
                                    float a = (float) num13 / (float) num14;
                                    float num15 = num10 <= 3
                                        ? Mathf.Lerp(a, 1f, 0.15f) + 0.01f
                                        : Mathf.Lerp(a, 1f, 0.45f) + 0.01f;
                                    if (random2.NextDouble() < (double) num15)
                                        goto label_63;
                                }
                                else
                                    goto label_63;
                            }

                            gasGiant = true;
                        }
                        else {
                            ++num9;
                            gasGiant = false;
                        }

                        label_63:
                        star.planets[index] = PlanetGen.CreatePlanet(galaxy, star, gameDesc, index, orbitAround,
                            orbitAround != 0 ? num9 : num10, orbitAround != 0 ? num9 : num8, gasGiant, info_seed,
                            gen_seed);
                        ++num10;
                        if (gasGiant) {
                            orbitAround = num8;
                            num9 = 0;
                        }

                        if (num9 >= 1 && num12 < 0.8) {
                            orbitAround = 0;
                            num9 = 0;
                        }
                    }
                }

                int num16 = 0;
                int num17 = 0;
                int index1 = 0;
                for (int index2 = 0; index2 < star.planetCount; ++index2) {
                    if (star.planets[index2].type == EPlanetType.Gas) {
                        num16 = star.planets[index2].orbitIndex;
                        break;
                    }
                }

                for (int index2 = 0; index2 < star.planetCount; ++index2) {
                    if (star.planets[index2].orbitAround == 0)
                        num17 = star.planets[index2].orbitIndex;
                }

                if (num16 > 0) {
                    int num8 = num16 - 1;
                    bool flag = true;
                    for (int index2 = 0; index2 < star.planetCount; ++index2) {
                        if (star.planets[index2].orbitAround == 0 && star.planets[index2].orbitIndex == num16 - 1) {
                            flag = false;
                            break;
                        }
                    }

                    if (flag && num4 < 0.2 + (double) num8 * 0.2)
                        index1 = num8;
                }

                int index3 = num5 >= 0.2 ? (num5 >= 0.4 ? (num5 >= 0.8 ? 0 : num17 + 1) : num17 + 2) : num17 + 3;
                if (index3 != 0 && index3 < 5)
                    index3 = 5;
                star.asterBelt1OrbitIndex = (float) index1;
                star.asterBelt2OrbitIndex = (float) index3;
                if (index1 > 0) {
                    star.asterBelt1Radius = StarGen.orbitRadius[index1] * (float) num6 * star.orbitScaler;
                }
                if (index3 <= 0) {
                    return false;
                }
                star.asterBelt2Radius = StarGen.orbitRadius[index3] * (float) num7 * star.orbitScaler;
                return false;
            }
        }
    }
}