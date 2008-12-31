// Compile with: csc.exe GenBowel.cs and then run the .exe file
namespace Theminds {
   using System;
   using System.Reflection;
   using System.Text.RegularExpressions;

   class GenMircRegex {
      static void Main() {
         Compile(new RegexCompilationInfo[] {
				Zap(@"\u0003[1-9]{1,2}", "MircRegex"),
				Zap(@"^\d\d\d ", "ServerPrefixNumberRegex")
			});
      }

      static RegexCompilationInfo Zap(string pat, string name) {
         return new RegexCompilationInfo(pat, 
            RegexOptions.IgnoreCase, name, "Bowel", true);
      }

      static void Compile(RegexCompilationInfo[] rciList) {
         AssemblyName an = new AssemblyName();
         an.Name = "Bowel";

         Regex.CompileToAssembly(rciList, an);
      }
   }
}