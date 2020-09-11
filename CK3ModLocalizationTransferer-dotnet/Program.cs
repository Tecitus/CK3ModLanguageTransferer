using System;
using System.IO;
using System.Text;

namespace CK3ModLocalizationTransfer
{
    class Program
    { 
        static void Main(string[] args)
        {
            Console.WriteLine("Write mod's directory.(Absolute path)");
            var directory = Console.ReadLine();
            Console.WriteLine("Write target language.(default: english)");
            var target = Console.ReadLine().Trim();
            if(target.Length == 0)
            {
                target = "english";
            }
            Console.WriteLine("Write desired language.(default: korean)");
            var desired = Console.ReadLine().Trim();
            if(desired.Length == 0)
            {
                desired = "korean";
            }

            CopyDirectory(Path.Combine(directory, "localization", target), Path.Combine(directory, "localization", desired));
            ChangeFile(Path.Combine(directory, "localization", desired), target, desired);
        }

        public static T GetLast<T>(T[] arr)
        {
            return arr[arr.Length - 1];
        }

        public static void CopyDirectory(string from, string to)
        {
            Directory.CreateDirectory(to);
            foreach(var f in Directory.GetFiles(from))
            {
                File.Copy(f, Path.Combine(to, Path.GetFileName(f)),true);
            }
            foreach(var d in Directory.GetDirectories(from))
            {
                CopyDirectory(d, Path.Combine(to, Path.GetFileName(d)));
            }
        }

        public static void ChangeFile(string dir, string target, string desired)
        {
            var oldlang = "l_" + target;
            var newlang = "l_" + desired;
            var oldname = "l_" + target + ".yml";
            var newname = "l_" + desired + ".yml";
            foreach (var f in Directory.GetFiles(dir))
            {
                if (f.EndsWith(oldname))
                {
                    File.WriteAllText(f,File.ReadAllText(f, Encoding.UTF8).Replace(oldlang + ":", newlang + ":"));
                    File.Move(f, f.Replace(oldname, newname));
                }
            }
            foreach (var d in Directory.GetDirectories(dir))
            {
                ChangeFile(d, target, desired);
            }
        }
    }
}
