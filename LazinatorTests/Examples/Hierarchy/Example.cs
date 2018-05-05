using System;
using Lazinator.Buffers;
using Lazinator.Core;
using Lazinator.Wrappers;
using static Lazinator.Core.LazinatorUtilities;

namespace LazinatorTests.Examples
{
    public partial class Example : IExample
    {
        public Example()
        {
            
        }

        public void LazinatorObjectVersionUpgrade(int oldFormatVersion)
        {
            if (oldFormatVersion < 3 && LazinatorObjectVersion >= 3)
            {
                MyNewString = "NEW " + MyOldString;
                MyOldString = null;
            }
        }

        public static NonLazinatorClass ConvertFromBytes_LazinatorTests_Examples_NonLazinatorClass(ReadOnlyMemory<byte> storage,
            DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate) =>
            Convert_NonLazinatorType.ConvertFromBytes_LazinatorTests_Examples_NonLazinatorClass(storage, deserializationFactory, informParentOfDirtinessDelegate);

        public static void ConvertToBytes_LazinatorTests_Examples_NonLazinatorClass(BinaryBufferWriter writer,
            NonLazinatorClass itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness) =>
            Convert_NonLazinatorType.ConvertToBytes_LazinatorTests_Examples_NonLazinatorClass(writer, itemToConvert,
                includeChildrenMode, verifyCleanness);
    }
}
