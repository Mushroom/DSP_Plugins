using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace DSP_Plugin {
    [BepInPlugin("touhma.dsp.plugins.galactic-scale-planets", "Galactic Scale - Planets Plug-In", "1.0.0.0")]
    public class MorePlanets : BaseUnityPlugin {
        internal void Awake() {
            Harmony.CreateAndPatchAll(typeof(PatchOnStarGen));
            Harmony.CreateAndPatchAll(typeof(PatchOnPlanetGen));
        }

        [HarmonyPatch(typeof(PlanetGen))]
        private class PatchOnPlanetGen {
            [HarmonyPrefix]
            [HarmonyPatch("CreatePlanet")]
            public static bool CreatePlanet(
                GalaxyData galaxy,
                StarData star,
                GameDesc gameDesc,
                int index,
                int orbitAround,
                int orbitIndex,
                int number,
                bool gasGiant,
                int info_seed,
                int gen_seed,
                ref PlanetData __result) {
                PlanetData planet = new PlanetData();
                System.Random random = new System.Random(info_seed);
                planet.index = index;
                planet.galaxy = star.galaxy;
                planet.star = star;
                planet.seed = gen_seed;
                planet.orbitAround = orbitAround;
                planet.orbitIndex = orbitIndex;
                planet.number = number;
                planet.id = star.id * 100 + index + 1;
                StarData[] stars = galaxy.stars;
                int num1 = 0;
                for (int index1 = 0; index1 < star.index; ++index1)
                    num1 += stars[index1].planetCount;
                int num2 = num1 + index;
                if (orbitAround > 0) {
                    for (int index1 = 0; index1 < star.planetCount; ++index1) {
                        if (orbitAround == star.planets[index1].number && star.planets[index1].orbitAround == 0) {
                            planet.orbitAroundPlanet = star.planets[index1];
                            if (orbitIndex > 1) {
                                planet.orbitAroundPlanet.singularity |= EPlanetSingularity.MultipleSatellites;
                                break;
                            }

                            break;
                        }
                    }

                    Assert.NotNull((object) planet.orbitAroundPlanet);
                }

                string str = star.planetCount > 20 ? (index + 1).ToString() : NameGen.roman[index + 1];
                planet.name = star.name + " " + str + "号星".Translate();
                double num3 = random.NextDouble();
                double num4 = random.NextDouble();
                double num5 = random.NextDouble();
                double num6 = random.NextDouble();
                double num7 = random.NextDouble();
                double num8 = random.NextDouble();
                double num9 = random.NextDouble();
                double num10 = random.NextDouble();
                double num11 = random.NextDouble();
                double num12 = random.NextDouble();
                double num13 = random.NextDouble();
                double num14 = random.NextDouble();
                double rand1 = random.NextDouble();
                double num15 = random.NextDouble();
                double rand2 = random.NextDouble();
                double rand3 = random.NextDouble();
                double rand4 = random.NextDouble();
                int theme_seed = random.Next();
                float a = Mathf.Pow(1.2f, (float) (num3 * (num4 - 0.5) * 0.5));
                float f1;
                if (orbitAround == 0) {
                    float b = StarGen.orbitRadius[orbitIndex] * star.orbitScaler;
                    float num16 = (float) (((double) a - 1.0) / (double) Mathf.Max(1f, b) + 1.0);
                    f1 = b * num16;
                }
                else
                    f1 = (float) (((1600.0 * (double) orbitIndex + 200.0) * (double) Mathf.Pow(star.orbitScaler, 0.3f) *
                        (double) Mathf.Lerp(a, 1f, 0.5f) + (double) planet.orbitAroundPlanet.realRadius) / 40000.0);

                planet.orbitRadius = f1;
                planet.orbitInclination = (float) (num5 * 16.0 - 8.0);
                if (orbitAround > 0)
                    planet.orbitInclination *= 2.2f;
                planet.orbitLongitude = (float) (num6 * 360.0);
                if (star.type >= EStarType.NeutronStar) {
                    if ((double) planet.orbitInclination > 0.0)
                        planet.orbitInclination += 3f;
                    else
                        planet.orbitInclination -= 3f;
                }

                planet.orbitalPeriod = planet.orbitAroundPlanet != null
                    ? Math.Sqrt(39.4784176043574 * (double) f1 * (double) f1 * (double) f1 / 1.08308421068537E-08)
                    : Math.Sqrt(39.4784176043574 * (double) f1 * (double) f1 * (double) f1 /
                                (1.35385519905204E-06 * (double) star.mass));
                planet.orbitPhase = (float) (num7 * 360.0);
                if (num15 < 0.0399999991059303) {
                    planet.obliquity = (float) (num8 * (num9 - 0.5) * 39.9);
                    if ((double) planet.obliquity < 0.0)
                        planet.obliquity -= 70f;
                    else
                        planet.obliquity += 70f;
                    planet.singularity |= EPlanetSingularity.LaySide;
                }
                else if (num15 < 0.100000001490116) {
                    planet.obliquity = (float) (num8 * (num9 - 0.5) * 80.0);
                    if ((double) planet.obliquity < 0.0)
                        planet.obliquity -= 30f;
                    else
                        planet.obliquity += 30f;
                }
                else
                    planet.obliquity = (float) (num8 * (num9 - 0.5) * 60.0);

                planet.rotationPeriod = (num10 * num11 * 1000.0 + 400.0) *
                                        (orbitAround != 0 ? 1.0 : (double) Mathf.Pow(f1, 0.25f)) *
                                        (!gasGiant ? 1.0 : 0.200000002980232);
                if (!gasGiant) {
                    if (star.type == EStarType.WhiteDwarf)
                        planet.rotationPeriod *= 0.5;
                    else if (star.type == EStarType.NeutronStar)
                        planet.rotationPeriod *= 0.200000002980232;
                    else if (star.type == EStarType.BlackHole)
                        planet.rotationPeriod *= 0.150000005960464;
                }

                planet.rotationPhase = (float) (num12 * 360.0);
                planet.sunDistance = orbitAround != 0 ? planet.orbitAroundPlanet.orbitRadius : planet.orbitRadius;
                planet.scale = 1f;
                double num17 = orbitAround != 0 ? planet.orbitAroundPlanet.orbitalPeriod : planet.orbitalPeriod;
                planet.rotationPeriod = 1.0 / (1.0 / num17 + 1.0 / planet.rotationPeriod);
                if (orbitAround == 0 && orbitIndex <= 4 && !gasGiant) {
                    if (num15 > 0.959999978542328) {
                        planet.obliquity *= 0.01f;
                        planet.rotationPeriod = planet.orbitalPeriod;
                        planet.singularity |= EPlanetSingularity.TidalLocked;
                    }
                    else if (num15 > 0.930000007152557) {
                        planet.obliquity *= 0.1f;
                        planet.rotationPeriod = planet.orbitalPeriod * 0.5;
                        planet.singularity |= EPlanetSingularity.TidalLocked2;
                    }
                    else if (num15 > 0.899999976158142) {
                        planet.obliquity *= 0.2f;
                        planet.rotationPeriod = planet.orbitalPeriod * 0.25;
                        planet.singularity |= EPlanetSingularity.TidalLocked4;
                    }
                }

                if (num15 > 0.85 && num15 <= 0.9) {
                    planet.rotationPeriod = -planet.rotationPeriod;
                    planet.singularity |= EPlanetSingularity.ClockwiseRotate;
                }

                planet.runtimeOrbitRotation = Quaternion.AngleAxis(planet.orbitLongitude, Vector3.up) *
                                              Quaternion.AngleAxis(planet.orbitInclination, Vector3.forward);
                if (planet.orbitAroundPlanet != null)
                    planet.runtimeOrbitRotation =
                        planet.orbitAroundPlanet.runtimeOrbitRotation * planet.runtimeOrbitRotation;
                planet.runtimeSystemRotation = planet.runtimeOrbitRotation *
                                               Quaternion.AngleAxis(planet.obliquity, Vector3.forward);
                float habitableRadius = star.habitableRadius;
                if (gasGiant) {
                    planet.type = EPlanetType.Gas;
                    planet.radius = 80f;
                    planet.scale = 10f;
                    planet.habitableBias = 100f;
                }
                else {
                    float num16 = Mathf.Ceil((float) star.galaxy.starCount * 0.29f);
                    if ((double) num16 < 11.0)
                        num16 = 11f;
                    float num18 = num16 - (float) star.galaxy.habitableCount;
                    float num19 = (float) (star.galaxy.starCount - star.index);
                    float sunDistance = planet.sunDistance;
                    float num20 = 1000f;
                    float f2 = 1000f;
                    if ((double) habitableRadius > 0.0 && (double) sunDistance > 0.0) {
                        f2 = sunDistance / habitableRadius;
                        num20 = Mathf.Abs(Mathf.Log(f2));
                    }

                    float num21 = Mathf.Clamp(Mathf.Sqrt(habitableRadius), 1f, 2f) - 0.04f;
                    float num22 = Mathf.Clamp(Mathf.Lerp(num18 / num19, 0.35f, 0.5f), 0.08f, 0.8f);
                    planet.habitableBias = num20 * num21;
                    planet.temperatureBias = (float) (1.20000004768372 / ((double) f2 + 0.200000002980232) - 1.0);
                    float num23 = Mathf.Pow(Mathf.Clamp01(planet.habitableBias / num22), num22 * 10f);
                    if (num13 > (double) num23 && star.index > 0 ||
                        planet.orbitAround > 0 && planet.orbitIndex == 1 && star.index == 0) {
                        planet.type = EPlanetType.Ocean;
                        ++star.galaxy.habitableCount;
                    }
                    else if ((double) f2 < 0.833333015441895) {
                        float num24 = Mathf.Max(0.15f, (float) ((double) f2 * 2.5 - 0.850000023841858));
                        planet.type = num14 >= (double) num24 ? EPlanetType.Vocano : EPlanetType.Desert;
                    }
                    else if ((double) f2 < 1.20000004768372) {
                        planet.type = EPlanetType.Desert;
                    }
                    else {
                        float num24 = (float) (0.899999976158142 / (double) f2 - 0.100000001490116);
                        planet.type = num14 >= (double) num24 ? EPlanetType.Ice : EPlanetType.Desert;
                    }

                    planet.radius = 200f;
                }

                if (planet.type != EPlanetType.Gas && planet.type != EPlanetType.None) {
                    planet.precision = 200;
                    planet.segment = 5;
                }
                else {
                    planet.precision = 64;
                    planet.segment = 2;
                }

                planet.luminosity = Mathf.Pow(planet.star.lightBalanceRadius / (planet.sunDistance + 0.01f), 0.6f);
                if ((double) planet.luminosity > 1.0) {
                    planet.luminosity = Mathf.Log(planet.luminosity) + 1f;
                    planet.luminosity = Mathf.Log(planet.luminosity) + 1f;
                    planet.luminosity = Mathf.Log(planet.luminosity) + 1f;
                }

                planet.luminosity = Mathf.Round(planet.luminosity * 100f) / 100f;
                PlanetGen.SetPlanetTheme(planet, star, gameDesc, 0, 0, rand1, rand2, rand3, rand4, theme_seed);
                star.galaxy.astroPoses[planet.id].uRadius = planet.realRadius;

                __result = planet;
                return false;
            }
        }

        
        [HarmonyPatch(typeof(StarGen))]
        private class PatchOnStarGen {
            [HarmonyPrefix]
            [HarmonyPatch("CreateStarPlanets")]
            public static bool CreateStarPlanets(GalaxyData galaxy, StarData star, GameDesc gameDesc) {
                double[] pgasRef = new double[10];

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