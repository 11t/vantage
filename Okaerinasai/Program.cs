namespace Okaerinasai
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string SongFolderPath = @"C:\Program Files (x86)\osu!\Songs";
            const string MapFolderName = @"179323 Sakamoto Maaya - Okaerinasai (tomatomerde Remix)";
            const string OsbFileName = @"Sakamoto Maaya - Okaerinasai (tomatomerde Remix) (Azer).osb";
            string mapFolderPath = System.IO.Path.Combine(SongFolderPath, MapFolderName);

            var storyboardGenerator = new Okaerinasai();
            storyboardGenerator.WriteStoryboard(mapFolderPath, OsbFileName);
        }
    }
}
