using LazinatorFuzzTestGenerator.Interfaces;

namespace LazinatorFuzzTestGenerator.ObjectValues
{
    public class Variable
    {
        public IObjectContents Contents { get; set; }
        public string VariableName { get; set; }

        public Variable(IObjectContents contents, string variableName)
        {
            Contents = contents;
            VariableName = variableName;
        }

        public string GetAssignmentStatement()
        {
            return VariableName + " = " + Contents.CodeToReplicateContents + ";";
        }
    }
}
