using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ConnectionDB
{
    public class iniReader: IDisposable
    {
        private static readonly string EXE = Assembly.GetExecutingAssembly().GetName().Name;
        public string DEFAULT = EXE;
        public string Path;

        public iniReader(string IniPath = null)
        {
            Path = new FileInfo(IniPath != null ? IniPath : EXE + ".vi").FullName;
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string Section, string Key, string Default,
            StringBuilder RetVal, int Size, string FilePath);

        [DllImport("kernel32")]
        private static extern int GetProfileString(string Section, string Key, string Default, StringBuilder RetVal,
            int Size, string FilePath);

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(1024);
            GetPrivateProfileString(Section, Key, null, RetVal, 1024, Path); //"c:\\tf"  != null ? Section : DEFAULT
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section != null ? Section : EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section != null ? Section : EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section != null ? Section : EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0 ? true : false;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}