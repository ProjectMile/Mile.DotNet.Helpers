using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Mile.DotNet.Helpers
{
    internal static class ImageAssets
    {
        public static IReadOnlyList<int> AssetSizes { get; } =
            Array.AsReadOnly(new int[]
            {
                16, 20, 24, 30,
                32, 36, 40, 48,
                60, 64, 72, 80,
                90, 96, 108, 120,
                128, 144, 160, 192,
                216, 256, 288, 320,
                384, 512, 768, 1024
            });

        public static IReadOnlyList<int> IconSizes { get; } =
            Array.AsReadOnly(new int[]
            {
                16, 20, 24, 32,
                40, 48, 64, 256
            });

        private struct AssetType
        {
            public string Name;
            public int Width;
            public int Height;
            public int IconSize;

            public AssetType(string Name, int Width, int Height, int IconSize)
            {
                this.Name = Name;
                this.Width = Width;
                this.Height = Height;
                this.IconSize = IconSize;

                if (this.IconSize == 54 || this.IconSize == 46)
                {
                    this.IconSize = 48;
                }
            }
        }

        private static int CeilToEven(int i)
        {
            return (i % 2 == 0) ? (i) : (i + 1);
        }

        private static void GenerateImageAsset(
            MagickImage Source,
            AssetType Asset,
            string OutputAssetsPath,
            string NameSuffix)
        {
            using (MagickImage SourceImage = new MagickImage(Source))
            using (MagickImage TargetImage = new MagickImage(
                MagickColors.Transparent,
                Convert.ToUInt32(Asset.Width),
                Convert.ToUInt32(Asset.Height)))
            {
                TargetImage.Composite(
                    SourceImage,
                    Gravity.Center,
                    CompositeOperator.Over);

                TargetImage.Write(Path.Combine(
                    OutputAssetsPath,
                    Asset.Name + NameSuffix + ".png"));
            }
        }

        public static void GeneratePackageApplicationImageAssets(
            IReadOnlyDictionary<int, MagickImage> StandardSources,
            IReadOnlyDictionary<int, MagickImage> ContrastBlackSources,
            IReadOnlyDictionary<int, MagickImage> ContrastWhiteSources,
            string outputAssetsPath)
        {
            List<AssetType> allAssetSizes = new List<AssetType>();
            {
                AssetType[] assetTypes = new AssetType[]
                {
                    new AssetType("LargeTile", 310, 310, 96),
                    //new AssetType("LockScreenLogo", 24, 24, 24),
                    new AssetType("SmallTile", 71, 71, 36),
                    //new AssetType("SplashScreen", 620, 300, 96),
                    new AssetType("Square44x44Logo", 44, 44, 32),
                    new AssetType("Square150x150Logo", 150, 150, 48),
                    new AssetType("StoreLogo", 50, 50, 36),
                    new AssetType("Wide310x150Logo", 310, 150, 48)
                };

                double[] scales = new double[]
                {
                    1.0, 1.25, 1.5, 2.0, 4.0
                };

                foreach (AssetType assetType in assetTypes)
                {
                    foreach (double scale in scales)
                    {
                        allAssetSizes.Add(new AssetType(
                            string.Format(
                                "{0}.scale-{1}",
                                assetType.Name,
                                Convert.ToInt32(scale * 100)),
                            Convert.ToInt32(Math.Ceiling(assetType.Width * scale)),
                            Convert.ToInt32(Math.Ceiling(assetType.Height * scale)),
                            CeilToEven(
                                Convert.ToInt32(assetType.IconSize * scale))));
                    }
                }

                foreach (int altForm in AssetSizes)
                {
                    string baseName = string.Format(
                        "Square44x44Logo.targetsize-{0}",
                        altForm);

                    allAssetSizes.Add(new AssetType(
                           baseName,
                           altForm,
                           altForm,
                           altForm));

                    allAssetSizes.Add(new AssetType(
                           baseName + "_altform-unplated",
                           altForm,
                           altForm,
                           altForm));

                    allAssetSizes.Add(new AssetType(
                           baseName + "_altform-lightunplated",
                           altForm,
                           altForm,
                           altForm));
                }
            }

            List<Task> tasks = new List<Task>();

            foreach (AssetType item in allAssetSizes)
            {
                tasks.Add(Task.Run(() => GenerateImageAsset(
                    StandardSources[item.IconSize],
                    item,
                    outputAssetsPath,
                    string.Empty)));

                tasks.Add(Task.Run(() => GenerateImageAsset(
                    ContrastBlackSources[item.IconSize],
                    item,
                    outputAssetsPath,
                    "_contrast-black")));

                tasks.Add(Task.Run(() => GenerateImageAsset(
                    ContrastWhiteSources[item.IconSize],
                    item,
                    outputAssetsPath,
                    "_contrast-white")));
            }

            Task.WaitAll(tasks.ToArray());
        }

        public static void GeneratePackageFileAssociationImageAssets(
            IReadOnlyDictionary<int, MagickImage> ImageSources,
            string outputAssetsPath,
            string outputAssetsPrefix)
        {
            List<Task> tasks = new List<Task>();

            foreach (int AssetSize in AssetSizes)
            {
                tasks.Add(Task.Run(() =>
                {
                    using (MagickImage sourceImage =
                        new MagickImage(ImageSources[AssetSize]))
                    using (MagickImage targetImage = new MagickImage(
                        MagickColors.Transparent,
                        Convert.ToUInt32(AssetSize),
                        Convert.ToUInt32(AssetSize)))
                    {
                        targetImage.Composite(
                            sourceImage,
                            Gravity.Center,
                            CompositeOperator.Over);
                        targetImage.Write(Path.Combine(
                            outputAssetsPath,
                            string.Format(
                                "{0}.targetsize-{1}.png",
                                outputAssetsPrefix,
                                AssetSize)));
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        public static void GenerateIconFile(
            IReadOnlyDictionary<int, MagickImage> IconSources,
            string OutputPath)
        {
            using (MagickImageCollection IconCollection =
                new MagickImageCollection())
            {
                foreach (int IconSize in IconSizes)
                {
                    IconCollection.Add(new MagickImage(IconSources[IconSize]));
                }

                IconCollection.Write(OutputPath);
            }
        }
    }
}
