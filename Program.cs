using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace superMarioRPGRemTextTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args[0].Contains(".txt"))
            {
                Rebuild(args[0]);
            }
            else
            {
                Extract(args[0]);
            }
        }
        public static void Extract(string tbl)
        {
            var reader = new BinaryReader(File.OpenRead(tbl));
            var count = reader.ReadInt32();
            int[] size = new int[count];
            string[] strings = new string[count];
            for (int i = 0; i < count; i++)
            {
                size[i] = reader.ReadInt16() - 2;
            }
            for (int i = 0;i < count; i++)
            {
                strings[i] = Encoding.Unicode.GetString(reader.ReadBytes(size[i]));
                strings[i] = strings[i].Replace("\n", "<lf>").Replace("\r", "<br>");
                reader.BaseStream.Position += 2;
            }
            File.WriteAllLines(tbl + ".txt", strings);
        }
        public static void Rebuild(string txt)
        {
            string[] strings = File.ReadAllLines(txt);
            short[] size = new short[strings.Length];
            using (BinaryWriter writer = new BinaryWriter(File.Create(txt + ".dat")))
            {
                for (int i = 0; i < strings.Length; i++)
                {
                    strings[i] = strings[i].Replace("<lf>", "\n").Replace("<br>", "\r");
                    size[i] = (short)((strings[i].Length * 2) + 2);
                }
                writer.Write(strings.Length);
                for (int i = 0; i < strings.Length; i++)
                {
                    writer.Write(size[i]);
                }
                for (int i = 0; i < strings.Length; i++)
                {
                    writer.Write(Encoding.Unicode.GetBytes(strings[i]));
                    writer.Write(new byte[2]);
                }
            }
        }
    }
}
