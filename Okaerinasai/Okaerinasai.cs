namespace Okaerinasai
{
    using System;
    using System.Diagnostics;
    using System.Net.Mime;

    using SharpDX;

    using Vantage;
    using Vantage.Animation2D.OsbTypes;
    using Vantage.Animation3D.Animation.EasingCurves;
    using Vantage.Animation3D.Layers;
    using Vantage.Animation3D.Layers.Text;
    using Vantage.Animation3D.Scenes;

    public class Okaerinasai : StoryboardGenerator
    {
        private const int TextLetterSpacingEN = -20;
        private const int TextLetterSpacingJP = 14;

        #region Lyrics

        private const string Lyrics = @"夏の残り雨に 駅まで走ってった君のうしろ姿
半袖の制服 慌てて追いかけた まつげに水玉はじけて

世界中の優しいもの 美しい色を知ってた
でも世界は知らなかった あの頃

おかえりなさい 思い出に
泣きたいとき 会いたいとき ここにいるよ
おかえりなさい 私たちが
夢見たもの 愛したもの 今も君を守ってるよ

髪をとかしながら 映画の中で見たあの子の真似をした
冷たい水で顔を洗うと悲しみは 全部流れていった

空の上に描いてたすべては いま目の前に降りてきて
重さも形も確かに感じる

おかえりなさい 思い出に
振り向くのも 変わることも 弱さじゃない
おかえりなさい わかってるよ
何も言わず 何も訊かず 君をそっと抱きしめよう

おかえりなさい 思い出に
泣きたいとき 会いたいとき そばにいるよ
おかえりなさい 気づいていて
生きることは忘れること 今がいつも一番輝いてる";

        #endregion

        private const float BPM = 176.0f;
        private const float BeatDuration = 60000.0f / BPM;

        private Font textFontJP;
        private Font textFontEN;

        protected override void Generate()
        {
            this.InitializeStoryboard();
            this.InitializeFonts();
            this.DisableBackground();
            // this.MakeHexagonScene();
            this.MakeRaindropsScene();
            this.MakeKiai1Scene();
            this.MakeBlankBackground();
            this.MakeVignette();
        }

        private void InitializeStoryboard()
        {
            this.Storyboard = new Storyboard(1366, 768);
        }

        private void InitializeFonts()
        {
            const string Alphanumeric = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,./~!@#$%^&*-=()[]<>";
            this.textFontJP = new Font(StoryboardSettings.Instance.Directory, Lyrics, "sb/lyrics/jp/", ".png");
            this.textFontEN = new Font(StoryboardSettings.Instance.Directory, Alphanumeric, "sb/lyrics/en/", ".png");
        }

        private void DisableBackground()
        {
            var bg = this.Storyboard.NewSprite2D("bg.jpg", "Background", "Centre");
            bg.Fade(0, 0, 0, 0, 0);
        }

        private void MakeRaindropsScene()
        {
            var rng = new Random();
            var scene = this.Storyboard.NewScene3D(BPM, 585, 68766);
            scene.ConversionSettings.MoveThreshold = 0;
            var mc = scene.MainCamera;
            var rl = scene.RootLayer;

            mc.SetPosition(0, 0, 0, 400);
            mc.SetTarget(0, 0, 0, 0);
            var marqueeLayer = new MarqueeLayer("sb/rounded-rect-100px.png", 110, 47, 8, new OsbColor(0.9, 0.9, 0.9))
                                   {
                                       Parent = rl,
                                       Additive = true
                                   };
            marqueeLayer.SetScale(0, 0.08f, 0.08f, 1);
            marqueeLayer.SetOpacity(585, 0);
            marqueeLayer.SetOpacity(585 + BeatDuration, 1);
            marqueeLayer.Display("ABCDABCD", 585, new OsbColor(0.2, 0.2, 0.3), BasicEasingCurve.Step);
            marqueeLayer.UpdateToTime(585);

            foreach (Sprite3D sprite in marqueeLayer.Children)
            {
                var z = rng.Next(-300, 300);
                sprite.SetPosition(22062, sprite.LocalPosition, CubicBezierEasingCurve.EaseIn);
                sprite.SetPosition(22403, sprite.LocalPosition.X, sprite.LocalPosition.Y, z);
            }

            var beatPattern = new BeatPattern(8);
            beatPattern.RepeatAddMeasure(new BeatMeasure(0, 6, 1, 1), 8);

            Vector3 previousPosition = new Vector3(0, 0, 400);
            Vector3 previousTarget = Vector3.Zero;
            mc.SetPosition(22403, 0, 0, 400);
            rl.UpdateToTime(22403);
            for (int i = 0; i < 8; i++)
            {
                float time1 = 22403 + (i * 8 * BeatDuration);
                float time2 = time1 + (6 * BeatDuration);
                float time3 = time2 + (1 * BeatDuration);
                float time4 = time3 + (1 * BeatDuration);

                Sprite3D targetSprite = marqueeLayer.Children[rng.Next(0, marqueeLayer.Children.Count)] as Sprite3D;
                Vector3 targetSpriteLocation = targetSprite.WorldPosition;

                Debug.WriteLine(targetSpriteLocation.ToString());
                Vector3 target = rng.NextSpherePoint(targetSpriteLocation, 2, 10);

                Vector3 position2 = rng.NextSpherePoint(previousPosition, 20, 80);
                //Vector3 position3 = rng.NextSpherePoint(target, 20, 100);
                Vector3 forwardToTarget = target - position2;
                Vector3 forwardToTargetNormalized = Vector3.Normalize(forwardToTarget);
                Vector3 position4 = position2 + forwardToTarget - forwardToTargetNormalized * 80;
                //Vector3 position4 = rng.NextSpherePoint(target, 20, 100);

                var textLayer = new TextLayer(textFontEN, TextLetterSpacingEN, 30, TextAlignment.Center);
                textLayer.Text = "Azer";
                textLayer.Additive = true;
                textLayer.SetOpacity(time3, 0);
                textLayer.SetOpacity(time4, 1);
                textLayer.SetPosition(0, targetSpriteLocation);
                textLayer.SetScale(0, 0.08f);
                textLayer.Parent = rl;
                var textRotation = Math3D.RotationFromTo(Vector3.ForwardLH, -forwardToTargetNormalized);
                textLayer.SetRotation(0, textRotation);

                mc.SetPosition(time3, position2, CubicBezierEasingCurve.EaseIn);
                //mc.SetPosition(time3, position3, CubicBezierEasingCurve.EaseIn);
                mc.SetPosition(time4, position4);
                mc.SetTarget(time2, previousTarget);
                mc.SetTarget(time3, target);

                previousPosition = position4;
                previousTarget = target;
            }
        }

        private void MakeHexagonScene()
        {
            var hexagonBeatPattern = new BeatPattern();
            var hexagonBeatMeasure = new BeatMeasure(0, 1, 1.5, 0.5, 1);
            hexagonBeatPattern.RepeatAddMeasure(hexagonBeatMeasure, 34);
            hexagonBeatPattern.Measures[7] = new BeatMeasure(0, 1, 1, 0.5, 0.5, 0.5, 0.5);
            hexagonBeatPattern.Measures[15] = new BeatMeasure(0, 0.5, 0.5, 1.5, 0.5, 1);
            hexagonBeatPattern.Measures[33] = new BeatMeasure(0, 0.5, 0.5, 0.5, 0.5, 1, 0.5, 0.5);

            var hexSceneGenerator = new OkaerinasaiHexagonSceneGenerator
            {
                HexagonSpriteName = @"sb/hexagon240.png",
                HexagonTextFont = this.textFontEN,
                HexagonAppearScale = 0.5f,
                HexagonBeginScale = 2
            };
            Scene3D scene = hexSceneGenerator.MakeScene(this.Storyboard, BPM, 22403, hexagonBeatPattern);
            Layer particleLayer = scene.RootLayer.NewLayer();
            particleLayer.Additive = true;
            Random rng = new Random();
            float invSqrt3 = (float)(1.0 / Math.Sqrt(3));
            for (int i = 0; i < 100; i++)
            {
                Sprite3D particleSprite = particleLayer.NewSprite("sb/circle4px.png");
                particleSprite.SetOpacity(0, 1);
                float z = rng.Next(5000, 500000);
                particleSprite.SetScale(0, z / 2000.0f);
                int range = (int)(invSqrt3 * (z + 10000));
                float x = rng.Next(-range, range);
                float y = rng.Next(-range, range);
                particleSprite.SetPosition(0, x, y, -z);
            }
        }

        private void MakeKiai1Scene()
        {
            Scene3D scene = Storyboard.NewScene3D(BPM, 87857, 136948, 8);
            scene.ConversionSettings.MoveThreshold = 12;
            Layer rl = scene.RootLayer;
            Camera mc = scene.MainCamera;

            var cbg = rl.NewSprite("sb/cloudbg0.jpg");
            var sbg = rl.NewSprite("sb/earth2.jpg");

            var kiaiLyricLayer = rl.NewLayer();
            string[] kiaiLyrics =
            {
                @"おかえりなさい",
                @"思い出に",
                @"泣きたいとき",
                @"会いたいとき",
                @"ここにいるよ",
                @"おかえりなさい", 
                @"私たちが",
                @"夢見たもの", 
                @"愛したもの", 
                @"今も君を守ってるよ"
            };

            int[] kiaiLyricTimes = 
            {
                88880,
                94675,
                99789,
                102516,
                105244,
                110698,
                116153,
                121607,
                124335,
                127062
            };

            float textScale = 0.3f;
            for (int i = 5; i >= 1; i--)
            {
                var t = new AdditiveEnhancedTextLayer(textFontJP, 14, 30, TextAlignment.Center);
                t.Parent = kiaiLyricLayer;
                t.Text = kiaiLyrics[i];
                float inTime = kiaiLyricTimes[i] - BeatDuration;
                t.SetOpacity(0, 0);
                t.SetOpacity(inTime, 0);
                t.SetOpacity(kiaiLyricTimes[i], 0.5f);
                t.SetPosition(0, 0, 0, -1000 - 400 * i);
                t.SetPosition(inTime, 0, 0, -1000 - 400 * i);
                t.SetPosition(kiaiLyricTimes[i], 0, 100, -1000 - 400 * i);
                t.SetScale(inTime, textScale, textScale, 1);
            }

            var okaerinasaiText = new Layer() { Parent = kiaiLyricLayer };
            var okaerinasaiJP = new AdditiveEnhancedTextLayer(textFontJP, TextLetterSpacingJP) { Parent = okaerinasaiText };
            var okaerinasaiEN = new AdditiveEnhancedTextLayer(textFontEN, TextLetterSpacingEN) { Parent = okaerinasaiText };
            var okDE = new AdditiveEnhancedTextLayer(textFontEN, TextLetterSpacingEN) { Parent = okaerinasaiText };
            var okFR = new AdditiveEnhancedTextLayer(textFontEN, TextLetterSpacingEN) { Parent = okaerinasaiText };
            var okES = new AdditiveEnhancedTextLayer(textFontEN, TextLetterSpacingEN) { Parent = okaerinasaiText };
            var okIL = new AdditiveEnhancedTextLayer(textFontEN, TextLetterSpacingEN) { Parent = okaerinasaiText };
            var okSW = new AdditiveEnhancedTextLayer(textFontEN, TextLetterSpacingEN) { Parent = okaerinasaiText };
            var okIN = new AdditiveEnhancedTextLayer(textFontEN, TextLetterSpacingEN) { Parent = okaerinasaiText };

            float okaerinasaiInTime = kiaiLyricTimes[0] - BeatDuration;
            okaerinasaiText.SetOpacity(0, 0);
            okaerinasaiText.SetOpacity(okaerinasaiInTime, 0);
            okaerinasaiText.SetOpacity(kiaiLyricTimes[0], 0.5f);
            okaerinasaiText.SetPosition(0, 0, 0, -1000);
            okaerinasaiText.SetPosition(okaerinasaiInTime, 0, 0, -1000);
            okaerinasaiText.SetPosition(kiaiLyricTimes[0], 0, 100, -1000);
            okaerinasaiText.SetScale(scene.StartTime, textScale, textScale, 1);

            okaerinasaiJP.SetOpacity(88539, 1);
            okaerinasaiJP.SetOpacity(89050, 1, CubicBezierEasingCurve.EaseIn);
            okaerinasaiJP.SetOpacity(89221, 0);
            okaerinasaiJP.SetOpacity(90414, 0, BasicEasingCurve.Step);
            okaerinasaiJP.SetOpacity(90585, 1);

            okDE.SetOpacity(0, 0);
            okDE.SetOpacity(89050, 0, CubicBezierEasingCurve.EaseIn);
            okDE.SetOpacity(89221, 1, CubicBezierEasingCurve.EaseIn);
            okDE.SetOpacity(89391, 0);

            okFR.SetOpacity(0, 0);
            okFR.SetOpacity(89221, 0, CubicBezierEasingCurve.EaseIn);
            okFR.SetOpacity(89391, 1, CubicBezierEasingCurve.EaseIn);
            okFR.SetOpacity(89562, 0);

            okES.SetOpacity(0, 0);
            okES.SetOpacity(89391, 0, CubicBezierEasingCurve.EaseIn);
            okES.SetOpacity(89562, 1, CubicBezierEasingCurve.EaseIn);
            okES.SetOpacity(89732, 0);

            okIL.SetOpacity(0, 0);
            okIL.SetOpacity(89562, 0, CubicBezierEasingCurve.EaseIn);
            okIL.SetOpacity(89732, 1, CubicBezierEasingCurve.EaseIn);
            okIL.SetOpacity(89903, 0);

            okSW.SetOpacity(0, 0);
            okSW.SetOpacity(89732, 0, CubicBezierEasingCurve.EaseIn);
            okSW.SetOpacity(89903, 1, CubicBezierEasingCurve.EaseIn);
            okSW.SetOpacity(90073, 0);

            okIN.SetOpacity(0, 0);
            okIN.SetOpacity(89903, 0, CubicBezierEasingCurve.EaseIn);
            okIN.SetOpacity(90073, 1, CubicBezierEasingCurve.EaseIn);
            okIN.SetOpacity(90244, 0);

            okaerinasaiEN.SetOpacity(0, 0);
            okaerinasaiEN.SetOpacity(90073, 0, CubicBezierEasingCurve.EaseIn);
            okaerinasaiEN.SetOpacity(90244, 1);
            okaerinasaiEN.SetOpacity(90414, 1, BasicEasingCurve.Step);
            okaerinasaiEN.SetOpacity(90585, 1);
            okaerinasaiEN.SetOpacity(90755, 0);

            okaerinasaiJP.Text = kiaiLyrics[0];
            okaerinasaiEN.Text = "welcome home";
            okDE.Text = "willkommen";
            okFR.Text = "bienvenue";
            okES.Text = "bienvenido";
            okIL.Text = "bienvenuti";
            okSW.Text = "valkommen";
            okIN.Text = "selamat";

            Random rng = new Random();
            foreach (var textLayer in okaerinasaiEN.Children)
            {
                foreach (Sprite3D letter in textLayer.Children)
                {
                    float z = -1000 - 500;
                    float x = rng.Next(-300, 300);
                    float y = rng.Next(-300, 300);
                    letter.UpdateToTime(90585);
                    letter.SetPosition(90585, letter.LocalPosition);
                    letter.SetPosition(90755, x, y, z);
                }
            }

            var c2 = rl.NewSprite("sb/cloud0.png");
            var c1 = rl.NewSprite("sb/cloud0.png");
            var c0 = rl.NewSprite("sb/cloud0.png");

            c2.Additive = true;
            c1.Additive = true;
            c0.Additive = true;

            c2.SetOpacity(87857, 0, BasicEasingCurve.Step);
            c1.SetOpacity(87857, 0, BasicEasingCurve.Step);
            c0.SetOpacity(87857, 0, BasicEasingCurve.Step);
            cbg.SetOpacity(87857, 0, BasicEasingCurve.Step);

            c2.SetOpacity(90585, 0.2f, BasicEasingCurve.Step);
            c1.SetOpacity(90585, 0.2f, BasicEasingCurve.Step);
            c0.SetOpacity(90585, 0.2f, BasicEasingCurve.Step);
            cbg.SetOpacity(90585, 1);

            c2.SetPosition(87857, 100, -300, -3000);
            c1.SetPosition(87857, -400, -200, -2000);
            c0.SetPosition(87857, -200, -100, -1000);

            c2.SetScale(87857, 1, 1, 1);
            c1.SetScale(87857, 2, 2, 1);
            c0.SetScale(87857, 1.2f, 1.2f, 1);

            cbg.SetPosition(87857, 0, 30000, -80000);
            cbg.SetScale(87857, 100, 100, 1);
            cbg.SetOpacity(111891, 1, BasicEasingCurve.Linear);
            cbg.SetOpacity(112147, 0);

            sbg.SetPosition(0, 0, 100000, 0);
            sbg.SetAngles(0, 90, 0, 0);
            sbg.SetScale(0, 70, 70, 1);
            sbg.SetOpacity(0, 0);
            sbg.SetOpacity(112317, 0);
            sbg.SetOpacity(112488, 1);
            sbg.SetOpacity(scene.EndTime - 8 * BeatDuration, 1);
            sbg.SetOpacity(scene.EndTime, 0);

            mc.SetPosition(scene.StartTime, 0, 0, 200);
            mc.SetTarget(scene.StartTime, -3000, 10, -10000);

            mc.SetTarget(110357, 3000, 10, -10000, CubicBezierEasingCurve.EaseIn);
            mc.SetTarget(110698, 0, 100, -4600);

            mc.SetTarget(111721, 0, 100, -4600, new CubicBezierEasingCurve(.79f, .02f, .99f, .27f));

            mc.SetPosition(112403, 0, 0, -2690);
            mc.SetPosition(scene.EndTime - 8 * BeatDuration, 0, 9000, 0);
        }

        private void MakeColorBackground()
        {
            var bg = this.Storyboard.NewSprite2D("sb/bg-pink.png", "Background", "Centre");
            bg.Fade(0, 22403, 22403, 0, 1);
            bg.Fade(0, 68766, 68766, 1, 0);
            bg.Scale(0, 0, 0, 1.25, 1.25);
            bg.Color(0, 0, 0, OsbColor.DarkSlateBlue, OsbColor.DarkSlateBlue);
        }

        private void MakeBlankBackground()
        {
            var bg = this.Storyboard.NewSprite2D("sb/blank.png", "Background", "Centre");
            bg.Fade(0, 585, 585, 0, 0.4);
            bg.Fade(0, 68766, 68766, 0.4, 0);
            bg.Scale(0, 0, 0, 0.625, 0.625);
        }

        private void MakeVignette()
        {
            var vignette = this.Storyboard.NewSprite2D("sb/vignette.png", "Foreground", "Centre");
            vignette.Fade(0, 585, 2490, 0, 1);
            vignette.Fade(0, 352403, 368184, 1, 0);
            vignette.Scale(0, 0, 0, 1.25, 1.25);
        }
    }
}
