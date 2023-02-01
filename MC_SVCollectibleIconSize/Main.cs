
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace MC_SVCollectibleIconSize
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string pluginGuid = "mc.starvalor.collectibleiconsize";
        public const string pluginName = "SV Collectible Icon Size";
        public const string pluginVersion = "1.0.0";

        private static ConfigEntry<KeyCodeSubset> configInputInc;
        private static ConfigEntry<KeyCodeSubset> configInputDec;
        private static ConfigEntry<KeyCodeSubset> configInputReset;
        private static ConfigEntry<float> configStepSize;
        private static ConfigEntry<int> configSteps;

        private static bool inc = false;
        private static bool dec = false;
        private static bool reset = false;

        public void Awake()
        {
            LoadConfig();
            Harmony.CreateAndPatchAll(typeof(Main));
        }

        private void LoadConfig()
        {
            configInputInc = Config.Bind("Input",
                "Increment",
                KeyCodeSubset.RightBracket,
                "Increase icon size key");
            configInputDec = Config.Bind("Input",
                "Decrement",
                KeyCodeSubset.LeftBracket,
                "Decrease icon size key");
            configInputReset = Config.Bind("Input",
                "Reset",
                KeyCodeSubset.Backspace,
                "Reset icon size key");
            configStepSize = Config.Bind("Config",
                "Step Size",
                1.25f,
                "Increase/decrease step size (multiplier)");
            configSteps = Config.Bind("Memory",
                "Steps",
                0,
                "So mod remembers your last scale.  0 = no scaling.  Step size is multiplied by this when a Collectible is created in game.");
        }

        public void Update()
        {
            if (GameManager.instance != null && GameManager.instance.inGame)
            {
                // Scaling inputs
                if (reset)
                    configSteps.Value = 0;
                if (inc = Input.GetKeyDown((KeyCode)configInputInc.Value))
                    configSteps.Value++;
                if (dec = Input.GetKeyDown((KeyCode)configInputDec.Value))
                    configSteps.Value--;

                reset = Input.GetKeyDown((KeyCode)configInputReset.Value);
            }
        }

        public void OnDestroy()
        {
            Config.Save();
        }

        [HarmonyPatch(typeof(Collectible), "Update")]
        [HarmonyPostfix]
        private static void Collectible_Update_Post(ref GameObject ___minimapIcon)
        {
            if (___minimapIcon != null)
            {
                if (inc)
                {
                    ___minimapIcon.transform.localScale *= configStepSize.Value;
                }
                if (dec)
                {
                    ___minimapIcon.transform.localScale /= configStepSize.Value;
                }

                if (reset && configSteps.Value > 0)
                    ___minimapIcon.transform.localScale /= Mathf.Pow(configStepSize.Value,configSteps.Value);
                else if (reset && configSteps.Value < 0)
                    ___minimapIcon.transform.localScale *= Mathf.Pow(configStepSize.Value, Mathf.Abs(configSteps.Value));
            }
        }

        [HarmonyPatch(typeof(Collectible), "Start")]
        [HarmonyPostfix]
        private static void Collectible_Start_Post(ref GameObject ___minimapIcon)
        {
            if (configSteps.Value < 0)
                ___minimapIcon.transform.localScale /= Mathf.Pow(configStepSize.Value, Mathf.Abs(configSteps.Value));
            else if (configSteps.Value > 0)
                ___minimapIcon.transform.localScale *= Mathf.Pow(configStepSize.Value, configSteps.Value);
        }
    }

    public enum KeyCodeSubset
    {
        None = 0,
        Backspace = 8,
        Delete = 0x7F,
        Tab = 9,
        Return = 13,
        Pause = 19,
        Escape = 27,
        Space = 0x20,
        Keypad0 = 0x100,
        Keypad1 = 257,
        Keypad2 = 258,
        Keypad3 = 259,
        Keypad4 = 260,
        Keypad5 = 261,
        Keypad6 = 262,
        Keypad7 = 263,
        Keypad8 = 264,
        Keypad9 = 265,
        KeypadPeriod = 266,
        KeypadDivide = 267,
        KeypadMultiply = 268,
        KeypadMinus = 269,
        KeypadPlus = 270,
        KeypadEnter = 271,
        KeypadEquals = 272,
        UpArrow = 273,
        DownArrow = 274,
        RightArrow = 275,
        LeftArrow = 276,
        Insert = 277,
        Home = 278,
        End = 279,
        PageUp = 280,
        PageDown = 281,
        F1 = 282,
        F2 = 283,
        F3 = 284,
        F4 = 285,
        F5 = 286,
        F6 = 287,
        F7 = 288,
        F8 = 289,
        F9 = 290,
        F10 = 291,
        F11 = 292,
        F12 = 293,
        F13 = 294,
        F14 = 295,
        F15 = 296,
        Alpha0 = 48,
        Alpha1 = 49,
        Alpha2 = 50,
        Alpha3 = 51,
        Alpha4 = 52,
        Alpha5 = 53,
        Alpha6 = 54,
        Alpha7 = 55,
        Alpha8 = 56,
        Alpha9 = 57,
        Exclaim = 33,
        DoubleQuote = 34,
        Hash = 35,
        Dollar = 36,
        Percent = 37,
        Ampersand = 38,
        Quote = 39,
        LeftParen = 40,
        RightParen = 41,
        Asterisk = 42,
        Plus = 43,
        Comma = 44,
        Minus = 45,
        Period = 46,
        Slash = 47,
        Colon = 58,
        Semicolon = 59,
        Less = 60,
        Equals = 61,
        Greater = 62,
        Question = 0x3F,
        At = 0x40,
        LeftBracket = 91,
        Backslash = 92,
        RightBracket = 93,
        Caret = 94,
        Underscore = 95,
        BackQuote = 96,
        A = 97,
        B = 98,
        C = 99,
        D = 100,
        E = 101,
        F = 102,
        G = 103,
        H = 104,
        I = 105,
        J = 106,
        K = 107,
        L = 108,
        M = 109,
        N = 110,
        O = 111,
        P = 112,
        Q = 113,
        R = 114,
        S = 115,
        T = 116,
        U = 117,
        V = 118,
        W = 119,
        X = 120,
        Y = 121,
        Z = 122,
        LeftCurlyBracket = 123,
        Pipe = 124,
        RightCurlyBracket = 125,
        Tilde = 126,
        RightShift = 303,
        LeftShift = 304,
        RightControl = 305,
        LeftControl = 306,
        RightAlt = 307,
        LeftAlt = 308,
        LeftCommand = 310,
        LeftApple = 310,
        LeftWindows = 311,
        RightCommand = 309,
        RightApple = 309,
        RightWindows = 312,
        AltGr = 313,
        Print = 316,
        Break = 318,
    }
}
