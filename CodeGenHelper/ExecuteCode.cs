using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenHelper
{
    public static class ExecuteCode
    {
        public static bool ExecuteTestingCode(Compilation compilation, string namespaceOfClass, string staticClassName, string staticMethodName)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    // Emit the compilation to the MemoryStream
                    var result = compilation.Emit(ms);

                    if (result.Success)
                    {
                        // Reset the position of the MemoryStream to the beginning
                        ms.Seek(0, SeekOrigin.Begin);

                        // Load the compiled assembly
                        var loadContext = new AssemblyLoadContext(null, isCollectible: true);
                        var assembly = loadContext.LoadFromStream(ms);

                        // Use reflection to invoke the static method
                        var type = assembly.GetType($"{namespaceOfClass}.{staticClassName}");
                        var method = type!.GetMethod(staticMethodName, BindingFlags.Static | BindingFlags.Public);

                        // Assuming the method is public and static, and does not require parameters
                        var methodResult = method!.Invoke(null, null);

                        bool isSuccess = (bool)methodResult!;

                        return isSuccess;

                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
