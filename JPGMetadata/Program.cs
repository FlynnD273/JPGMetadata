using System;
using System.IO;
using System.Reflection;
using ExifLibrary;

namespace JPGMetadata
{
    class Program
    {
        static void Main(string[] args)
        {
            string workingPath = Environment.CurrentDirectory;
            string exePath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            string defaultsPath = Path.GetFullPath(Path.Combine(exePath, "Def.txt"));

            if (!File.Exists(defaultsPath))
            {
                File.WriteAllLines(defaultsPath, new string[] { "Make Canon", "Model Canon Powershot ELPH 180" });
            }

            string[] defaults = File.ReadAllLines(defaultsPath);
            if (defaults.Length < 2)
            {
                defaults = new string[2];
            }

            if (args.Length == 1 && args[0] == "defaults")
            {
                Console.WriteLine($"Default Camera Make: {defaults[0]}");
                Console.WriteLine($"Default Camera Model: {defaults[1]}");

                Console.Write("Default Camera Make: ");
                string make = Console.ReadLine();
                Console.Write("Default Camera Model: ");
                string model = Console.ReadLine();

                defaults[0] = make;
                defaults[1] = model;

                File.WriteAllLines(defaultsPath, defaults);
            }
            else
            {
                string make = defaults[0];
                string model = defaults[1];

                if (args.Length == 2)
                {
                    make = args[1];
                    model = args[2];
                }

                string path = workingPath;
                Console.WriteLine(path);

                string[] files = Directory.GetFiles(path, "*.jpg");
                foreach (string file in files)
                {
                    Console.WriteLine("Editing file " + file);
                    ImageFile image = ImageFile.FromFile(file);
                    image.Properties.Set(ExifTag.Make, make);
                    image.Properties.Set(ExifTag.Model, model);
                    File.Delete(file);
                    image.Save(Path.Combine(Path.GetDirectoryName(file), "Converted - " + Path.GetFileName(file)));
                }
            }
        }

        private static void Error ()
        {
            Console.WriteLine("Invalid arguments. Press Enter to quit.");
            Console.ReadLine();
        }
    }
}
