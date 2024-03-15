namespace LazinatorFuzzTestGenerator
{
    public class Variable
    {
        public IObjectContents Contents { get; set; }
        public int VariableNumber { get; set; }
        public string VariableName => "v" + VariableNumber;

        public Variable(IObjectContents contents, int variableNumber)
        {
            Contents = contents;
            VariableNumber = variableNumber;
        }

        public string GetAssignmentStatement()
        {
            return VariableName + " = " + Contents.CodeToGetValue + ";";
        }
    }
}
